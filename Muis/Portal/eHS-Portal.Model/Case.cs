using System;
using System.Collections.Generic;

namespace eHS.Portal.Model
{
  public enum CaseStatus
  {
    Open = 100,
    PendingInspection = 200,
    PendingShowCause = 300,
    ShowCauseForApproval = 400,
    PendingAcknowledgement = 500,
    PendingFOC = 600,
    FOCForApproval = 700,
    PendingFOCDecision = 750,
    PendingSanctionLetter = 800,
    PendingPayment = 900,
    CertificateCollection = 1000,
    PendingReinstateInspection = 1100,
    PendingAppeal = 1200,
    PendingApprovedAppealLetter = 1210,
    PendingRejectedAppealLetter = 1220,
    ProceedingInProgress = 1250,
    ReinstateCertificate = 1275,
    Closed = 1300,
    Dismissed = 1400
  }

  public enum CaseMinorStatus
  {
    InspectionDone = 210,
    InspectionInProgress = 220,
    InspectionCancelled = 230,
    SanctionLetterSent = 810,
    PaymentReceived = 910,
    PaymentProcessed = 920,
    PaymentRejected = 930,
    CertificateCollected = 1010
  }

  public enum CaseType
  {
    Enforcement
  }

  public class Case
  {
    public long ID { get; set; }

    public string RefID { get; set; }

    public string Title { get; set; }

    public Source? Source { get; set; }

    public CaseStatus Status { get; set; }

    public CaseMinorStatus? MinorStatus { get; set; }

    public CaseStatus? OldStatus { get; set; }

    public CaseStatus? OtherStatus { get; set; }

    public CaseMinorStatus? OtherStatusMinor { get; set; }

    public CaseType? Type { get; set; }

    public int Occurrence { get; set; }

    public Sanction? Sanction { get; set; }

    public string Background { get; set; }

    public Guid? OffenderID { get; set; }

    public Guid? ManagedByID { get; set; }

    public Guid? AssignedToID { get; set; }

    public DateTimeOffset? SuspendedFrom { get; set; }

    public Guid? CreatedByID { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public string SourceText { get; set; }

    public string StatusText { get; set; }

    public string OldStatusText { get; set; }

    public string StatusMinorText { get; set; }

    public string SanctionText { get; set; }

    public string TypeText { get; set; }

    public Offender Offender { get; set; }

    public Officer ManagedBy { get; set; }

    public Officer AssignedTo { get; set; }

    public Person ReportedBy { get; set; }

    public IList<Master> BreachCategories { get; set; }

    public IList<Master> Offences { get; set; }

    public IList<Attachment> Attachments { get; set; }

    public IList<Certificate> Certificates { get; set; }

    public IList<Premise> Premises { get; set; }

    public IList<SanctionInfo> SanctionInfos { get; set; }

    public IList<Activity> Activities { get; set; }

    #endregion
  }
}
