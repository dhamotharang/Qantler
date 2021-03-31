using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using Request.Model;

namespace Request.API.Services.Commands.Cert
{
  public class GetCertByIDCommand : IUnitOfWorkCommand<CertificateBatch>
  {
    readonly long _id;
    readonly bool _includeAll;

    public GetCertByIDCommand(long id, bool includeAll)
    {
      _id = id;
      _includeAll = includeAll;
    }

    public async Task<CertificateBatch> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);

      return _includeAll
        ? await dbContext.Certificate.GetCertificateBatchByIDFull(_id)
        : await dbContext.Certificate.GetCertificateBatchByID(_id);
    }
  }
}
