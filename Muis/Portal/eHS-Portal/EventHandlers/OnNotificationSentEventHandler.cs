using System.Linq;
using System.Threading.Tasks;
using Core.EventBus;
using eHS.Portal.Events;
using eHS.Portal.Hubs;
using Microsoft.Extensions.Logging;

namespace eHS.Portal.EventHandlers
{
  public class OnNotificationSentEventHandler : IEventHandler<OnNotificationSentEvent>
  {
    readonly ISignalRClient _signalRClient;

    readonly ILogger<OnNotificationSentEventHandler> _logger;

    public OnNotificationSentEventHandler(
      ISignalRClient signalRClient,
      ILogger<OnNotificationSentEventHandler> logger)
    {
      _logger = logger;
      _signalRClient = signalRClient;
    }

    public async Task Handle(OnNotificationSentEvent @event)
    {
      _logger.LogInformation($"Portal:OnNotificationSentEventHandler: {@event.Id}");

      await _signalRClient.SendMessageToUser(new SignalRMessage
      {
        Topic = "notification",
        Module = @event.Module,
        RefID = @event.RefID,
        Title = @event.Title,
        Level = (SignalRLevel)@event.Level,
        Message = @event.Preview
      }, @event.To.ToArray());
    }
  }
}
