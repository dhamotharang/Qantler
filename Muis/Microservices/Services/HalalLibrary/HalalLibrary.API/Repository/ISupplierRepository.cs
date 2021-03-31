using HalalLibrary.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HalalLibrary.API.Repository
{
  public interface ISupplierRepository
  {
    public Task<IEnumerable<Supplier>> Select();

    /// <summary>
    /// get supplier by name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Task<Supplier> GetSupplierByName(string name);

    public Task<long> InsertSupplier(Supplier data);
  }
}
