using Core.Model;
using Dapper;
using Identity.Model;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Identity.API.Repository.Mappers;
using Core.API.Repository;
using System.Collections.Generic;
using Identity.API.Services;

namespace Identity.API.Repository
{
  public class CustomerRepository : ICustomerRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public CustomerRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<Customer> GetCustomerContactInfo(string name, string altID)
    {
      var param = new DynamicParameters();
      param.Add("@Name", name);
      param.Add("@AltID", altID);

      var customerMapper = new CustomerMapper();

      return (await SqlMapper.QueryAsync(
        _unitOfWork.Connection,
        "GetCustomerContactInfo",
        new[]
        {
          typeof(Customer),
          typeof(Premise)
        },
        obj =>
        {
          var cust = obj[0] as Customer;

          return customerMapper.Map(cust);
        },
        param,
        splitOn: "ID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task InsertCustomer(Customer customer)
    {
      var param = new DynamicParameters();
      param.Add("@ID", customer.ID);
      param.Add("@Name", customer.Name);
      param.Add("@IDType", customer.IDType);
      param.Add("@AltID", customer.AltID);
      param.Add("@Status", customer.Status);
      param.Add("@ParentID", customer.ParentID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertCustomer",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public bool ValidateCustomerStatus(string AltID, string Name)
    {
      var param = new DynamicParameters();

      param.Add("@AltID", AltID);
      param.Add("@Name", Name);
      param.Add("@valid",
        dbType: DbType.Boolean,
        direction: ParameterDirection.Output);

      SqlMapper.Execute(
        _unitOfWork.Connection,
        "ValidateCustomerStatus",
        param, commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<bool>("@valid");
    }

    public async Task<Customer> GetCustomerByID(Guid ID)
    {
      var param = new DynamicParameters();
      param.Add("@ID", ID);

      var mapper = new CustomerMapper();

      return (await SqlMapper.QueryAsync(
        _unitOfWork.Connection,
        "GetCustomerByID",
        new[]
        {
          typeof(Customer),
          typeof(Customer)
        },
        obj =>
        {
          return mapper.Map(obj[0] as Customer,
            obj[1] as Customer);
        },
        param,
        splitOn: "ID,PID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<IList<Customer>> Select(CustomerFilter filter)
    {
      var param = new DynamicParameters();

      return (await SqlMapper.QueryAsync<Customer>(_unitOfWork.Connection,
        "SelectCustomer",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }

    public async Task UpdateCustomer(Customer customer)
    {
      var param = new DynamicParameters();
      param.Add("@ID", customer.ID);
      param.Add("@Name", customer.Name);
      param.Add("@IDType", customer.IDType);
      param.Add("@AltID", customer.AltID);
      param.Add("@Status", customer.Status);
      param.Add("@ParentID", customer.ParentID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateCustomer",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }
  }
}
