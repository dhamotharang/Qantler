using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Repository
{
  public interface IFindingsRepository
  {
    /// <summary>
    /// Insert new  Findings.
    /// </summary>
    /// <returns>The ID of newly inserted Findings.</returns>
    /// <param name="req">Findings model</param>
    public Task<long> InsertFindings(Findings findings);

    public Task<Findings> GetFindingsByID(long id);
  }
}
