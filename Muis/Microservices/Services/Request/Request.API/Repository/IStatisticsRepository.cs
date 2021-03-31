using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Request.Model;

namespace Request.API.Repository
{
  public interface IStatisticsRepository
  {
    /// <summary>
    /// Retrieve performance statistics.
    /// </summary>
    Task<IList<StatisticsPerformance>> Performance(IList<string> keys,
      DateTimeOffset? from = null,
      DateTimeOffset? to = null);

    /// <summary>
    /// Retrieve overview statistics.
    /// </summary>
    Task<IList<StatisticsOverview>> Overview(DateTimeOffset? from = null,
      DateTimeOffset? to = null);

    /// <summary>
    /// Retrieve status statistics.
    /// </summary>
    Task<IList<StatisticsStatus>> Status(DateTimeOffset? from = null,
      DateTimeOffset? to = null);
  }
}
