using eHS.Portal.Models.HalalLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Client
{
  public interface IHalalLibrarySdk
  {
    Task<IndexModel> Search(HalalLibraryOptions options, int lastId, int pageSize);

    Task<long> InsertHalalLibrary(Model.HLIngredient data);

    Task<long> UpdateHalalLibrary(Model.HLIngredient data);

    Task<bool> DeleteHalalLibrary(long id);
  }
}
