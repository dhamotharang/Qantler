using JobOrder.Jobs.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobOrder.Jobs.Actions
{
  public class SchedulePeriodicInspectionAction : IAction
  {
    readonly IPeriodicInspectionService _service;
    public SchedulePeriodicInspectionAction(IPeriodicInspectionService service)
    {
      _service = service;
    }
    public async Task Invoke()
    {
      await _service.PeriodicScheduler();
    }
  }

}
