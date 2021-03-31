using Core.EventBus;
using JobOrder.Model;

namespace JobOrder.Events
{
  public class OnJobOrderStatusChangedEvent : Event
  {
    public long ID { get; set; }

    public long? RefID { get; set; }

    public JobOrderType? Type { get; set; }

    public JobOrderStatus? NewStatus { get; set; }

    public JobOrderStatus? OldStatus { get; set; }
  }
}
