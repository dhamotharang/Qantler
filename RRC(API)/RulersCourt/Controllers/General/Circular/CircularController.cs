using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Circular;
using RulersCourt.Models.General.Circular;
using RulersCourt.Models.UserProfile;
using RulersCourt.Repository.Circular;
using RulersCourt.Translators.UserProfile;
using Shark.PdfConvert;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Workflow.Interface;

namespace RulersCourt.Controllers
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class CircularController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IOptions<APIConfigModel> apiConfigSettings;
        private readonly IWorkflowClient workflow;
        private readonly IHostingEnvironment environment;

        public CircularController(IOptions<ConnectionSettingsModel> app, IOptions<APIConfigModel> appConfig, IServiceProvider serviceProvider, IHostingEnvironment env)
             : base(app, env)
        {
            appSettings = app;
            apiConfigSettings = appConfig;
            environment = env;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [Route("Circular/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetAllCircular(int pageNumber, int pageSize)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramType = parameters["Type"];
            var paramUserName = parameters["UserName"];
            var paramstatus = parameters["Status"];
            var paramSourceOU = parameters["Source"];
            if (!string.IsNullOrEmpty(paramSourceOU) & !string.IsNullOrEmpty(paramSourceOU))
            {
                paramSourceOU = paramSourceOU.Replace("amp;", "&");
            }

            var paramstringDestinationOU = parameters["Destination"];
            if (!string.IsNullOrEmpty(paramstringDestinationOU) & !string.IsNullOrEmpty(paramstringDestinationOU))
            {
                paramstringDestinationOU = paramstringDestinationOU.Replace("amp;", "&");
            }

            var paramPriority = parameters["Priority"];
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            DateTime? paramDateFrom = string.IsNullOrEmpty(parameters["DateRangeFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["DateRangeFrom"]);
            DateTime? paramDateTo = string.IsNullOrEmpty(parameters["DateRangeTo"]) ? (DateTime?)null : DateTime.Parse(parameters["DateRangeTo"]);
            var paramSmartSearch = parameters["SmartSearch"];
            var result = DbClientFactory<CircularClient>.Instance.GetCircular(appSettings.Value.ConnectionString, pageNumber, pageSize, paramType, paramUserName, paramstatus, paramSourceOU, paramstringDestinationOU, paramPriority, paramDateFrom, paramDateTo, paramSmartSearch, lang);
            return Ok(result);
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("Circular/{id}")]
        public IActionResult GetCircularByID(int id)
        {
            try
            {
                int userID = int.Parse(Request.Query["UserID"]);
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<CircularClient>.Instance.GetCircularByID(appSettings.Value.ConnectionString, id, userID, lang);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("Circular")]
        public IActionResult SaveCircular([FromBody]CircularPostModel circular)
        {
            var result = DbClientFactory<CircularClient>.Instance.PostCircular(appSettings.Value.ConnectionString, circular);
            CircularSaveResponseModel res = new CircularSaveResponseModel();
            res.CircularId = result.CircularId;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new GetCircularWorkflow().GetCircularsWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.CircularId ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPut]
        [Route("Circular")]
        public IActionResult UpdateCircular([FromBody]CircularPutModel circular)
        {
            var result = DbClientFactory<CircularClient>.Instance.PutCircular(appSettings.Value.ConnectionString, circular);
            CircularSaveResponseModel res = new CircularSaveResponseModel();
            res.CircularId = result.CircularId;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new GetCircularWorkflow().GetCircularsWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.CircularId ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpDelete]
        [Route("Circular/{id:int}")]
        public IActionResult DeleteCircular(int id)
        {
            var result = DbClientFactory<CircularClient>.Instance.DeleteCircular(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpGet]
        [Route("Circular/Clone/{id}")]
        public IActionResult CircularClone(int id)
        {
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<CircularClient>.Instance.SaveCircularClone(appSettings.Value.ConnectionString, id, userID);
            return Ok(result);
        }

        [HttpPatch]
        [Route("Circular/{id}")]
        public IActionResult Modifycircular(int id, [FromBody]JsonPatchDocument<CircularPutModel> value)
        {
            var result = DbClientFactory<CircularClient>.Instance.PatchCircular(appSettings.Value.ConnectionString, id, value);
            CircularSaveResponseModel res = new CircularSaveResponseModel();
            res.CircularId = result.CircularId;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new GetCircularWorkflow().GetCircularsWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.CircularId ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPost]
        [Route("Circular/export")]
        public async Task<IActionResult> Export([FromBody]CircularReportRequestModel report, string type)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<CircularClient>.Instance.GetCircularReportExportList(appSettings.Value.ConnectionString, report, lang);
            return await this.Export(type, result);
        }

        [HttpPost]
        [Route("Circular/Download/{id}")]
        public IActionResult DownloadPDF(int id)
        {
            try
            {
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                int userID = int.Parse(Request.Query["UserID"]);
                var result = DbClientFactory<CircularClient>.Instance.GetCircularPreview(appSettings.Value.ConnectionString, id, userID, lang);
                PreviewHtmlToPdf(result, lang);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private void PreviewHtmlToPdf(CircularPreviewModel result, string lang)
        {
            string appPath = Directory.GetCurrentDirectory();
            string downloadPath = appPath + @"\Downloads\";
            string finalPdf = string.Empty;
            if (!Directory.Exists(downloadPath)) { Directory.CreateDirectory(downloadPath); }
            finalPdf = downloadPath + result.ReferenceNumber + ".pdf";
            string htmlString = System.IO.File.ReadAllText(appPath + @"/Templates/CircularPDFTemplate.html");

            ArabicConstantModel arabicValues = new ArabicConstantModel();
            htmlString = htmlString.Replace("{{RefNoValue}}", result.ReferenceNumber);
            htmlString = htmlString.Replace("{{Date}}", result.CreatedDateTime?.ToString("dd/MM/yyyy"));

            htmlString = htmlString.Replace("{{TitleValue}}", result.Title);
            htmlString = htmlString.Replace("{{DescValue}}", result.Details);

            htmlString = htmlString.Replace("{{ApproverNameValue}}", result.ApproverName);

            if (!string.IsNullOrEmpty(result.SignaturePhotoApprover))
            {
                SqlParameter[] getparam = {
                        new SqlParameter("@P_UserProfileId", result.ApproverID),
                        new SqlParameter("@P_Language", lang)
                };

                List<UserProfileGetModel> attachments = SqlHelper.ExecuteProcedureReturnData<List<UserProfileGetModel>>(appSettings.Value.ConnectionString, "Get_UserProfileByID", r => r.TranslateAsUserProfileList(), getparam);

                foreach (var item in attachments)
                {
                    if (!string.IsNullOrEmpty(item.SignaturePhoto) && !string.IsNullOrEmpty(item.SignaturePhoto))
                    {
                        htmlString = htmlString.Replace("{{ShowApproverSignature}}", string.Empty);
                        htmlString = htmlString.Replace("{{ApproverSignatureValue}}", Path.Combine(apiConfigSettings.Value.Url + @"/api/attachment/download?filename=" + item.SignaturePhoto + @"&guid=" + item.SignaturePhotoID));
                    }
                }
            }
            else
            {
                htmlString = htmlString.Replace("{{ApproverSignatureValue}}", string.Empty);
                htmlString = htmlString.Replace("{{ShowApproverSignature}}", "none");
            }

            PdfConvert.Convert(new PdfConversionSettings
            {
                Content = htmlString,
                OutputPath = finalPdf
            });
        }
    }
}
