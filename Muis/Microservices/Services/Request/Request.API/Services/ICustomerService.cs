using Request.API.Models;
using Request.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Services
{
  public interface ICustomerService
  {
    /// <summary>
    /// Set customer code.
    /// </summary>
    Task<Customer> SetCode(Guid customerID, long? codeID);

    /// <summary>
    /// Set customer group code.
    /// </summary>
    Task<Customer> SetGroupCode(Guid customerID, long? groupCodeID);

    /// <summary>
    /// Set customer officer incharge.
    /// </summary>
    /// <param name="customerID"></param>
    /// <param name="officerInCharge"></param>
    /// <returns></returns>
    Task<Customer> SetOfficer(Guid customerID, Guid? officerInCharge);
    /// <summary>
    /// Get customer by Id
    /// </summary>
    /// <param name="customerID"></param>
    /// <returns></returns>
    Task<Customer> GetByID(Guid customerID);

    /// <summary>
    /// Get customer with  specified param
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    Task<IList<Certificate360>> QueryCustomer(CustomerOptions filter);

    /// <summary>
    /// Get recent application for specified customer
    /// </summary>
    /// <param name="customerID"></param>
    /// <returns></returns>
    Task<IEnumerable<Model.Request>> GetRecentRequest(Guid customerID);

    /// <summary>
    /// Get Customers
    /// </summary>
    /// <returns></returns>
    Task<IList<Model.Customer>> GetCustomers();

    /// <summary>
    /// Insert Customers
    /// </summary>
    /// <returns></returns>
    Task<Model.Customer> CreateCustomer(Model.Customer data);
  }
}
