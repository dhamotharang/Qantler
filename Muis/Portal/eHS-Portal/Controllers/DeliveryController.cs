using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eHS.Portal.Client;
using eHS.Portal.Infrastructure.Filters;
using eHS.Portal.Helpers;
using eHS.Portal.Model;
using eHS.Portal.Models.Delivery;
using eHS.Portal.Services.Certificate;
using IronPdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace eHS.Portal.Controllers
{
  public class DeliveryController : Controller
  {
    readonly ApiClient _apiClient;
    readonly ICertificateService _certificateService;

    public DeliveryController(ApiClient apiClient, ICertificateService certificateService)
    {
      _apiClient = apiClient;
      _certificateService = certificateService;
    }


    [HttpGet]
    public IActionResult Index()
    {
      var model = new IndexModel
      {
        DefaultStatuses = new List<CertificateDeliveryStatus>
        {
          CertificateDeliveryStatus.Pending
        },
      };
      return View(model);
    }

    [Route("api/[controller]/index")]
    public async Task<IList<Certificate>> IndexData(string customerCode = null,
      string customerName = null,
      string premise = null,
      string postal = null,
      string serialNo = null,
      DateTimeOffset? issuedOnFrom = null,
      DateTimeOffset? issuedOnTo = null,
      [FromQuery] IList<CertificateDeliveryStatus> status = null)
    {

      var result = await _certificateService.CertDeliveryFilter(new CertificateDeliveryOptions
      {
        CustomerCode = customerCode,
        CustomerName = customerName,
        Premise = premise,
        Postal = postal,
        SerialNo = serialNo,
        IssuedOnFrom = issuedOnFrom,
        IssuedOnTo = issuedOnTo,
        Status = (status != null && status.Count > 0) ? status : new List<CertificateDeliveryStatus>
        {
          CertificateDeliveryStatus.Pending
        },
      });
      return result;
    }


    [HttpPost]
    [Route("api/[controller]/deliver")]
    public async Task<string> Deliver([FromBody] long[] IDs)
    {
      await _certificateService.ExecuteCertDelivery(IDs);
      return "Ok";
    }
  }
}
