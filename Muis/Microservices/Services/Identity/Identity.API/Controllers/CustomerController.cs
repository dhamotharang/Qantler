using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Identity.API.Services;
using Identity.Model;
using System.Collections.Generic;

namespace Identity.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CustomerController : ControllerBase
  {
    readonly ICustomerService _customerservice;

    public CustomerController(ICustomerService customerservice)
    {
      _customerservice = customerservice;
    }

    [HttpGet]
    [Route("list")]
    public async Task<IList<Customer>> List()
    {
      return await _customerservice.Filter(new CustomerFilter());
    }

    [HttpGet]
    [Route("find")]
    public async Task<Customer> Get(string name, string altID)
    {
      return await _customerservice.GetCustomerContactInfo(name, altID);
    }

    [HttpPost]
    public async Task<Customer> Post([FromBody] Customer customer)
    {
      return await _customerservice.CreateCustomer(customer);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<Customer> Get(Guid id)
    {
      return await _customerservice.GetCustomerByID(id);
    }

    [HttpPut]
    [Route("{id}/parent/{parentID}")]
    public async Task<Customer> SetParent(Guid id, Guid parentID)
    {
      return await _customerservice.SetParent(id, parentID);
    }
  }
}