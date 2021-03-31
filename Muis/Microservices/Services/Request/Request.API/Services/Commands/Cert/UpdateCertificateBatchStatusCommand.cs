using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.Model;
using Request.API.Repository;
using Request.Model;

namespace Request.API.Services.Commands.Cert
{
  public class UpdateCertificateBatchStatusCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long _batchID;
    readonly CertificateBatchStatus _status;

    public UpdateCertificateBatchStatusCommand(long batchID, CertificateBatchStatus status)
    {
      _batchID = batchID;
      _status = status;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      await DbContext.From(unitOfWork).Certificate.UpdateCertificateBatchStatus(_batchID, _status);

      return Unit.Default;
    }
  }
}
