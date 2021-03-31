using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Repository
{
  public interface IPeriodicSchedulerRepository
  {
    /// <summary>
    /// Insert or replace periodic scheduler.
    /// </summary>
    Task InsertOrReplace(PeriodicScheduler scheduler);

    /// <summary>
    /// Retrieve periodic scheduler instance based on premise id.
    /// </summary>
    Task<PeriodicScheduler> GetPeriodicSchedulerByPremise(long id);
  }
}
