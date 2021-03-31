using System;
using System.Collections.Generic;
using Core.EventBus;
using Finance.Model;

namespace Finance.Events
{
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
