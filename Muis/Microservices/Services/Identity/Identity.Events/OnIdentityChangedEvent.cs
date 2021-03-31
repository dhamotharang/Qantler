using System;
using Core.EventBus;

namespace Identity.Events
{
  public class OnIdentityChangedEvent : Event
  {
    public Guid ID { get; set; }

    public string Name { get; set; }
  }
}
