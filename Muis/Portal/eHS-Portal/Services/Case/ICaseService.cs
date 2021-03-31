using eHS.Portal.Client;
using eHS.Portal.DTO;
using eHS.Portal.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Services.Case
{
  public interface ICaseService
  {
    /// <summary>
    /// Retrieve Case details list.
    /// </summary>
    Task<IList<Model.Case>> Query(CaseOptions options);

    /// <summary>
    /// Retrieve Case details with specified ID.
    /// </summary>
    Task<Model.Case> GetCaseByID(long id);

    /// <summary>
    /// Insert Case.
    /// </summary>
    Task<long> InsertCase(Model.Case data);

    /// <summary>
    /// Insert Activity.
    /// </summary>
    Task<long> InsertActivity(long caseID, Activity data);

    /// <summary>
    /// Schedule Inspection
    /// </summary>
    Task<long> ScheduleInspection(long caseID, CaseScheduleInspectionParam data, Officer officer);

    /// <summary>
    /// Insert Show Cause Letter.
    /// </summary>
    Task<long> ShowCauseLetter(long caseID, Letter data);

    /// <summary>
    /// Acknowledge Show Cause Letter.
    /// </summary>
    Task<long> AcknowledgeShowCause(long id, AcknowledgeShowCauseParam data);

    /// <summary>
    /// FOC Draft Letter.
    /// </summary>
    Task<long> FOCDraftLetter(long caseID, CaseFOCParam data);

    /// <summary>
    /// FOC Final Letter.
    /// </summary>
    Task<long> FOCFinalLetter(long caseID, CaseFOCParam data);

    /// <summary>
    /// FOC Revert.
    /// </summary>
    Task<long> FOCRevert(long caseID, ReviewFOCParam data);

    /// <summary>
    /// FOC Approve.
    /// </summary>
    Task<long> FOCApprove(long caseID, ReviewFOCParam data);

    /// <summary>
    /// FOC Approver Draft.
    /// </summary>
    Task<long> FOCReviewDraft(long caseID, ReviewFOCParam data);

    /// <summary>
    /// FOC Final Decision.
    /// </summary>
    Task<long> FOCDecision(long caseID, FOCDecisionParam data);

    /// <summary>
    /// Draft Sanction Letter.
    /// </summary>
    Task<long> SanctionDraftLetter(long caseID, Letter letter);

    /// <summary>
    /// Final Sanction Letter.
    /// </summary>
    Task<long> SanctionFinalLetter(long caseID, Letter letter);

    /// <summary>
    /// Add Composition Payment.
    /// </summary>
    Task<long> CompositionPayment(long caseID, PaymentForComposition payment);

    /// <summary>
    /// Collect Certificate.
    /// </summary>
    Task<long> CollectCertificate(long caseID, Attachment attachment, string senderName);

    /// <summary>
    /// Reinstate Certificate.
    /// </summary>
    Task<long> ReinstateCertificate(long caseID, Attachment attachment, string senderName);

    /// <summary>
    /// Reinstate Decision.
    /// </summary>
    Task<long> ReinstateDecision(long caseID, ReinstateDecisionParam data);

    /// <summary>
    /// Submit Case Appeal.
    /// </summary>
    Task<long> CaseAppeal(long caseID, CaseAppealParam data);

    /// <summary>
    /// Submit Case Appeal Decision.
    /// </summary>
    Task AppealDecision(long caseID, AppealDecisionParam data);

    /// <summary>
    /// Submit Case File To Court
    /// </summary>
    Task<long> FileCaseToCourt(long caseID, CaseCourtParam data);

    /// <summary>
    ///Case Verdict.
    /// </summary>
    Task<long> CaseVerdict(long caseID, CaseCourtParam data);

    /// <summary>
    ///Case Dismiss.
    /// </summary>
    Task<long> CaseDismiss(long caseID, CaseDismissParam data);

    /// <summary>
    ///Case Immediate Suspension.
    /// </summary>
    Task<long> CaseImmediateSuspension(long caseID, ImmediateSuspensionParam data);

    /// <summary>
    ///Case Close.
    /// </summary>
    Task<long> CaseClose(long caseID, CaseClose data);

    /// <summary>
    ///Case Reopen.
    /// </summary>
    Task<long> CaseReopen(long caseID, CaseReopen data);
  }
}
