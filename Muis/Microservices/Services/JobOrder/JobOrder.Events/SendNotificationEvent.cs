using System;
using System.Collections.Generic;
using Core.EventBus;

namespace JobOrder.Events
{
  public class SendNotificationEvent : Event
  {
    public string Title { get; set; }

    public string Preview { get; set; }

    public string Body { get; set; }

    public string Module { get; set; }

    public string RefID { get; set; }

    public int Category { get; set; }

    public int Level { get; set; }

    public int ContentType { get; set; }

    public IList<string> To { get; set; }
  }
}
