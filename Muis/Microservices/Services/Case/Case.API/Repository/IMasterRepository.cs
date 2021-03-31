using Case.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Case.API.Repository
{
  public interface IMasterRepository
  {
    /// <summary>
    /// Get master list by type
    /// </summary>
    public Task<IEnumerable<Master>> GetMasterList(MasterType type);
  }
}
