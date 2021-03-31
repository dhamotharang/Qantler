using Core.API;
using Core.API.Repository;
using Identity.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.Services.Commands.Settings
{
  public class GetSettingsCommand : IUnitOfWorkCommand<List<Model.Settings>>
  {
    public GetSettingsCommand()
    {
    }

    public async Task<List<Model.Settings>> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Settings.GetSettings();
    }
  }
}

