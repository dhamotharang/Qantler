using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using Request.Model;

namespace Request.API.Services.Commands.Statistics
{
  public class OverviewCommand : IUnitOfWorkCommand<IList<StatisticsOverview>>
  {
    readonly DateTimeOffset? _from;
    readonly DateTimeOffset? _to;

    public OverviewCommand(DateTimeOffset? from, DateTimeOffset? to)
    {
      _from = from;
      _to = to;
    }

    public Task<IList<StatisticsOverview>> Invoke(IUnitOfWork uow)
    {
      return new DbContext(uow).Statistics.Overview(_from, _to);
    }
  }
}
