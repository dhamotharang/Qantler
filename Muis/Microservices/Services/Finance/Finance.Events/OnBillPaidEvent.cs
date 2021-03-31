using System;
using Core.EventBus;
using Finance.Model;

namespace Finance.Events
{
  public class OnBillPaidEvent : Event
  {
    public long ID { get; set; }

    public string RefNo { get; set; }

    public BillType Type { get; set; }

    public BillStatus Status { get; set; }

    public long? RequestID { get; set; }

    public string RefID { get; set; }
  }
}
