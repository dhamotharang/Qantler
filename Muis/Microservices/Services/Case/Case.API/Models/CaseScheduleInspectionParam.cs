using Case.Events;
using System;

namespace Case.API.Params
{
  public class CaseScheduleInspectionParam
  {
    public long JobOrderID { get; set; }

    public DateTimeOffset? ScheduledOn { get; set; }

    public DateTimeOffset? ScheduledOnTo { get; set; }

    public long PremiseID { get; set; }

    public string PremisesText { get; set; }

    public JobOrderType? Type { get; set; }

    public string Notes { get; set; }
  }
}
