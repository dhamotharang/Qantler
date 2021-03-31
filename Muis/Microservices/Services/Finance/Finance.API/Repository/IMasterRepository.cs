using Finance.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finance.API.Repository
{
  public interface IMasterRepository
  {
    /// <summary>
    /// Get master list by type
    /// </summary>
    public Task<IEnumerable<Master>> GetMasterList(MasterType type);
  }
}
