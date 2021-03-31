using System;
using System.Collections.Generic;

namespace eHS.Portal.Model
{
  public enum RequestType
  {
    New,
    Renewal,
    HC01,
    HC02,
    HC03,
    HC04
  }

  public enum RequestStatus
  {
    Draft = 100,
    Open = 200,
    PendingReviewApproval = 250,
    ForInspection = 300,
    PendingApproval = 400,
    Approved = 500,
    PendingMuftiAck = 550,
    PendingBill = 600,
    PendingPayment = 700,
    Issuance = 800,
    DeliveryInProgress = 850,
    Delivered = 860,
    Closed = 900,
    Rejected = 1000,
    Cancelled = 1100,
    Expired = 1200,
    KIV = 1300
  }

  public enum RequestStatusMinor
  {
    PendingBilling = 110,
    PendingCustomerCode = 120,
    PendingOfflinePayment = 150,
    ReviewInProgress = 210,
    InspectionInProgress = 310,
    InspectionDone = 320,
    ApprovalInProgress = 410,
    BillReady = 610
  }

  public enum EscalateStatus
  {
    Default = 0,
    Open = 100,
    Closed = 200
  }

  public class Request
  {
    public long ID { get; set; }

    public int Step { get; set; }

    public Guid RequestorID { get; set; }

    public Guid? AgentID { get; set; }

    public Guid CustomerID { get; set; }

    public string CustomerName { get; set; }

    public long? CodeID { get; set; }

    public long? GroupCodeID { get; set; }

    public RequestType Type { get; set; }

    public RequestStatus Status { get; set; }

    public RequestStatusMinor? StatusMinor { get; set; }

    public RequestStatus? OldStatus { get; set; }

    public bool Expedite { get; set; }

    public string RefID { get; set; }

    public long? ParentID { get; set; }

    public long? JobID { get; set; }

    public EscalateStatus EscalateStatus { get; set; }

    public Guid? AssignedTo { get; set; }

    public DateTimeOffset SubmittedOn { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? TargetCompletion { get; set; }

    public DateTimeOffset? DueOn { get; set; }

    public DateTimeOffset? LastAction { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool? IsDeleted { get; set; }

    #region Generated properties

    public string TypeText { get; set; }

    public string StatusText { get; set; }

    public string StatusMinorText { get; set; }

    public Code CustomerCode { get; set; }

    public Code GroupCode { get; set; }

    public string AssignedToName { get; set; }

    public Customer Customer { get; set; }

    public Person Requestor { get; set; }

    public Person Agent { get; set; }

    public IList<RequestLineItem> LineItems { get; set; }

    public IList<HalalTeam> Teams { get; set; }

    public IList<Premise> Premises { get; set; }

    public IList<Menu> Menus { get; set; }

    public IList<Ingredient> Ingredients { get; set; }

    public IList<Characteristic> Characteristics { get; set; }

    public IList<Attachment> Attachments { get; set; }

    public IList<RFA> RFAs { get; set; }

    public IList<Log> Logs { get; set; }

    public IList<Review> Reviews { get; set; }

    #endregion
  }
}
