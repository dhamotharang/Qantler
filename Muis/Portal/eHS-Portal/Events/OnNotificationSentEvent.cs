using System;
using System.Collections.Generic;
using eHS.Portal.Model;

namespace eHS.Portal.Events
{
  public class OnNotificationSentEvent : Core.EventBus.Event
  {
    public long ID { get; set; }

    public string Title { get; set; }

    public string Preview { get; set; }

    public string Module { get; set; }

    public string RefID { get; set; }

    public Category Category { get; set; }

    public Level Level { get; set; }

    public ContentType ContentType { get; set; }

    public IList<string> To { get; set; }
  }
}
