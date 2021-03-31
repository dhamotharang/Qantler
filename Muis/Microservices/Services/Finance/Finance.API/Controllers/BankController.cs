using Finance.API.Services;
using Finance.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BankController : ControllerBase
  {
    readonly IBankService _bankService;

    public BankController(IBankService bankService)
    {
      _bankService = bankService;
    }

    [HttpGet]
    [Route("list")]
    public async Task<IList<Model.Bank>> List(string accountName = null, string accountNo = null,
      string branchCode = null, string bankName = null, DDAStatus? ddaStatus = null)
    {
      return await _bankService.Filter(new BankFilter
      {
        AccountName = accountName,
        AccountNo = accountNo,
        BranchCode = branchCode,
        BankName = bankName,
        DDAStatus = ddaStatus
      });
    }
  }
}
