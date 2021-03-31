using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobOrder.Jobs.Repository
{
  public interface IJobOrderRepository
  {
    /// <summary>
    /// Insert job order entity.
    /// </summary>
    /// <param name="model">the entity to insert</param>
    /// <param name="userID">user ID</param>
    /// <param name="userName">user name</param>
    /// <returns>the ID of the newly inserted entity</returns>
    Task<long> InsertJobOrder(JobOrder.Model.JobOrder model);
  }
}
