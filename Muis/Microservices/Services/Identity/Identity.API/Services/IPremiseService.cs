using Identity.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.Services
{
  public interface IPremiseService
  {
    /// <summary>
    /// Get premise list
    /// </summary>
    /// <returns></returns>
    Task<IList<Premise>> GetPremises();
  }
}
