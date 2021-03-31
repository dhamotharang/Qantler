using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Identity.API.Repository;

namespace Identity.API.Services.Commands.Identity
{
  public class ListOfIdentityCommand : IUnitOfWorkCommand<IList<Model.Identity>>
  {
    readonly IdentityFilter _filter;
    
    public ListOfIdentityCommand(IdentityFilter filter)
    {
      _filter = filter;
    }

    public async Task<IList<Model.Identity>> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Identity.Query(_filter);
    }
  }
}
