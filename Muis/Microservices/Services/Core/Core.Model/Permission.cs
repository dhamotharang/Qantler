using System;

namespace Core.Model
{
  public enum Permission
  {
    RequestRead,
    RequestReview,
    RequestApprove,
    RequestInvoice,
    RequestIssuance,
    RequestReassign,
    RequestEscalateAction,
    RequestOverride,
    PeriodicInspectionRead,
    PeriodicInspectionReadWrite,
    MuftiRead,
    MuftiAcknowledge,
    MuftiCommentsRead,
    MuftiCommentsReadWrite,
    PaymentRead,
    PaymentReadWrite,
    TransactionCodeRead,
    TransactionCodeReadWrite,
    UserRead,
    UserReadWrite,
    SystemRead,
    SystemReadWrite,
    RFASupport,
    SetupCustomerCode,
    CaseRead = 23,
    CaseReadWrite = 24,
    RequestReviewApproval = 30,
    CaseFOCReviewer = 31,
    HalalLibraryRead = 40,
    HalalLibraryReadWrite = 41
  }
}
