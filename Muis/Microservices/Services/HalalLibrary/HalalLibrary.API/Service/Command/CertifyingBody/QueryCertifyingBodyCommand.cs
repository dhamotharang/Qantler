using Core.API;
using Core.API.Repository;
using HalalLibrary.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HalalLibrary.API.Services.Commands.CertifyingBody
{
  public class QueryCertifyingBodyCommand : IUnitOfWorkCommand<IEnumerable<Model.CertifyingBody>>
  {
    public QueryCertifyingBodyCommand()
    {
      
    }

    public async Task<IEnumerable<Model.CertifyingBody>> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).CertifyingBody.Select();
    }
  }
}
