using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.API.Repository;
using Dapper;
using Finance.Model;

namespace Finance.API.Repository
{
  public class SettingsRepository : ISettingsRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public SettingsRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
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
  }
}
