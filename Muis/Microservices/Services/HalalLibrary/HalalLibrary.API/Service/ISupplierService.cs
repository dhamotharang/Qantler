using HalalLibrary.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HalalLibrary.API.Services
{
  public interface ISupplierService
  {
    /// <summary>
    /// Gets all requets for a particular user.
    /// </summary>
    /// <returns>The list of supplier data.</returns>
    Task<IEnumerable<Supplier>> Select();

    /// <summary>
    /// Insert Supplier data.
    /// </summary>
    /// <param name="data">Supplier data</param>
    Task<long> InsertSupplier(Supplier data);
  }
}
