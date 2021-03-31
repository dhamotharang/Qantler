using Case.API.Services.Commands.Master;
using Case.Model;
using Core.API;
using Core.API.Provider;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Case.API.Service
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
  }
}
