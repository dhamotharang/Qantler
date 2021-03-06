using Core.API;
using Core.API.Provider;
using JobOrder.API.Services.Commands.Masterlist;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobOrder.API.Services
{
  public class MasterService : TransactionalService,
                               IMasterService
  {
    public MasterService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {
    }

    public async Task<IEnumerable<Master>> GetMaster(MasterType type)
    {
      return await Execute(new GetMasterCommand(type));
    }

    public async Task InsertMaster(Master masterData)
    {
      await Execute(new InsertMasterCommand(masterData));
    }

    public async Task UpdateMaster(Master masterData)
    {
      await Execute(new UpdateMasterCommand(masterData));
    }

    public async Task DeleteMaster(Guid id)
    {
      await Execute(new DeleteMasterCommand(id));
    }
  }
}
