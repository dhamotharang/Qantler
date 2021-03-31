using eHS.Portal.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Services
{
  public interface IPremiseService
  {
    /// <summary>
    /// Retrieve list of premise
    /// </summary>
    /// <returns></returns>
    Task<IList<Premise>> GetPremise(Guid? customerID = null);

    /// <summary>
    /// Create Premises
    /// </summary>
    /// <returns></returns>
    Task<Premise> CreatePremise(Premise premise);
  }
}