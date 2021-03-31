using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using Request.Model;

namespace Request.API.Services.Commands.Cert
{
  public class CertificateFilterCommand : IUnitOfWorkCommand<IList<Certificate>>
  {
    readonly CertificateFilter _filter;

    public CertificateFilterCommand(CertificateFilter filter)
    {
      _filter = filter;
    }

    public async Task<IList<Certificate>> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Certificate.CertificateFilter(_filter);
    }
  }
}
