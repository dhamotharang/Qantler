using Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobOrder.Jobs.Services
{
  public interface IPeriodicInspectionService
  {
    /// <summary>
    /// Create periodic inspection job order
    /// </summary>
    public Task<Unit> PeriodicScheduler();
  }
}
