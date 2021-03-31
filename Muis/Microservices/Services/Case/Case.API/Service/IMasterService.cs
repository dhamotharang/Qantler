using Case.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Case.API.Service
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
