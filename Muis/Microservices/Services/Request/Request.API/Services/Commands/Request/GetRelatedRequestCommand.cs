using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Request
{
  public class GetRelatedRequestCommand : IUnitOfWorkCommand<IEnumerable<Model.Request>>
  {
    readonly long _id;

    public GetRelatedRequestCommand(long id)
    {
      _id = id;
    }

    public async Task<IEnumerable<Model.Request>> Invoke(IUnitOfWork unitOfWork)
    {
      var result = await DbContext.From(unitOfWork).Request.SelectRelatedRequests(_id);

      return result;
    }
  }
}