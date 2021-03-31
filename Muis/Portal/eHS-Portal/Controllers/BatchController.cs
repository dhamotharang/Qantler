using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Infrastructure.Filters;
using eHS.Portal.Model;
using eHS.Portal.Models.Batch;
using eHS.Portal.Services.Certificate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eHS.Portal.Controllers
{
  [Authorize]
  [ServiceFilter(typeof(SessionAwareFilter))]
  [PermissionFilter(Permission.RequestIssuance)]
  public class BatchController : Controller
  {
    readonly ICertificateService _certificateService;

    public BatchController(ICertificateService certificateService)
    {
      _certificateService = certificateService;
    }

    [HttpGet]
    public async Task<IActionResult> Certificate()
    {
      var data = await _certificateService.GetCertificateBatch(new Client.CertificateBatchOptions
      {
        From = DateTimeOffset.UtcNow.AddMonths(-1),
        Status = new List<CertificateBatchStatus>
        {
          CertificateBatchStatus.Acknowledged,
          CertificateBatchStatus.Downloaded,
          CertificateBatchStatus.SentToCourier,
          CertificateBatchStatus.Delivered
        }
      });

      return View(new BatchCertificateModel
      {
        Data = data
      });
    }
  }
}
