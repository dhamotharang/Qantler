using System;
using Core.EventBus;

namespace JobOrder.Events
{
  public class OnIdentityChangedEvent : Event
  {
    public Guid ID { get; set; }

    public string Name { get; set; }
  }
}
