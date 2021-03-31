using Core.Model;
using System;
using System.Collections.Generic;

namespace Finance.Model
{
  public enum PaymentStatus
  {
    Draft = 100,
    Pending = 200,
    Processed = 300,
    Rejected = 400,
    Failed = 500,
    Expired = 600,
    Canceled = 700
  }

  public enum PaymentMode
  {
    Online,
    Offline
  }

  public enum PaymentMethod
  {
    Cash,
    PayPal,
    GIRO,
    BankTransfer,
    DDA
  }

  public class Payment
  {
    public long ID { get; set; }

    public string AltID { get; set; }

    public string RefNo { get; set; }

    public PaymentStatus Status { get; set; }

    public PaymentMode Mode { get; set; }

    public PaymentMethod Method { get; set; }

    public string TransactionNo { get; set; }

    public string ReceiptNo { get; set; }

    public Guid AccountID { get; set; }

    public string Name { get; set; }

    public decimal Amount { get; set; }

    public decimal GSTAmount { get; set; }

    public decimal GST { get; set; }

    public DateTimeOffset? PaidOn { get; set; }

    public Guid? ProcessedBy { get; set; }

    public Guid? ContactPersonID { get; set; }

    public DateTimeOffset? ProcessedOn { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public string StatusText { get; set; }

    public string ModeText { get; set; }

    public string MethodText { get; set; }

    public string ProcessedByName { get; set; }

    public Person ContactPerson { get; set; }

    public Bank Bank { get; set; }

    public IList<Bill> Bills { get; set; }

    public IList<Note> Notes { get; set; }

    public IList<Log> Logs { get; set; }

    #endregion
  }
}
