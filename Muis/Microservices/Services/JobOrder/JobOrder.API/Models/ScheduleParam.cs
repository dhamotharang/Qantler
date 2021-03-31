using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Models
{
  public class ScheduleParam
  {
    public string Notes { get; set; }

    public DateTimeOffset? ScheduledOn { get; set; }

    public DateTimeOffset? ScheduledOnTo { get; set; }
  }
}
