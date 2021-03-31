using System;
using JobOrder.Model;

namespace JobOrder.API.Models
{
  public class RescheduleParam
  {
    public Master Reason { get; set; }

    public string Notes { get; set; }

    public DateTimeOffset? ScheduledOn { get; set; }

    public DateTimeOffset? ScheduledOnTo { get; set; }
  }
}
