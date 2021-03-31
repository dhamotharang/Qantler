using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Request.API.Services.Commands.Certificate360
{
  public class GetCertHistoryByCertIDCommand : IUnitOfWorkCommand<IList<Model.Certificate360History>>
  {
    readonly long _ID;

    public GetCertHistoryByCertIDCommand(long ID)
    {
      _ID = ID;
    }

    public async Task<IList<Model.Certificate360History>> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Certificate360
        .GetCertificate360History(_ID);
    }

  }
}