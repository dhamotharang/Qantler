using eHS.Portal.Client;
using eHS.Portal.DTO;
using eHS.Portal.Extensions;
using eHS.Portal.Model;
using eHS.Portal.Models.Case;
using eHS.Portal.Services;
using eHS.Portal.Services.Case;
using eHS.Portal.Services.Master;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Controllers
{
  [Authorize]
  public class CaseController : Controller
  {
    readonly ICaseService _caseService;
    readonly IIdentityService _identityService;
    readonly IMasterService _masterService;
    readonly IPremiseService _premiseService;


    public CaseController(ICaseService caseService,
                                          IIdentityService identityService,
                                          IMasterService masterService,
                                          IPremiseService premiseService)
    {
      _caseService = caseService;
      _identityService = identityService;
      _masterService = masterService;
      _premiseService = premiseService;
    }

    public async Task<IActionResult> Index()
    {
      var model = new IndexModel
      {
        OffenceType = await _masterService.GetMasterList(MasterType.Offence),
        Users = await _identityService.List(new IdentityFilter())
      };

      return View(model);
    }

    [Route("api/[controller]/index")]
    public async Task<IList<Model.Case>> IndexData(
      long? id = null,
      long? source = null,
      DateTimeOffset? from = null,
      DateTimeOffset? to = null,
      [FromQuery] long[] status = null,
      [FromQuery] Guid[] offenceType = null,
      [FromQuery] Guid[] assignedTo = null,
      [FromQuery] Guid[] managedBy = null)
    {
      var options = new CaseOptions
      {
        ID = id,
        OffenceType = offenceType,
        Source = source,
        ManagedBy = managedBy,
        AssignedTo = assignedTo,
        Status = status,
        From = from,
        To = to
      };

      return await _caseService.Query(options);
    }


    [Route("[controller]/details/{id}")]
    public async Task<IActionResult> Details(long id)
    {
      var Data = await _caseService.GetCaseByID(id);
      return View(Data);
    }

    [Route("[controller]/create")]
    public async Task<IActionResult> Create(long id)
    {
      var Data = new DetailModel
      {
        OffenceType = await _masterService.GetMasterList(MasterType.Offence),
        Premises = await _premiseService.GetPremise()
      };
      return View(Data);
    }

    [HttpPost]
    [Route("api/[controller]")]
    public async Task<long> Post([FromBody] Case data)
    {
      return await _caseService.InsertCase(data);
    }

    [HttpPost]
    [Route("api/[controller]/{id:int}/activity")]
    public async Task<long> AddActivity(long id, [FromBody] Activity data)
    {
      return await _caseService.InsertActivity(id, data);
    }

    [HttpPost]
    [Route("api/[controller]/{id:int}/ScheduleInspection")]
    public async Task<long> ScheduleInspection(long id,
      [FromBody] CaseScheduleInspectionParam data)
    {
      return await _caseService.ScheduleInspection(id, data, new Officer(User.GetUserId(), User.GetName()));
    }

    [HttpPost]
    [Route("api/[controller]/{id:int}/showcause")]
    public async Task<long> ShowCause(
      long id, [FromBody] Letter letter)
    {
      return await _caseService.ShowCauseLetter(id, letter);
    }

    [HttpPost]
    [Route("api/[controller]/{id:int}/acknowledge")]
    public async Task<long> Acknowledge(long id,
      [FromBody] AcknowledgeShowCauseParam acknowledgeShowCause)
    {
      return await _caseService.AcknowledgeShowCause(id, acknowledgeShowCause);
    }

    [HttpPut]
    [Route("api/[controller]/{id:int}/foc")]
    public async Task<long> FOCDraft(long id, [FromBody] CaseFOCParam fOCParam)
    {
      return await _caseService.FOCDraftLetter(id, fOCParam);
    }

    [HttpPost]
    [Route("api/[controller]/{id:int}/foc")]
    public async Task<long> FOCFinal(long id, [FromBody] CaseFOCParam fOCParam)
    {
      return await _caseService.FOCFinalLetter(id, fOCParam);
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/review-foc")]
    public async Task<long> FOCApprove(long id, [FromBody] ReviewFOCParam data)
    {
      return await _caseService.FOCApprove(id, data);
    }

    [HttpPut]
    [Route("/api/[controller]/{id}/review-foc")]
    public async Task<long> FOCReviewDraft(long id, [FromBody] ReviewFOCParam data)
    {
      return await _caseService.FOCReviewDraft(id, data);
    }

    [HttpDelete]
    [Route("/api/[controller]/{id}/review-foc")]
    public async Task<long> FOCRevert(long id, [FromBody] ReviewFOCParam data)
    {
      return await _caseService.FOCRevert(id, data);
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/foc-decision")]
    public async Task<long> FOCDecision(long id, [FromBody] FOCDecisionParam data)
    {
      return await _caseService.FOCDecision(id, data);
    }

    [HttpPut]
    [Route("/api/[controller]/{id}/letter/sanction")]
    public async Task<long> SanctionDraftLetter(long id, [FromBody] Letter letter)
    {
      return await _caseService.SanctionDraftLetter(id, letter);
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/letter/sanction")]
    public async Task<long> SanctionFinalLetter(long id, [FromBody] Letter letter)
    {
      return await _caseService.SanctionFinalLetter(id, letter);
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/payment/composition")]
    public async Task<long> CompositionPayment(long id, [FromBody] PaymentForComposition payment)
    {
      return await _caseService.CompositionPayment(id, payment);
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/certificate/collect")]
    public async Task<long> CollectCertificate(long id, [FromBody] Attachment attachment, string senderName)
    {
      return await _caseService.CollectCertificate(id, attachment, senderName);
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/certificate/reinstate")]
    public async Task<long> ReinstateCertificate(long id, [FromBody] Attachment attachment, string senderName)
    {
      return await _caseService.ReinstateCertificate(id, attachment, senderName);
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/reinstate/decision")]
    public async Task<long> ReinstateDecision(long id, [FromBody] ReinstateDecisionParam data)
    {
      return await _caseService.ReinstateDecision(id, data);
    }

    [Route("/api/[controller]/{id}/appeal")]
    public async Task<long> CaseAppeal(long id, [FromBody] CaseAppealParam data)
    {
      return await _caseService.CaseAppeal(id, data);
    }

    [Route("/api/[controller]/{id}/appeal/decision")]
    public async Task<string> AppealDecision(long id, [FromBody] AppealDecisionParam param)
    {
      await _caseService.AppealDecision(id, param);
      return "Ok";
    }

    [Route("/api/[controller]/{id}/court")]
    public async Task<long> FileCaseToCourt(long id, [FromBody] CaseCourtParam data)
    {
      return await _caseService.FileCaseToCourt(id, data);
    }

    [Route("/api/[controller]/{id}/verdict")]
    public async Task<long> CaseVerdict(long id, [FromBody] CaseCourtParam data)
    {
      return await _caseService.CaseVerdict(id, data);
    }

    [Route("/api/[controller]/{id}/dismiss")]
    public async Task<long> CaseDismiss(long id, [FromBody] CaseDismissParam data)
    {
      return await _caseService.CaseDismiss(id, data);
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/immediate-suspension")]
    public async Task<long> CaseImmediateSuspension(long id, [FromBody] ImmediateSuspensionParam data)
    {
      return await _caseService.CaseImmediateSuspension(id, data);
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/close")]
    public async Task<long> CaseClose(long id, [FromBody] CaseClose data)
    {
      return await _caseService.CaseClose(id, data);
    }

    [HttpPost]
    [Route("/api/[controller]/{id}/reopen")]
    public async Task<long> CaseReopen(long id, [FromBody] CaseReopen data)
    {
      return await _caseService.CaseReopen(id, data);
    }
  }
}
