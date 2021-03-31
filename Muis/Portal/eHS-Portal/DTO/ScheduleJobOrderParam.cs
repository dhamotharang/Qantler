using eHS.Portal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eHS.Portal.DTO
{
  public class ScheduleJobOrderParam : JobOrder
  {
    public string Email { get; set; }
  }
}
