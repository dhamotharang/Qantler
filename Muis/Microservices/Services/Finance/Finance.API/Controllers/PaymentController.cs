using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Model;
using Finance.API.DTO;
using Finance.API.Services;
using Finance.Model;
using Microsoft.AspNetCore.Mvc;

namespace Finance.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PaymentController : Controller
  {
    readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
      _paymentService = paymentService;
    }

    [HttpGet]
    [Route("list")]
    public async Task<IList<Payment>> List(long? id = null, Guid? customerId = null, string transactionNo = null,
      string name = null, PaymentStatus? status = null, PaymentMode? mode = null,
      PaymentMethod? method = null, DateTimeOffset? from = null, DateTimeOffset? to = null)
    {
      return await _paymentService.Filter(new PaymentFilter
      {
        ID = id,
        CustomerID = customerId,
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
    [Route("{id:int}")]
    public async Task<Payment> GetByID(long id)
    {
      return await _paymentService.GetPaymentByID(id);
    }

    [HttpGet]
    [Route("{id}/recent")]
    public async Task<IList<Payment>> GetCustomerRecentPayment(Guid id)
    {
      return await _paymentService.GetCustomerRecentPayment(id);
    }

    [HttpPost]
    [Route("{id:int}/action")]
    public async Task<Payment> PaymentAction(long id, PaymentStatus status, Bank bank, Guid userID,
      string userName)
    {
      return await _paymentService.PaymentAction(id, status, bank, new Officer
      {
        ID = userID,
        Name = userName
      });
    }

    [HttpPost]
    [Route("{id:int}/note")]
    public async Task<Note> AddNote(long id, [FromBody] Note note, Guid userID,
      string userName)
    {
      return await _paymentService.AddNote(id, note, new Officer
      {
        ID = userID,
        Name = userName
      });
    }

    [HttpPost]
    [Route("composition")]
    public async Task<Payment> PaymentForComposition([FromBody] PaymentForComposition param,
      Guid userID,
      string userName)
    {
      param.Officer = new Officer(userID, userName);

      return await _paymentService.PaymentForComposition(param);
    }
  }
}
