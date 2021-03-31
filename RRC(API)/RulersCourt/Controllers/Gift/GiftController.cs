using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Gift;
using RulersCourt.Repository.Gift;
using Shark.PdfConvert;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace RulersCourt.Controllers.Gift
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class GiftController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;

        public GiftController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            environment = env;
            appSettings = app;
        }

        [HttpGet]
        [Route("Gift/Count/{UserID:int}")]
        public IActionResult GetGiftcount(int userID)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<GiftClient>.Instance.GetModuleCount(appSettings.Value.ConnectionString, userID, lang);
            return Ok(result);
        }

        [HttpGet]
        [Route("Gift/ListView{PageNumber:int},{PageSize:int}")]
        public IActionResult GiftListView(int pageNumber, int pageSize)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];
            var paramReceivedPurchasedBy = parameters["RecievedPurchasedBy"];
            var paramStatus = parameters["Status"];
            var paramGiftType = parameters["GiftType"];
            var paramSmartSearch = parameters["SmartSearch"];
            var result = DbClientFactory<GiftClient>.Instance.GetGift(appSettings.Value.ConnectionString, pageNumber, pageSize, paramReceivedPurchasedBy, paramUserID, paramStatus, paramSmartSearch, paramGiftType, lang);
            return Ok(result);
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("Gift/{id:int}")]
        public IActionResult GetGiftByID(int id)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int? paramUserID = Convert.ToInt32(parameters["UserID"]);
            var result = DbClientFactory<GiftClient>.Instance.GetGiftID(appSettings.Value.ConnectionString, id, paramUserID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("Gift")]
        public IActionResult SaveGift([FromBody]GiftPostModel meeting)
        {
            try
            {
                var result = DbClientFactory<GiftClient>.Instance.PostGift(appSettings.Value.ConnectionString, meeting);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPut]
        [Route("Gift")]
        public IActionResult UpdateGift([FromBody]GiftPutModel design)
        {
            var result = DbClientFactory<GiftClient>.Instance.PutGift(appSettings.Value.ConnectionString, design);
            return Ok(result);
        }

        [HttpPatch]
        [Route("Gift/{id}")]
        public IActionResult ModifyGift(int id, [FromBody]JsonPatchDocument<GiftPutModel> value)
        {
            var result = DbClientFactory<GiftClient>.Instance.PatchGift(appSettings.Value.ConnectionString, id, value);
            return Ok(result);
        }

        [HttpPost]
        [Route("Gift/SendForDelivery/{id:int}")]
        public IActionResult SendForDelivery(int id)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
                DbClientFactory<GiftClient>.Instance.SendForDelviery(appSettings.Value.ConnectionString, id);
                var result = DbClientFactory<GiftClient>.Instance.GetGiftDeliveryNote(appSettings.Value.ConnectionString, id, lang);
                GiftsPreviewToPdf(result);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("Gift/confirm")]
        public IActionResult ConfirmGift([FromBody]GiftConfirmModel meeting)
        {
            var result = DbClientFactory<GiftClient>.Instance.ConfirmGift(appSettings.Value.ConnectionString, meeting);
            return Ok(result);
        }

        [HttpPost]
        [Route("Gift/Report")]
        public async Task<IActionResult> Export([FromBody]GiftReportPostModel report, string type)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<GiftClient>.Instance.GetGiftReport(appSettings.Value.ConnectionString, report, lang);
                return await this.Export(type, result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        private void GiftsPreviewToPdf(GiftDeliveryNoteModel result)
        {
            string appPath = Directory.GetCurrentDirectory();
            string downloadPath = appPath + @"\Downloads\";
            string finalPdf = downloadPath + result.ReferenceNumber + ".pdf";
            string htmlString = string.Empty;
            if (result.GiftType.Equals("Gift Received"))
                htmlString = System.IO.File.ReadAllText(appPath + @"/Templates/GiftsReceivedPDFTemplate.html");
            else
                htmlString = System.IO.File.ReadAllText(appPath + @"/Templates/GiftsPurchasedPDFTemplate.html");

            ArabicConstantModel arabicValues = new ArabicConstantModel();
            htmlString = htmlString.Replace("{{GiftsTitle}}", arabicValues.GetReport);

            htmlString = htmlString.Replace("{{RefNoValue}}", result.ReferenceNumber);
            htmlString = htmlString.Replace("{{RefNoLabel}}", arabicValues.GetReferenceNumber);
            htmlString = htmlString.Replace("{{CreatorValue}}", result.CreatedBy);
            htmlString = htmlString.Replace("{{CreatorLabel}}", arabicValues.GetCreator);

            htmlString = htmlString.Replace("{{GiftTypeValue}}", result.GiftType);
            htmlString = htmlString.Replace("{{GiftTypeLabel}}", arabicValues.GetGiftType);
            htmlString = htmlString.Replace("{{DateValue}}", result.CreatedDateTime?.ToString("dd/MM/yyyy"));
            htmlString = htmlString.Replace("{{DateLabel}}", arabicValues.GetDate);

            htmlString = htmlString.Replace("{{ReceivedFromValue}}", result.RecievedFromOrganization);
            htmlString = htmlString.Replace("{{ReceivedFromLabel}}", arabicValues.GetReceivedFromOrganisation);

            htmlString = htmlString.Replace("{{PurchasedFromValue}}", result.RecievedFromName);
            htmlString = htmlString.Replace("{{PurchasedFromLabel}}", arabicValues.GetPurchasedFromName);

            htmlString = htmlString.Replace("{{ReceivingDateValue}}", result.RecievedDate?.ToString("dd/MM/yyyy"));
            htmlString = htmlString.Replace("{{ReceivingDateLabel}}", arabicValues.GetDateofReceiving);

            PdfConvert.Convert(new PdfConversionSettings
            {
                Content = htmlString,
                OutputPath = finalPdf
            });
        }
    }
}