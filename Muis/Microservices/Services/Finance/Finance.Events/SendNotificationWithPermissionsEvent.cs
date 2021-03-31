using System.Collections.Generic;
using Core.EventBus;
using Core.Model;

namespace Finance.Events
{
  public class SendNotificationWithPermissionsEvent : Event
  {
    public string Title { get; set; }

    public string Preview { get; set; }

    public string Body { get; set; }

    public string Module { get; set; }

    public string RefID { get; set; }

    public IList<int> RequestTypes { get; set; }

    public IList<Permission> Permissions { get; set; }

    public IList<string> Excludes { get; set; }
  }
}
