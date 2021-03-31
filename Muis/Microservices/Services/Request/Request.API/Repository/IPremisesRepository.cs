using Request.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Repository
{
  public interface IPremisesRepository
  {
    /// <summary>
    /// Retrieve Premises.
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public Task<IList<Premise>> Select(Guid? customerID = null);

    /// <summary>
    /// Retrieve Premise with specified ID.
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public Task<Premise> GetByID(long id);

    /// <summary>
    /// Get recent application for specified customer
    /// </summary>
    /// <param name="premise">Premise</param>
    /// <returns></returns>
    public Task<long> CreatePremise(Premise premise);
  }
}
