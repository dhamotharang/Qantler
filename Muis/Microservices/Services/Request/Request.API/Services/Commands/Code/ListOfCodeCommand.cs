using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using Request.Model;

namespace Request.API.Services.Commands.Code
{
  public class ListOfCodeCommand : IUnitOfWorkCommand<IList<Model.Code>>
  {
    readonly CodeType _type;

    public ListOfCodeCommand(CodeType type)
    {
      _type = type;
    }

    public async Task<IList<Model.Code>> Invoke(IUnitOfWork uow)
    {
      return await DbContext.From(uow).Code.Select(_type);
    }
  }
}
