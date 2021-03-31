using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Request.Model;

namespace Request.API.Services
{
  public interface IStatisticsService
  {
    /// <summary>
    /// Retrieve performance statistics.
    ///
    /// Keys:
    /// - New
    /// - Closed
    /// - [UserID]
    /// </summary>
    /// <returns></returns>
    Task<IList<StatisticsPerformance>> Performance(IList<string> keys,
      DateTimeOffset? from = null,
      DateTimeOffset? to = null);

    /// <summary>
    /// Retrieve overview statistics.
    /// </summary>
    /// <returns></returns>
    Task<IList<StatisticsOverview>> Overview(DateTimeOffset? from = null,
      DateTimeOffset? to = null);

    /// <summary>
    /// Retrieve status statistics.
    /// </summary>
    /// <returns></returns>
    Task<IList<StatisticsStatus>> Status(DateTimeOffset? from = null,
      DateTimeOffset? to = null);
  }
}
