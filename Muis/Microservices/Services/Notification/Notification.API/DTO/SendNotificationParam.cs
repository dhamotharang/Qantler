using System;
using System.Collections.Generic;

namespace Notification.API.DTO
{
  public class SendNotificationParam
  {
    public Model.Notification Notification { get; set; }

    public IList<Guid> To { get; set; }
  }
}
