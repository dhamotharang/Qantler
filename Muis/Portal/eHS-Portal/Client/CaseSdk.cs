using Core.Http;
using eHS.Portal.DTO;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eHS.Portal.Client
{
  public class CaseSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public CaseSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public Task<IList<Case>> Query(CaseOptions options)
    {
      var builder = _requestProvider.BuildUpon(_url)
        .Uri("/api/case/list")
        .AddInterceptor(new JsonDeserializerInterceptor());

      if (options.ID > 0L)
      {
        builder.AddParam("id", $"{options.ID}");
      }

      if (!string.IsNullOrEmpty(options.Source.ToString()))
      {
        builder.AddParam("source", options.Source.ToString());
      }

      if (options.OffenceType?.Any() ?? false)
      {
        for (int i = 0; i < options.OffenceType.Count; i++)
        {
          builder.AddParam($"offenceType[{i}]", options.OffenceType[i].ToString());
        }
      }

      if (options.ManagedBy?.Any() ?? false)
      {
        for (int i = 0; i < options.ManagedBy.Count; i++)
        {
          builder.AddParam($"managedBy[{i}]", options.ManagedBy[i].ToString());
        }
      }

      if (options.AssignedTo?.Any() ?? false)
      {
        for (int i = 0; i < options.AssignedTo.Count; i++)
        {
          builder.AddParam($"assignedTo[{i}]", options.AssignedTo[i].ToString());
        }
      }

      if (options.Status?.Any() ?? false)
      {
        for (int i = 0; i < options.Status.Count; i++)
        {
          builder.AddParam($"status[{i}]", options.Status[i].ToString());
        }
      }

      builder.AddParam("from", options.From?.UtcDateTime.ToString())
        .AddParam("to", options.To?.UtcDateTime.ToString());

      return builder.Execute<IList<Case>>();
    }

    public Task<Case> GetCaseByID(long id)
    {
      var builder = _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{id}")
        .AddInterceptor(new JsonDeserializerInterceptor());

      return builder.Execute<Case>();
    }

    public async Task<long> InsertCase(Model.Case data)
    {
      return await _requestProvider.BuildUpon(_url)
       .Uri($"/api/case")
       .Method(HttpMethod.Post)
       .Content(new JsonContent(data))
       .AddInterceptor(new JsonDeserializerInterceptor())
       .Execute<long>();
    }

    public async Task<long> InsertActivity(long id, Activity data)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{id}/activity")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(data))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public async Task<long> ScheduleInspection(long id, CaseScheduleInspectionParam data)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{id}/inspection")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(data))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public async Task<long> ShowCauseLetter(long id, Letter data)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{id}/showcause")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(data))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public async Task<long> AcknowledgeShowCause(long caseID, AcknowledgeShowCauseParam data)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{caseID}/acknowledge")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(data))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public async Task<long> FOCDraftLetter(long caseID, CaseFOCParam data)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{caseID}/foc")
        .Method(HttpMethod.Put)
        .Content(new JsonContent(data))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public async Task<long> FOCFinalLetter(long caseID, CaseFOCParam data)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{caseID}/foc")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(data))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public async Task<long> FOCApprove(long caseID, ReviewFOCParam data)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{caseID}/review-foc")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(data))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public async Task<long> FOCReviewDraft(long caseID, ReviewFOCParam data)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{caseID}/review-foc")
        .Method(HttpMethod.Put)
        .Content(new JsonContent(data))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public async Task<long> FOCRevert(long caseID, ReviewFOCParam data)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{caseID}/review-foc")
        .Method(HttpMethod.Delete)
        .Content(new JsonContent(data))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public async Task<long> FOCDecision(long caseID, FOCDecisionParam data)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{caseID}/foc-decision")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(data))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public async Task<long> SanctionDraftLetter(long caseID, Letter letter)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{caseID}/letter/sanction")
        .Method(HttpMethod.Put)
        .Content(new JsonContent(letter))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public async Task<long> SanctionFinalLetter(long caseID, Letter letter)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{caseID}/letter/sanction")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(letter))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public async Task<long> AddPayment(long caseID, PaymentForComposition payment)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{caseID}/payment")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(payment))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public async Task<long> CollectCertificate(long caseID, Attachment attachment, string senderName)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{caseID}/certificate/collect")
         .AddParam("senderName", senderName)
        .Method(HttpMethod.Post)
        .Content(new JsonContent(attachment))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public async Task<long> ReinstateCertificate(long caseID, Attachment attachment, string senderName)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{caseID}/certificate/reinstate")
         .AddParam("senderName", senderName)
        .Method(HttpMethod.Post)
        .Content(new JsonContent(attachment))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public async Task<long> ReinstateDecision(long caseID, ReinstateDecisionParam data)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{caseID}/reinstate/decision")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(data))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public async Task<long> CaseAppeal(long caseID, CaseAppealParam data)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{caseID}/appeal")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(data))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public async Task<Unit> AppealDecision(long caseID, AppealDecisionParam data)
    {
      await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{caseID}/appeal/decision")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(data))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();

      return Unit.Default;
    }

    public async Task<long> FileCaseToCourt(long caseID, CaseCourtParam data)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{caseID}/court")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(data))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public async Task<long> CaseVerdict(long caseID, CaseCourtParam data)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{caseID}/verdict")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(data))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public async Task<long> CaseDismiss(long caseID, CaseDismissParam data)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{caseID}/dismiss")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(data))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public async Task<long> CaseImmediateSuspension(long caseID, ImmediateSuspensionParam data)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{caseID}/immediate-suspension")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(data))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public async Task<long> CaseClose(long caseID, CaseClose data)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{caseID}/close")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(data))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public async Task<long> CaseReopen(long caseID, CaseReopen data)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/case/{caseID}/reopen")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(data))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }
  }
}
