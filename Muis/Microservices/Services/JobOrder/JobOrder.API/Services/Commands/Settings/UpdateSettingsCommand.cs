using Core.API;
using Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API.Repository;
using JobOrder.API.Repository;

namespace JobOrder.API.Services.Commands.JobOrders.Settings
{
  public class UpdateSettingsCommand : IUnitOfWorkCommand<Unit>
  {
    readonly IList<JobOrder.Model.Settings> _settings;

    readonly Guid _userID;

    readonly string _userName;

    public UpdateSettingsCommand(IList<Model.Settings> settings, Guid userID, string userName)
    {
      _settings = settings;
      _userID = userID;
      _userName = userName;
    }

    public async Task<Unit> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      await dbContext.Settings.UpdateSettings(_settings, _userID, _userName);

      uow.Commit();

      return Unit.Default;
    }
  }
}
