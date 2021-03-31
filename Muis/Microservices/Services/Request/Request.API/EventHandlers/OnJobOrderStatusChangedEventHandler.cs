using System;
using System.Threading.Tasks;
using Core.EventBus;
using Request.API.Services;
using Request.Events;

namespace Request.API.EventHandlers
{
  public class OnJobOrderStatusChangedEventHandler : IEventHandler<OnJobOrderStatusChangedEvent>
  {
    readonly IRequestService _requestService;

    public OnJobOrderStatusChangedEventHandler(IRequestService requestService)
    {
      _requestService = requestService;
    }

    public async Task Handle(OnJobOrderStatusChangedEvent @event)
    {
      if (@event.Type == JobOrderType.Audit)
      {
        var request = await _requestService.GetRequestByIDBasic(@event.RefID.Value);
        if (request == null)
        {
          return;
        }

        var oldStatusMinor = request.StatusMinor;
        var newStatusMinor = oldStatusMinor;

        if (@event.NewStatus == JobOrderStatus.Done)
        {
          newStatusMinor = Model.RequestStatusMinor.InspectionDone;
        }
        else if (@event.NewStatus == JobOrderStatus.Cancelled)
        {
          newStatusMinor = Model.RequestStatusMinor.InspectionCancelled;
        }

        if (oldStatusMinor != newStatusMinor)
        {
          await _requestService.UpdateRequestStatus(@event.RefID.Value,
            Model.RequestStatus.ForInspection,
            newStatusMinor, null);
        }
      }
    }
  }
}
