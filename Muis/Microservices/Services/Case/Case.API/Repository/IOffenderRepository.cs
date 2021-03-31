using Case.API.Params;
using Case.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Case.API.Repository
{
  public interface IOffenderRepository
  {
    /// <summary>
    /// insert offender
    /// </summary>
    public Task InsertOffender(Offender offender);

    /// <summary>
    /// get offender by ID
    /// </summary>
    public Task<Offender> GetOffenderByID(Guid id);

  }
}
