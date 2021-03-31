using System;
using Core.EventBus;
using JobOrder.Model;

namespace JobOrder.Events
{
  public class OnJobOrderRescheduledEvent : Event
  {
    public long ID { get; set; }

    public long? RefID { get; set; }

    public JobOrderType Type { get; set; }

    public DateTimeOffset OldScheduledOn { get; set; }

    public DateTimeOffset NewScheduledOn { get; set; }
  }
}
