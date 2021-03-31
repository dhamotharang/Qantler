using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Premises
{
  public class SelectPremiseCommand : IUnitOfWorkCommand<IList<Model.Premise>>
  {
    readonly Guid? _customerID;

    public SelectPremiseCommand(Guid? customerID)
    {
      _customerID = customerID;
    }

    public async Task<IList<Model.Premise>> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Premises.Select(_customerID);
    }
  }
}