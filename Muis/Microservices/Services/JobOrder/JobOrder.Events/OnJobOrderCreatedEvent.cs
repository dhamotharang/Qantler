using System;
using Core.EventBus;
using JobOrder.Model;

namespace JobOrder.Events
{
  public class OnJobOrderCreatedEvent : Event
  {
    public long ID { get; set; }

    public long? RefID { get; set; }

    public JobOrderType? Type { get; set; }

    public DateTimeOffset? ScheduledOn { get; set; }
  }
}
