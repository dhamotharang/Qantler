using System;
using eHS.Portal.Model;

namespace eHS.Portal.DTO
{
  public class RescheduleParam
  {
    public Master Reason { get; set; }

    public string Notes { get; set; }

    public DateTimeOffset? ScheduledOn { get; set; }

    public DateTimeOffset? ScheduledOnTo { get; set; }
  }
}
