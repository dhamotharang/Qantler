using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using RulersCourt.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RulersCourt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;

        public CommonController(IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
        {
            appSettings = app;
            environment = env;
        }

        internal async Task<IActionResult> Export(string type, dynamic dataCollection)
        {
            string downloadDir = string.Empty;
            if (environment.IsDevelopment())
            {
                downloadDir = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");
            }
            else
            {
                downloadDir = Path.Combine(environment.ContentRootPath, "Downloads");
            }

            if (!Directory.Exists(downloadDir))
            { Directory.CreateDirectory(downloadDir); }
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

        internal string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        internal void WriteContent(string multiLineString, PdfStamper stamper, float xVal, float yVal)
        {
            var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            PdfContentByte canvas = stamper.GetOverContent(1);
            ColumnText ct = new ColumnText(canvas)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_RTL
            };

            var styles = new StyleSheet();
            styles.LoadTagStyle("body", "size", "2");
            using (var sr = new StringReader(multiLineString))
            {
                var elements = HtmlWorker.ParseToList(sr, styles);
                foreach (IElement e in elements)
                {
                    ct.AddElement(e);
                }
            }

            ct.SetSimpleColumn(xVal, yVal, 500, 100, 24, Element.ALIGN_RIGHT);
            ct.Go();
            yVal = yVal - 10;
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                { ".txt", "text/plain" },
                { ".pdf", "application/pdf" },
                { ".doc", "application/vnd.ms-word" },
                { ".docx", "application/vnd.ms-word" },
                { ".xls", "application/vnd.ms-excel" },
                { ".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet" },
                { ".png", "image/png" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".gif", "image/gif" },
                { ".csv", "text/csv" },
                { ".mp3", "audio/mpeg" },
                { ".m4a", "audio/m4a" },
                { ".amr", "audio/amr" },
                { ".wav", "audio/wav" },
                { ".zip", "application/zip" }
            };
        }
    }
}