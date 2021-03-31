using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Identity.Model;

namespace Identity.API.Services
{
  public interface ICustomerService
  {
    /// <summary>
    /// Filter customers.
    /// </summary>
    Task<IList<Customer>> Filter(CustomerFilter filter);

    /// Get Contact Info for a Customer.
    /// </summary>
    /// <returns>The contact info for a customer</returns>
    /// <param name="name">Customer Nmae</param>
    /// <param name="name">Customer AltID</param>
    Task<Customer> GetCustomerContactInfo(string name, string altID);

    /// insert  Customer data.
    /// </summary>
    /// <returns>Customer Model </returns>
    /// <param name="customer">customer</param>
    Task<Customer> CreateCustomer(Model.Customer customer);

    /// Get customer by id.
    /// </summary>
    /// <returns>Customer Model </returns>
    /// <param name="id">customer id</param>
    Task<Customer> GetCustomerByID(Guid id);

    /// <summary>
    /// Set customer parent.
    /// </summary>
    Task<Customer> SetParent(Guid id, Guid parentID);
  }

  public class CustomerFilter
  {
  }
}
