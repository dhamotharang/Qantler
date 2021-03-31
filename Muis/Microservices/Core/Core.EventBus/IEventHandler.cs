using System;
using System.Threading.Tasks;

namespace Core.EventBus
{
  public interface IEventHandler<in TEvent> : IEventHandler
    where TEvent : Event
  {
    Task Handle(TEvent @event);
  }

  public interface IEventHandler
  {
  }
}
