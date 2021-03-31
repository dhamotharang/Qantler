using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finance.API.Services;
using Finance.Model;
using Microsoft.AspNetCore.Mvc;

namespace Finance.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BillController : Controller
  {
    readonly IBillService _billService;

    public BillController(IBillService billService)
    {
      _billService = billService;
    }

    [Route("list")]
    public async Task<IList<Bill>> List(long? id = null, long? requestID = null,
      string refNo = null, string refID = null, string customerName = null, BillStatus? status = null,
      DateTimeOffset? from = null, DateTimeOffset? to = null, string invoiceNo = null,
      BillType? type = null)
    {
      return await _billService.Filter(new BillFilter
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
        Type = type
      });
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<string> Update(long id, [FromBody] Bill bill)
    {
      bill.ID = id;
      await _billService.UpdateBill(bill);
      return "Ok";
    }

    [HttpPost]
    [Route("{id:int}/lineItem")]
    public async Task<string> AddLineItem(long id, [FromBody] IList<BillLineItem> lineItems)
    {
      await _billService.AddLineItem(id, lineItems);
      return "Ok";
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<Bill> GetByID(long id)
    {
      return await _billService.GetBillByID(id);
    }
  }
}
