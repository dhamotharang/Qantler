using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.General.Memo;
using RulersCourt.Models.UserProfile;
using RulersCourt.Repository;
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
    public class MemoController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IWorkflowClient workflow;
        private readonly IHostingEnvironment environment;
        private readonly IOptions<APIConfigModel> apiConfigSettings;

        public MemoController(IServiceProvider serviceProvider, IOptions<APIConfigModel> apiConfig, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            environment = env;
            appSettings = app;
            apiConfigSettings = apiConfig;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [Route("memos/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetAllMemo(int pageNumber, int pageSize)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramType = parameters["Type"];
            var paramUsername = parameters["Username"];
            var paramstatus = parameters["Status"];
            var paramSourceOU = parameters["SourceOU"];
            if (!string.IsNullOrEmpty(paramSourceOU) & !string.IsNullOrEmpty(paramSourceOU))
            {
                paramSourceOU = paramSourceOU.Replace("amp;", "&");
            }

            var paramstringDestinationOU = parameters["DestinationOU"];
            if (!string.IsNullOrEmpty(paramstringDestinationOU) & !string.IsNullOrEmpty(paramstringDestinationOU))
            {
                paramstringDestinationOU = paramstringDestinationOU.Replace("amp;", "&");
            }

            var paramPrivate = parameters["Private"];
            var paramPriority = parameters["Priority"];
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            DateTime? paramDateFrom = string.IsNullOrEmpty(parameters["DateFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["DateFrom"]);
            DateTime? paramDateTo = string.IsNullOrEmpty(parameters["DateTo"]) ? (DateTime?)null : DateTime.Parse(parameters["DateTo"]);
            var paramSmartSearch = parameters["SmartSearch"];
            var result = DbClientFactory<MemoClient>.Instance.GetMemo(appSettings.Value.ConnectionString, pageNumber, pageSize, paramType, paramUsername, paramstatus, paramSourceOU, paramstringDestinationOU, paramPrivate, paramPriority, paramDateFrom, paramDateTo, paramSmartSearch, lang);
            return Ok(result);
        }

        [ServiceFilter(typeof(AccessControlAttribute))]
        [HttpGet]
        [Route("memo/{id}")]
        public IActionResult GetMemoByID(int id)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = int.Parse(Request.Query["UserID"]);
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<MemoClient>.Instance.GetMemoByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("memo")]
        public IActionResult SaveMemo([FromBody]MemoPostModel memo)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<MemoClient>.Instance.PostMemo(appSettings.Value.ConnectionString, memo, lang);
            MemoSaveResponseModel res = new MemoSaveResponseModel();
            res.MemoId = result.MemoId;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new MemoGetWorkflow().GetMemoWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.MemoId ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPut]
        [Route("memo")]
        public IActionResult UpdateMemo([FromBody]MemoPutModel memo)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<MemoClient>.Instance.PutMemo(appSettings.Value.ConnectionString, memo, lang);
            MemoSaveResponseModel res = new MemoSaveResponseModel();
            res.MemoId = result.MemoId;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new MemoGetWorkflow().GetMemoWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.MemoId ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpDelete]
        [Route("Memo/{id:int}")]
        public IActionResult DeleteMemo(int id)
        {
            var result = DbClientFactory<MemoClient>.Instance.DeleteMemo(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpGet]
        [Route("Memo/Clone/{id}")]
        public IActionResult CircularClone(int id)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<MemoClient>.Instance.SaveMemoClone(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPatch]
        [Route("Memo/{id}")]
        public IActionResult ModifyMemo(int id, [FromBody]JsonPatchDocument<MemoPutModel> value)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<MemoClient>.Instance.PatchMemo(appSettings.Value.ConnectionString, id, value, lang);
            MemoSaveResponseModel res = new MemoSaveResponseModel();
            res.MemoId = result.MemoId;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new MemoGetWorkflow().GetMemoWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.MemoId ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPost]
        [Route("Memo/Report")]
        public async Task<IActionResult> Export([FromBody]ReportRequestModel report, string type)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<ReportClient>.Instance.GetReporExporttList(appSettings.Value.ConnectionString, report, lang);
                return await this.Export(type, result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("Memo/preview/{id:int}")]
        public IActionResult PrintPreview(int id)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                int userID = int.Parse(Request.Query["UserID"]);
                var result = DbClientFactory<MemoClient>.Instance.GetMemoPreview(appSettings.Value.ConnectionString, id, userID, lang);
                PreviewHtmlToPdf(result, lang);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("Memo/Keywords/{id}")]
        public IActionResult SaveKeywords(int id, [FromBody]List<MemoKeywordsModel> memo)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int paramUserID = parameters["UserID"] is null ? 0 : int.Parse(parameters["UserID"]);
            DbClientFactory<MemoClient>.Instance.Postkeyword(appSettings.Value.ConnectionString, memo, id, paramUserID);

            return Ok(id);
        }

        private void PreviewHtmlToPdf(MemoPreviewModel result, string lang)
        {
            string appPath = Directory.GetCurrentDirectory();
            string downloadPath = appPath + @"\Downloads\";
            string finalPdf = downloadPath + result.ReferenceNumber + ".pdf";
            string htmlString = System.IO.File.ReadAllText(appPath + @"/Templates/MemoPDFTemplate.html");

            ArabicConstantModel arabicValues = new ArabicConstantModel();
            htmlString = htmlString.Replace("{{MemoTitle}}", arabicValues.GetInternalMemo);

            htmlString = htmlString.Replace("{{RefNoValue}}", result.ReferenceNumber);
            htmlString = htmlString.Replace("{{RefNoLabel}}", arabicValues.GetMemoReference);

            htmlString = htmlString.Replace("{{Date}}", result.CreatedDateTime?.ToString("dd/MM/yyyy"));
            htmlString = htmlString.Replace("{{DateLabel}}", arabicValues.GetDate);

            htmlString = htmlString.Replace("{{ApproverValue}}", result.ApproverNameID);
            htmlString = htmlString.Replace("{{ApproverLabel}}", arabicValues.GetFrom);

            htmlString = htmlString.Replace("{{DestinationValue}}", result.DestinationTitle);
            htmlString = htmlString.Replace("{{DestinationLabel}}", arabicValues.GetTo);

            htmlString = htmlString.Replace("{{SubjectValue}}", result.Title);
            htmlString = htmlString.Replace("{{SubjectLabel}}", arabicValues.GetSubject);

            htmlString = htmlString.Replace("{{DescValue}}", result.Details);
            htmlString = htmlString.Replace("{{DescLabel}}", arabicValues.GetDescription);

            if (!string.IsNullOrEmpty(result.SignaturePhotoApprover))
            {
                SqlParameter[] getparam = {
                    new SqlParameter("@P_UserProfileId", result.ApproverID),
                    new SqlParameter("@P_Language", lang)
                };
                List<UserProfileGetModel> attachments = SqlHelper.ExecuteProcedureReturnData(appSettings.Value.ConnectionString, "Get_UserProfileByID", r => r.TranslateAsUserProfileList(), getparam);

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

            htmlString = htmlString.Replace("{{ApproverSignatureLabel}}", arabicValues.GetSignatureofApprover);

            if (!string.IsNullOrEmpty(result.SignaturePhotoRedirector))
            {
                SqlParameter[] getparam = {
                    new SqlParameter("@P_UserProfileId", result.RedirectID),
                    new SqlParameter("@P_Language", lang)
                };
                List<UserProfileGetModel> attachments = SqlHelper.ExecuteProcedureReturnData(appSettings.Value.ConnectionString, "Get_UserProfileByID", r => r.TranslateAsUserProfileList(), getparam);

                foreach (var item in attachments)
                {
                    if (!string.IsNullOrEmpty(item.SignaturePhoto) && !string.IsNullOrEmpty(item.SignaturePhoto))
                    {
                        htmlString = htmlString.Replace("{{ShowRedirectSignature}}", string.Empty);
                        htmlString = htmlString.Replace("{{RedirectSignatureValue}}", Path.Combine(apiConfigSettings.Value.Url + @"/api/attachment/download?filename=" + item.SignaturePhoto + @"&guid=" + item.SignaturePhotoID));
                    }
                }
            }
            else
            {
                htmlString = htmlString.Replace("{{ShowRedirectSignature}}", "none");
                htmlString = htmlString.Replace("{{RedirectSignatureValue}}", string.Empty);
            }

            htmlString = htmlString.Replace("{{RedirectSignatureLabel}}", arabicValues.GetRedirection);

            PdfConvert.Convert(new PdfConversionSettings
            {
                Content = htmlString,
                OutputPath = finalPdf
            });
        }
    }
}