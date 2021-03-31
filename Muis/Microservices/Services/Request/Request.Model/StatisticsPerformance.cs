using System;

namespace Request.Model
{
  public class StatisticsPerformance
  {
    public DateTimeOffset MonthYear { get; set; }

    public int Assigned { get; set; }

    public int Processed { get; set; }

    public int New { get; set; }

    public int Closed { get; set; }
  }
}
