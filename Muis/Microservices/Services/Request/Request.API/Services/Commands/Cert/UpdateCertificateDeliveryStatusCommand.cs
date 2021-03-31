using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.Model;
using Request.API.Repository;
using Request.Model;

namespace Request.API.Services.Commands.Cert
{
  public class UpdateCertificateDeliveryStatusCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long[] _ids;
    public UpdateCertificateDeliveryStatusCommand(long[] ids)
    {
      _ids = ids;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      unitOfWork.BeginTransaction();

      foreach (var item in _ids)
      {
        await DbContext.From(unitOfWork).Certificate.UpdateCertificateDeliveryStatus(item, CertificateDeliveryStatus.Delivered);
      }

      unitOfWork.Commit();

      return Unit.Default;
    }
  }
}
