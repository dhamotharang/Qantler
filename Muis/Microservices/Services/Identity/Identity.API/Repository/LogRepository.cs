using System;
using System.Data;
using System.Threading.Tasks;
using Core.API.Repository;
using Core.Model;
using Dapper;

namespace Identity.API.Repository
{
  public class LogRepository : ILogRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public LogRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<long> InsertLog(Log log)
    {
      var param = new DynamicParameters();
      param.Add("@Type", log.Type);
      param.Add("@RefID", log.RefID);
      param.Add("@Action", log.Action);
      param.Add("@Raw", log.Raw);
      param.Add("@Notes", log.Notes);
      param.Add("@UserID", log.UserID);
      param.Add("@UserName", log.UserName);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertLog",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<long>("@ID");
    }
  }
}
