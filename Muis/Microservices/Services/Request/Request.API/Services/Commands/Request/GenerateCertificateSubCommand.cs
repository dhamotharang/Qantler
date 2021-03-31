using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Model;
using Request.API.Helpers;
using Request.API.Repository;
using Request.API.Strategies.Certificate360;
using Request.Model;

namespace Request.API.Services.Commands.Request
{
  public class GenerateCertificateSubCommand
  {
    readonly Model.Request _requestBasic;
    readonly Scheme _scheme;

    readonly Officer _officer;

    readonly DbContext _dbContext;

    public GenerateCertificateSubCommand(Model.Request requestBasic, Scheme scheme, Officer officer,
      DbContext dbContext)
    {
      _requestBasic = requestBasic;
      _scheme = scheme;
      _officer = officer;

      _dbContext = dbContext;
    }

    public async Task<Certificate> Invoke()
    {
      if (   _requestBasic.Type != RequestType.New
          && _requestBasic.Type != RequestType.Renewal
          && _requestBasic.Type != RequestType.HC03)
      {
        return null;
      }

      var request = await _dbContext.Request.GetRequestByID(_requestBasic.ID);

      var lineItem = request.LineItems.FirstOrDefault(e => e.Scheme == _scheme);
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
      if (certificateNo == null)
      {
        var seqNo = await _dbContext.Certificate.GenerateCertificateSequence();
        certificateNo = CertificateHelper.GenerateCertificateNo(lineItem.Scheme.Value,
          lineItem.SubScheme,
          seqNo);
      }

      // Generate serial no
      var serialNo = CertificateHelper.GenerateSerialNo(
        await _dbContext.Certificate.GenerateCertificateSerialNo());

      // Certificate template
      var template = CertificateHelper.CertificateTemplate(
        lineItem.Scheme.Value,
        lineItem.SubScheme);

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

      // Calculate certificate expiry

      var durationInYears = 0;
      if (_requestBasic.Type != RequestType.HC03)
      {
        durationInYears = int.Parse(
          request.Characteristics.First(e => e.Type == RequestCharType.Duration).Value);
      }

      var certificate360 = await _dbContext.Certificate.GetCertificate360ByNo(certificateNo);

      var refCertificateExpiry = customer.GroupCode?.CertificateExpiry
                              ?? customer.Code?.CertificateExpiry;

      var duration = CertificateHelper.CalculateCertificateDuration(
        _requestBasic.Type,
        certificate360?.ExpiresOn,
        refCertificateExpiry,
        durationInYears);

      IList<Model.Menu> menus = null;
      if (lineItem.Scheme == Scheme.FoodManufacturing)
      {
        var index = 0;

        menus = await _dbContext.Menu.Query(new MenuFilter
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
      }

      var certificate = new Certificate
      {
        RequestType = request.Type,
        Number = certificateNo,
        Status = CertificateDeliveryStatus.Pending,
        CodeID = request.CodeID,
        SerialNo = serialNo,
        StartsFrom = duration.Item1,
        ExpiresOn = duration.Item2,
        Template = batch.Template,
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

      await _dbContext.Certificate.MapCertificate(batch.ID, request.ID, _scheme, certificate.ID);

      /* Update customer billing cycle */
      if (   refCertificateExpiry == null
          || refCertificateExpiry < duration.Item2)
      {
        var from = duration.Item1.Month;
        var billingCycle = $"BC{$"{from}".PadLeft(2, '0')}";

        var code = customer.Code;
        if (code != null)
        {
          code.BillingCycle = billingCycle;
          code.CertificateExpiry = duration.Item2;

          await _dbContext.Code.Update(code);
        }

        var group = customer.GroupCode;
        if (group != null)
        {
          group.BillingCycle = billingCycle;
          group.CertificateExpiry = duration.Item2;

          await _dbContext.Code.Update(group);
        }
      }
      
      return certificate;
    }
  }
}
