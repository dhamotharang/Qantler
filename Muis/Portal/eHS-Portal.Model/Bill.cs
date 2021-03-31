using System;
using System.Collections.Generic;

namespace eHS.Portal.Model
{
  public enum BillStatus
  {
    Draft = 100,
    Pending = 200,
    Paid = 300,
    Overdue = 400,
    Canceled = 500
  }

  public enum BillType
  {
    Stage1,
    Stage2
  }

  public enum BillRequestType
  {
    New = 0,
    Renewal = 1,
    HC03 = 4
  }

  public class Bill
  {
    public long ID { get; set; }

    public string RefNo { get; set; }

    public BillStatus Status { get; set; }

    public BillType Type { get; set; }

    public BillRequestType RequestType { get; set; }

    public string InvoiceNo { get; set; }

    public decimal Amount { get; set; }

    public decimal GSTAmount { get; set; }

    public decimal GST { get; set; }

    public long? RequestID { get; set; }

    public string RefID { get; set; }

    public Guid CustomerID { get; set; }

    public string CustomerName { get; set; }

    public DateTimeOffset? IssuedOn { get; set; }

    public DateTimeOffset? DueOn { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public string StatusText { get; set; }

    public string TypeText { get; set; }

    public string RequestTypeText { get; set; }

    public decimal TotalAmount => Amount + GSTAmount;

    public IList<BillLineItem> LineItems { get; set; }

    public IList<Payment> Payments { get; set; }

    #endregion
  }
}
