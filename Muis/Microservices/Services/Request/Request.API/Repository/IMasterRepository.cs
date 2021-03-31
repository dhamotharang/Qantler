using Request.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Repository
{
  public interface IMasterRepository
  {
    public Task<IEnumerable<Master>> GetMasterList(MasterType type);
    public Task InsertMaster(Master data);
    public Task UpdateMaster(Master data);
    public Task DeleteMaster(Guid id);
  }
}
