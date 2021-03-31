using System;
using System.Linq;
using System.Threading.Tasks;
using Core.EventBus;
using Notification.API.Services;
using Notification.Events;

namespace Notification.API.EventHandlers
{
  public class SendNotificationEventHandler : IEventHandler<SendNotificationEvent>
  {
    readonly INotificationService _service;

    public SendNotificationEventHandler(INotificationService service)
    {
      _service = service;
    }

    public async Task Handle(SendNotificationEvent @event)
    {
      await _service.Send(new Model.Notification
      {
        Title = @event.Title,
        Body = @event.Body,
        Preview = @event.Preview,
        Module = @event.Module,
        RefID = @event.RefID,
        Level = @event.Level,
        Category = @event.Category,
        ContentType = @event.ContentType
      }, @event.To.Select(e => new Guid(e)).ToList());
    }
  }
}
