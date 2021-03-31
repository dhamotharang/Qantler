using Case.API.Models;
using Case.API.Params;
using Case.API.Service;
using Case.Model;
using Core.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Case.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CaseController : ControllerBase
  {
    readonly ICaseService _caseService;
    readonly IActivityService _activityService;

    public CaseController(ICaseService caseService, IActivityService activityService, ILetterService letterService)
    {
      _caseService = caseService;
      _activityService = activityService;
    }

    [HttpGet]
    [Route("list")]
    public async Task<IEnumerable<Model.Case>> List(
      long? id = null,
      long? source = null,
      DateTimeOffset? from = null,
      DateTimeOffset? to = null,
      [FromQuery] long[] status = null,
      [FromQuery] Guid[] offenceType = null,
      [FromQuery] Guid[] assignedTo = null,
      [FromQuery] Guid[] managedBy = null)
    {
      return await _caseService.SelectCase(new CaseOptions
      {
        ID = id,
        OffenceType = offenceType,
        Source = source,
        ManagedBy = managedBy,
        AssignedTo = assignedTo,
        Status = status,
        From = from,
        To = to
      });
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<Model.Case> GetByID(long id)
    {
      return await _caseService.GetByID(id);
    }

    [HttpPost]
    public async Task<long> Post([FromBody] Model.Case data, Guid userID, string username)
    {
      return await _caseService.InsertCase(data, new Officer(userID, username));
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/activity")]
    public async Task<long> InsertAttachment(long id, [FromBody] Model.Activity data, Guid userID, string username)
    {
      return await _activityService.InsertActivity(data, new Officer(userID, username), id);
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/inspection")]
    public async Task<long> Inspection(long id, CaseScheduleInspectionParam data,
      Guid userID, string username)
    {
      return await _caseService.ScheduleInspection(id, data, new Officer(userID, username));
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/showcause")]
    public async Task<long> ShowCause(long id,
      Letter letter,
      Guid userID,
      string username)
    {
      return await _caseService.ShowCauseLetter(letter, id, new Officer(userID, username));
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/acknowledge")]
    public async Task<long> Acknowledge(long id,
      AcknowledgeShowCauseParam data,
      Guid userID,
      string username)
    {
      return await _caseService.AcknowledgeShowCause(id, data, new Officer(userID, username));
    }

    [HttpPut]
    [Route("/api/[controller]/{id}/foc")]
    public async Task<long> FOCDraft(long id,
      CaseFOCParam data,
      Guid userID,
      string username)
    {
      return await _caseService.FOCDraftLetter(id, data, new Officer(userID, username));
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/foc")]
    public async Task<long> FOCFinal(long id,
      CaseFOCParam data,
      Guid userID,
      string username)
    {
      return await _caseService.FOCFinalLetter(id, data, new Officer(userID, username));
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/review-foc")]
    public async Task<long> FOCApprove(long id,
     ReviewFOCParam data,
     Guid userID,
     string username)
    {
      return await _caseService.FOCApprove(id, data, new Officer(userID, username));
    }

    [HttpPut]
    [Route("/api/[controller]/{id}/review-foc")]
    public async Task<long> FOCReviewDraft(long id,
      ReviewFOCParam data,
      Guid userID,
      string username)
    {
      return await _caseService.FOCReviewDraft(id, data, new Officer(userID, username));
    }

    [HttpDelete]
    [Route("/api/[controller]/{id}/review-foc")]
    public async Task<long> FOCRevert(long id,
      ReviewFOCParam data,
      Guid userID,
      string username)
    {
      return await _caseService.FOCRevert(id, data, new Officer(userID, username));
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/foc-decision")]
    public async Task<long> FOCDecision(long id,
      FOCDecisionParam data,
      Guid userID,
      string username)
    {
      return await _caseService.FOCDecision(id, data, new Officer(userID, username));
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/letter/sanction")]
    public async Task<long> SanctionFinalLetter(long id, Letter letter, Guid userID, string username)
    {
      return await _caseService.SanctionFinalLetter(id, letter, new Officer(userID, username));
    }

    [HttpPut]
    [Route("/api/[controller]/{id}/letter/sanction")]
    public async Task<long> SanctionDraftLetter(long id, Letter letter, Guid userID, string username)
    {
      return await _caseService.SanctionDraftLetter(id, letter, new Officer(userID, username));
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/payment")]
    public async Task<long> AddPayment(long id, PaymentForComposition payment, Guid userID, string username)
    {
      return await _caseService.AddPayment(id, payment, new Officer(userID, username));
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/certificate/collect")]
    public async Task<long> CollectCertificate(long id, Attachment attachment, string senderName,
      Guid userID, string username)
    {
      return await _caseService.CollectCertificate(id, attachment, senderName, new Officer(userID, username));
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/certificate/reinstate")]
    public async Task<long> ReinstateCertificate(long id, Attachment attachment, string senderName,
      Guid userID, string username)
    {
      return await _caseService.ReinstateCertificate(id, attachment, senderName, new Officer(userID, username));
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/reinstate/decision")]
    public async Task<long> ReinstateDecision(long id, ReinstateDecisionParam data,
      Guid userID, string username)
    {
      return await _caseService.ReinstateDecision(id, data, new Officer(userID, username));
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/appeal")]
    public async Task<long> CaseAppeal(long id, CaseAppealParam data,
      Guid userID, string username)
    {
      return await _caseService.CaseAppeal(id, data, new Officer(userID, username));
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/appeal/decision")]
    public async Task<string> CaseAppeal(long id, [FromBody] AppealDecisionParam param,
      Guid userID, string username)
    {
      await _caseService.AppealDecision(id, param, new Officer(userID, username));
      return "Ok";
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/court")]
    public async Task<long> FileCaseToCourt(long id, CaseCourtParam data,
      Guid userID, string username)
    {
      return await _caseService.FileCaseToCourt(id, data, new Officer(userID, username));
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/verdict")]
    public async Task<long> CaseVerdict(long id, CaseCourtParam data,
      Guid userID, string username)
    {
      return await _caseService.CaseVerdict(id, data, new Officer(userID, username));
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/dismiss")]
    public async Task<long> CaseDismiss(long id, CaseDismissParam data,
     Guid userID, string username)
    {
      return await _caseService.CaseDismiss(id, data, new Officer(userID, username));
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/immediate-suspension")]
    public async Task<long> CaseImmediateSuspension(long id, ImmediateSuspensionParam data,
     Guid userID, string username)
    {
      return await _caseService.CaseImmediateSuspension(id, data, new Officer(userID, username));
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/close")]
    public async Task<long> CaseClose(long id, CaseClose data,
     Guid userID, string username)
    {
      return await _caseService.CaseClose(id, data, new Officer(userID, username));
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/reopen")]
    public async Task<long> CaseReopen(long id, CaseReopen data,
     Guid userID, string username)
    {
      return await _caseService.CaseReopen(id, data, new Officer(userID, username));
    }
  }
}