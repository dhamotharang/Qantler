using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eHS.Portal.Client;
using eHS.Portal.Infrastructure.Filters;
using eHS.Portal.Helpers;
using eHS.Portal.Model;
using eHS.Portal.Models.Certificates;
using eHS.Portal.Services.Certificate;
using IronPdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using eHS.Portal.Services.Certificate360;
using System.Collections.Generic;

namespace eHS.Portal.Controllers
{
  public class CertificateController : Controller
  {
    readonly ApiClient _apiClient;
    readonly ICertificateService _certificateService;
    readonly ICertificate360Service _certificate360Service;

    public CertificateController(ApiClient apiClient,
      ICertificateService certificateService,
      ICertificate360Service certificate360Service)
    {
      _apiClient = apiClient;
      _certificateService = certificateService;
      _certificate360Service = certificate360Service;
    }

    [HttpGet]
    [Route("[controller]/batch/preview/{id:int}")]
    public async Task<IActionResult> CertificatePreview(long id, bool isPreview)
    {
      var batch = await _certificateService.GetCertificateBatchByID(id, true);

      var data = batch.Certificates.OrderBy(e => e.SerialNo)
        .Select(e => CertificateHelper.Convert(e))
        .ToList();

      return View("Certificate", new CertificateModel
      {
        IsPreview = isPreview,
        Title = batch.TemplateText,
        Certificates = data
      });
    }

    [HttpGet]
    [Route("api/[controller]/preview")]
    public async Task<CertificateModel> CertificatePreviewApi(bool isPreview,
      [FromQuery] long[] ids)
    {
      var certificates = await _certificateService.GetCertificates(new CertificateOptions
      {
        IDs = ids
      });

      var data = certificates.OrderBy(e => e.SerialNo)
        .Select(e => CertificateHelper.Convert(e))
        .ToList();

      return new CertificateModel
      {
        IsPreview = isPreview,
        Certificates = data
      };
    }

    [HttpGet]
    [Route("[controller]/details/{id:int}")]
    public async Task<IActionResult> Details(long id)
    {
      return View(new DetailsModel
      {
        Data = await _certificate360Service.GetByID(id)
      });
    }

    [HttpGet]
    [Route("api/[controller]/{id:int}/history")]
    public async Task<IList<Certificate360History>> GetHistoryByID(long id)
    {
      return await _certificate360Service.GetHistoryByID(id);
    }

    [HttpGet]
    [Route("api/[controller]/{id:int}/menu")]
    public async Task<IList<Menu>> GetMenuByID(long id)
    {
      return await _certificate360Service.GetMenuByID(id);
    }

    [HttpGet]
    [Route("api/[controller]/{id:int}/ingredient")]
    public async Task<IList<Ingredient>> GetIngredientByID(long id)
    {
      return await _certificate360Service.GetIngredientByID(id);
    }

    [HttpGet]
    [Route("api/[controller]/with-ingredient")]
    public async Task<IList<Certificate360>> GetIngredientByID(string name = null,
      string brand = null,
      string supplier = null,
      string certifyingBody = null)
    {
      var result = await _certificate360Service.GetWithIngredient(
        new Certificate360IngredientOptions
        {
          Name = name,
          BrandName = brand,
          SupplierName = supplier,
          CertifyingBodyName = certifyingBody
        });
      return result;
    }

    [HttpGet]
    [Route("api/[controller]/batch/preview/{id:int}")]
    public async Task<CertificateModel> CertificatePreviewApi(long id, bool isPreview)
    {
      var batch = await _certificateService.GetCertificateBatchByID(id, true);

      var data = batch.Certificates.OrderBy(e => e.SerialNo)
        .Select(e => CertificateHelper.Convert(e))
        .ToList();

      return new CertificateModel
      {
        IsPreview = isPreview,
        Title = batch.TemplateText,
        Certificates = data
      };
    }

    [HttpGet]
    [Route("[controller]/batch/preview/{id:int}/pdf")]
    public async Task<IActionResult> CertificatePdf(long id)
    {
      var stream = await Task.Run(() => GenerateCertificateBatchPdf(id));
      return File(stream, "application/pdf");
    }

    [HttpGet]
    [Route("api/[controller]/batch/preview/{id:int}/generate/pdf")]
    public async Task<Model.File> CertificateGeneratePdf(long id)
    {
      var batch = await _certificateService.GetCertificateBatchByID(id, true);

      var stream = GenerateCertificateBatchPdf(id);

      var result = await _apiClient.FileSdk.Upload(new Core.Http.FileContent
      {
        FileName = $"{batch.Code}.pdf",
        Steam = stream
      });

      await _certificateService.MapFileToCertificateBatch(id, result[0].ID);

      if (batch.Status == CertificateBatchStatus.Acknowledged)
      {
        await _certificateService.UpdateCertificateBatchStatus(id,
          CertificateBatchStatus.Downloaded);
      }

      return result[0];
    }

    [HttpPut]
    [Route("api/[controller]/batch/{id:int}/status")]
    [Authorize]
    [PermissionFilter(Permission.RequestIssuance)]
    public async Task<CertificateBatch> UpdateCertificateBatchStatus(long id,
      CertificateBatchStatus status)
    {
      await _certificateService.UpdateCertificateBatchStatus(id, status);

      return await _certificateService.GetCertificateBatchByID(id, false);
    }

    Stream GenerateCertificateBatchPdf(long batchID)
    {
      var ironPdfRender = new HtmlToPdf();
      ironPdfRender.PrintOptions.MarginLeft = 0;
      ironPdfRender.PrintOptions.MarginTop = 2;
      ironPdfRender.PrintOptions.MarginRight = 0;
      ironPdfRender.PrintOptions.MarginBottom = 0;
      ironPdfRender.PrintOptions.EnableJavaScript = true;
      ironPdfRender.PrintOptions.PaperSize = PdfPrintOptions.PdfPaperSize.Custom;
      ironPdfRender.PrintOptions.InputEncoding = Encoding.UTF8;
      ironPdfRender.PrintOptions.RenderDelay = 500;

      var pdfDoc = ironPdfRender.RenderUrlAsPdf(
        $"{Request.Scheme}://{Request.Host}/Certificate/batch/preview/{batchID}");

      var headerFooter = new HtmlHeaderFooter();
      headerFooter.Height = 0;
      headerFooter.Spacing = 0;
      pdfDoc.AddHTMLFooters(headerFooter, 0, 0, 0);
      pdfDoc.AddHTMLHeaders(headerFooter, 0, 0, 0);

      return pdfDoc.Stream;
    }
  }
}
