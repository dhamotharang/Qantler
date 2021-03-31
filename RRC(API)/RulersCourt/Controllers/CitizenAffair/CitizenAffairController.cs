using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.CitizenAffair;
using RulersCourt.Models.UserProfile;
using RulersCourt.Repository.CitizenAffair;
using RulersCourt.Services;
using RulersCourt.Translators.UserProfile;
using Shark.PdfConvert;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Workflow.Interface;

namespace RulersCourt.Controllers.Announcement
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class CitizenAffairController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IOptions<APIConfigModel> apiConfigSettings;
        private readonly IHostingEnvironment environment;
        private readonly IWorkflowClient workflow;
        private readonly EncryptionService encryptionService;

        public CitizenAffairController(IServiceProvider serviceProvider, EncryptionService encryptionSvc, IOptions<ConnectionSettingsModel> app, IOptions<APIConfigModel> apiConfig, IHostingEnvironment env)
            : base(app, env)
        {
            appSettings = app;
            environment = env;
            apiConfigSettings = apiConfig;
            workflow = serviceProvider.GetService<IWorkflowClient>();
            encryptionService = encryptionSvc;
        }

        [HttpGet]
        [Route("CitizenAffair/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetAllCitizenAffair(int pageNumber, int pageSize)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramstatus = parameters["Status"] == "null" ? string.Empty : parameters["Status"];
            var paramReqType = parameters["RequestType"] == "null" ? null : parameters["RequestType"];
            var paramRefNo = parameters["ReferenceNumber"];
            var paramUserID = parameters["UserID"];
            var paramType = parameters["Type"];
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var paramPersonalLocationName = parameters["PersonalLocationName"];
            var paramPhoneNo = parameters["PhoneNumber"] == "null" ? null : parameters["PhoneNumber"];
            DateTime? paramDateFrom = string.IsNullOrEmpty(parameters["ReqDateFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["ReqDateFrom"]);
            DateTime? paramDateTo = string.IsNullOrEmpty(parameters["ReqDateTo"]) ? (DateTime?)null : DateTime.Parse(parameters["ReqDateTo"]);
            var paramSmartSearch = parameters["SmartSearch"];
            var paramSourceName = parameters["sourcename"];
            var result = DbClientFactory<CitizenAffairClient>.Instance.GetCitizenAffair(appSettings.Value.ConnectionString, pageNumber, pageSize, paramstatus, paramReqType, paramUserID, paramRefNo, paramPersonalLocationName, paramPhoneNo, paramDateFrom, paramDateTo, paramSmartSearch, paramType, lang, paramSourceName);
            return Ok(result);
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("CitizenAffair/{id}")]
        public IActionResult GetCitizenAffairByID(int id)
        {
            try
            {
                int userID = int.Parse(Request.Query["UserID"]);
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<CitizenAffairClient>.Instance.GetCitizenAffairByID(appSettings.Value.ConnectionString, id, userID, lang);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("CitizenAffair")]
        public IActionResult SaveCitizenAffair([FromBody]CitizenAffairPostModel citizenAffair)
        {
            try
            {
                CitizenAffairSaveResponseModel response = new CitizenAffairSaveResponseModel();
                var result = DbClientFactory<CitizenAffairClient>.Instance.PostCitizenAffair(appSettings.Value.ConnectionString, citizenAffair);
                response.CitizenAffairId = result.CitizenAffairID;
                response.ReferenceNumber = result.ReferenceNumber;
                response.Status = result.Status;
                Workflow.WorkflowBO bo = new GetCitizenAffairWorkflow().GetWorkflow(result, appSettings.Value.ConnectionString, apiConfigSettings.Value.Url, encryptionService, environment);
                bo.ServiceID = result.CitizenAffairID ?? 0;
                workflow.StartWorkflow(bo);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPut]
        [Route("CitizenAffair")]
        public IActionResult UpdateCitizenAffair([FromBody]CitizenAffairPutModel citizenAffair)
        {
            CitizenAffairSaveResponseModel response = new CitizenAffairSaveResponseModel();
            var result = DbClientFactory<CitizenAffairClient>.Instance.PutCitizenAffair(appSettings.Value.ConnectionString, citizenAffair);
            response.CitizenAffairId = result.CitizenAffairID;
            response.ReferenceNumber = result.ReferenceNumber;
            response.Status = result.Status;
            Workflow.WorkflowBO bo = new GetCitizenAffairWorkflow().GetWorkflow(result, appSettings.Value.ConnectionString, apiConfigSettings.Value.Url, encryptionService, environment);
            bo.ServiceID = result.CitizenAffairID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(response);
        }

        [HttpDelete]
        [Route("CitizenAffair/{id:int}")]
        public IActionResult DeleteCitizenAffair(int id)
        {
            var result = DbClientFactory<CitizenAffairClient>.Instance.DeleteCitizenAffair(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpPatch]
        [Route("CitizenAffair/{id}")]
        public IActionResult ModifyCitizenAffair(int id, [FromBody]JsonPatchDocument<CitizenAffairPutModel> value)
        {
            CitizenAffairSaveResponseModel response = new CitizenAffairSaveResponseModel();
            var result = DbClientFactory<CitizenAffairClient>.Instance.PatchCitizenAffair(appSettings.Value.ConnectionString, id, value);
            response.CitizenAffairId = result.CitizenAffairID;
            response.ReferenceNumber = result.ReferenceNumber;
            response.Status = result.Status;
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var res = DbClientFactory<CitizenAffairClient>.Instance.GetCitizenAffairPreview(appSettings.Value.ConnectionString, id, result.FromID, lang);
            if (res.RequestType == "1")
                PersonalReportPreviewToPdf(res, lang);
            if (res.RequestType == "0")
                FieldVisitPreviewToPdf(res, lang);
            Workflow.WorkflowBO bo = new GetCitizenAffairWorkflow().GetWorkflow(result, appSettings.Value.ConnectionString, apiConfigSettings.Value.Url, encryptionService, environment);
            bo.ServiceID = result.CitizenAffairID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(response);
        }

        [HttpPost]
        [Route("CitizenAffair/Report")]
        public async Task<IActionResult> Export([FromBody]CitizenAffairReportRequestModel report, string type)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<CitizenAffairClient>.Instance.GetReportExportList(appSettings.Value.ConnectionString, report, lang);
                return await this.Export(type, result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpGet]
        [Route("CitizenAffair/AllModulesPendingCount/{UserID:int}")]
        public IActionResult GetAllModulesPendingTasksCount(int userID)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var response = DbClientFactory<CitizenAffairClient>.Instance.GetAllModulesPendingTasksCount(appSettings.Value.ConnectionString, userID, lang);
            return Ok(response);
        }

        [HttpPost]
        [Route("CitizenAffair/preview/{id:int}")]
        public IActionResult PrintPreview(int id)
        {
            try
            {
                int userID = int.Parse(Request.Query["UserID"]);
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<CitizenAffairClient>.Instance.GetCitizenAffairPreview(appSettings.Value.ConnectionString, id, userID, lang);
                if (!System.IO.File.Exists(Directory.GetCurrentDirectory() + @"\Downloads\" + result.ReferenceNumber + ".pdf"))
                {
                    if (result.RequestType == "1")
                        PersonalReportPreviewToPdf(result, lang);
                    if (result.RequestType == "0")
                        FieldVisitPreviewToPdf(result, lang);
                }

                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private void PersonalReportPreviewToPdf(CitizenAffairPreview_model result, string lang)
        {
            string appPath = Directory.GetCurrentDirectory();
            string downloadPath = appPath + @"\Downloads\";
            string finalPdf = downloadPath + result.ReferenceNumber + ".pdf";
            string htmlString = System.IO.File.ReadAllText(appPath + @"/Templates/CA_PersonalVisitPDFTemplate.html");

            ArabicConstantModel arabicValues = new ArabicConstantModel();
            htmlString = htmlString.Replace("{{CAPVTitle}}", arabicValues.GetCAReport);

            htmlString = htmlString.Replace("{{NameValue}}", result.PersonalReport.Name);
            htmlString = htmlString.Replace("{{NameLabel}}", arabicValues.GetCAName);

            htmlString = htmlString.Replace("{{DesigValue}}", result.PersonalReport.Destination);
            htmlString = htmlString.Replace("{{DesigLabel}}", arabicValues.GetCADestination);
            htmlString = htmlString.Replace("{{EmployerValue}}", result.PersonalReport.Employer);
            htmlString = htmlString.Replace("{{EmployerLabel}}", arabicValues.GetEmployee);

            htmlString = htmlString.Replace("{{MaritalValue}}", result.PersonalReport.MaritalStatus);
            htmlString = htmlString.Replace("{{MaritalLabel}}", arabicValues.GetMaritalStatus);
            htmlString = htmlString.Replace("{{SalaryValue}}", result.PersonalReport.MonthlySalary);
            htmlString = htmlString.Replace("{{SalaryLabel}}", arabicValues.GetMonthlySalary);

            htmlString = htmlString.Replace("{{RefValue}}", result.ReferenceNumber);
            htmlString = htmlString.Replace("{{RefLabel}}", arabicValues.GetReferenceNo);
            htmlString = htmlString.Replace("{{PhNoValue}}", result.PersonalReport.PhoneNumber);
            htmlString = htmlString.Replace("{{PhNoLabel}}", arabicValues.GetPhoneNumber);
            htmlString = htmlString.Replace("{{KidNoValue}}", result.PersonalReport.NoOfChildrens);
            htmlString = htmlString.Replace("{{KidNoLabel}}", arabicValues.GetNoOfKids);

            htmlString = htmlString.Replace("{{DateValue}}", result.CreatedDateTime?.ToString("dd/MM/yyyy"));
            htmlString = htmlString.Replace("{{DateLabel}}", arabicValues.GetDate);
            htmlString = htmlString.Replace("{{AgeValue}}", result.PersonalReport.Age);
            htmlString = htmlString.Replace("{{AgeLabel}}", arabicValues.GetAge);
            htmlString = htmlString.Replace("{{CityValue}}", result.PersonalReport.Emirates);
            htmlString = htmlString.Replace("{{CityLabel}}", arabicValues.GetEmiritesCities);

            htmlString = htmlString.Replace("{{ReportObjValue}}", result.PersonalReport.ReportObjectives);
            htmlString = htmlString.Replace("{{ReportObjLabel}}", arabicValues.GetReportObjectives);

            htmlString = htmlString.Replace("{{NotesValue}}", result.PersonalReport.FindingNotes);
            htmlString = htmlString.Replace("{{NotesLabel}}", arabicValues.GetFindingNotes);

            htmlString = htmlString.Replace("{{RecommendationValue}}", result.PersonalReport.Recommendation);
            htmlString = htmlString.Replace("{{RecommendationLabel}}", arabicValues.GetRecommendation);

            htmlString = htmlString.Replace("{{CreatorSignLabel}}", arabicValues.GetSignatureofApprover);
            if (result.SignaturePhotoCreator != null)
            {
                SqlParameter[] getparam = {
                    new SqlParameter("@P_UserProfileId", result.CreatedBy),
                    new SqlParameter("@P_Language", lang)
                };
                List<UserProfileGetModel> attachments = SqlHelper.ExecuteProcedureReturnData<List<UserProfileGetModel>>(appSettings.Value.ConnectionString, "Get_UserProfileByID", r => r.TranslateAsUserProfileList(), getparam);

                foreach (var item in attachments)
                {
                    if (!string.IsNullOrEmpty(item.SignaturePhoto) && !string.IsNullOrEmpty(item.SignaturePhoto))
                    {
                        htmlString = htmlString.Replace("{{ShowCTdEmpty}}", "none");
                        htmlString = htmlString.Replace("{{ShowCTdImage}}", string.Empty);
                        htmlString = htmlString.Replace("{{ShowCreatorSignature}}", string.Empty);
                        htmlString = htmlString.Replace("{{CreatorSignatureValue}}", Path.Combine(apiConfigSettings.Value.Url + @"/api/attachment/download?filename=" + item.SignaturePhoto + @"&guid=" + item.SignaturePhotoID));
                    }
                }
            }
            else
            {
                htmlString = htmlString.Replace("{{ShowCTdEmpty}}", string.Empty);
                htmlString = htmlString.Replace("{{ShowCTdImage}}", "none");
                htmlString = htmlString.Replace("{{CreatorSignatureValue}}", string.Empty);
                htmlString = htmlString.Replace("{{ShowCreatorSignature}}", "none");
            }

            htmlString = htmlString.Replace("{{CreatorValue}}", result.Creator);
            htmlString = htmlString.Replace("{{CreatorLabel}}", arabicValues.GetCreatorName);

            if (result.SignaturePhotoApprover != null)
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
                        htmlString = htmlString.Replace("{{ShowATdEmpty}}", "none");
                        htmlString = htmlString.Replace("{{ShowATdImage}}", string.Empty);
                        htmlString = htmlString.Replace("{{ShowApproverSignature}}", string.Empty);
                        htmlString = htmlString.Replace("{{ApproverSignatureValue}}", Path.Combine(apiConfigSettings.Value.Url + @"/api/attachment/download?filename=" + item.SignaturePhoto + @"&guid=" + item.SignaturePhotoID));
                    }
                }
            }
            else
            {
                htmlString = htmlString.Replace("{{ShowATdEmpty}}", string.Empty);
                htmlString = htmlString.Replace("{{ShowATdImage}}", "none");
                htmlString = htmlString.Replace("{{ApproverSignatureValue}}", string.Empty);
                htmlString = htmlString.Replace("{{ShowApproverSignature}}", "none");
            }

            htmlString = htmlString.Replace("{{ApproverSignLabel}}", arabicValues.GetSignatureofApprover);

            htmlString = htmlString.Replace("{{ApproverValue}}", result.ApproverNameId);
            htmlString = htmlString.Replace("{{ApproverLabel}}", arabicValues.GetAppproverName);

            if (!string.IsNullOrEmpty(result.PersonalReport.PersonalProfileName))
            {
                htmlString = htmlString.Replace("{{ShowPhoto}}", string.Empty);
                htmlString = htmlString.Replace("{{PhotoValue}}", Path.Combine(apiConfigSettings.Value.Url + @"/api/attachment/download?filename=" + result.PersonalReport.PersonalProfileName + @"&guid=" + result.PersonalReport.ProfilePhotoID));
            }
            else
            {
                htmlString = htmlString.Replace("{{PhotoValue}}", string.Empty);
                htmlString = htmlString.Replace("{{ShowPhoto}}", "none");
            }

            PdfConvert.Convert(new PdfConversionSettings
            {
                Content = htmlString,
                OutputPath = finalPdf
            });
        }

        private void FieldVisitPreviewToPdf(CitizenAffairPreview_model result, string lang)
        {
            string appPath = Directory.GetCurrentDirectory();
            string downloadPath = appPath + @"\Downloads\";
            string finalPdf = downloadPath + result.ReferenceNumber + ".pdf";
            string htmlString = System.IO.File.ReadAllText(appPath + @"/Templates/CA_FieldVisitPDFTemplate.html");

            ArabicConstantModel arabicValues = new ArabicConstantModel();
            htmlString = htmlString.Replace("{{CAFVTitle}}", arabicValues.Getfieldvisitreport);

            htmlString = htmlString.Replace("{{RefNoValue}}", result.ReferenceNumber);
            htmlString = htmlString.Replace("{{RefNoLabel}}", arabicValues.GetCAReference);

            htmlString = htmlString.Replace("{{ReqDetails}}", arabicValues.GetRequestDetails);
            htmlString = htmlString.Replace("{{VisitDetails}}", arabicValues.GetVisitDetails);

            htmlString = htmlString.Replace("{{NameValue}}", result.FieldVisit?.Name);
            htmlString = htmlString.Replace("{{NameLabel}}", arabicValues.GetCAName);
            htmlString = htmlString.Replace("{{DayValue}}", arabicValues.GetArabicDay(result.FieldVisit?.Date?.DayOfWeek.ToString()));
            htmlString = htmlString.Replace("{{DayLabel}}", arabicValues.GetDay);

            htmlString = htmlString.Replace("{{PhNoValue}}", result.FieldVisit?.PhoneNumber);
            htmlString = htmlString.Replace("{{PhNoLabel}}", arabicValues.GetPhoneNumber);
            htmlString = htmlString.Replace("{{DateValue}}", result.FieldVisit.Date?.ToString("dd/MM/yyyy"));
            htmlString = htmlString.Replace("{{DateLabel}}", arabicValues.GetDate);

            htmlString = htmlString.Replace("{{ReqTypeValue}}", "تقرير الزيارة الميدانية");
            htmlString = htmlString.Replace("{{ReqTypeLabel}}", arabicValues.GetCARequestType);
            htmlString = htmlString.Replace("{{LocValue}}", result.FieldVisit?.LocationEmirites);
            htmlString = htmlString.Replace("{{LocLabel}}", arabicValues.GetEmiritesCities);

            htmlString = htmlString.Replace("{{PrepByValue}}", "الديوان");
            htmlString = htmlString.Replace("{{PrepByLabel}}", arabicValues.GetPreparedBy);
            htmlString = htmlString.Replace("{{ReqByalue}}", result.FieldVisit?.RequetsedBy);
            htmlString = htmlString.Replace("{{ReqByLabel}}", arabicValues.GetRequestedBy);

            htmlString = htmlString.Replace("{{VisitObjValue}}", result.FieldVisit.VisitObjective);
            htmlString = htmlString.Replace("{{VisitObjLabel}}", arabicValues.GetFieldVisit);

            htmlString = htmlString.Replace("{{NotesValue}}", result.FieldVisit.FindingNotes);
            htmlString = htmlString.Replace("{{NotesLabel}}", arabicValues.GetFindingNotes);

            var sb = new StringBuilder();
            if (result.Documents != null)
            {
                sb.Append("<div align='right'>");
                foreach (var item in result.Documents)
                {
                    if (!string.IsNullOrEmpty(item.AttachmentGuid) || !string.IsNullOrEmpty(item.AttachmentGuid))
                    {
                        string filePath = Path.Combine(apiConfigSettings.Value.Url + @"/api/attachment/download?filename=" + item.AttachmentsName + @"&guid=" + item.AttachmentGuid);
                        sb.AppendFormat("<a href='{0}' target='_blank'><u>{1}</u></a>", filePath, item.AttachmentsName);
                        sb.Append(" ");
                    }
                }

                sb.Append("</div>");
            }

            htmlString = htmlString.Replace("{{AttachmentValue}}", sb.ToString());
            htmlString = htmlString.Replace("{{AttachmentLabel}}", arabicValues.GetAttachments);

            htmlString = htmlString.Replace("{{CreatorSignLabel}}", arabicValues.GetSignatureofApprover);
            if (result.SignaturePhotoCreator != null)
            {
                SqlParameter[] getparam = {
                    new SqlParameter("@P_UserProfileId", result.CreatedBy),
                    new SqlParameter("@P_Language", lang)
                };
                List<UserProfileGetModel> attachments = SqlHelper.ExecuteProcedureReturnData<List<UserProfileGetModel>>(appSettings.Value.ConnectionString, "Get_UserProfileByID", r => r.TranslateAsUserProfileList(), getparam);

                foreach (var item in attachments)
                {
                    if (!string.IsNullOrEmpty(item.SignaturePhoto) && !string.IsNullOrEmpty(item.SignaturePhoto))
                    {
                        htmlString = htmlString.Replace("{{ShowCTdEmpty}}", "none");
                        htmlString = htmlString.Replace("{{ShowCTdImage}}", string.Empty);
                        htmlString = htmlString.Replace("{{ShowCreatorSignature}}", string.Empty);
                        htmlString = htmlString.Replace("{{CreatorSignatureValue}}", Path.Combine(apiConfigSettings.Value.Url + @"/api/attachment/download?filename=" + item.SignaturePhoto + @"&guid=" + item.SignaturePhotoID));
                    }
                }
            }
            else
            {
                htmlString = htmlString.Replace("{{ShowCTdEmpty}}", string.Empty);
                htmlString = htmlString.Replace("{{ShowCTdImage}}", "none");
                htmlString = htmlString.Replace("{{CreatorSignatureValue}}", string.Empty);
                htmlString = htmlString.Replace("{{ShowCreatorSignature}}", "none");
            }

            htmlString = htmlString.Replace("{{CreatorValue}}", result.Creator);
            htmlString = htmlString.Replace("{{CreatorLabel}}", arabicValues.GetCreatorName);

            if (result.SignaturePhotoApprover != null)
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
                        htmlString = htmlString.Replace("{{ShowATdEmpty}}", "none");
                        htmlString = htmlString.Replace("{{ShowATdImage}}", string.Empty);
                        htmlString = htmlString.Replace("{{ShowApproverSignature}}", string.Empty);
                        htmlString = htmlString.Replace("{{ApproverSignatureValue}}", Path.Combine(apiConfigSettings.Value.Url + @"/api/attachment/download?filename=" + item.SignaturePhoto + @"&guid=" + item.SignaturePhotoID));
                    }
                }
            }
            else
            {
                htmlString = htmlString.Replace("{{ShowATdEmpty}}", string.Empty);
                htmlString = htmlString.Replace("{{ShowATdImage}}", "none");
                htmlString = htmlString.Replace("{{ApproverSignatureValue}}", string.Empty);
                htmlString = htmlString.Replace("{{ShowApproverSignature}}", "none");
            }

            htmlString = htmlString.Replace("{{ApproverSignLabel}}", arabicValues.GetSignatureofApprover);

            htmlString = htmlString.Replace("{{ApproverValue}}", result.ApproverNameId);
            htmlString = htmlString.Replace("{{ApproverLabel}}", arabicValues.GetAppproverName);

            PdfConvert.Convert(new PdfConversionSettings
            {
                Content = htmlString,
                OutputPath = finalPdf
            });
        }
    }
}