using Core.API;
using Core.API.Provider;
using HalalLibrary.API.Services;
using HalalLibrary.API.Services.Commands.CertifyingBody;
using HalalLibrary.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HalalLibrary.Service
{
  public class CertifyingBodyService : TransactionalService,
                                ICertifyingBodyService
  {

    public CertifyingBodyService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {

    }

    public async Task<IEnumerable<CertifyingBody>> Select()
    {
      return await Execute(new QueryCertifyingBodyCommand());
    }

    public async Task<long> InsertCertifyingBody(CertifyingBody data)
    {
      return await Execute(new InsertCertifyingBodyCommand(data));
    }
  }
}
