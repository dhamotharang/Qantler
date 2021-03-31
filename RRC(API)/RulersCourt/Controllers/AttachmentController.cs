using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Web;

namespace RulersCourt.Controllers
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class AttachmentController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;
        private readonly EncryptionService encryptionService;
        private readonly string aesEncryptedFileExtension = ".aes";

        public AttachmentController(IOptions<ConnectionSettingsModel> app, IHostingEnvironment env, EncryptionService encryptionSvc)
            : base(app, env)
        {
            appSettings = app;
            environment = env;
            encryptionService = encryptionSvc;
        }

        [Route("attachment/upload")]
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult UploadFilev2()
        {
            try
            {
                var files = Request.Form.Files;
                var fileNames = new List<string>();
                string uploadDir = string.Empty;
                if (environment.IsDevelopment())
                {
                    uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                }
                else
                {
                    uploadDir = Path.Combine(environment.ContentRootPath, "Uploads");
                }

                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);
                string guid = Guid.NewGuid().ToString("N");
                if (files.Count > 0)
                {
                    if (!Directory.Exists(Path.Combine(uploadDir, guid))) { Directory.CreateDirectory(Path.Combine(uploadDir, guid)); }
                    foreach (IFormFile file in files)
                    {
                        string encFileName = file.FileName + aesEncryptedFileExtension;
                        var encPath = Path.Combine(uploadDir, guid, encFileName);
                        var encStream = encryptionService.Encrypt(file.OpenReadStream());
                        using (var stream = new FileStream(encPath, FileMode.Create))
                        {
                            encStream.Seek(0, SeekOrigin.Begin);
                            encStream.CopyTo(stream);
                            encStream.Close();
                        }

                        fileNames.Add(file.FileName);
                    }
                }

                if (fileNames.Count > 0)
                {
                    var jsonObj = "{ \"" + guid + "\" : { \"file\" : \"" + string.Join(',', fileNames.ToArray()) + "\" } }";
                    var response = new AttachmentuploadResponseModel();
                    response.Guid = guid;
                    response.FileName = fileNames;
                    return Ok(response);
                }
                else
                {
                    return BadRequest("Error");
                }
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("attachment/download")]
        public async Task<IActionResult> Downloadv2(string filename)
        {
            try
            {
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var guidValue = parameters["guid"];
                if (filename == null)
                    return Content("filename not present");
                string[] arrfile = filename.Split('|');
                string uploadDir = string.Empty;
                if (environment.IsDevelopment())
                {
                    uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                }
                else
                {
                    uploadDir = Path.Combine(environment.ContentRootPath, "Uploads");
                }

                if (arrfile.Length <= 1)
                {
                    var path = Path.Combine(uploadDir, guidValue, filename);
                    var encFilePath = Path.Combine(uploadDir, guidValue, filename + aesEncryptedFileExtension);
                    if (System.IO.File.Exists(encFilePath))
                    {
                        var stream = new FileStream(encFilePath, FileMode.Open);
                        var decStream = encryptionService.Decrypt(stream);
                        decStream.Position = 0;
                        return File(decStream, GetContentType(path), Path.GetFileName(path));
                    }
                    else
                    {
                        var stream = new FileStream(path, FileMode.Open);
                        stream.Position = 0;
                        return File(stream, GetContentType(path), Path.GetFileName(path));
                    }
                }
                else
                {
                    var zipFilename = guidValue + ".zip";
                    var zipFile = Path.Combine(uploadDir, zipFilename);

                    var encFiles = Directory.GetFiles(Path.Combine(uploadDir, guidValue), "*.aes");
                    if (encFiles.Length > 0)
                    {
                        var decryptedFolder = Path.Combine(uploadDir, guidValue, "decrypted");
                        if (!Directory.Exists(decryptedFolder)) { Directory.CreateDirectory(decryptedFolder); }

                        foreach (var encFile in encFiles)
                        {
                            var stream = new FileStream(encFile, FileMode.Open);
                            var decStream = encryptionService.Decrypt(stream);
                            decStream.Position = 0;
                            using (Stream file = System.IO.File.Create(Path.Combine(decryptedFolder, Path.GetFileNameWithoutExtension(encFile))))
                            {
                                decStream.CopyTo(file);
                            }
                        }

                        ZipFile.CreateFromDirectory(decryptedFolder, zipFile);

                        try
                        {
                            if (Directory.Exists(decryptedFolder))
                                Directory.Delete(decryptedFolder, true);
                        }
                        catch { }
                    }
                    else
                    {
                        ZipFile.CreateFromDirectory(Path.Combine(uploadDir, guidValue), zipFile);
                    }

                    var memory = new MemoryStream();
                    using (var stream = new FileStream(zipFile, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }

                    memory.Position = 0;
                    if (System.IO.File.Exists(zipFile))
                        System.IO.File.Delete(zipFile);
                    return File(memory, GetContentType(zipFile), Path.GetFileName(zipFile));
                }
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }
        }

        [HttpGet]
        [Route("files/delete")]
        public IActionResult DeleteFiles(string filename)
        {
            try
            {
                string downloadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");
                if (Directory.Exists(downloadFolder))
                {
                    var fileToDelete = Path.Combine(downloadFolder, filename);
                    new FileInfo(fileToDelete).Delete();

                    string[] files = Directory.GetFiles(downloadFolder);
                    foreach (string file in files)
                    {
                        FileInfo fi = new FileInfo(file);
                        if (fi.LastAccessTime < DateTime.Now.AddMinutes(-30))
                            fi.Delete();
                    }
                }

                return Ok("Success");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }
        }
    }
}