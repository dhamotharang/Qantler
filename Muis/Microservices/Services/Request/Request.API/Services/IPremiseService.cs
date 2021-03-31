using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Services
{
  public interface IPremiseService
  {
    /// <summary>
    /// Get Premises
    /// </summary>
    public Task<IList<Model.Premise>> GetPremises(Guid? customerID);

    /// <summary>
    /// Create Premises
    /// </summary>
    public Task<Model.Premise> CreatePremise(Model.Premise data);
  }
}
