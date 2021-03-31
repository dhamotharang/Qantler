using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using Request.Model;

namespace Request.API.Services.Commands.Statistics
{
  public class StatusCommand : IUnitOfWorkCommand<IList<StatisticsStatus>>
  {
    readonly DateTimeOffset? _from;
    readonly DateTimeOffset? _to;

    public StatusCommand(DateTimeOffset? from, DateTimeOffset? to)
    {
      _from = from;
      _to = to;
    }

    public Task<IList<StatisticsStatus>> Invoke(IUnitOfWork uow)
    {
      return new DbContext(uow).Statistics.Status(_from, _to);
    }
  }
}
