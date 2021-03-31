using Request.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Request.Events
{
  public class OnRFAClosedEvent : Core.EventBus.Event
  {
    public long RFAID { get; set; }
    public long RequestID { get; set; }
    public string RefID { get; set; }
 }
  
}
