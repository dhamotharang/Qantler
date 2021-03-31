using Core.API.Repository;
using Dapper;
using Finance.API.Services;
using Finance.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.API.Repository
{
  public class BankRepository : IBankRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public BankRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }
    public async Task<long> Insert(Bank bank)
    {
      var param = new DynamicParameters();
      param.Add("@AccountNo", bank.AccountNo);
      param.Add("@AccountName", bank.AccountName);
      param.Add("@BankName", bank.BankName);
      param.Add("@BranchCode", bank.BranchCode);
      param.Add("@DDAStatus", bank.DDAStatus);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertBank",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<long>("@ID");
    }

    public async Task<IList<Bank>> Query(BankFilter options)
    {
      var param = new DynamicParameters();
      param.Add("@AccountName", options.AccountName);
      param.Add("@AccountNo", options.AccountNo);
      param.Add("@BankName", options.BankName);
      param.Add("@BranchCode", options.BranchCode);
      param.Add("@DDAStatus", options.DDAStatus);

      return (await SqlMapper.QueryAsync<Bank>(_unitOfWork.Connection,
        "SelectBank",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).ToList();
    }

    public async Task Update(Bank bank)
    {
      var param = new DynamicParameters();
      param.Add("@ID", bank.ID);
      param.Add("@AccountNo", bank.AccountNo);
      param.Add("@AccountName", bank.AccountName);
      param.Add("@BankName", bank.BankName);
      param.Add("@BranchCode", bank.BranchCode);
      param.Add("@DDAStatus", bank.DDAStatus);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateBank",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task Delete(long id)
    {
      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        $"DELETE FROM [Bank] WHERE [ID] = @BankID",
        new
        {
          BankID = id
        },
        transaction: _unitOfWork.Transaction);
    }
  }
}
