using Case.Model;
using Core.API.Repository;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Repository
{
  public class SanctionInfoRepository : ISanctionInfoRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public SanctionInfoRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<long> InsertSanctionInfo(SanctionInfo data)
    {
      var caseParam = new DynamicParameters();

      caseParam.Add("@Type", data.Type);
      caseParam.Add("@Sanction", data.Sanction);
      caseParam.Add("@Value", data.Value);
      caseParam.Add("@CaseID", data.CaseID);
      caseParam.Add("@Out", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertSanctionInfo",
        caseParam,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      var caseID = caseParam.Get<long>("@Out");

      return caseID;
    }

    public async Task<List<SanctionInfo>> GetSanctionInfo(long? caseID)
    {
      var param = new DynamicParameters();
      param.Add("@CaseID", caseID);

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetSanctionInfo",
        new[]
        {
            typeof(SanctionInfo)
        },
        obj =>
        {
          return obj[0] as SanctionInfo;
        },
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }
  }
}