using Core.API;
using Core.API.Repository;
using JobOrder.API.Models;
using JobOrder.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobOrder.API.Services.Commands.JobOrders
{
  public class QueryJobOrderCommand : IUnitOfWorkCommand<IEnumerable<Model.JobOrder>>
  {
    readonly JobOrderOptions _options;

    public QueryJobOrderCommand(JobOrderOptions options)
    {
      _options = options;
    }

    public Task<IEnumerable<Model.JobOrder>> Invoke(IUnitOfWork uow)
    {
      return DbContext.From(uow).JobOrder.Select(_options);
    }
  }
}
