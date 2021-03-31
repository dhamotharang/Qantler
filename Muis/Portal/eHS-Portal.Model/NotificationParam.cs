using System;
using System.Collections.Generic;
using System.Text;

namespace eHS.Portal.Model
{
  public class NotificationParam
  {
    public IList<Guid> UserIDs { get; set; }

    public Notification Data { get; set; }
  }
}
