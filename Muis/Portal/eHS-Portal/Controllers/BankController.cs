using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eHS.Portal.Client;
using eHS.Portal.Infrastructure.Filters;
using eHS.Portal.Model;
using eHS.Portal.Models.Bank;
using Microsoft.AspNetCore.Authorization;
using eHS.Portal.Services.Bank;
using eHS.Portal.Services.Master;

namespace eHS.Portal.Controllers
{
  [Authorize]
  [ServiceFilter(typeof(SessionAwareFilter))]
  [PermissionFilter(Permission.PaymentRead,
    Permission.PaymentReadWrite)]
  public class BankController : Controller
  {
    readonly IBankService _bankService;

    readonly IMasterService _masterService;
    public BankController(IBankService bankService, IMasterService masterService)
    {
      _bankService = bankService;

      _masterService = masterService;
    }

    public async Task<IActionResult> Index()
    {
      return View(new IndexModel
      {
        Dataset = await IndexList(),
        Banks = await _masterService.GetMasterList(MasterType.Bank)
      });
    }

    [HttpGet]
    [Route("api/[controller]/index/list")]
    public async Task<IList<Bank>> IndexList(string accountName = null, string accountNo = null,
      string branchCode = null, string bankName = null, DDAStatus? status = null)
    {
      return await _bankService.Filter(new BankFilter
      {
        AccountName = accountName,
        AccountNo = accountNo,
        BranchCode = branchCode,
        BankName = bankName,
        DDAStatus = status
      });
    }
  }
}
