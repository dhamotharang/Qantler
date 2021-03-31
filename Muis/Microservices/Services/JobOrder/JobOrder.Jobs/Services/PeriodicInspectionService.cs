using Core.Base;
using Core.Model;
using Core.Base.Repository;
using JobOrder.Jobs.Services.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.EventBus;

namespace JobOrder.Jobs.Services
{
  public class PeriodicInspectionService : TransactionalService, IPeriodicInspectionService
  {
    readonly IEventBus _eventBus;
    public PeriodicInspectionService(IDbConnectionProvider connectionProvider, IEventBus eventBus)
      : base(connectionProvider)
    {
      _eventBus = eventBus;
    }

    public async Task<Unit> PeriodicScheduler()
    {
      return await Execute(new CreateJobOrderCommand(_eventBus));
    }
  }
}
