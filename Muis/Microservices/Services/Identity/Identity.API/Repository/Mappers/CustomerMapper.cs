using Identity.Model;
using System;
using System.Collections.Generic;

namespace Identity.API.Repository.Mappers
{
  public class CustomerMapper
  {
    readonly Dictionary<Guid, Customer> _customerCache
      = new Dictionary<Guid, Customer>();

    public Customer Map(Customer customer, Customer parent = null)
    {
      if (!_customerCache.TryGetValue(customer.ID, out Customer result))
      {
        _customerCache[customer.ID] = customer;
        result = customer;
        customer.Parent = parent;
      }

      return result;
    }
  }
}
