using Core.API;
using Core.API.Repository;
using Identity.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.Services.Commands.Premise
{
  public class GetPremiseCommand : IUnitOfWorkCommand<IList<Model.Premise>>
  {
    public GetPremiseCommand()
    {
    }

    public async Task<IList<Model.Premise>> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Premise.Select();
    }
  }
}
