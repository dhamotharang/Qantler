using Core.Http.Exceptions;
using eHS.Portal.Client;
using eHS.Portal.Configs;
using eHS.Portal.DTO;
using eHS.Portal.Model;
using IronPdf;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eHS.Portal.Services.Case
{
  public class CaseService : ICaseService
  {
    readonly ApiClient _apiClient;
    readonly IOptions<UrlConfig> _urlConfig;

    public CaseService(ApiClient apiClient, IOptions<UrlConfig> urlConfig)
    {
      _apiClient = apiClient;
      _urlConfig = urlConfig;
    }

    public async Task<Model.Case> GetCaseByID(long id)
    {
      var result = await _apiClient.CaseSdk.GetCaseByID(id);
      return result;
    }

    public async Task<long> InsertCase(Model.Case data)
    {
      if (data.Premises.Count > 0)
      {
        var certificates = await _apiClient.Certificate360.Query(
          new Certificate360Options
          {
            PremiseIDs = data.Premises.Select(x => x.ID).ToArray()
          });
        data.Certificates = certificates.Where(x => x.ExpiresOn >= DateTime.UtcNow
          && x.CustomerID == data.Offender.ID).ToList();
      }
      var result = await _apiClient.CaseSdk.InsertCase(data);
      return result;
    }

    public async Task<IList<Model.Case>> Query(CaseOptions options)
    {
      var result = await _apiClient.CaseSdk.Query(options);
      return result;
    }

    public async Task<long> InsertActivity(long caseID, Model.Activity data)
    {
      var result = await _apiClient.CaseSdk.InsertActivity(caseID, data);
      return result;
    }

    public async Task<long> ScheduleInspection(long caseID,
      CaseScheduleInspectionParam data, Officer officer)
    {
      var @case = await _apiClient.CaseSdk.GetCaseByID(caseID);

      var premise = @case.Premises.Where(x => x.ID == data.PremiseID).FirstOrDefault();
      var lineItems = new List<JobOrderLineItem>();

      var certificates = @case.Certificates.Where(x => x.PremiseID == data.PremiseID).GroupBy(x => x.Scheme).Select(x => x.FirstOrDefault()).ToList();

      foreach (var caseLineItem in certificates)
      {
        var checklist = _apiClient.ChecklistSdk.GetLatest(caseLineItem.Scheme);

        lineItems.Add(new JobOrderLineItem
        {
          Scheme = caseLineItem.Scheme,
          SubScheme = caseLineItem.SubScheme,
          ChecklistHistoryID = checklist.Id
        });
      }

      var jobOrder = new ScheduleJobOrderParam
      {
        RefID = caseID,
        Type = data.Type,
        Status = JobOrderStatus.Pending,
        ScheduledOn = data.ScheduledOn,
        ScheduledOnTo = data.ScheduledOnTo,
        Notes = data.Notes,
        Officer = new Officer
        {
          ID = officer.ID,
          Name = officer.Name
        },
        Customer = new Model.Customer
        {
          ID = @case.Offender.ID,
          Name = @case.Offender.Name
        },
        Premises = new List<Premise>
        {
          premise
        },
        LineItems = lineItems
      };

      data.JobOrderID = (await _apiClient.JobOrderSdk.Submit(jobOrder)).ID;

      var result = await _apiClient.CaseSdk.ScheduleInspection(caseID, data);

      return result;
    }

    public async Task<long> ShowCauseLetter(long caseID, Letter data)
    {

      // TODO temporary mail with attachment. Delete later
      data.Email = new Email { Attachments = new List<EmailAttachment>() };

      var fileName = $"{caseID + "_Letter"}.pdf";

      var file = GenerateLetterToPdf(data.Body);

      data.Email.Attachments.Add(new EmailAttachment
      {
        Data = Convert.ToBase64String(file),
        Name = fileName
      });

      return await _apiClient.CaseSdk.ShowCauseLetter(caseID, data);
    }

    public async Task<long> AcknowledgeShowCause(long caseID, AcknowledgeShowCauseParam data)
    {
      return await _apiClient.CaseSdk.AcknowledgeShowCause(caseID, data);
    }

    public async Task<long> FOCDraftLetter(long caseID, CaseFOCParam data)
    {
      var result = await _apiClient.CaseSdk.FOCDraftLetter(caseID, data);

      return result;
    }

    public async Task<long> FOCFinalLetter(long caseID, CaseFOCParam data)
    {
      IdentityFilter identityFilter = new IdentityFilter
      {
        Permissions = new Permission[] { Permission.CaseFOCReviewer }
      };

      data.Reviewer = await _apiClient.IdentitySdk.List(identityFilter);

      if (!data.Reviewer.Any())
      {
        throw new BadRequestException("Unable to submit FOC for approval. " +
          "No user with Case.FOCReviewer permission. " +
          "Please contact administrator");
      }

      var result = await _apiClient.CaseSdk.FOCFinalLetter(caseID, data);

      return result;
    }

    public async Task<long> FOCRevert(long caseID, ReviewFOCParam data)
    {
      return await _apiClient.CaseSdk.FOCRevert(caseID, data);
    }

    public async Task<long> FOCApprove(long caseID, ReviewFOCParam data)
    {
      return await _apiClient.CaseSdk.FOCApprove(caseID, data);
    }

    public async Task<long> FOCReviewDraft(long caseID, ReviewFOCParam data)
    {
      return await _apiClient.CaseSdk.FOCReviewDraft(caseID, data);
    }

    public async Task<long> FOCDecision(long caseID, FOCDecisionParam data)
    {
      return await _apiClient.CaseSdk.FOCDecision(caseID, data);
    }

    public async Task<long> SanctionDraftLetter(long caseID, Letter letter)
    {
      return await _apiClient.CaseSdk.SanctionDraftLetter(caseID, letter);
    }

    public async Task<long> SanctionFinalLetter(long caseID, Letter letter)
    {
      return await _apiClient.CaseSdk.SanctionFinalLetter(caseID, letter);
    }

    public async Task<long> CompositionPayment(long caseID, PaymentForComposition payment)
    {
      payment.Mode = PaymentMode.Offline;
      payment.Method = PaymentMethod.BankTransfer;

      payment.RefID = (await _apiClient.PaymentSdk.
        PaymentForComposition(payment)).ID.ToString();

      return await _apiClient.CaseSdk.AddPayment(caseID, payment);
    }

    public async Task<long> CollectCertificate(long caseID, Attachment attachment, string senderName)
    {
      return await _apiClient.CaseSdk.CollectCertificate(caseID, attachment, senderName);
    }

    public async Task<long> ReinstateCertificate(long caseID, Attachment attachment, string senderName)
    {
      return await _apiClient.CaseSdk.ReinstateCertificate(caseID, attachment, senderName);
    }

    public async Task<long> ReinstateDecision(long caseID, ReinstateDecisionParam data)
    {
      return await _apiClient.CaseSdk.ReinstateDecision(caseID, data);
    }

    public async Task<long> CaseAppeal(long caseID, CaseAppealParam data)
    {
      return await _apiClient.CaseSdk.CaseAppeal(caseID, data);
    }

    public async Task AppealDecision(long caseID, AppealDecisionParam param)
    {
      await _apiClient.CaseSdk.AppealDecision(caseID, param);
    }

    public async Task<long> FileCaseToCourt(long caseID, CaseCourtParam data)
    {
      return await _apiClient.CaseSdk.FileCaseToCourt(caseID, data);
    }

    public async Task<long> CaseVerdict(long caseID, CaseCourtParam data)
    {
      return await _apiClient.CaseSdk.CaseVerdict(caseID, data);
    }

    public async Task<long> CaseDismiss(long caseID, CaseDismissParam data)
    {
      return await _apiClient.CaseSdk.CaseDismiss(caseID, data);
    }

    public async Task<long> CaseImmediateSuspension(long caseID, ImmediateSuspensionParam data)
    {
      return await _apiClient.CaseSdk.CaseImmediateSuspension(caseID, data);
    }

    public async Task<long> CaseClose(long caseID, CaseClose data)
    {
      return await _apiClient.CaseSdk.CaseClose(caseID, data);
    }

    public async Task<long> CaseReopen(long caseID, CaseReopen data)
    {
      return await _apiClient.CaseSdk.CaseReopen(caseID, data);
    }

    public byte[] GenerateLetterToPdf(string body)
    {

      var regex = new Regex("<img.*?src=\"(.*?)\"[^>]+>",
          RegexOptions.IgnoreCase);
      var matches = regex.Matches(body);

      for (int i = 0; i < matches.Count; i++)
      {
        var key = matches[i].Groups[1].Value;
        var uri = key.Replace("../..", _urlConfig.Value.File);
        body = body.Replace(key, uri, StringComparison.InvariantCultureIgnoreCase);
      }

      var ironPdfRender = new HtmlToPdf();
      ironPdfRender.PrintOptions.MarginLeft = 0;
      ironPdfRender.PrintOptions.MarginTop = 2;
      ironPdfRender.PrintOptions.MarginRight = 0;
      ironPdfRender.PrintOptions.MarginBottom = 0;
      ironPdfRender.PrintOptions.EnableJavaScript = true;
      ironPdfRender.PrintOptions.PaperSize = PdfPrintOptions.PdfPaperSize.Custom;
      ironPdfRender.PrintOptions.InputEncoding = Encoding.UTF8;
      ironPdfRender.PrintOptions.RenderDelay = 500;

      var pdfDoc = ironPdfRender.RenderHtmlAsPdf(body);

      var headerFooter = new HtmlHeaderFooter();
      headerFooter.Height = 0;
      headerFooter.Spacing = 0;
      pdfDoc.AddHTMLFooters(headerFooter, 0, 0, 0);
      pdfDoc.AddHTMLHeaders(headerFooter, 0, 0, 0);

      return pdfDoc.BinaryData;
    }
  }
}
