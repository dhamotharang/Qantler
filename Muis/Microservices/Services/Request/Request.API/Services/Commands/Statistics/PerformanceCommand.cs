using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using Request.Model;

namespace Request.API.Services.Commands.Statistics
{
  public class PerformanceCommand : IUnitOfWorkCommand<IList<StatisticsPerformance>>
  {
    readonly IList<string> _keys;
    readonly DateTimeOffset? _from;
    readonly DateTimeOffset? _to;

    public PerformanceCommand(IList<string> keys, DateTimeOffset? from, DateTimeOffset? to)
    {
      _keys = keys;
      _from = from;
      _to = to;
    }

    public Task<IList<StatisticsPerformance>> Invoke(IUnitOfWork uow)
    {
      return new DbContext(uow).Statistics.Performance(_keys, _from, _to);
    }
  }
}
