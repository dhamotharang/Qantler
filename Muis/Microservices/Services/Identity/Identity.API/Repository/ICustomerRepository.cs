using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Identity.API.Services;
using Identity.Model;

namespace Identity.API.Repository
{
  public interface ICustomerRepository
  {
    Task<IList<Customer>> Select(CustomerFilter filter);

    /// Get Contact Info for a Customer.
    /// </summary>
    /// <returns>The contact info for a customer</returns>
    /// <param name="name">Customer Nmae</param>
    /// <param name="name">Customer AltID</param>
    Task<Model.Customer> GetCustomerContactInfo(string name, string altID);

    /// <summary>
    /// Insert Customer.
    /// </summary>
    public Task InsertCustomer(Customer customer);

    /// Get Customer data by Customer AltID.
    /// </summary>
    /// <returns>customer Details</returns>
    /// <param name="AltID">Customer AltID</param>

    public bool ValidateCustomerStatus(string AltID, string Name);

    /// Get Details for a Customer.
    /// </summary>
    /// <returns>The Details for a customer</returns>
    /// <param name="name">Customer ID</param>
    Task<Customer> GetCustomerByID(Guid ID);

    /// <summary>
    /// Updates customer.
    /// </summary>
    Task UpdateCustomer(Customer customer);
  }
}
