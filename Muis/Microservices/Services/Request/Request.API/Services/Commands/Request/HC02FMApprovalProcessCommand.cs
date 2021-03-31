using Core.API.Exceptions;
using Core.EventBus;
using Core.Model;
using Request.API.Helpers;
using Request.API.Models;
using Request.API.Repository;
using Request.API.Services.Commands.Request.SubCommands;
using Request.API.Services.Commands.Request.Validators;
using Request.API.Strategies.Certificate360;
using Request.Events;
using Request.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Request
{
  public class HC02FMApprovalProcessCommand
  {
    readonly Review _review;
    readonly Model.Request _requestBasic;
    readonly IEventBus _eventBus;

    readonly DbContext _dbContext;

    public HC02FMApprovalProcessCommand(Model.Request requestBasic, Review review, 
      IEventBus eventBus, DbContext dbContext)
    {
      _requestBasic = requestBasic;
      _review = review;

      _eventBus = eventBus;

      _dbContext = dbContext;
    }

    public async Task Invoke()
    {
      await new ApprovalPolicyValidator(_review, _dbContext)
        .Next(new HC02ApprovalValidator(_review, _dbContext))
        .Next(new AmmendCertificateValidator(_review, _dbContext))
        .Invoke();

      var oldStatus = _requestBasic.Status;

      var approvedLineItems = _review.LineItems.Where(e => e.Approved.Value).ToList();
      var approved = approvedLineItems.Count() >= 1;
      RequestStatus newStatus = approved ? RequestStatus.PendingMuftiAck : RequestStatus.Rejected;

      if (approved && _requestBasic.CodeID == null)
      {
        throw new BadRequestException(
          await _dbContext.Transalation.GetTranslation(Locale.EN, "RequestApproveNoCode"));
      }

      await _dbContext.Request.InsertReview(_review);

      var logText = await _dbContext.Transalation.GetTranslation(Locale.EN,
          approved ? "RequestApproved" : "RequestRejected");

      var logID = await _dbContext.Log.InsertLog(new Log
      {
        Action = logText,
        UserID = _review.ReviewerID.Value,
        UserName = _review.ReviewerName,
      });

      await _dbContext.Request.MapLog(_review.RequestID, logID);

      await _dbContext.Request.InsertActionHistory(new RequestActionHistory
      {
        Action = RequestActionType.Approved,
        RequestID = _review.RequestID,
        Officer = new Officer
        {
          ID = _review.ReviewerID.Value,
          Name = _review.ReviewerName
        }
      });

      await _dbContext.Request.UpdateStatus(_review.RequestID, newStatus, null);

      IList<Certificate> certificates = null;

      if (approved)
      {
        certificates = await GenerateCertificate(approvedLineItems,
          new Officer(_review.ReviewerID.Value, _review.ReviewerName));

      }

      var _holder = new RequestDataHolder()
      {
        Request = _requestBasic,
        OldStatus = oldStatus,
        NewStatus = newStatus,
        Certificates = certificates
      };

      PostCommit(_holder);

      await new SyncNewIngredientSubCommand(
        _review,
        _dbContext,
        _eventBus)
        .Invoke();
    }

    void PostCommit(RequestDataHolder holder)
    {
      _eventBus.Publish(new OnRequestStatusChangedEvent
      {
        ID = holder.Request.ID,
        RefID = holder.Request.RefID,
        OldStatus = holder.OldStatus,
        NewStatus = holder.NewStatus
      });

      var schemes = new List<SchemeHolder>();

      foreach (var item in holder.Request.LineItems)
      {
        var certificate = holder.Certificates?.FirstOrDefault(e => e.Scheme == item.Scheme
          && e.SubScheme == item.SubScheme);

        if (certificate != null)
        {
          var premise = holder.Request.Premises?.FirstOrDefault(e => e.IsPrimary);

          if (!float.TryParse(premise?.Area, out float area))
          {
            area = 0f;
          }

          schemes.Add(new SchemeHolder
          {
            Scheme = item.Scheme.Value,
            SubScheme = item.SubScheme,
            NoOfProducts = certificate.Menus?.Count() ?? 0,
            StartsFrom = certificate.StartsFrom.Value,
            ExpiresOn = certificate.ExpiresOn.Value,
            Area = area
          });
        }
      }

      if (schemes.Any())
      {
        _eventBus.Publish(new GenerateBillEvent
        {
          RequestID = holder.Request.ID,
          RefID = holder.Request.RefID,
          Type = BillType.Stage2,
          RequestType = holder.Request.Type,
          CustomerID = holder.Request.CustomerID,
          CustomerName = holder.Request.CustomerName,
          Expedite = holder.Request.Expedite,
          Schemes = schemes,
          ReferenceDate = holder.Request.SubmittedOn
        });
      }
    }

    async Task<IList<Certificate>> GenerateCertificate(IList<ReviewLineItem> approvedLineItems, 
      Officer officer)
    {
      if (!(approvedLineItems?.Any() ?? false))
      {
        return null;
      }

      var result = new List<Certificate>();

      foreach (var item in approvedLineItems)
      {
        var request = await _dbContext.Request.GetRequestByID(_requestBasic.ID);

        var lineItem = request.LineItems.FirstOrDefault(e => e.Scheme == item.Scheme);
        if (lineItem == null)
        {
          return null;
        }

        var premise = request.Premises.First(e => e.IsPrimary);
        var mailingPremise = request.Premises.FirstOrDefault(e => e.Type == PremiseType.Mailing)
                           ?? premise;

        var customer = await _dbContext.Customer.GetByID(request.CustomerID);

        var certificateNo = lineItem.Characteristics?.FirstOrDefault(e =>
          e.Type == RequestCharType.IssuedCertificate)?.Value;

        // Certificate is null for new application.
        if (string.IsNullOrEmpty(certificateNo))
        {
          throw new BadRequestException(
          await _dbContext.Transalation.GetTranslation(Locale.EN, 
          "RequestApproveNoCertificate"));
        }

        // Generate serial no
        var serialNo = CertificateHelper.GenerateSerialNo(
          await _dbContext.Certificate.GenerateCertificateSerialNo());

        var certificate360 = await _dbContext.Certificate.GetCertificate360ByNo(certificateNo);

        // Certificate batch
        var batchDraft = CertificateHelper.CertificateBatch(
          DateTime.UtcNow,
          lineItem.Scheme.Value,
          lineItem.SubScheme);

        var batch = await _dbContext.Certificate.GetCertificateBatchByCode(batchDraft.Code);
        if (batch == null)
        {
          var batchID = await _dbContext.Certificate.InsertCertificateBatch(batchDraft);
          batch = await _dbContext.Certificate.GetCertificateBatchByIDBasic(batchID);
        }
        
        var index = 0;

        IList<Model.Menu> menus = await _dbContext.Menu.Query(new MenuFilter
        {
          RequestID = request.ID,
          Scheme = lineItem.Scheme
        });

        menus = menus.Where(e =>
           e.Scheme == lineItem.Scheme
        && e.ChangeType != ChangeType.Delete)
          .OrderBy(e => e.Text)
          .Select(e =>
          {
            index++;

            e.Group = 1;
            e.Index = index;

            return e;
          }).ToList();

        var now = DateTimeOffset.UtcNow.Date;
        DateTime date = now.AddMonths(1);
        var certificateStartsFrom = new DateTime(date.Year, date.Month, 1);

        var certificate = new Certificate
        {
          RequestType = request.Type,
          Number = certificateNo,
          Status = CertificateDeliveryStatus.Pending,
          CodeID = request.CodeID,
          SerialNo = serialNo,
          StartsFrom = certificateStartsFrom,
          ExpiresOn = certificate360.ExpiresOn,
          Template = certificate360.Template,
          Scheme = lineItem.Scheme.Value,
          SubScheme = lineItem.SubScheme,
          CustomerID = customer.ID,
          CustomerName = customer.Name,
          PremiseID = premise.ID,
          MailingPremiseID = mailingPremise.ID,
          RequestID = request.ID,
          Menus = menus
        };

        certificate.ID = await _dbContext.Certificate.InsertCertificate(certificate);

        await _dbContext.Certificate.MapCertificate(batch.ID, request.ID, item.Scheme.Value, 
          certificate.ID);

        await new HC02Certificate360Strategy(_dbContext, request, certificate.Number, 
          officer).Invoke();

        result.Add(certificate);
      }

      return result;
    }

  }
}
