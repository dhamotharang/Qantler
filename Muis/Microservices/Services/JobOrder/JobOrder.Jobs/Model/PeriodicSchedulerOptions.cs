using System;
using System.Collections.Generic;
using System.Text;

namespace JobOrder.Jobs.Model
{
  public class PeriodicSchedulerOptions
  {
    public long? ID { get; set; }

    public long? LastJobID { get; set; }

    public int? Status { get; set; }

    public long? PremiseID { get; set; }
  }
}
