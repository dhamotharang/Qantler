using Core.Model;
using Dapper;
using Request.API.Repository.Mappers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Request.Model;
using Core.API.Repository;

namespace Request.API.Repository
{
  public class SettingsRepository : ISettingsRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public SettingsRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<List<Settings>> GetSettings()
    {
      var settingsMapper = new SettingsMappers();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetRequestSettings",
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
        transaction: _unitOfWork.Transaction)).ToList();
    }

    public async Task<Settings> GetSettingsByType(SettingsType type)
    {
      var param = new DynamicParameters();
      param.Add("@Type", type);

      return (await SqlMapper.QueryAsync<Settings>(_unitOfWork.Connection,
        "GetSettingsByType",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task UpdateSettings(IList<Settings> settings, Guid userID, string userName)
    {
      foreach (Settings setting in settings)
      {
        var param = new DynamicParameters();
        param.Add("@Type", setting.Type);
        param.Add("@Value", setting.Value);
        param.Add("@UserID", userID);
        param.Add("@UserName", userName);

        await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
          "UpdateRequestSettings",
          param,
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction);
      }
    }
  }
}