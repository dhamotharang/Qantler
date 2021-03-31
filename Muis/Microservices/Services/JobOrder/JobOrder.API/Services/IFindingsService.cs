using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Services
{
  public interface IFindingsService
  {
    /// <summary>
    /// Submit new findings
    /// </summary>
    /// <param name="model">the job order instance to create</param>
    /// <returns>the newly created findings</returns>
    Task<Findings> Submit(Findings findings);
  }
}
