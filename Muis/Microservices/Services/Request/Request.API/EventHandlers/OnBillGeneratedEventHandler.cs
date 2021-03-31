using System;
using System.Threading.Tasks;
using Core.EventBus;
using Request.API.Services;
using Request.Events;

namespace Request.API.EventHandlers
{
  public class OnBillGeneratedEventHandler : IEventHandler<OnBillGeneratedEvent>
  {
    readonly IRequestService _requestService;

    public OnBillGeneratedEventHandler(IRequestService requestService)
    {
      _requestService = requestService;
    }

    public async Task Handle(OnBillGeneratedEvent @event)
    {
      await _requestService.OnBillGeneratedForRequest(@event.RequestID, @event.ID);
    }
  }
}
