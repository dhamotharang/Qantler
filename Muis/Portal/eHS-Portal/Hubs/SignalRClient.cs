using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace eHS.Portal.Hubs
{
  public class SignalRClient : ISignalRClient
  {
    readonly IHubContext<SignalRHub> _context;

    public SignalRClient(IHubContext<SignalRHub> context)
    {
      _context = context;
    }

    public async Task SendMessage(SignalRMessage msg)
    {
      await _context.Clients.All.SendAsync("message", msg);
    }

    public async Task SendMessageToGroup(SignalRMessage msg, params string[] groups)
    {
      await _context.Clients.Groups(groups).SendAsync("message", msg);
    }

    public async Task SendMessageToUser(SignalRMessage msg, params string[] users)
    {
      await _context.Clients.Users(users).SendAsync("message", msg);
    }
  }
}
