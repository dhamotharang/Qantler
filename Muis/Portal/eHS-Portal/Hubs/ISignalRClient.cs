using System;
using System.Threading.Tasks;

namespace eHS.Portal.Hubs
{
  public interface ISignalRClient
  {
    Task SendMessage(SignalRMessage msg);

    Task SendMessageToUser(SignalRMessage msg, params string[] users);

    Task SendMessageToGroup(SignalRMessage msg, params string[] groups);
  }
}
