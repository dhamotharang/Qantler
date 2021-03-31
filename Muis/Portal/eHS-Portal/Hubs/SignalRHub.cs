using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace eHS.Portal.Hubs
{
  public class SignalRHub : Hub
  {
    readonly ILogger<SignalRHub> _logger;

    public SignalRHub(ILogger<SignalRHub> logger)
    {
      _logger = logger;
    }

    public override Task OnConnectedAsync()
    {
      _logger.LogInformation($"SignalR.Connected: {Context.UserIdentifier}");
      return base.OnConnectedAsync();
    }
  }
}
