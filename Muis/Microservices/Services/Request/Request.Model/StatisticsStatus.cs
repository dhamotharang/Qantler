using System;

namespace Request.Model
{
  public class StatisticsStatus
  {
    public DateTimeOffset MonthYear { get; set; }

    public int New { get; set; }

    public int Closed { get; set; }

    public int Rejected { get; set; }
  }
}
