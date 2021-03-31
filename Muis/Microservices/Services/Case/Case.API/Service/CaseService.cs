using Case.API.Models;
using Case.API.Params;
using Case.API.Service.Commands.Case;
using Case.API.Services.Commands.Case;
using Case.Events;
using Case.Model;
using Core.API;
using Core.API.Provider;
using Core.API.Smtp;
using Core.EventBus;
using Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Case.API.Service
{
  public class CaseService : TransactionalService,
                                               ICaseService
  {
    readonly IEventBus _eventBus;
    readonly ISmtpProvider _smtpProvider;

    public CaseService(IDbConnectionProvider connectionProvider, IEventBus eventBus
      , ISmtpProvider smtpProvider)
        : base(connectionProvider)
    {
      _eventBus = eventBus;
      _smtpProvider = smtpProvider;
    }

    public async Task<IEnumerable<Model.Case>> SelectCase(CaseOptions options)
    {
      return await Execute(new SelectCaseCommand(options));
    }

    public async Task<Model.Case> GetByID(long id)
    {
      return await Execute(new GetCaseByIDCommand(id));
    }

    public async Task<long> InsertCase(Model.Case data, Officer user)
    {
      return await Execute(new InsertCaseCommand(data, user));
    }

    public async Task<long> ScheduleInspection(long caseID,
      CaseScheduleInspectionParam data, Officer user)
    {
      return await Execute(new ScheduleInspectionCommand(caseID, data, user));
    }

    public async Task OnInspectionDone(JobOrderStatus inspectionStatus, long caseID)
    {
      await Execute(new OnJobOrderStatusChangeCommand(inspectionStatus, caseID));
    }

    public async Task<long> ShowCauseLetter(Letter letter, long caseID, Officer user)
    {
      return await Execute(new OnShowCauseLetterCommand(letter, caseID, user, _smtpProvider));
    }

    public async Task<long> AcknowledgeShowCause(long caseID, AcknowledgeShowCauseParam data, Officer user)
    {
      return await Execute(new AcknowledgeShowCauseCommand(caseID, data, user));
    }

    public async Task<long> FOCDraftLetter(long caseID, CaseFOCParam data, Officer user)
    {
      return await Execute(new CaseFOCDraftLetterCommand(caseID, data, user, _eventBus));
    }

    public async Task<long> FOCFinalLetter(long caseID, CaseFOCParam data, Officer user)
    {
      return await Execute(new CaseFOCFinalLetterCommand(caseID, data, user, _eventBus));
    }

    public async Task<long> FOCRevert(long caseID, ReviewFOCParam data, Officer user)
    {
      return await Execute(new CaseFOCRevertCommand(caseID, data, user, _eventBus));
    }

    public async Task<long> FOCApprove(long caseID, ReviewFOCParam data, Officer user)
    {
      return await Execute(new CaseFOCApproveCommand(caseID, data, user, _eventBus));
    }

    public async Task<long> FOCReviewDraft(long caseID, ReviewFOCParam data, Officer user)
    {
      return await Execute(new CaseFOCReviewDraftCommand(caseID, data, user));
    }

    public async Task<long> FOCDecision(long caseID, FOCDecisionParam data, Officer user)
    {
      return await Execute(new FOCDecisionCommand(caseID, data, user, _eventBus));
    }

    public async Task<long> SanctionDraftLetter(long caseID, Letter letter, Officer user)
    {
      return await Execute(new CaseSanctionDraftLetterCommand(caseID, letter, user));
    }

    public async Task<long> SanctionFinalLetter(long caseID, Letter letter, Officer user)
    {
      return await Execute(new CaseSanctionFinalLetterCommand(caseID, letter, user, _eventBus));
    }

    public async Task<long> AddPayment(long caseID, PaymentForComposition payment, Officer user)
    {
      return await Execute(new CaseAddPaymentCommand(caseID, payment, user));
    }

    public async Task<long> OnCompositionPaymentProcessed(long caseID, PaymentStatus PaymentStatus)
    {
      return await Execute(new OnCompositionPaymentProcessedCommand(caseID, PaymentStatus, _eventBus));
    }

    public async Task<long> CollectCertificate(long caseID, Attachment attachment, string senderName, Officer user)
    {
      return await Execute(new CollectCertificateCommand(caseID, attachment, senderName, user, _eventBus));
    }

    public async Task<long> ReinstateCertificate(long caseID, Attachment attachment, string senderName, Officer user)
    {
      return await Execute(new CaseReinstateCertificateCommand(caseID, attachment, senderName, user, _eventBus));
    }

    public async Task<long> ReinstateDecision(long caseID, ReinstateDecisionParam data, Officer user)
    {
      return await Execute(new ReinstateDecisionCommand(caseID, data, user, _eventBus));
    }

    public async Task<long> CaseAppeal(long caseID, CaseAppealParam data, Officer user)
    {
      return await Execute(new CaseAppealCommand(caseID, data, user));
    }

    public async Task AppealDecision(long caseID, AppealDecisionParam param, Officer user)
    {
      await Execute(new AppealDecisionCommand(caseID, param, user, _eventBus));
    }

    public async Task<long> FileCaseToCourt(long caseID, CaseCourtParam data, Officer user)
    {
      return await Execute(new FileCaseToCourtCommand(caseID, data, user));
    }

    public async Task<long> CaseVerdict(long caseID, CaseCourtParam data, Officer user)
    {
      return await Execute(new CaseVerdictCommand(caseID, data, user));
    }

    public async Task<long> CaseDismiss(long caseID, CaseDismissParam data, Officer user)
    {
      return await Execute(new CaseDismissCommand(caseID, data, user, _eventBus));
    }

    public async Task<long> CaseImmediateSuspension(long caseID, ImmediateSuspensionParam data, Officer user)
    {
      return await Execute(new CaseImmediateSuspensionCommand(caseID, data, user, _eventBus));
    }

    public async Task<long> CaseClose(long caseID, CaseClose data, Officer user)
    {
      return await Execute(new CaseCloseCommand(caseID, data, user));
    }

    public async Task<long> CaseReopen(long caseID, CaseReopen data, Officer user)
    {
      return await Execute(new CaseReopenCommand(caseID, data, user));
    }
  }
}