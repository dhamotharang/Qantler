using Core.API;
using Core.API.Repository;
using Identity.API.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.Services.Commands.Settings
{
  public class UpdateSettingsCommand : IUnitOfWorkCommand<bool>
  {
    readonly IList<Model.Settings> _settings;

    readonly Guid _userID;

    readonly string _userName;

    public UpdateSettingsCommand(IList<Model.Settings> settings, Guid userID, string userName)
    {
      _settings = settings;
      _userID = userID;
      _userName = userName;
    }

    public async Task<bool> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var result = await dbContext.Settings.UpdateSettings(_settings, _userID, _userName);

      unitOfWork.Commit();

      return result;
    }
  }
}