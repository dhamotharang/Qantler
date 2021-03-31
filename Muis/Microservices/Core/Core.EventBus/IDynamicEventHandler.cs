using System;
using System.Threading.Tasks;

namespace Core.EventBus
{
  public interface IDynamicEventHandler
  {
    Task Handle(dynamic eventData);
  }
}
