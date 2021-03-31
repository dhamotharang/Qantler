using Case.API.Service;
using Case.Events;
using Case.Model;
using Core.EventBus;
using System.Threading.Tasks;

namespace Case.API.EventHandlers
{
  public class OnJobOrderStatusChangedEventHandler : IEventHandler<OnJobOrderStatusChangedEvent>
  {
    readonly ICaseService _caseService;

    public OnJobOrderStatusChangedEventHandler(ICaseService caseService)
    {
      _caseService = caseService;
    }

    public async Task Handle(OnJobOrderStatusChangedEvent @event)
    {
      if (@event.Type != JobOrderType.Enforcement
        && @event.Type != JobOrderType.Reinstate)
      {
        return;
      }

      await _caseService.OnInspectionDone(@event.NewStatus.Value, @event.RefID.Value);
    }
  }
}
