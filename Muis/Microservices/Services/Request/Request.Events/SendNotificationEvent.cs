using System;
using System.Collections.Generic;
using Core.EventBus;

namespace Request.Events
{
  public enum Category
  {
    Push,
    Email
  }

  public enum Level
  {
    Info,
    Warning,
    Danger
  }

  public enum ContentType
  {
    Text,
    Html
  }

  public class SendNotificationEvent : Event
  {
    public string Title { get; set; }

    public string Preview { get; set; }

    public string Body { get; set; }

    public string Module { get; set; }

    public string RefID { get; set; }

    public Category Category { get; set; }

    public Level Level { get; set; }

    public ContentType ContentType { get; set; }

    public IList<string> To { get; set; }
  }
}
