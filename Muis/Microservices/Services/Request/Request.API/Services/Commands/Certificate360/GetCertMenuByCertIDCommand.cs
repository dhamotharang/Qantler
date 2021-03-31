using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Request.API.Services.Commands.Certificate360
{
  public class GetCertMenuByCertIDCommand : IUnitOfWorkCommand<IList<Model.Menu>>
  {
    readonly long _ID;

    public GetCertMenuByCertIDCommand(long ID)
    {
      _ID = ID;
    }

    public async Task<IList<Model.Menu>> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Certificate360
        .GetCertificate360Menus(_ID);
    }

  }
}