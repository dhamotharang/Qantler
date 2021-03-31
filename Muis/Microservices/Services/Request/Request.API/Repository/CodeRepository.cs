using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.API.Repository;
using Dapper;
using Request.Model;

namespace Request.API.Repository
{
  public class CodeRepository : ICodeRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public CodeRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public CodeRepository()
    {
    }

    public async Task<Code> GetByID(long id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      return (await SqlMapper.QueryAsync<Code>(_unitOfWork.Connection,
        "GetCodeByID",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<long> Insert(Code entity)
    {
      var param = new DynamicParameters();
      param.Add("@Type", entity.Type);
      param.Add("@Value", entity.Value);
      param.Add("@Text", entity.Text);
      param.Add("@BillingCycle", entity.BillingCycle);
      param.Add("@CertificateExpiry", entity.CertificateExpiry);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertCode",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<long>("@ID");
    }

    public async Task Update(Code entity)
    {
      var param = new DynamicParameters();
      param.Add("@ID", entity.ID);
      param.Add("@Type", entity.Type);
      param.Add("@Value", entity.Value);
      param.Add("@Text", entity.Text);
      param.Add("@BillingCycle", entity.BillingCycle);
      param.Add("@CertificateExpiry", entity.CertificateExpiry);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateCode",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<IList<Code>> Select(CodeType type)
    {
      var param = new DynamicParameters();
      param.Add("@Type", type);

      return (await SqlMapper.QueryAsync<Code>(_unitOfWork.Connection,
        "SelectCode",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }

    public async Task<int> GenerateCodeSeries(CodeType type)
    {
      var param = new DynamicParameters();
      param.Add("@Type", type);
      param.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "GenerateCodeSeries",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<int>("@Result");
    }

    public async Task SyncCodeToRequest(Guid customerID, long? codeID, long? groupCodeID)
    {
      var param = new DynamicParameters();
      param.Add("@CustomerID", customerID);
      param.Add("@CodeID", codeID);
      param.Add("@GroupCodeID", groupCodeID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "SyncCodeToRequest",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task SyncOfficerToRequest(Guid customerID, Guid? officerInCharge)
    {
      var param = new DynamicParameters();
      param.Add("@CustomerID", customerID);
      param.Add("@OfficerInCharge", officerInCharge);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "SyncOfficerToRequest",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

  }
}
