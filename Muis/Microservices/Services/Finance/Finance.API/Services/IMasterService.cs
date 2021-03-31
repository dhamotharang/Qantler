using Finance.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finance.API.Services
{
  public interface IMasterService
  {
    /// <summary>
    ///  List of master data.
    /// </summary>
    /// <returns>the list of master data</returns>
    Task<IEnumerable<Master>> GetMaster(MasterType type);
  }
}
