using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using Request.Model;

namespace Request.API.Services.Commands.Cert
{
  public class CertBatchFilterCommand : IUnitOfWorkCommand<IList<CertificateBatch>>
  {
    readonly CertificateBatchFilter _filter;

    readonly bool _includeAll;

    public CertBatchFilterCommand(CertificateBatchFilter filter, bool includeAll)
    {
      _filter = filter;

      _includeAll = includeAll;
    }

    public async Task<IList<CertificateBatch>> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);

      return _includeAll
        ? await dbContext.Certificate.BatchFullFilter(_filter)
        : await dbContext.Certificate.BatchFilter(_filter);
    }
  }
}
