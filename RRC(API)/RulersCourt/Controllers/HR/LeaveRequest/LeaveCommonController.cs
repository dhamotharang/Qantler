using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using RulersCourt.Models;
using RulersCourt.Models.LeaveRequest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RulersCourt.Controllers.LeaveRequest
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LeaveCommonController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;

        public LeaveCommonController(IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            appSettings = app;
            environment = env;
        }

        internal async Task<IActionResult> Export(string type, List<LeaveReport> dataCollection)
        {
            string downloadDir = string.Empty;
            if (environment.IsDevelopment())
            {
                downloadDir = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");
            }
            else
            {
                downloadDir = Path.Combine(environment.WebRootPath, "Downloads");
            }

            if (!Directory.Exists(downloadDir)) { Directory.CreateDirectory(downloadDir); }
            string excelName = $"Reports-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            var file = Path.Combine(downloadDir, excelName);
            if (System.IO.File.Exists(file))
            {
                System.IO.File.Delete(file);
                file = Path.Combine(downloadDir, excelName);
            }

            using (var package = new ExcelPackage(new FileInfo(file)))
            {
                var workSheet = package.Workbook.Worksheets.Add("Report");
                workSheet.Cells.LoadFromCollection(dataCollection, true);
                package.Save();
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(file, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, GetContentType(file), Path.GetFileName(file));
        }
    }
}