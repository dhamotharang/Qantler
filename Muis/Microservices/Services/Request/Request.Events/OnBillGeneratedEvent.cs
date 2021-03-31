using System;
using Core.EventBus;
using Request.Model;

namespace Request.Events
{
  public class OnBillGeneratedEvent : Event
  {
    public long ID { get; set; }

    public string RefNo { get; set; }

    public long RequestID { get; set; }

    public string RefID { get; set; }

    public BillType Type { get; set; }
  }
}
