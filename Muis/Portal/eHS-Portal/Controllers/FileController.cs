using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using eHS.Portal.Client;
using eHS.Portal.Model;
using HeyRed.Mime;
using Microsoft.AspNetCore.Mvc;

namespace eHS.Portal.Controllers
{
  public class FileController : Controller
  {
    readonly ApiClient _apiClient;

    public FileController(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    [HttpGet]
    [Route("api/[controller]/{guid}")]
    public async Task<IActionResult> Get(Guid guid, string fileName)
    {
      var file = await _apiClient.FileSdk.Download(guid);

      return File(new MemoryStream(file),
                  MimeTypesMap.GetMimeType(fileName));
    }

    [HttpPost]
    [RequestSizeLimit(5000000)]
    [Route("api/[controller]")]
    public async Task<IList<Attachment>> PostAsync()
    {
      var response = new List<Attachment>();

      if (Request.HasFormContentType)
      {
        var form = Request.Form;

        foreach (var formFile in form.Files)
        {
          using (var stream = new MemoryStream())
          {
            formFile.CopyTo(stream);

            stream.Seek(0, SeekOrigin.Begin);

            var result = await _apiClient.FileSdk.Upload(new Core.Http.FileContent
            {
              FileName = formFile.FileName,
              Steam = stream
            });

            response.Add(new Attachment
            {
              FileID = result[0].ID,
              FileName = result[0].FileName,
              Extension = result[0].Extension,
              Size = result[0].Size,
            });
          }
        }
      }
      return response;
    }

    [HttpDelete]
    [Route("api/[controller]/{guid}")]
    public async Task<string> Delete(Guid guid)
    {
      await Task.CompletedTask;
      return "success";
    }
  }
}
