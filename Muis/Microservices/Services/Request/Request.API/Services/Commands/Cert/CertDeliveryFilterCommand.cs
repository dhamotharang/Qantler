using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using Request.Model;

namespace Request.API.Services.Commands.Cert
{
  public class CertDeliveryFilterCommand : IUnitOfWorkCommand<IList<Certificate>>
  {
    readonly CertificateDeliveryFilter _filter;
    public CertDeliveryFilterCommand(CertificateDeliveryFilter filter)
    {
      _filter = filter;
    }

    public async Task<IList<Certificate>> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Certificate.CertDeliveryFilter(_filter);
    }
  }
}
