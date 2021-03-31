using Core.API;
using Core.API.Provider;
using Request.API.Services.Commands.Premises;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Services
{
  public class PremiseService : TransactionalService,
                                 IPremiseService
  {
    public PremiseService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {
    }

    public async Task<IList<Model.Premise>> GetPremises(Guid? customerID)
    {
      return await Execute(new SelectPremiseCommand(customerID));
    }

    public async Task<Model.Premise> CreatePremise(Model.Premise data)
    {
      return await Execute(new CreatePremiseCommand(data));
    }
  }
}
