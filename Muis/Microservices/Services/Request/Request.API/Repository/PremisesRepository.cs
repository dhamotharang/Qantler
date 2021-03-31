using Core.API.Repository;
using Core.Model;
using Dapper;
using Request.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Request.API.Repository
{
  public class PremisesRepository : IPremisesRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public PremisesRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<long> CreatePremise(Premise premise)
    {
      var param = new DynamicParameters();
      param.Add("@Type", premise.Type);
      param.Add("@BuildingName", premise.BuildingName);
      param.Add("@BlockNo", premise.BlockNo);
      param.Add("@FloorNo", premise.FloorNo);
      param.Add("@UnitNo", premise.UnitNo);
      param.Add("@Street", premise.Address1);
      param.Add("@Postal", premise.Postal);
      param.Add("@IsLocal", premise.IsLocal);
      param.Add("@CustomerID", premise.CustomerID);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(
        _unitOfWork.Connection,
        "InsertPremise",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<long>("@ID");
    }

    public async Task<IList<Premise>> Select(Guid? customerID)
    {
      var param = new DynamicParameters();
      param.Add("@CustomerID", customerID);

      return (await SqlMapper.QueryAsync(
        _unitOfWork.Connection,
        "SelectPremise",
        new[]
        {
        typeof(Premise),
        typeof(Customer)
        },
        obj =>
        {
          var _premise = obj[0] as Premise;
          var _customer = obj[1] as Customer;
          _premise.Customer = _customer;
          return _premise;
        },
        param,
        splitOn: "PremiseCustomerID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }

    public async Task<Premise> GetByID(long id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      return (await SqlMapper.QueryAsync(
        _unitOfWork.Connection,
        "GetPremiseByID",
        new[]
        {
        typeof(Premise),
        typeof(Customer)
        },
        obj =>
        {
          var _premise = obj[0] as Premise;
          var _customer = obj[1] as Customer;
          _premise.Customer = _customer;
          return _premise;
        },
        param,
        splitOn: "PremiseCustomerID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct().FirstOrDefault();
    }
  }
}
