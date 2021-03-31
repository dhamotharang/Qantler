using Case.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Case.API.Repository
{
  public interface ISanctionInfoRepository
  {
    /// <summary>
    /// Insert Sanction Info.
    /// </summary>
    public Task<long> InsertSanctionInfo(SanctionInfo data);

    /// <summary>
    /// Get Sanction Info.
    /// </summary>
    public Task<List<SanctionInfo>> GetSanctionInfo(long? caseID);

  }
}