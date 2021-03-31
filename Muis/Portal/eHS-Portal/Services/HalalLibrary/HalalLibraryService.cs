using eHS.Portal.Client;
using eHS.Portal.Model;
using eHS.Portal.Models.HalalLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Services.HalalLibrary
{
  public class HalalLibraryService : IHalalLibraryService
  {
    readonly ApiClient _apiClient;

    public HalalLibraryService(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public Task<IndexModel> Search(HalalLibraryOptions options, int offsetRows, int nextRows)
    {
      return _apiClient.HalalLibrarySdk.Search(options, offsetRows, nextRows);
    }

    public Task<long> InsertHalalLibrary(HLIngredient data)
    {
      return _apiClient.HalalLibrarySdk.InsertHalalLibrary(data);
    }

    public Task<long> UpdateHalalLibrary(HLIngredient data)
    {
      return _apiClient.HalalLibrarySdk.UpdateHalalLibrary(data);
    }

    public Task<bool> DeleteHalalLibrary(long id)
    {
      return _apiClient.HalalLibrarySdk.DeleteHalalLibrary(id);
    }

    public Task<IEnumerable<CertifyingBody>> GetCertifyingBody()
    {
      return _apiClient.HalalLibrarySdk.GetCertifyingBody();
    }

    public Task<long> InsertCertifyingBody(CertifyingBody data)
    {
      return _apiClient.HalalLibrarySdk.InsertCertifyingBody(data);
    }

    public Task<IEnumerable<Supplier>> GetSupplier()
    {
      return _apiClient.HalalLibrarySdk.GetSupplier();
    }

    public Task<long> InsertSupplier(Supplier data)
    {
      return _apiClient.HalalLibrarySdk.InsertSupplier(data);
    }
  }
}
