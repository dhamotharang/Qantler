using Core.API;
using JobOrder.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API.Repository;

namespace JobOrder.API.Services.Commands.Settings
{
  public class GetSettingsCommand : IUnitOfWorkCommand<List<Model.Settings>>
  {
    public Task<List<Model.Settings>> Invoke(IUnitOfWork uow)
    {
      return DbContext.From(uow).Settings.GetSettings();
    }
  }
}
