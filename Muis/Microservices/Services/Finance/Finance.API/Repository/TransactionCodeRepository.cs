using Core.API;
using Core.API.Repository;
using Core.Model;
using Dapper;
using Finance.API.Models;
using Finance.API.Repository.Mappers;
using Finance.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.API.Repository
{
  public class TransactionCodeRepository : ITransactionCodeRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public TransactionCodeRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Model.TransactionCode>> Select(
      TransactionCodeOptions options)
    {
      var param = new DynamicParameters();
      param.Add("@ID", options.ID);
      param.Add("@Code", StringUtils.NullIfEmptyOrNull(options.Code));
      param.Add("@GLEntry", StringUtils.NullIfEmptyOrNull(options.GLEntry));
      param.Add("@Description", StringUtils.NullIfEmptyOrNull(options.Text));

      var mapper = new TransactionCodeMapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetFinanceTransactionCode",
        new[]
        {
          typeof(TransactionCode),
          typeof(Price)
        },
        obj =>
        {
          return mapper.Map(
            obj[0] as TransactionCode,
            price: obj[1] as Price);
        },
        param,
        splitOn: "ID,PriceID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct();
    }

    public async Task<TransactionCode> GetTransactionCodeByID(long id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);
      var transactionCodeMapper = new TransactionCodeMapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetFinanceTransactionCodeByID",
        new[]
        {
          typeof(TransactionCode),
          typeof(Condition),
          typeof(Log),
          typeof(Price)
        },
        obj =>
        {
          return transactionCodeMapper.Map(
            obj[0] as TransactionCode,
            obj[1] as Condition,
            obj[2] as Log,
            obj[3] as Price);
        },
        param,
        splitOn: "ID,ConditionID,LogID,PriceID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<bool> AddPrice(
      long id, string nAmount, string nEDate,
      Guid userID,
      string userName)
    {
      var param = new DynamicParameters();
      param.Add("@Amount", nAmount);
      param.Add("@EffectiveFrom", nEDate);
      param.Add("@TransactionCodeID", id);
      param.Add("@UserID", userID);
      param.Add("@UserName", userName);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertPrice",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return true;
    }

    public async Task<bool> UpdatePrice(
      IList<Price> priceList,
      Guid userID,
      string userName)
    {
      foreach (Price price in priceList)
      {
        var param = new DynamicParameters();
        param.Add("@ID", price.ID);
        param.Add("@Amount", price.Amount);
        param.Add("@EffectiveFrom", price.EffectiveFrom);
        param.Add("@TransactionCodeID", price.TransactionCodeID);
        param.Add("@UserID", userID);
        param.Add("@UserName", userName);

        await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
          "UpdatePriceHistory",
          param,
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction);
      }

      return true;
    }

    public async Task<Price> GetLatestPrice(string code, DateTimeOffset refDate)
    {
      var param = new DynamicParameters();
      param.Add("@Code", code);
      param.Add("@RefDate", refDate);

      return (await SqlMapper.QueryAsync<Price>(_unitOfWork.Connection,
        "GetLatestPrice",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<TransactionCode> GetTransactionCodeByCode(string code, DateTimeOffset? refDate)
    {
      var param = new DynamicParameters();
      param.Add("@Code", code);
      param.Add("@RefDate", refDate);

      var mapper = new TransactionCodeMapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetTransactionCodeByCode",
        new[]
        {
            typeof(TransactionCode),
            typeof(Price),
            typeof(Condition)
        },
        obj =>
        {
          var trxCode = obj[0] as TransactionCode;
          var price = obj[1] as Price;
          var condition = obj[2] as Condition;
          return mapper.Map(trxCode, condition: condition, price: price);
        },
        param,
        splitOn: "ID,PriceID,ConditionID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }
  }
}