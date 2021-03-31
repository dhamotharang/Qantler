using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eHS.Portal.Client;
using eHS.Portal.Infrastructure.Filters;
using eHS.Portal.Model;
using eHS.Portal.Models.Customer;
using eHS.Portal.Services.Customer;
using eHS.Portal.Services.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eHS.Portal.Controllers
{
  [Authorize]
  public class CustomerController : Controller
  {
    readonly ICustomerService _customerService;

    readonly IPaymentService _paymentService;

    public CustomerController(ICustomerService customerService, IPaymentService paymentService)
    {
      _customerService = customerService;

      _paymentService = paymentService;
    }

    [HttpGet]
    [PermissionFilter(Permission.CustomerRead,
      Permission.CustomerReadWrite)]   
    public IActionResult Index()
    {
      var model = new IndexModel
      {
        DefaultStatuses = new List<CertificateStatus>
        {
          CertificateStatus.Active
        },
      };

      return View(model);
    }

    [Route("api/[controller]/index")]
    public async Task<IList<Certificate360>> IndexData(string name = null,
      string code = null,
      string groupCode = null,
      string certificateNo = null,
      string premise = null,
      long? premiseID = null,
      [FromQuery] IList<CertificateStatus> status = null)
    {

      var result = await _customerService.GetCustomers(new CustomerOptions
      {
        Name = name,
        Code = code,
        GroupCode = groupCode,
        CertificateNo = certificateNo,
        Premise = premise,
        PremiseID = premiseID,
        Status = (status != null && status.Count > 0) ? status : null,
      });

      return result;
    }

    [Route("[controller]/details/{id}")]
    public async Task<IActionResult> Details(Guid id)
    {
      return View(new DetailsModel
      {
        ID = id,
        Data = await _customerService.GetByID(id)
      });
    }

    [HttpGet]
    [Route("api/[controller]/list")]
    public async Task<IList<Customer>> List()
    {
      return await _customerService.List();
    }

    [HttpGet]
    [Route("api/[controller]/{id}/request/recent")]
    public async Task<IList<Request>> GetRecentRequest(Guid id)
    {
      return await _customerService.GetCustomerRecentRequest(id);
    }

    [HttpGet]
    [Route("api/[controller]/{id}/premise")]
    public async Task<IList<Certificate360>> GetPremises(Guid id)
    {
      return await _customerService.GetCustomers(new CustomerOptions
      {
        ID = id,
        Status = null
      });
    }

    [HttpGet]
    [Route("api/[controller]/{id}/payment/recent")]
    public async Task<IList<Payment>> GetRecentPayment(Guid id)
    {
      return await _paymentService.GetCustomerRecentPayment(id);
    }

    [HttpPut]
    [Route("api/[controller]/{id}/code")]
    public async Task<Customer> UpdateCode(Guid id, [FromBody] Code code)
    {
      return await _customerService.UpdateCode(id, code);
    }

    [HttpPut]
    [Route("api/[controller]/{id}/officer/{officerID}")]
    public async Task<Customer> SetOfficerInCharge(Guid id, Guid officerID)
    {
      return await _customerService.SetOfficerInCharge(id, officerID);
    }

    [HttpPut]
    [Route("api/[controller]/{id}/parent/{parentID}")]
    public async Task<Customer> SetParent(Guid id, Guid parentID)
    {
      return await _customerService.SetParent(id, parentID);
    }

    [HttpPost]
    [Route("api/[controller]")]
    public async Task<Customer> CreateCustomer([FromBody] Customer customer)
    {
      return await _customerService.CreateCustomer(customer);
    }
  }
}
