using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Request.API.Models;
using Request.Model;

namespace Request.API.Repository
{
  public interface ICustomerRepository
  {
    /// <summary>
    /// Retrieve customer with specified ID.
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    Task<Customer> GetByID(Guid ID);

    /// <summary>
    /// Get recent application for specified customer
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public Task<IEnumerable<Model.Request>> GetRecentRequest(Guid ID,
      long? rowFrom = 0,
      long? rowCount = 10);

    /// <summary>
    /// Get customer with  specified param
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    Task<IList<Certificate360>> QueryCustomer(CustomerOptions filter);

    /// <summary>
    /// insert customer to Request DB
    /// </summary>
    /// <param name="customer"></param>
    /// <returns></returns>
    Task InsertCustomer(Customer customer);

    /// <summary>
    /// Updates customer.
    /// </summary>
    Task UpdateCustomer(Customer customer);

    /// <summary>
    /// Get customers.
    /// </summary>
    Task<IList<Customer>> GetCustomers();
  }
}
