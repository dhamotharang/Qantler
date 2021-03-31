using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Request.API.Services
{
  public interface IComplianceService
  {
    /// Get Compliance History for a Scheme.
    /// </summary>
    /// <returns>The compliance history data.</returns>
    /// <param name="scheme">Scheme Type</param>
    Task<IEnumerable<Model.ComplianceHistory>> GetComplianceByScheme(Scheme scheme);

    /// <summary>
    /// Get Compliance for an ID.
    /// </summary>
    /// <returns>The compliance history data.</returns>
    /// <param name="Id">Compliance identifier.</param>
    Task<Model.ComplianceHistory> GetComplianceByID(long id);
  }
}
