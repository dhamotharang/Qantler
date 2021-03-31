using Core.API.Repository;
using Core.Model;
using Dapper;
using JobOrder.API.Repository.Mappers;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Repository
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
      var mapper = new SettingsMapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetJobOrderSettings",
        new[]
        {
          typeof(Settings),
          typeof(Log)
        },
        obj =>
        {
          Settings bs = obj[0] as Settings;
          Log l = obj[1] as Log;
          return mapper.Map(bs, l);
        },
        splitOn: "ID,LogID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).ToList();
    }

    public async Task UpdateSettings(IList<Settings> settings,
      Guid userID, string userName)
    {
      foreach (Settings setting in settings)
      {
        var param = new DynamicParameters();
        param.Add("@Type", setting.Type);
        param.Add("@Value", setting.Value);
        param.Add("@UserID", userID);
        param.Add("@UserName", userName);

        await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
          "UpdateJobOrderSettings",
          param,
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction);
      }
    }
  }
}