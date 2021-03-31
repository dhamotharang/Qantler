using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Request.API.Params;
using Request.API.Repository;

namespace Request.API.Services.Commands.Request
{
  public class QueryRequestCommand : IUnitOfWorkCommand<IEnumerable<Model.Request>>
  {
    readonly RequestOptions _options;

    public QueryRequestCommand(RequestOptions options)
    {
      _options = options;
    }

    public async Task<IEnumerable<Model.Request>> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Request.Select(_options);
    }
  }
}
