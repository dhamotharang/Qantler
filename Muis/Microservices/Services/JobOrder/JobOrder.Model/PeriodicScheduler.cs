using Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobOrder.Model
{
  public enum PreiodicSchedulerStatus
  {
    Pending=100,
    Scheduled=200,
    Suspended=300
  }
  public class PeriodicScheduler
  {
    public long ID { get; set; }

    public long? LastJobID { get; set; }

    public DateTimeOffset? LastScheduledOn { get; set; }

    public DateTimeOffset? NextTargetInspection { get; set; }

    public PreiodicSchedulerStatus Status { get; set; }

    public long PremiseID { get; set; }
  }
}
