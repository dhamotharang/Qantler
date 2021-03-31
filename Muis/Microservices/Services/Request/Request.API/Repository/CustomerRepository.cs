using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.Model;
using Dapper;
using Request.API.Models;
using Request.API.Repository.Mappers;
using Request.Model;

namespace Request.API.Repository
{
  public class CustomerRepository : ICustomerRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public CustomerRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<Customer> GetByID(Guid ID)
    {
      var param = new DynamicParameters();
      param.Add("@ID", ID);

      var mapper = new CustomerMapper();

      return (await SqlMapper.QueryAsync<Customer>(_unitOfWork.Connection,
        "GetCustomerByID",
        new[]
        {
          typeof(Customer),
          typeof(Code),
          typeof(Code),
          typeof(Officer)
        },
        obj =>
        {
          return mapper.Map(obj[0] as Customer,
            obj[1] as Code,
            obj[2] as Code,
            obj[3] as Officer);
        },
        param,
        splitOn: "ID,CodeID,GroupCodeID,OfficerID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<IEnumerable<Model.Request>> GetRecentRequest(Guid ID,
      long? rowFrom = 0,
      long? rowCount = 10)
    {
      var mapper = new RequestMapper();

      var param = new DynamicParameters();
      param.Add("@ID", ID);
      param.Add("@RowFrom", rowFrom);
      param.Add("@RowCount", rowCount);

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetCustomerRecentRequest",
        new[]
        {
          typeof(Model.Request),
          typeof(RequestLineItem)
        },
        obj =>
        {
          return mapper.Map(obj[0] as Model.Request,
            obj[1] as RequestLineItem);
        },
        param,
        splitOn: "ID,LineItemID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct();
    }

    public async Task<IList<Certificate360>> QueryCustomer(CustomerOptions filter)
    {
      var statusTable = new DataTable();
      statusTable.Columns.Add("Val", typeof(int));

      if (filter.Status?.Any() ?? false)
      {
        foreach (var status in filter.Status)
        {
          statusTable.Rows.Add(status);
        }
      }

      var param = new DynamicParameters();
      param.Add("@ID", filter.ID);
      param.Add("@Code", StringUtils.NullIfEmptyOrNull(filter.Code));
      param.Add("@GroupCode", StringUtils.NullIfEmptyOrNull(filter.GroupCode));
      param.Add("@Name", StringUtils.NullIfEmptyOrNull(filter.Name));
      param.Add("@CertificateNo", StringUtils.NullIfEmptyOrNull(filter.CertificateNo));
      param.Add("@Premise", StringUtils.NullIfEmptyOrNull(filter.Premise));
      param.Add("@PremiseID", filter.PremiseID);
      param.Add("@Status", statusTable.AsTableValuedParameter("dbo.SmallIntType"));

      var mapper = new Certificate360Mapper();

      return (await SqlMapper.QueryAsync(
          _unitOfWork.Connection,
          "SelectCustomer",
          new[]
          {
            typeof(Certificate360),
            typeof(Customer),
            typeof(Code),
            typeof(Code),
            typeof(Premise),
            typeof(Officer)
          },
          obj =>
          {
            var cert = obj[0] as Certificate360;
            var certCustomer = obj[1] as Customer;
            var certCustCode = obj[2] as Code;
            var certCustGroupCode = obj[3] as Code;
            var certPremise = obj[4] as Premise;
            var certCustOfficer = obj[5] as Officer;
            return mapper.Map(cert,
              certCustomer,
              certCustCode,
              certCustGroupCode,
              certCustOfficer,
              certPremise);
          },
          param,
          splitOn: "ID, CustID, CustCodeID, CustCodeGroupID, PremID, OfficerID",
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction)).ToList();
    }

    public async Task InsertCustomer(Customer customer)
    {
      var param = new DynamicParameters();
      param.Add("@CustomerID", customer.ID);
      param.Add("@CustomName", customer.Name);
      param.Add("@AltID", customer.AltID);
      param.Add("@IDType", customer.IDType);

      await SqlMapper.ExecuteAsync(
        _unitOfWork.Connection,
        "InsertCustomer",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task UpdateCustomer(Customer customer)
    {
      var param = new DynamicParameters();
      param.Add("@ID", customer.ID);
      param.Add("@Name", customer.Name);
      param.Add("@CodeID", customer.CodeID);
      param.Add("@GroupCodeID", customer.GroupCodeID);
      param.Add("@OfficerInCharge", customer.OfficerInCharge);

      await SqlMapper.ExecuteAsync(
        _unitOfWork.Connection,
        "UpdateCustomer",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<IList<Customer>> GetCustomers()
    {
      var customerMapper = new CustomerMapper();

      return (await SqlMapper.QueryAsync(
        _unitOfWork.Connection,
        "GetCustomers",
        new[]
        {
          typeof(Customer),
          typeof(Code),
          typeof(Code)
        },
        obj =>
        {
          var cust = obj[0] as Customer;
          var code = obj[1] as Code;
          var groupCode = obj[2] as Code;

          return customerMapper.Map(cust, code, groupCode);
        },
        splitOn: "CusCodeID,CusGroupCodeID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).ToList();
    }
  }
}
