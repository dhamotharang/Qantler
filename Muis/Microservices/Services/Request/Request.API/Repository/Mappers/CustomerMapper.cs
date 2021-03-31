using System;
using Core.Model;
using Request.Model;

namespace Request.API.Repository.Mappers
{
  public class CustomerMapper
  {
    public Customer Map(Customer customer,
      Code code = null,
      Code groupCode = null,
      Officer officer = null)
    {
      customer.Code = code;
      customer.GroupCode = groupCode;
      customer.Officer = officer;
      return customer;
    }
  }
}
