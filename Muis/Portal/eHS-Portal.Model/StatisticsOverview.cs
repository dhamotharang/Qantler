using System;

namespace eHS.Portal.Model
{
  public class StatisticsOverview
  {
    public DateTimeOffset MonthYear { get; set; }

    public int New { get; set; }

    public int Renewal { get; set; }

    public int Amend { get; set; }
  }
}
