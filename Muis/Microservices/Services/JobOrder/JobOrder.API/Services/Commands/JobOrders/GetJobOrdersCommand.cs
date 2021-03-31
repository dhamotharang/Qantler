using Core.API;
using Core.API.Repository;
using JobOrder.API.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobOrder.API.Services.Commands.JobOrders
{
  public class GetJobOrdersCommand : IUnitOfWorkCommand<IEnumerable<Model.JobOrder>>
  {
    readonly Guid _assignedTo;
    readonly DateTimeOffset? _lastupdatedOn;

    public GetJobOrdersCommand(Guid assignedTo, DateTimeOffset? lastupdatedOn)
    {
      _assignedTo = assignedTo;
      _lastupdatedOn = lastupdatedOn;
    }

    public Task<IEnumerable<Model.JobOrder>> Invoke(IUnitOfWork uow)
    {
      return DbContext.From(uow).JobOrder.GetJobOrders(_assignedTo, _lastupdatedOn);
    }
  }
}