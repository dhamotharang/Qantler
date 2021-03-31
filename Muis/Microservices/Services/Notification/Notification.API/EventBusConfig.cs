using System;
namespace Notification.API
{
  public class EventBusConfig
  {
    public string Provider { get; set; }
    public string Channel { get; set; }
    public string Host { get; set; }
    public string ClientName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public int RetryCount { get; set; } = 5;
  }
}
