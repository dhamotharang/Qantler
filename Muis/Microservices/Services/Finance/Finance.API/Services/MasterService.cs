using Finance.Model;
using Core.API;
using Core.API.Provider;
using System.Collections.Generic;
using System.Threading.Tasks;
using Finance.API.Services.Commands.Master;

namespace Finance.API.Services
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
