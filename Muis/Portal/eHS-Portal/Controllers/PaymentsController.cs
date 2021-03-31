using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eHS.Portal.Client;
using eHS.Portal.Infrastructure.Filters;
using eHS.Portal.Model;
using eHS.Portal.Models.Payments;
using eHS.Portal.Services;
using eHS.Portal.Services.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eHS.Portal.Controllers
{
  [Authorize]
  [ServiceFilter(typeof(SessionAwareFilter))]
  [PermissionFilter(Permission.PaymentRead,
    Permission.PaymentReadWrite)]
  public class PaymentsController : Controller
  {
    readonly IPaymentService _paymentService;

    readonly IRequestService _requestService;

    public PaymentsController(IPaymentService paymentService, IRequestService requestService)
    {
      _paymentService = paymentService;

      _requestService = requestService;
    }

    public async Task<IActionResult> Index(Guid? customerID = null)
    {
      PaymentStatus? defaultStatus = null;

      Guid? defaultCustomerID = null;

      if (customerID != null)
      {
        defaultCustomerID = customerID;
      }
      else
      {
        defaultStatus = PaymentStatus.Pending;
      }

      return View(new IndexModel
      {
        DefaultStatus = defaultStatus,
        Dataset = await (IndexList(customerID: defaultCustomerID, status: defaultStatus))
      });
    }

    [HttpGet]
    [Route("api/[controller]/index/list")]
    public async Task<IList<Payment>> IndexList(long? id = null,
      Guid ? customerID = null,
      string transactionNo = null,
      string name = null, 
      PaymentStatus? status = null, 
      PaymentMode? mode = null,
      PaymentMethod? method = null, 
      DateTimeOffset? from = null, 
      DateTimeOffset? to = null)
    {
      return await _paymentService.List(new PaymentFilter
      {
        ID = id,
        CustomerID = customerID,
        Name = name,
        TransactionNo = transactionNo,
        Status = status,
        Mode = mode,
        Method = method,
        From = from,
        To = to
      });
    }

    [HttpGet]
    [Route("api/[controller]/{id:int}")]
    public async Task<Payment> GetPaymentByID(long id)
    {
      return await _paymentService.GetPaymentByID(id);
    }

    [HttpGet]
    [Route("api/[controller]/{id}/recent")]
    public async Task<IList<Payment>> GetCustomerRecentPayment(Guid id)
    {
      return await _paymentService.GetCustomerRecentPayment(id);
    }

    [HttpPost]
    [Route("api/[controller]/{id:int}/action")]
    [PermissionFilter(Permission.PaymentReadWrite)]
    public async Task<Payment> PaymentAction(long id, PaymentStatus status, 
      [FromBody] Model.Bank bank)
    {
      return await _paymentService.PaymentAction(id, status, bank);
    }

    [Route("[controller]/details/{id}")]
    public async Task<IActionResult> Details(long id)
    {
      var payment = await _paymentService.GetPaymentByID(id);

      var bill = payment?.Bills?.FirstOrDefault();
      if (bill?.RequestID != null)
      {
        var request = await _requestService.GetByIDBasic(bill.RequestID.Value);
        payment.ContactPerson = request.Agent ?? request.Requestor;
      }

      return View(new DetailsModel
      {
        ID = id,
        Data = payment
      });
    }

    [HttpPost]
    [Route("api/[controller]/{id:int}/note")]
    public async Task<Notes> AddNote(long id, [FromBody] Notes note)
    {
      return await _paymentService.AddNote(id, note);
    }
  }
}
