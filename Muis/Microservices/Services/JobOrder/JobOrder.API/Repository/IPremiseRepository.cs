using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Model;
using JobOrder.Model;

namespace JobOrder.API.Repository
{
  public interface IPremiseRepository
  {
    /// <summary>
    /// Insert or replace premise.
    /// </summary>
    Task InsertOrReplace(Premise premise);

    /// <summary>
    /// Retrieve premise instance based on specified id.
    /// </summary>
    Task<Premise> GetPremiseByID(long id);
  }
}
