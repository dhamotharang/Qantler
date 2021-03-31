using Core.Base.Repository;
using Core.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace JobOrder.Jobs.Repository
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
      param.Add("@UserID", Guid.Empty);
      param.Add("@UserName", string.Empty);
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
