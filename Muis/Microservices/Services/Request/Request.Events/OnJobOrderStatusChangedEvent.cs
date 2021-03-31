using Core.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Request.Events
{
  public enum JobOrderStatus
  {
    Draft = 100,
    Pending = 200,
    WIP = 300,
    Done = 400,
    Cancelled = 500,
    Expired = 600,
    Closed = 700
  }

  public enum JobOrderType
  {
    Audit,
    Periodic,
    Enforcement,
    Reinstate
  }

  public class OnJobOrderStatusChangedEvent : Event
  {
    public long ID { get; set; }

    public long? RefID { get; set; }

    public JobOrderType? Type { get; set; }

    public JobOrderStatus? NewStatus { get; set; }

    public JobOrderStatus? OldStatus { get; set; }
  }
}
