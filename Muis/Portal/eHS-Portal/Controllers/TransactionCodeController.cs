using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Services.TransactionCode;
using Microsoft.AspNetCore.Mvc;
using eHS.Portal.Models.TransactionCode;
using eHS.Portal.Model;
using System;
using Microsoft.AspNetCore.Authorization;
using eHS.Portal.Infrastructure.Filters;

namespace eHS.Portal.Controllers
{
  [Authorize]
  [ServiceFilter(typeof(SessionAwareFilter))]
  public class TransactionCodeController : Controller
  {
    readonly ITransactionCodeService _transactionCodeService;

    public TransactionCodeController(ITransactionCodeService transactionCodeService)
    {
      _transactionCodeService = transactionCodeService;
    }

    [Route("api/[controller]/index")]
    [PermissionFilter(Permission.TransactionCodeRead,
      Permission.TransactionCodeReadWrite)]
    public async Task<IList<TransactionCode>> IndexData(long id = 0,
      string code = null,
      string glentry = null,
      string description = null)
    {
      var options = new TransactionCode
      {
        ID = id,
        Code = code,
        GLEntry = glentry,
        Text = description
      };

      return await _transactionCodeService.Search(options);
    }

    [Route("[controller]")]
    [PermissionFilter(Permission.TransactionCodeRead,
      Permission.TransactionCodeReadWrite)]
    public async Task<IActionResult> TransactionCode()
    {
      await Task.CompletedTask;
      return View(new TransactionCodeModel
      {
      });
    }

    [Route("[controller]/details/{id}")]
    [PermissionFilter(Permission.TransactionCodeRead,
      Permission.TransactionCodeReadWrite)]
    public async Task<IActionResult> Details(long id)
    {
      var TDetails = await _transactionCodeService.GetByID(id);
      string condition = "";
      if (TDetails.Conditions != null)
      {
        for (int i = 0; i < TDetails.Conditions.Count; i++)
        {
          if (i > 0)
          {
            condition += " " + Enum.GetName(typeof(Logical), TDetails.Conditions[i].Logical) + " ";
          }
          condition += Enum.GetName(typeof(FieldType), TDetails.Conditions[i].Field) + " " +
            Enum.GetName(typeof(Operator), TDetails.Conditions[i].Operator) + " " +
            TDetails.Conditions[i].Value;
        }
      }
      return View(new DetailsModel
      {
        ID = id,
        Data = TDetails,
        Cond = condition
      });
    }

    [HttpPut]
    [Route("api/[controller]")]
    [PermissionFilter(Permission.TransactionCodeReadWrite)]
    public async Task<bool> Put([FromBody] List<Price> priceDetails)
    {
      return await _transactionCodeService.UpdatePrice(priceDetails);
    }

    [HttpGet]
    [Route("api/[controller]/list")]
    public async Task<IList<TransactionCode>> List()
    {
      return await _transactionCodeService.Search(new TransactionCode());
    }
  }
}
