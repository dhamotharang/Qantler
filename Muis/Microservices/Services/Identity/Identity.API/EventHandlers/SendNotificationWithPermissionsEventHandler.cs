using System;
using System.Linq;
using System.Threading.Tasks;
using Core.EventBus;
using Identity.API.Services;
using Identity.Events;
using Notification.Events;

namespace Identity.API.EventHandlers
{
  public class SendNotificationWithPermissionsEventHandler
             : IEventHandler<SendNotificationWithPermissionsEvent>
  {
    readonly IEventBus _eventBus;
    readonly IIdentityService _identityService;
    
    public SendNotificationWithPermissionsEventHandler(IIdentityService identityService,
      IEventBus eventBus)
    {
      _identityService = identityService;
      _eventBus = eventBus;
    }

    public async Task Handle(SendNotificationWithPermissionsEvent @event)
    {
      var identities = await _identityService.List(new IdentityFilter
      {
        RequestTypes = @event.RequestTypes?.ToArray(),
        Permissions = @event.Permissions?.ToArray()
      });

      if ((identities?.Count() ?? 0) > 0)
      {
        if ((@event.Excludes?.Count() ?? 0) > 0)
        {
          identities = identities.Where(e => !@event.Excludes.Contains(e.ID.ToString())).ToList();
        }

        _eventBus.Publish(new SendNotificationEvent
        {
          Title = @event.Title,
          Preview = @event.Preview,
          Body = @event.Body,
          Module = @event.Module,
          RefID = @event.RefID,
          Category = @event.Category,
          Level = @event.Level,
          ContentType = @event.ContentType,
          To = identities.Where(e => e.Status == Model.IdentityStatus.Active)
            .Select(e => e.ID.ToString())
            .ToList()
        });
      }
    }
  }
}
