using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eHS.Portal.Infrastructure.Filters;
using eHS.Portal.Model;
using eHS.Portal.Models.Mufti;
using eHS.Portal.Services.Certificate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eHS.Portal.Controllers
{
  [Authorize]
  [ServiceFilter(typeof(SessionAwareFilter))]
  [PermissionFilter(Permission.MuftiRead,
    Permission.MuftiAcknowledge,
    Permission.MuftiCommentsReadWrite)]
  public class MuftiController : Controller
  {
    readonly ICertificateService _certificateService;

    public MuftiController(ICertificateService certificateService)
    {
      _certificateService = certificateService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
      var model = new IndexModel
      {
        Data = await _certificateService.GetCertificateBatch(new Client.CertificateBatchOptions
        {
          From = DateTime.UtcNow.AddMonths(-1),
          Status = new List<CertificateBatchStatus>
          {
            CertificateBatchStatus.Pending,
            CertificateBatchStatus.Acknowledged,
            CertificateBatchStatus.Downloaded,
            CertificateBatchStatus.SentToCourier,
            CertificateBatchStatus.Delivered
          }
        })
      };
      return View(model);
    }

    [HttpGet]
    [Route("[controller]/details/{id:int}")]
    public async Task<IActionResult> Details(long id)
    {
      var model = await _certificateService.GetCertificateBatchByID(id, true);
      return View(model);
    }

    [HttpGet]
    [Route("[controller]/details")]
    public async Task<IActionResult> PendingDetails()
    {
      var data = await _certificateService.GetCertificateBatch(new Client.CertificateBatchOptions
      {
        From = DateTime.UtcNow.AddMonths(-1),
        Status = new List<CertificateBatchStatus>
          {
            CertificateBatchStatus.Pending
          }
      }, true);

      return View("PendingDetails", data);
    }

    [HttpGet]
    [Route("api/[controller]/{id:int}/comments")]
    public async Task<IList<Comment>> GetComments(long id)
    {
      return await _certificateService.GetCertificateBatchComments(id);
    }

    [HttpPost]
    [Route("api/[controller]/{id:int}/comment")]
    [PermissionFilter(Permission.MuftiCommentsReadWrite)]
    public async Task<Comment> MuftiComment(long id, string text)
    {
      return await _certificateService.AddComment(id, text);
    }

    [HttpPost]
    [Route("api/[controller]/acknowledge")]
    [PermissionFilter(Permission.MuftiAcknowledge)]
    public async Task<string> Acknowledge([FromBody] long[] ids)
    {
      await _certificateService.AcknowledgeCertificateBatch(ids);

      return "Ok";
    }
  }
}
