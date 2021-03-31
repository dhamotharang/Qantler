using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Request.API.Repository;

namespace Request.API.Services.Commands.Code
{
  public class GetCodeByID : IUnitOfWorkCommand<Model.Code>
  {
    readonly long _id;

    public GetCodeByID(long id)
    {
      _id = id;
    }

    public async Task<Model.Code> Invoke(IUnitOfWork uow)
    {
      var result = await DbContext.From(uow).Code.GetByID(_id);
      if (result == null)
      {
        throw new NotFoundException();
      }

      return result;
    }
  }
}
