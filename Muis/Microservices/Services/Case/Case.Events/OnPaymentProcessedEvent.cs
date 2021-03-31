using System;
using System.Collections.Generic;
using Core.EventBus;

namespace Case.Events
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

  public enum BillType
  {
    Stage1,
    Stage2,
    CompositionSum
  }

  public class OnPaymentProcessedEvent : Event
  {
    public long ID { get; set; }

    public string AltID { get; set; }

    public string RefNo { get; set; }

    public Guid AccountID { get; set; }

    public PaymentStatus Status { get; set; }

    public IList<Bill> Bills { get; set; }
  }

  public class Bill
  {
    public long ID { get; set; }

    public string RefID { get; set; }

    public BillType Type { get; set; }
  }
}
