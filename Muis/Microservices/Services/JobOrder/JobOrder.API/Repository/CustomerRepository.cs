using Core.API.Repository;
using Dapper;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Repository
{
  public class CustomerRepository : ICustomerRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public CustomerRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<Customer> GetCustomerByID(Guid id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      return (await SqlMapper.QueryAsync<Customer>(_unitOfWork.Connection,
        "GetCustomerByID",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task InsertOrReplace(Customer customer)
    {
      var param = new DynamicParameters();
      param.Add("@ID", customer.ID);
      param.Add("@Name", customer.Name);
      param.Add("@Code", customer.Code);
      param.Add("@GroupCode", customer.GroupCode);      

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertOrReplaceCustomer",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }
  }
}
