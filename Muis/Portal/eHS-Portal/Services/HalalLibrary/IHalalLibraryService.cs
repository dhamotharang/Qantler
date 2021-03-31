using eHS.Portal.Client;
using eHS.Portal.Model;
using eHS.Portal.Models.HalalLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Services.HalalLibrary
{
  public interface IHalalLibraryService
  {
    /// <summary>
    /// Get list of halal library
    /// </summary>
    Task<IndexModel> Search(HalalLibraryOptions options, int offsetRows, int nextRows);

    /// <summary>
    /// Create halal library
    /// </summary>
    Task<long> InsertHalalLibrary(Model.HLIngredient data);

    /// <summary>
    /// Update halal library
    /// </summary>
    Task<long> UpdateHalalLibrary(Model.HLIngredient data);

    /// <summary>
    /// Delete halal library
    /// </summary>
    Task<bool> DeleteHalalLibrary(long id);

    /// <summary>
    /// Get list of supplier
    /// </summary>
    Task<IEnumerable<Supplier>> GetSupplier();

    /// <summary>
    /// Create Supplier
    /// </summary>
    Task<long> InsertSupplier(Model.Supplier data);

    /// <summary>
    /// Get list of Certifying Body
    /// </summary>
    Task<IEnumerable<CertifyingBody>> GetCertifyingBody();

    /// <summary>
    /// Create Certifying Body
    /// </summary>
    Task<long> InsertCertifyingBody(Model.CertifyingBody data);
  }
}
