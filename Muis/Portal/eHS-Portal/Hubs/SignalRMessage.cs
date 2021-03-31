using System;

namespace eHS.Portal.Hubs
{
  public enum SignalRLevel
  {
    Info,
    Warning,
    Danger
  }

  public class SignalRMessage
  {
    public string Topic { get; set; }
    public string Module { get; set; }
    public string RefID { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public SignalRLevel Level { get; set; } = SignalRLevel.Info;
  }
}
