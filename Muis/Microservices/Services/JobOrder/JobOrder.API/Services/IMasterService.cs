using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobOrder.API.Services
{
  public interface IMasterService
  {
    /// <summary>
    ///  List of master data.
    /// </summary>
    /// <returns>the list of master data</returns>
    Task<IEnumerable<Master>> GetMaster(MasterType type);

    /// <summary>
    /// Insert master data.
    /// </summary>
    /// <param name="data">Master data</param>
    /// <returns>the result boolean</returns>
    Task InsertMaster(Master data);

    /// <summary>
    /// updated list of master data.
    /// </summary>
    /// <param name="data">Master data</param>
    /// <returns>the result boolean</returns>
    Task UpdateMaster(Master data);

    /// <summary>
    /// delete master data.
    /// </summary>
    /// <param name="id">Master ID</param>
    /// <returns>the result boolean</returns>
    Task DeleteMaster(Guid id);
  }
}
