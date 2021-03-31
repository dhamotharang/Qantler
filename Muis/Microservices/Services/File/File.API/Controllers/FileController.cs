using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using File.API.Services;
using System.IO;
using HeyRed.Mime;

namespace File.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FileController : ControllerBase
  {
    readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
      _fileService = fileService;
    }

    [HttpGet("{guid}")]
    public async Task<IActionResult> Get(Guid guid)
    {
      var file = await _fileService.Get(guid);

      if (file == null)
      {
        return NotFound();
      }

      return File(await _fileService.Download(guid),
                  MimeTypesMap.GetMimeType(file.FileName));
    }

    [HttpPost]
    [RequestSizeLimit(5000000)]
    public async Task<IList<Model.File>> Post()
    {
      var response = new List<Model.File>();

      if (Request.HasFormContentType)
      {
        var form = Request.Form;

        foreach (var formFile in form.Files)
        {
          var tmpFile = $"{Path.GetTempPath()}{Path.GetRandomFileName()}";

          using (var stream = new FileStream(tmpFile, FileMode.Create))
          {
            formFile.CopyTo(stream);
          }

          var fileInfo = new FileInfo(tmpFile);
          var extension = formFile.FileName.Contains('.')
                        ? formFile.FileName.Substring(formFile.FileName.IndexOf('.'))
                        : "";

          var model = await _fileService.Upload("uploads", tmpFile, extension, fileInfo.Length,
            formFile.FileName);

          response.Add(new Model.File
          {
            ID = model.ID,
            FileName = model.FileName,
            Directory = model.Directory,
            Extension = model.Extension,
            Size = model.Size,
            CreatedOn = model.CreatedOn
          });
        }
      }
      return response;
    }

    [HttpDelete("{guid}")]
    public async Task<string> Delete(Guid guid)
    {
      await _fileService.Delete(guid);
      return "Ok";
    }
  }
}