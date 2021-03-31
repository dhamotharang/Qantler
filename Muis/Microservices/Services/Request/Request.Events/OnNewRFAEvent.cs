using System;
using System.Collections.Generic;
using System.Text;

namespace Request.Events
{
  public class OnNewRFAEvent : Core.EventBus.Event
  {
    public long ID { get; set; }
    public long RequestID { get; set; }
    public string RefID { get; set; }
  }
}
