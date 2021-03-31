using Case.API.Repository;
using Case.Model;
using Core.API;
using Core.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Master
{
  public class GetMasterCommand : IUnitOfWorkCommand<IEnumerable<Model.Master>>
  {
    readonly MasterType _type;

    public GetMasterCommand(MasterType type)
    {
      _type = type;
    }

    public async Task<IEnumerable<Model.Master>> Invoke(IUnitOfWork uow)
    {
      var dbContext = new DbContext(uow);
      return await dbContext.Master.GetMasterList(_type);
    }
  }
}
