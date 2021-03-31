using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using RulersCourt.Models;
using RulersCourt.Repository;
using RulersCourt.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace RulersCourt.Controllers
{
    [Produces("application/json")]
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class M_MasterLookupController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;
        private readonly EncryptionService encryptionService;
        private readonly string aesEncryptedFileExtension = ".aes";

        public M_MasterLookupController(IOptions<ConnectionSettingsModel> app, IHostingEnvironment env, EncryptionService encryptionSvc)
        {
            appSettings = app;
            environment = env;
            encryptionService = encryptionSvc;
        }

        [HttpGet]
        [Route("Master")]
        public IActionResult GetMaster()
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
            int type = parameters["Type"] == null ? 0 : Convert.ToInt32(parameters["Type"]);
            var search = parameters["Search"];
            var result = DbClientFactory<M_MasterLookupClient>.Instance.GetMasterLookups(appSettings.Value.ConnectionString, userID, type, search, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("Master")]
        public IActionResult SaveMaster([FromBody]M_MasterLookupsPostModel master)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
            int type = parameters["Type"] == null ? 0 : Convert.ToInt32(parameters["Type"]);
            int country = parameters["Country"] == null ? 0 : Convert.ToInt32(parameters["Country"]);
            int emirates = parameters["Emirates"] == null ? 0 : Convert.ToInt32(parameters["Emirates"]);
            var result = DbClientFactory<M_MasterLookupClient>.Instance.PostMasterLookup(appSettings.Value.ConnectionString, userID, master, country, type, lang, emirates);
            return Ok(result);
        }

        [HttpPut]
        [Route("Master")]
        public IActionResult UpdateMaster([FromBody]M_MasterLookupsPutModel master)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
            int type = parameters["Type"] == null ? 0 : Convert.ToInt32(parameters["Type"]);
            int country = parameters["Country"] == null ? 0 : Convert.ToInt32(parameters["Country"]);
            int emirates = parameters["Emirates"] == null ? 0 : Convert.ToInt32(parameters["Emirates"]);
            var result = DbClientFactory<M_MasterLookupClient>.Instance.PutMasterlookup(appSettings.Value.ConnectionString, userID, master, country, type, lang, emirates);
            return Ok(result);
        }

        [HttpDelete]
        [Route("Master/{id:int}")]
        public IActionResult DeleteMaster(int id)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
            int type = parameters["Type"] == null ? 0 : Convert.ToInt32(parameters["Type"]);
            var result = DbClientFactory<M_MasterLookupClient>.Instance.DeleteMasterlookup(appSettings.Value.ConnectionString, userID, id, type);
            return Ok(result);
        }

        [HttpGet]
        [Route("Master/Approver")]
        public IActionResult GetApprover()
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
            int departmentID = parameters["DepartmentID"] == null ? 0 : Convert.ToInt32(parameters["DepartmentID"]);
            var result = DbClientFactory<M_MasterLookupClient>.Instance.GetApprovers(appSettings.Value.ConnectionString, userID, departmentID);
            return Ok(result);
        }

        [HttpPost]
        [Route("Master/Approver")]
        public IActionResult SaveApprover([FromBody]M_ApproverConfigurationModel master)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
            int departmentID = parameters["DepartmentID"] == null ? 0 : Convert.ToInt32(parameters["DepartmentID"]);
            var result = DbClientFactory<M_MasterLookupClient>.Instance.SaveApprovers(appSettings.Value.ConnectionString, userID, departmentID, master);
            return Ok(result);
        }

        [HttpPost]
        [Route("Master/CanApproverRemoved")]
        public IActionResult CheckUserBinding([FromBody]M_ApproverBindingModel master)
        {
            try
            {
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
                var result = DbClientFactory<M_MasterLookupClient>.Instance.CanApproverRemoved(appSettings.Value.ConnectionString, userID, master);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpGet]
        [Route("Master/Departments")]
        public IActionResult Departments()
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
            var result = DbClientFactory<M_MasterLookupClient>.Instance.GetDepartments(appSettings.Value.ConnectionString, userID, lang);
            return Ok(result);
        }

        [HttpGet]
        [Route("Master/Sections")]
        public IActionResult GetSections()
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
            var result = DbClientFactory<M_MasterLookupClient>.Instance.GetSections(appSettings.Value.ConnectionString, userID, lang);
            return Ok(result);
        }

        [HttpGet]
        [Route("Master/Units")]
        public IActionResult GetUnits()
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
            var result = DbClientFactory<M_MasterLookupClient>.Instance.GetUnits(appSettings.Value.ConnectionString, userID, lang);
            return Ok(result);
        }

        [HttpGet]
        [Route("Master/Users")]
        public IActionResult GetUsers()
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
            int departmentID = parameters["DepartmentID"] == null ? 0 : Convert.ToInt32(parameters["DepartmentID"]);
            var search = parameters["Search"];
            var result = DbClientFactory<M_MasterLookupClient>.Instance.GetUsers(appSettings.Value.ConnectionString, userID, search, departmentID, lang);
            return Ok(result);
        }

        [HttpGet]
        [Route("Master/UserManagement{PageNumber:int},{PageSize:int}")]
        public IActionResult GetUserManagement(int pageNumber, int pageSize)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
            var search = parameters["Search"];
            var result = DbClientFactory<M_MasterLookupClient>.Instance.GetUserManagement(appSettings.Value.ConnectionString, userID, search, pageNumber, pageSize, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("Master/UserManagement")]
        public IActionResult SaveUserManagement([FromBody]M_UserManagementModel master)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
            var result = DbClientFactory<M_MasterLookupClient>.Instance.SaveUsers(appSettings.Value.ConnectionString, userID, master);
            return Ok(result);
        }

        [HttpGet]
        [Route("Master/MailRemainder")]
        public IActionResult GetMailRemainder()
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
            var search = parameters["Search"];
            var result = DbClientFactory<M_MasterLookupClient>.Instance.GetMailremainders(appSettings.Value.ConnectionString, userID);
            return Ok(result);
        }

        [HttpPost]
        [Route("Master/MailRemainder")]
        public IActionResult SaveMailRemainder([FromBody]M_MailRemainderModel master)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
            var result = DbClientFactory<M_MasterLookupClient>.Instance.SaveMailremainders(appSettings.Value.ConnectionString, userID, master);
            return Ok(result);
        }

        [HttpPut]
        [Route("Master/MailRemainder")]
        public IActionResult UpdateMailRemainder([FromBody]M_MailRemainderModel master)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
            var result = DbClientFactory<M_MasterLookupClient>.Instance.SaveMailremainders(appSettings.Value.ConnectionString, userID, master);
            return Ok(result);
        }

        [HttpGet]
        [Route("Master/Holiday/{Month:int},{Year:int}")]
        public IActionResult GetHolidays(int month, int year)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
            var result = DbClientFactory<M_HolidayClient>.Instance.GetHoliday(appSettings.Value.ConnectionString, userID, month, year);
            return Ok(result);
        }

        [HttpGet]
        [Route("Master/Holiday")]
        public IActionResult GetHolidaysAttachment()
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
            var result = DbClientFactory<M_HolidayClient>.Instance.GetAttachment(appSettings.Value.ConnectionString);
            return Ok(result);
        }

        [HttpPost]
        [Route("Master/Holiday")]
        public IActionResult UpdateHoliday()
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var fileName = parameters["FileName"];
            var guid = parameters["Guid"];
            int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
            var result = DbClientFactory<M_HolidayClient>.Instance.SaveAttachment(appSettings.Value.ConnectionString, fileName, guid);

            string uploadDir = string.Empty;
            if (environment.IsDevelopment())
            {
                uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            }
            else
            {
                uploadDir = Path.Combine(environment.ContentRootPath, "Uploads");
            }

            try
            {
                string rootFolder = Path.Combine(uploadDir, guid);
                var stream = new FileStream(Path.Combine(rootFolder, fileName + aesEncryptedFileExtension), FileMode.Open);
                var decStream = encryptionService.Decrypt(stream);
                decStream.Position = 0;
                using (ExcelPackage package = new ExcelPackage())
                {
                    package.Load(decStream);
                    ExcelWorksheet workSheet = package.Workbook.Worksheets["Sheet1"];
                    int totalRows = workSheet.Dimension.Rows;
                    List<M_HolidayModel> holidays = new List<M_HolidayModel>();
                    for (int i = 2; i <= totalRows; i++)
                    {
                        holidays.Add(new M_HolidayModel
                        {
                            Holiday = Convert.ToDateTime(workSheet.Cells[i, 1].Value.ToString()),
                            Message = workSheet.Cells[i, 2].Value.ToString()
                        });
                        DbClientFactory<M_HolidayClient>.Instance.SaveHoliday(appSettings.Value.ConnectionString, userID, holidays[holidays.Count - 1]);
                    }
                }
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }

            return Ok(true);
        }
    }
}