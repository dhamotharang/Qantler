using Case.API.Models;
using Case.API.Params;
using Case.Events;
using Case.Model;
using Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Case.API.Service
{
  public interface ICaseService
  {
    /// <summary>
    /// Gets all case.
    /// </summary>
    /// <returns>The case data.</returns>
    /// <param name="options">Case Options.</param>
    Task<IEnumerable<Model.Case>> SelectCase(CaseOptions options);

    /// <summary>
    /// Gets case by ID.
    /// </summary>
    /// <returns>The case data.</returns>
    /// <param name="id">long.</param>
    Task<Model.Case> GetByID(long id);

    /// <summary>
    /// Insert case.
    /// </summary>
    /// <returns>Case ID</returns>
    /// <param name="id">Case</param>
    /// <param name="user">Officier</param>
    Task<long> InsertCase(Model.Case data, Officer user);

    /// <summary>
    /// Insert case.
    /// </summary>
    /// <returns>Case ID</returns>
    /// <param name="data">CaseScheduleInspectionParam</param>
    /// <param name="user">Officier</param>
    /// <param name="caseID">long</param>
    Task<long> ScheduleInspection(long caseID, CaseScheduleInspectionParam data, Officer user);

    /// <summary>
    /// Update inspection action.
    /// </summary>
    /// <param name="inspectionStatus">JobOrderStatus</param>
    /// <param name="caseID">long</param>
    Task OnInspectionDone(JobOrderStatus inspectionStatus, long caseID);

    /// <summary>
    /// Show Cause Letter.
    /// </summary>
    /// <param name="letter">Letter</param>
    /// <param name="caseID">long</param>
    /// <param name="user">Officer</param>
    Task<long> ShowCauseLetter(Letter letter, long caseID, Officer user);

    /// <summary>
    /// Show Cause Letter.
    /// </summary>
    /// <param name="acknowledge">data</param>
    /// <param name="caseID">long</param>
    /// <param name="user">Officer</param>
    Task<long> AcknowledgeShowCause(long caseID, AcknowledgeShowCauseParam data, Officer user);

    /// <summary>
    /// Draft FOC Letter.
    /// </summary>
    /// <param name="data">CaseFOCParam</param>
    /// <param name="caseID">long</param>
    /// <param name="user">Officer</param>
    Task<long> FOCDraftLetter(long caseID, CaseFOCParam data, Officer user);

    /// <summary>
    /// Final FOC Letter.
    /// </summary>
    /// <param name="data">CaseFOCParam</param>
    /// <param name="caseID">long</param>
    /// <param name="user">Officer</param>
    Task<long> FOCFinalLetter(long caseID, CaseFOCParam data, Officer user);

    /// <summary>
    /// FOC Revert.
    /// </summary>
    /// <param name="data">ReviewFOCParam</param>
    /// <param name="caseID">long</param>
    /// <param name="user">Officer</param>
    Task<long> FOCRevert(long caseID, ReviewFOCParam data, Officer user);

    /// <summary>
    /// FOC Approve.
    /// </summary>
    /// <param name="data">ReviewFOCParam</param>
    /// <param name="caseID">long</param>
    /// <param name="user">Officer</param>
    Task<long> FOCApprove(long caseID, ReviewFOCParam data, Officer user);

    /// <summary>
    /// FOC Approver Draft.
    /// </summary>
    /// <param name="data">ReviewFOCParam</param>
    /// <param name="caseID">long</param>
    /// <param name="user">Officer</param>
    Task<long> FOCReviewDraft(long caseID, ReviewFOCParam data, Officer user);

    /// <summary>
    /// FOC final decision.
    /// </summary>
    /// <param name="FOCDecisionParam">data</param>
    /// <param name="caseID">long</param>
    /// <param name="user">Officer</param>
    Task<long> FOCDecision(long caseID, FOCDecisionParam data, Officer user);

    /// <summary>
    /// Sanction Draft Letter.
    /// </summary>
    /// <param name="Letter">letter</param>
    /// <param name="caseID">long</param>
    /// <param name="user">Officer</param>
    Task<long> SanctionDraftLetter(long caseID, Letter letter, Officer user);

    // <summary>
    /// Sanction Final Letter.
    /// </summary>
    /// <param name="Letter">letter</param>
    /// <param name="caseID">long</param>
    /// <param name="user">Officer</param>
    Task<long> SanctionFinalLetter(long caseID, Letter letter, Officer user);

    // <summary>
    /// Add Payment.
    /// </summary>
    /// <param name="CompositionPayment">payment</param>
    /// <param name="caseID">long</param>
    /// <param name="user">Officer</param>
    Task<long> AddPayment(long caseID, PaymentForComposition payment, Officer user);

    // <summary>
    ///OnCompositionPaymentProcessed.
    /// </summary>
    /// <param name="PaymentStatus">PaymentStatus</param>
    /// <param name="caseID">long</param>
    Task<long> OnCompositionPaymentProcessed(long caseID, PaymentStatus PaymentStatus);

    // <summary>
    ///Collect Certificate.
    /// </summary>
    /// <param name="attachment">Attachment</param>
    /// <param name="caseID">long</param>
    /// <param name="user">Officer</param>
    /// <param name="senderName">string</param>
    Task<long> CollectCertificate(long caseID, Attachment attachment, string senderName, Officer user);

    // <summary>
    ///Reinstate Certificate.
    /// </summary>
    /// <param name="attachment">Attachment</param>
    /// <param name="caseID">long</param>
    /// <param name="user">Officer</param>
    /// <param name="senderName">string</param>
    Task<long> ReinstateCertificate(long caseID, Attachment attachment, string senderName, Officer user);

    // <summary>
    ///Reinstate Decision.
    /// </summary>
    /// <param name="data">ReinstateDecisionParam</param>
    /// <param name="caseID">long</param>
    /// <param name="user">Officer</param>
    Task<long> ReinstateDecision(long caseID, ReinstateDecisionParam data, Officer user);

    // <summary>
    ///Submit Case Appeal.
    /// </summary>
    /// <param name="data">CaseAppealParam</param>
    /// <param name="caseID">long</param>
    /// <param name="user">Officer</param>
    Task<long> CaseAppeal(long caseID, CaseAppealParam data, Officer user);

    // <summary>
    ///Submit Case Appeal Decision.
    /// </summary>
    /// <param name="param">AppealDecisionParam</param>
    /// <param name="caseID">long</param>
    /// <param name="user">Officer</param>
    Task AppealDecision(long caseID, AppealDecisionParam data, Officer user);

    // <summary>
    ///File Case to Court.
    /// </summary>
    /// <param name="data">CaseCourtParam</param>
    /// <param name="caseID">long</param>
    /// <param name="user">Officer</param>
    Task<long> FileCaseToCourt(long caseID, CaseCourtParam data, Officer user);

    // <summary>
    ///Case Verdict.
    /// </summary>
    /// <param name="data">CaseCourtParam</param>
    /// <param name="caseID">long</param>
    /// <param name="user">Officer</param>
    Task<long> CaseVerdict(long caseID, CaseCourtParam data, Officer user);

    // <summary>
    ///Case Verdict.
    /// </summary>
    /// <param name="data">CaseDismissParam</param>
    /// <param name="caseID">long</param>
    /// <param name="user">Officer</param>
    Task<long> CaseDismiss(long caseID, CaseDismissParam data, Officer user);

    // <summary>
    ///Case Immediate Suspension.
    /// </summary>
    /// <param name="data">ImmediateSuspensionParam</param>
    /// <param name="caseID">long</param>
    /// <param name="user">Officer</param>
    Task<long> CaseImmediateSuspension(long caseID, ImmediateSuspensionParam data, Officer user);

    // <summary>
    ///Submit Case Close.
    /// </summary>
    /// <param name="data">CaseClose</param>
    /// <param name="caseID">long</param>
    /// <param name="user">Officer</param>
    Task<long> CaseClose(long caseID, CaseClose data, Officer user);

    // <summary>
    ///Submit Case reopen.
    /// </summary>
    /// <param name="data">CaseReopen</param>
    /// <param name="caseID">long</param>
    /// <param name="user">Officer</param>
    Task<long> CaseReopen(long caseID, CaseReopen data, Officer user);
  }
}
