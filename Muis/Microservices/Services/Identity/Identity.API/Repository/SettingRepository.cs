using Core.API.Repository;
using Core.Model;
using Dapper;
using Identity.API.Repository.Mappers;
using Identity.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Repository
{
  public class SettingRepository : ISettingRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public SettingRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<List<Settings>> GetSettings()
    {
      await Task.CompletedTask;
      var settingsMapper = new SettingsMappers();

      return SqlMapper.Query<Settings>(
        _unitOfWork.Connection,
        "GetIdentitySettings",
         new[]
        {
              typeof(Settings),
              typeof(Log)
        },
        obj =>
        {
          Settings bs = obj[0] as Settings;
          Log l = obj[1] as Log;
          return settingsMapper.Map(bs, l);
        },
        splitOn: "ID,LogID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction).ToList();
    }

    public async Task<bool> UpdateSettings(IList<Settings> settings, Guid userID, string userName)
    {
      foreach (Settings setting in settings)
      {
        var param = new DynamicParameters();
        param.Add("@Type", setting.Type);
        param.Add("@Value", setting.Value);
        param.Add("@UserID", userID);
        param.Add("@UserName", userName);

        await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
          "UpdateIdentitySettings",
          param,
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction);
      }
      return true;
    }
  }
}