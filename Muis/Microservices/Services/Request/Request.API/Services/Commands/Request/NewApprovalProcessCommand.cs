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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Request
{
  public class NewApprovalProcessCommand
  {
    readonly Review _review;
    readonly Model.Request _requestBasic;

    readonly IEventBus _eventBus;

    readonly DbContext _dbContext;

    public NewApprovalProcessCommand(Model.Request requestBasic, Review review, 
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
        .Invoke();

      var oldStatus = _requestBasic.Status;

      var approvedLineItems = _review.LineItems.Where(e => e.Approved.Value).ToList();
      var approved = approvedLineItems.Count() >= 1;
      RequestStatus newStatus = approved ? RequestStatus.PendingMuftiAck : RequestStatus.Rejected;

      if (approved
        && _requestBasic.CodeID == null)
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

      var stage2Items = holder.Request.LineItems.Where(e => RequestHelper.HasStage2Payment(
        holder.Request.Type,
        e.Scheme,
        e.SubScheme)).ToList();

      if (stage2Items.Any())
      {
        var schemes = new List<SchemeHolder>();

        foreach (var item in stage2Items)
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
        var _certificate = await new GenerateCertificateSubCommand(_requestBasic, 
          item.Scheme.Value, officer, _dbContext).Invoke();

        await new NewCertificate360Strategy(_dbContext, _requestBasic,
         _certificate, officer).Invoke();

        result.Add(_certificate);
      }

      return result;
    }
  }
}
