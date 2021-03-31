using Core.API;
using Core.API.Provider;
using Identity.API.Services.Commands.Premise;
using Identity.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.Services
{
  public class PremiseService : TransactionalService,
                                                     IPremiseService
  {
    public PremiseService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {
    }

    public async Task<IList<Premise>> GetPremises()
    {
      return await Execute(new GetPremiseCommand());
    }
  }
}