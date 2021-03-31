using Finance.API.Services;
using Finance.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finance.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TransactionCodeController : ControllerBase
  {
    readonly ITransactionCodeService _transactionCodeService;

    public TransactionCodeController(ITransactionCodeService transactionCodeService)
    {
      _transactionCodeService = transactionCodeService;
    }

    [HttpGet]
    [Route("query")]
    public async Task<IEnumerable<TransactionCode>> Query(
      long? id = null,
      string Code = null,
      string GLEntry = null,
      string Text = null,
      string Currency = null)
    {
      return await _transactionCodeService.QueryTransactionCode(new Models.TransactionCodeOptions
      {
        ID = id,
        Code = Code,
        GLEntry = GLEntry,
        Text = Text,
        Currency = Currency
      });
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<TransactionCode> Get(
      long id)
    {
      return await _transactionCodeService.GetTransactionCodeByID(id);
    }

    [HttpPut]
    public async Task<bool> Put([FromBody] IList<Price> priceList, Guid userID, string userName)
    {
      return await _transactionCodeService.UpdatePrice(priceList, userID, userName);
    }
  }
}
