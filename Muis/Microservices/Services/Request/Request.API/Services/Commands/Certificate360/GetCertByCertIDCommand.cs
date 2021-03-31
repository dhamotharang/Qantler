using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Certificate360
{
  public class GetCertByCertIDCommand : IUnitOfWorkCommand<Model.Certificate360>
  {
    readonly long _ID;

    public GetCertByCertIDCommand(long ID)
    {
      _ID = ID;
    }

    public async Task<Model.Certificate360> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Certificate360.GetCertificateByCertID(_ID);
    }

  }
}