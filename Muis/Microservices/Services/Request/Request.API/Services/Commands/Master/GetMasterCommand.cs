using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using Request.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Masterlist
{
  public class GetMasterCommand : IUnitOfWorkCommand<IEnumerable<Master>>
  {
    readonly MasterType _type;

    public GetMasterCommand(MasterType type)
    {
      _type = type;
    }

    public async Task<IEnumerable<Master>> Invoke(IUnitOfWork uow)
    {
      var dbContext = new DbContext(uow);
      return await dbContext.Master.GetMasterList(_type);
    }
  }
}
