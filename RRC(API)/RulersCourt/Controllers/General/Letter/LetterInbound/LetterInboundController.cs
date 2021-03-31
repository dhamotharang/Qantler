using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.General.Letter.LetterInbound;
using RulersCourt.Models.Letter.LetterInbound;
using RulersCourt.Repository.Letter.LetterInbound;
using RulersCourt.Translators;
using RulersCourt.Translators.Letter.LetterInbound;
using Shark.PdfConvert;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Workflow.Interface;

namespace RulersCourt.Controllers.Letter.LetterInbound
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class LetterInboundController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IOptions<APIConfigModel> apiConfigSettings;
        private readonly IHostingEnvironment environment;
        private readonly IWorkflowClient workflow;

        public LetterInboundController(IServiceProvider serviceProvider, IOptions<APIConfigModel> apiConfig, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            appSettings = app;
            environment = env;
            apiConfigSettings = apiConfig;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [Route("InboundLetters/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetInboundLetter(int pageNumber, int pageSize)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramType = parameters["Type"];
            var paramUserID = parameters["UserID"];
            var paramstatus = parameters["Status"];
            var paramSource = parameters["Source"];
            var paramstringDestination = parameters["Destination"];
            var paramstringUserName = parameters["UserName"];
            var paramPriority = parameters["Priority"];
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            DateTime? paramDateFrom = string.IsNullOrEmpty(parameters["DateRangeFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["DateRangeFrom"]);
            DateTime? paramDateTo = string.IsNullOrEmpty(parameters["DateRangeTo"]) ? (DateTime?)null : DateTime.Parse(parameters["DateRangeTo"]);
            var paramSmartSearch = parameters["SmartSearch"];
            var result = DbClientFactory<LetterInboundClient>.Instance.GetInboundLetter(appSettings.Value.ConnectionString, pageNumber, pageSize, paramType, paramUserID, paramstatus, paramSource, paramstringDestination, paramstringUserName, paramPriority, paramDateFrom, paramDateTo, paramSmartSearch, lang);
            return Ok(result);
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("InboundLetter/{id}")]
        public IActionResult GetInboundLetterByID(int id)
        {
            int userID = int.Parse(Request.Query["UserID"]);
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<LetterInboundClient>.Instance.GetLetterByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("InboundLetter")]
        public IActionResult SaveInboundLetter([FromBody]LetterInboundPostModel letter)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<LetterInboundClient>.Instance.PostLetter(appSettings.Value.ConnectionString, letter, lang);
            LetterInboundSaveResponseModel res = new LetterInboundSaveResponseModel();
            res.LetterID = result.LetterID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new GetLetterInboundWorkflow().GetLetterWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.LetterID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPut]
        [Route("InboundLetter")]
        public IActionResult UpdateInboundLetter([FromBody]LetterInboundPutModel letter)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<LetterInboundClient>.Instance.PutLetter(appSettings.Value.ConnectionString, letter, lang);
            LetterInboundSaveResponseModel response = new LetterInboundSaveResponseModel();
            response.LetterID = result.LetterID;
            response.ReferenceNumber = result.ReferenceNumber;
            response.Status = result.Status;
            Workflow.WorkflowBO bo = new GetLetterInboundWorkflow().GetLetterWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = response.LetterID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(response);
        }

        [HttpDelete]
        [Route("InboundLetter/{id:int}")]
        public IActionResult DeleteInboundLetter(int id)
        {
            var result = DbClientFactory<LetterInboundClient>.Instance.DeleteLetter(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpGet]
        [Route("InboundLetter/Clone/{id}")]
        public IActionResult CircularClone(int id)
        {
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<LetterInboundClient>.Instance.SaveInboundClone(appSettings.Value.ConnectionString, id, userID);
            return Ok(result);
        }

        [HttpPatch]
        [Route("InboundLetter/{id}")]
        public IActionResult ModifyInboundLetter(int id, [FromBody]JsonPatchDocument<LetterInboundPutModel> value)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<LetterInboundClient>.Instance.PatchLetter(appSettings.Value.ConnectionString, id, value, lang);
            LetterInboundSaveResponseModel response = new LetterInboundSaveResponseModel();
            response.LetterID = result.LetterID;
            response.ReferenceNumber = result.ReferenceNumber;
            response.Status = result.Status;
            Workflow.WorkflowBO bo = new GetLetterInboundWorkflow().GetLetterWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = response.LetterID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(response);
        }

        [HttpGet]
        [Route("InboundLetter/RelatedOutgoingLetters/{UserID}")]
        public IActionResult GetRelatedOutgoingLetters(int userID)
        {
            var result = DbClientFactory<LetterInboundClient>.Instance.GetRelatedLetter(appSettings.Value.ConnectionString, userID, 1);
            return Ok(result);
        }

        [HttpGet]
        [Route("InboundLetter/RelatedIncomingLetters/{UserID}")]
        public IActionResult GetRelatedIncomingLetters(int userID)
        {
            var result = DbClientFactory<LetterInboundClient>.Instance.GetRelatedLetter(appSettings.Value.ConnectionString, userID, 0);
            return Ok(result);
        }

        [HttpGet]
        [Route("InboundLetter/Entity/{Entity}")]
        public IActionResult GetGovernmentEntity(int entity)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramEntityName = parameters["EntityName"];
            var paramEntityID = string.IsNullOrEmpty(parameters["EntityID"]) ? "0" : parameters["EntityID"];
            var result = DbClientFactory<LetterInboundClient>.Instance.GetEntity(appSettings.Value.ConnectionString, entity, paramEntityName, int.Parse(paramEntityID));
            return Ok(result);
        }

        [HttpGet]
        [Route("InboundLetter/RelatedInOutLetterswithRef/{UserID}")]
        public IActionResult GetRelatedInOutLetterswithRef(int userID)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int paramType = int.Parse(parameters["Type"]);
            string paramRefNo = parameters["RefNo"];
            var result = DbClientFactory<LetterInboundClient>.Instance.GetRelatedLetterinout(appSettings.Value.ConnectionString, userID, paramType, paramRefNo);
            return Ok(result);
        }

        [HttpPost]
        [Route("InboundLetter/Download/{id}")]
        public async Task<IActionResult> DownloadPDFAsync(int id)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var fileName = new List<string>();
                string guid = Guid.NewGuid().ToString("N");
                int userID = int.Parse(Request.Query["UserID"]);
                var letterResult = DbClientFactory<LetterInboundClient>.Instance.GetLetterPreview(appSettings.Value.ConnectionString, id, userID, lang);
                LetterInBoundPreviewModel letterDetails = new LetterInBoundPreviewModel();
                SqlParameter[] destinationUserparam = {
                    new SqlParameter("@P_LetterID", id),
                    new SqlParameter("@P_Language", lang),
                };
                letterDetails.DestinationUserId = SqlHelper.ExecuteProcedureReturnData<List<LetterInboundDestinationUsersModel>>(appSettings.Value.ConnectionString, "Get_LetterInboundDestinationUsers", r => r.TranslateAsLetterInboundDestinationUserList(), destinationUserparam);
                int count = letterDetails.DestinationUserId.Count();
                if (count == 0)
                {
                    var result = DbClientFactory<LetterInboundClient>.Instance.GetLetterPreview(appSettings.Value.ConnectionString, id, userID, lang);
                    fileName.Add(PreviewHtmlToPdf(result, guid, 0, string.Empty));
                }

                foreach (LetterInboundDestinationUsersModel userid in letterDetails.DestinationUserId)
                {
                    var result = DbClientFactory<LetterInboundClient>.Instance.GetLetterPreview(appSettings.Value.ConnectionString, id, userID, lang);
                    fileName.Add(PreviewHtmlToPdf(result, guid, count, userid.LetterDestinationUsersName));
                }

                if (count > 1)
                {
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");
                    var zipFilename = letterResult.LetterReferenceNumber + ".zip";
                    var zipFile = Path.Combine(uploadDir, zipFilename);
                    if (System.IO.File.Exists(zipFile))
                        System.IO.File.Delete(zipFile);
                    ZipFile.CreateFromDirectory(Path.Combine(uploadDir, guid), zipFile);
                    var memory = new MemoryStream();
                    using (var stream = new FileStream(zipFile, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }

                    memory.Position = 0;
                    var a = File(memory, GetContentType(zipFile), Path.GetFileName(zipFile));
                    return Ok(true);
                }
                else
                {
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");

                    var path = Path.Combine(uploadDir, fileName[0], fileName[0]);
                    var memory = new MemoryStream();
                    using (var stream = new FileStream(path, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }

                    memory.Position = 0;
                    return Ok(true);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("InboundLetter/Report")]
        public async Task<IActionResult> Export([FromBody]LetterReportRequestModel report, string type)
        {
            try
            {
                var result = DbClientFactory<LetterInboundClient>.Instance.GetReporExporttList(appSettings.Value.ConnectionString, report);
                return await this.Export(type, result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        private static string GetName(int? userID, string conn, string lang)
        {
            SqlParameter[] param = { new SqlParameter("@P_UserID", userID), new SqlParameter("@P_Department", null), new SqlParameter("@P_Language", lang) };
            var temp = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(conn, "Get_User", r => r.TranslateAsUserList(), param);
            return temp.FirstOrDefault().EmployeeName;
        }

        private string PreviewHtmlToPdf(LetterInBoundPreviewModel result, string guid, int fileCount, string destinationUserName)
        {
            string appPath = Directory.GetCurrentDirectory();
            string downloadPath = appPath + @"\Downloads\";
            string finalPdf = string.Empty;
            finalPdf = downloadPath + result.LetterReferenceNumber + ".pdf";
            string htmlString = System.IO.File.ReadAllText(appPath + @"/Templates/LetterPDFTemplate.html");

            ArabicConstantModel arabicValues = new ArabicConstantModel();
            htmlString = htmlString.Replace("{{RefNoValue}}", result.LetterReferenceNumber);
            htmlString = htmlString.Replace("{{Date}}", result.CreatedDateTime?.ToString("dd/MM/yyyy"));

            htmlString = htmlString.Replace("{{DestinationUserValue}}", destinationUserName);
            htmlString = htmlString.Replace("{{IslamicGreetings}}", arabicValues.GetArabicGreetings);

            htmlString = htmlString.Replace("{{TitleValue}}", result.Title + ":" + arabicValues.GetSubject);
            htmlString = htmlString.Replace("{{DescValue}}", result.LetterDetails);
            htmlString = htmlString.Replace("{{Regards}}", arabicValues.GetBestRegards);

            htmlString = htmlString.Replace("{{ApproverNameValue}}", result.ApproverName);
            htmlString = htmlString.Replace("{{ApproverDesgValue}}", result.ApproverDesignation);

            if (!string.IsNullOrEmpty(result.SignaturePhotoID) && !string.IsNullOrEmpty(result.SignaturePhotoName))
            {
                htmlString = htmlString.Replace("{{ShowApproverSignature}}", string.Empty);
                htmlString = htmlString.Replace("{{ApproverSignatureValue}}", Path.Combine(apiConfigSettings.Value.Url + @"/api/attachment/download?filename=" + result.SignaturePhotoName + @"&guid=" + result.SignaturePhotoID));
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

            return finalPdf;
        }
    }
}