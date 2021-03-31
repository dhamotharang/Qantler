using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Request
{
  public class GetRequestByIDBasicCommand : IUnitOfWorkCommand<Model.Request>
  {
    readonly long _id;

    public GetRequestByIDBasicCommand(long id)
    {
      _id = id;
    }

    public async Task<Model.Request> Invoke(IUnitOfWork unitOfWork)
    {
      var result = await DbContext.From(unitOfWork).Request.GetRequestByIDBasic(_id);

      return result;
    }
  }
}
