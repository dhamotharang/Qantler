using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.General.Letter.LetterOutbound;
using RulersCourt.Models.Letter;
using RulersCourt.Repository;
using RulersCourt.Repository.Letter;
using RulersCourt.Translators;
using RulersCourt.Translators.Letter;
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

namespace RulersCourt.Controllers.Letter
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class LetterOutboundController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IOptions<APIConfigModel> apiConfigSettings;
        private readonly IHostingEnvironment environment;
        private readonly IWorkflowClient workflow;

        public LetterOutboundController(IServiceProvider serviceProvider, IOptions<APIConfigModel> apiConfig, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            appSettings = app;
            environment = env;
            apiConfigSettings = apiConfig;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [Route("OutboundLetter/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetLetter(int pageNumber, int pageSize)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramType = parameters["Type"];
            var paramUserID = parameters["UserID"];
            var paramstatus = parameters["Status"];
            var paramSource = parameters["Source"];
            var paramstringDestination = parameters["Destination"];
            var paramstringUserName = parameters["UserName"];
            var paramPriority = parameters["Priority"];
            var paramSenderName = parameters["SenderName"];
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            DateTime? paramDateFrom = string.IsNullOrEmpty(parameters["DateFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["DateFrom"]);
            DateTime? paramDateTo = string.IsNullOrEmpty(parameters["DateTo"]) ? (DateTime?)null : DateTime.Parse(parameters["DateTo"]);
            var paramSmartSearch = parameters["SmartSearch"];
            var result = DbClientFactory<LetterOutboundClient>.Instance.GetLetter(appSettings.Value.ConnectionString, pageNumber, pageSize, paramType, paramUserID, paramstatus, paramSource, paramstringDestination, paramstringUserName, paramPriority, paramSenderName, paramDateFrom, paramDateTo, paramSmartSearch, lang);
            return Ok(result);
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("OutboundLetter/{id}")]
        public IActionResult GetLetterByID(int id)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);

            int userID = int.Parse(Request.Query["UserID"]);
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<LetterOutboundClient>.Instance.GetLetterByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("OutboundLetter")]
        public IActionResult SaveLetter([FromBody]LetterOutboundPostModel letter)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<LetterOutboundClient>.Instance.PostLetter(appSettings.Value.ConnectionString, letter, lang);
            LetterOutboundSaveResponseModel res = new LetterOutboundSaveResponseModel();
            res.LetterID = result.LetterID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new GetLetterOutboundWorkflow().GetLetterWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.LetterID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPut]
        [Route("OutboundLetter")]
        public IActionResult UpdateLetter([FromBody]LetterOutboundPutModel letter)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<LetterOutboundClient>.Instance.PutLetter(appSettings.Value.ConnectionString, letter, lang);
            LetterOutboundSaveResponseModel response = new LetterOutboundSaveResponseModel();
            response.LetterID = result.LetterID;
            response.ReferenceNumber = result.ReferenceNumber;
            response.Status = result.Status;
            Workflow.WorkflowBO bo = new GetLetterOutboundWorkflow().GetLetterWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = response.LetterID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(response);
        }

        [HttpDelete]
        [Route("OutboundLetter/{id:int}")]
        public IActionResult DeleteLetter(int id)
        {
            var result = DbClientFactory<LetterOutboundClient>.Instance.DeleteLetter(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpGet]
        [Route("OutboundLetter/Clone/{id}")]
        public IActionResult CircularClone(int id)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<LetterOutboundClient>.Instance.SaveOutboundClone(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPatch]
        [Route("OutboundLetter/{id}")]
        public IActionResult ModifyLetter(int id, [FromBody]JsonPatchDocument<LetterOutboundPutModel> value)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<LetterOutboundClient>.Instance.PatchLetter(appSettings.Value.ConnectionString, id, value, lang);
            LetterOutboundSaveResponseModel response = new LetterOutboundSaveResponseModel();
            response.LetterID = result.LetterID;
            response.ReferenceNumber = result.ReferenceNumber;
            response.Status = result.Status;
            Workflow.WorkflowBO bo = new GetLetterOutboundWorkflow().GetLetterWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = response.LetterID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(response);
        }

        [HttpPost]
        [Route("OutboundLetter/BulkApproval")]
        public IActionResult BulkApproval([FromBody]LetterOutboundBulkActionlModel letter)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int items = letter.LettersID.Count;
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            for (int i = 0; i < items; i++)
            {
                int letterID = letter.LettersID[i].LetterID;
                var result = DbClientFactory<LetterOutboundClient>.Instance.GetLetterBulkWorkflowByID(appSettings.Value.ConnectionString, letterID, letter.ActionBy, letter.ActionDateTime, lang);
                result.FromID = Convert.ToInt32(parameters["UserID"]);
                Workflow.WorkflowBO bo = new GetLetterOutboundWorkflow().GetLetterBulkApprovalWorkflow(result, appSettings.Value.ConnectionString);
                bo.ServiceID = letterID;
                workflow.StartWorkflow(bo);
            }

            return Ok(1);
        }

        [HttpPost]
        [Route("OutboundLetter/Report")]
        public async Task<IActionResult> Export([FromBody]LetterReportRequestModel report, string type)
        {
            try
            {
                var result = DbClientFactory<LetterOutboundClient>.Instance.GetReporExporttList(appSettings.Value.ConnectionString, report);
                return await this.Export(type, result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("OutboundLetter/DeliveryNote")]
        public async Task<IActionResult> DeliveryNoteAsync([FromBody]LetterOutboundDeliveryNoteModel letter)
        {
            try
            {
                var fileName = new List<string>();
                int userID = int.Parse(Request.Query["UserID"]);
                string guid = Guid.NewGuid().ToString("N");
                int count = letter.LettersID.Count;
                foreach (LetterOutboundIDModel id in letter.LettersID)
                {
                    var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                    var result = DbClientFactory<LetterOutboundClient>.Instance.GetLetterPreview(appSettings.Value.ConnectionString, id.LetterID, userID, lang);
                    fileName.Add(PreviewDeliveryNoteHtmlToPdf(result, guid, count));
                }

                if (count > 1)
                {
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");
                    var zipFilename = "DeliveryNote" + ".zip";
                    var zipFile = Path.Combine(uploadDir, zipFilename);
                    if (System.IO.File.Exists(zipFile))
                        System.IO.File.Delete(uploadDir + "\\" + zipFilename);
                    ZipFile.CreateFromDirectory(Path.Combine(uploadDir, guid), zipFile);
                    var memory = new MemoryStream();
                    using (var stream = new FileStream(zipFile, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }

                    memory.Position = 0;
                    return File(memory, GetContentType(zipFile), Path.GetFileName(zipFile));
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
                    return File(memory, GetContentType(path), Path.GetFileName(path));
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("OutboundLetter/Entity/{Entity}")]
        public IActionResult GetGovernmentEntity(int entity)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramEntityName = parameters["EntityName"];
            var paramEntityID = string.IsNullOrEmpty(parameters["EntityID"]) ? "0" : parameters["EntityID"];
            var result = DbClientFactory<LetterOutboundClient>.Instance.GetEntity(appSettings.Value.ConnectionString, entity, paramEntityName, int.Parse(paramEntityID));
            return Ok(result);
        }

        [HttpGet]
        [Route("OutboundLetter/RelatedOutgoingLetters/{UserID}")]
        public IActionResult GetRelatedOutgoingLetters(int userID)
        {
            var result = DbClientFactory<LetterOutboundClient>.Instance.GetRelatedLetter(appSettings.Value.ConnectionString, userID, 1);
            return Ok(result);
        }

        [HttpGet]
        [Route("OutboundLetter/RelatedIncomingLetters/{UserID}")]
        public IActionResult GetRelatedIncomingLetters(int userID)
        {
            var result = DbClientFactory<LetterOutboundClient>.Instance.GetRelatedLetter(appSettings.Value.ConnectionString, userID, 0);
            return Ok(result);
        }

        [HttpPost]
        [Route("OutboundLetter/Download/{id}")]
        public async Task<IActionResult> DownloadPDFAsync(int id)
        {
            try
            {
                var fileName = new List<string>();
                string guid = Guid.NewGuid().ToString("N");
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                int userID = int.Parse(Request.Query["UserID"]);
                var letterResult = DbClientFactory<LetterOutboundClient>.Instance.GetLetterPreview(appSettings.Value.ConnectionString, id, userID, lang);

                LetterOutboundPreviewModel letterDetails = new LetterOutboundPreviewModel();

                SqlParameter[] destinationDepartmentparam = {
                    new SqlParameter("@P_LetterID", id)
                };
                letterDetails.DestinationEntity = SqlHelper.ExecuteProcedureReturnData<List<LetterOutboundDestinationDepartmentModel>>(appSettings.Value.ConnectionString, "Get_LetterOutboundDestinationEntity", r => r.TranslateAsLetterDestinationDepartmentList(), destinationDepartmentparam);
                int count = letterDetails.DestinationEntity.Count();
                if (count == 0)
                {
                    var result = DbClientFactory<LetterOutboundClient>.Instance.GetLetterPreview(appSettings.Value.ConnectionString, id, userID, lang);

                    fileName.Add(PreviewHtmlToPdf(result, guid, 0, string.Empty, 0));
                }
                else
                {
                    foreach (LetterOutboundDestinationDepartmentModel destinationid in letterDetails.DestinationEntity)
                    {
                        var result = DbClientFactory<LetterOutboundClient>.Instance.GetLetterPreview(appSettings.Value.ConnectionString, id, userID, lang);

                        fileName.Add(PreviewHtmlToPdf(result, guid, count, destinationid.LetterDestinationUserName, destinationid.LetterDestinationID));
                    }
                }

                if (count > 1)
                {
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");
                    var zipFilename = letterResult.LetterReferenceNumber + ".zip";
                    var zipFile = Path.Combine(uploadDir, zipFilename);
                    System.IO.File.Delete(uploadDir + string.Empty + "\\" + zipFilename);
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

        private static string GetName(int? userID, string conn, string lang)
        {
            SqlParameter[] param = { new SqlParameter("@P_UserID", userID), new SqlParameter("@P_Department", null), new SqlParameter("@P_Language", lang) };
            var temp = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(conn, "Get_User", r => r.TranslateAsUserList(), param);
            return temp.FirstOrDefault().EmployeeName;
        }

        private string PreviewHtmlToPdf(LetterOutboundPreviewModel result, string guid, int fileCount, string destinationUserName, int? letterDestinationID)
        {
            string appPath = Directory.GetCurrentDirectory();
            string downloadPath = appPath + @"\Downloads\";
            if (fileCount > 1)
            {
                if (!System.IO.Directory.Exists(downloadPath + guid))
                    System.IO.Directory.CreateDirectory(downloadPath + guid);

                downloadPath = downloadPath + guid + @"\";
            }

            string finalPdf = string.Empty;
            if (!string.IsNullOrEmpty(destinationUserName) && letterDestinationID != null) { finalPdf = downloadPath + result.LetterReferenceNumber + "_" + letterDestinationID + ".pdf"; }
            else { finalPdf = downloadPath + result.LetterReferenceNumber + ".pdf"; }

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

        private string PreviewDeliveryNoteHtmlToPdf(LetterOutboundPreviewModel result, string guid, int fileCount)
        {
            string appPath = Directory.GetCurrentDirectory();
            string downloadPath = appPath + @"\Downloads\";
            string finalPdf = string.Empty;
            if (fileCount > 1)
            {
                string a = "/";
                downloadPath = string.Concat(downloadPath, guid, a);
            }

            if (!Directory.Exists(downloadPath)) { Directory.CreateDirectory(downloadPath); }
            finalPdf = downloadPath + result.LetterReferenceNumber + ".pdf";

            string htmlString = System.IO.File.ReadAllText(appPath + @"/Templates/DeliveryNotePDFTemplate.html");

            ArabicConstantModel arabicValues = new ArabicConstantModel();
            htmlString = htmlString.Replace("{{DeliveryNoteHeading}}", arabicValues.GetMailDeliveryNote);
            htmlString = htmlString.Replace("{{DateValue}}", result.CreatedDateTime?.ToString("dd/MM/yyyy"));
            htmlString = htmlString.Replace("{{DateLabel}}", arabicValues.GetDate);

            htmlString = htmlString.Replace("{{RefNoValue}}", result.LetterReferenceNumber);
            htmlString = htmlString.Replace("{{RefNoLabel}}", arabicValues.GetReference);

            htmlString = htmlString.Replace("{{FromValue}}", result.SourceName);
            htmlString = htmlString.Replace("{{FromLabel}}", arabicValues.GetFrom);

            htmlString = htmlString.Replace("{{ToValue}}", result.DestinationTitle);
            htmlString = htmlString.Replace("{{ToLabel}}", arabicValues.GetTo);

            htmlString = htmlString.Replace("{{SubjectValue}}", result.Title);
            htmlString = htmlString.Replace("{{SubjectLabel}}", arabicValues.GetSubject);

            htmlString = htmlString.Replace("{{PhoneLabel}}", arabicValues.GetPhoneNumber);
            htmlString = htmlString.Replace("{{SignatureLabel}}", arabicValues.GetSignature);
            htmlString = htmlString.Replace("{{SectionLabel}}", arabicValues.GetSection);
            htmlString = htmlString.Replace("{{RecipientLabel}}", arabicValues.GetRecipientName);

            PdfConvert.Convert(new PdfConversionSettings
            {
                Content = htmlString,
                OutputPath = finalPdf
            });

            return finalPdf;
        }
    }
}
