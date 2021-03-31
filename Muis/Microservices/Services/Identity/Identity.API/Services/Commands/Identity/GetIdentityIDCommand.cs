using Core.API;
using Core.API.Repository;
using Identity.API.Repository;
using System;
using System.Threading.Tasks;

namespace Identity.API.Services.Commands.Identity
{  
  public class GetIdentityIDCommand : IUnitOfWorkCommand<Model.Identity>
  {
    readonly Guid _id;

    public GetIdentityIDCommand(Guid id)
    {
      _id = id;
    }

    public async Task<Model.Identity> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Identity.GetIdentityByID(_id);
    }
  }
}
