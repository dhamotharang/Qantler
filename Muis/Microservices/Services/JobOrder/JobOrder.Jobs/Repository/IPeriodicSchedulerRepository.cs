using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JobOrder.Jobs.Model;
using JobOrder.Model;

namespace JobOrder.Jobs.Repository
{
  public interface IPeriodicSchedulerRepository
  {
    /// <summary>
    /// Get eligible schedulers for periodic inspection
    /// </summary>
    public Task<IEnumerable<PeriodicScheduler>> GetSchedulers();

    /// <summary>
    /// Get periodic scheduler instances
    /// </summary>
    public Task<IList<PeriodicScheduler>> Select(PeriodicSchedulerOptions scheduler);

    /// <summary>
    /// Update periodic scheduler lastjobid by premise
    /// </summary>
    public Task InsertOrReplace(PeriodicScheduler scheduler);
  }
}
