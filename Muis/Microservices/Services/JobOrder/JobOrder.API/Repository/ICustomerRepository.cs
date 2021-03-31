using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Repository
{
  public interface ICustomerRepository
  {
    /// <summary>
    /// Insert or replace customer.
    /// </summary>
    Task InsertOrReplace(Customer customer);

    /// <summary>
    /// Retrieve customer based on specified id.
    /// </summary>
    Task<Customer> GetCustomerByID(Guid id);
  }
}
