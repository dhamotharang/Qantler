using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Client;
using eHS.Portal.Infrastructure.Filters;
using eHS.Portal.Model;
using eHS.Portal.Services.Bill;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using eHS.Portal.Models.Invoice;
using eHS.Portal.Models.Bill;
using Core.Http.Exceptions;

namespace eHS.Portal.Controllers
{
  [Authorize]
  [ServiceFilter(typeof(SessionAwareFilter))]
  public class BillController : Controller
  {
    readonly IBillService _billService;

    public BillController(IBillService billService)
    {
      _billService = billService;
    }

    public async Task<IActionResult> Index()
    {
      return View(new IndexModel
      {
        Dataset = await List(status: BillStatus.Pending)
      });
    }

    [Route("api/[controller]/list")]
    public async Task<IList<Bill>> List(long? id = null, string invoiceNo = null,
      long? requestID = null, string refNo = null, string refID = null, string customerName = null,
      BillStatus? status = null, DateTimeOffset? from = null, DateTimeOffset? to = null, 
      BillType? type=null)
    {
      var data = await _billService.Filter(new BillFilter
      {
        ID = id,
        InvoiceNo = invoiceNo,
        RequestID = requestID,
        RefNo = refNo,
        RefID = refID,
        CustomerName = customerName,
        Status = status,
        From = from,
        To = to,
        Type=type
      });

      return data;
    }

    [Route("[controller]/details/{id}")]
    [ServiceFilter(typeof(SessionAwareFilter))]
    public async Task<IActionResult> Details(long id)
    {
      return View(new DetailsModel
      {
        ID = id,
        Data = await _billService.GetByID(id)
      });
    }

    [HttpGet]
    [Route("api/[controller]/{id}")]
    public async Task<Bill> GetByID(long id)
    {
      var result = await _billService.GetByID(id);
      if (result == null)
      {
        throw new NotFoundException();
      }
      return result;
    }
  }
}
