using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Models
{
  public class ScheduleJobOrderParam : JobOrder.Model.JobOrder
  {
    public string Email { get; set; }
  }
}
