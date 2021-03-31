using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using Request.Model;

namespace Request.API.Services.Commands.Cert
{
  public class GetCertificateBatchCommentsCommand : IUnitOfWorkCommand<IList<Comment>>
  {
    readonly long _id;

    public GetCertificateBatchCommentsCommand(long id)
    {
      _id = id;
    }

    public async Task<IList<Comment>> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Certificate.GetCertificateBatchComments(_id);
    }
  }
}
