using Core.API;
using Request.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API.Repository;

namespace Request.API.Services.Commands.Settings
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
