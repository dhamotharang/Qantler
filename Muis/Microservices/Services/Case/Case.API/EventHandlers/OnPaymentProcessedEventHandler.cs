using Case.API.Service;
using Case.Events;
using Core.EventBus;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.EventHandlers
{
  public class OnPaymentProcessedEventHandler : IEventHandler<OnPaymentProcessedEvent>
  {
    readonly ICaseService _caseService;

    public OnPaymentProcessedEventHandler(ICaseService caseService)
    {
      _caseService = caseService;
    }

    public async Task Handle(OnPaymentProcessedEvent @event)
    {
      if (@event.Bills.Any(x => x.Type == BillType.CompositionSum))
      {
        await _caseService.OnCompositionPaymentProcessed(long.Parse(@event.RefNo),
          @event.Status);
      }
    }
  }
}
