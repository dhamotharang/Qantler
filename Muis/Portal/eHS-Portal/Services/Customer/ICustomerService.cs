using eHS.Portal.Client;
using eHS.Portal.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Services.Customer
{
  public interface ICustomerService
  {
    /// <summary>
    /// List of customers.
    /// </summary>
    /// <returns></returns>
    Task<IList<Model.Customer>> List();

    /// <summary>
    /// Updates customer code.
    /// </summary>
    Task<Model.Customer> UpdateCode(Guid id, Model.Code code);

    /// <summary>
    /// Get customer by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Model.Customer> GetByID(Guid id);

    /// <summary>
    /// Set customer parent.
    /// </summary>
    Task<Model.Customer> SetParent(Guid id, Guid parentID);

    /// <summary>
    /// Set customer office incharge
    /// </summary>
    /// <param name="id"></param>
    /// <param name="officerID"></param>
    /// <returns></returns>
    Task<Model.Customer> SetOfficerInCharge(Guid id, Guid officerID);

    /// <summary>
    /// Create customer.
    /// </summary>
    Task<Model.Customer> CreateCustomer(Model.Customer customer);

    /// <summary>
    /// Get customer with  specified param
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    Task<IList<Model.Certificate360>> GetCustomers(CustomerOptions options);

    /// <summary>
    ///  Get recent application for specified customer
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IList<Request>> GetCustomerRecentRequest(Guid id);
  }
}
