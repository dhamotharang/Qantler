using System;
using System.Threading.Tasks;
using Core.EventBus;
using Request.API.Services;
using Request.Events;

namespace Request.API.EventHandlers
{
  public class OnBillPaidEventHandler : IEventHandler<OnBillPaidEvent>
  {
    readonly IRequestService _requestService;

    public OnBillPaidEventHandler(IRequestService requestService)
    {
      _requestService = requestService;
    }

    public async Task Handle(OnBillPaidEvent @event)
    {
      if (@event.RequestID.HasValue)
      {
        await _requestService.OnBillPaid(@event.RequestID.Value, @event.ID);
      }
    }
  }
}
