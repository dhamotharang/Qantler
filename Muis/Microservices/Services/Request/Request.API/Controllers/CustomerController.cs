using Microsoft.AspNetCore.Mvc;
using Request.API.Models;
using Request.API.Services;
using Request.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CustomerController : Controller
  {
    readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
      _customerService = customerService;
    }

    [HttpGet]
    [Route("query")]
    public async Task<IEnumerable<Certificate360>> Query(Guid? id,
      string name = null,
      string code = null,
      string codeGroup = null,
      string certificateNo = null,
      string premise = null,
      long? premiseID = null,
      [FromQuery] CertificateStatus[] status = null)
    {
      return await _customerService.QueryCustomer(new CustomerOptions
      {
        ID = id,
        Name = name,
        Code = code,
        GroupCode = codeGroup,
        CertificateNo = certificateNo,
        Premise = premise,
        PremiseID = premiseID,
        Status = status
      });
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<Customer> GetByID(Guid id)
    {
      return await _customerService.GetByID(id);
    }

    [HttpPut]
    [Route("{id}/code")]
    public async Task<Customer> SetCode(Guid id, long? codeID = null)
    {
      return await _customerService.SetCode(id, codeID);
    }

    [HttpPut]
    [Route("{id}/groupCode")]
    public async Task<Customer> SetGroupCode(Guid id, long? codeID = null)
    {
      return await _customerService.SetGroupCode(id, codeID);
    }

    [HttpPut]
    [Route("{id}/officerInCharge")]
    public async Task<Customer> SetOfficer(Guid id, Guid? officerID = null)
    {
      return await _customerService.SetOfficer(id, officerID);
    }

    [HttpGet]
    [Route("{id}/request/recent")]
    public async Task<IEnumerable<Model.Request>> GetRecentRequest(Guid id)
    {
      return await _customerService.GetRecentRequest(id);
    }

    [HttpGet]
    [Route("list")]
    public async Task<IList<Model.Customer>> GetCustomers()
    {
      return await _customerService.GetCustomers();
    }

    [HttpPost]
    [Route("")]
    public async Task<Model.Customer> CreateCustomer(Model.Customer data)
    {
      return await _customerService.CreateCustomer(data);
    }
  }
}
