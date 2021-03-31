using System;
using System.Threading.Tasks;
using Core.EventBus;
using Core.Model;
using Request.API.Services;
using Request.Events;

namespace Request.API.EventHandlers
{
  public class OnIdentityChangedEventHandler : IEventHandler<OnIdentityChangedEvent>
  {
    readonly IUserService _userService;

    public OnIdentityChangedEventHandler(IUserService userService)
    {
      _userService = userService;
    }

    public async Task Handle(OnIdentityChangedEvent @event)
    {
      await _userService.InsertOrReplaceOfficer(new Officer
      {
        ID = @event.ID,
        Name = @event.Name
      });
    }
  }
}
