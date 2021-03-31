using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.CVBank;
using RulersCourt.Models.HR.CVBank;
using RulersCourt.Repository.HR.CVBank;
using System;
using System.Threading.Tasks;
using System.Web;
using Workflow.Interface;

namespace RulersCourt.Controllers.CVBank
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class CVBankController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IWorkflowClient workflow;
        private readonly IHostingEnvironment environment;

        public CVBankController(IOptions<ConnectionSettingsModel> app, IServiceProvider serviceProvider, IHostingEnvironment env)
            : base(app, env)
        {
            appSettings = app;
            environment = env;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("CVBank/{id}")]
        public IActionResult GetCVBankByID(int id)
        {
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<CVBankClient>.Instance.GetCVBankByID(appSettings.Value.ConnectionString, id, userID);
            return Ok(result);
        }

        [HttpPost]
        [Route("CVBank")]
        public IActionResult SaveCVBank([FromBody]CVBankPostModel cvBank)
        {
            var result = DbClientFactory<CVBankClient>.Instance.PostCVBank(appSettings.Value.ConnectionString, cvBank);
            CVBankSaveResponseModel res = new CVBankSaveResponseModel();
            res.CVBankId = result.CVBankId;
            res.ReferenceNumber = result.ReferenceNumber;
            return Ok(res);
        }

        [HttpPut]
        [Route("CVBank")]
        public IActionResult UpdateCVBank([FromBody]CVBankPutModel cvBank)
        {
            var result = DbClientFactory<CVBankClient>.Instance.PutCVBank(appSettings.Value.ConnectionString, cvBank);
            CVBankSaveResponseModel res = new CVBankSaveResponseModel();
            res.CVBankId = result.CVBankId;
            res.ReferenceNumber = result.ReferenceNumber;
            return Ok(res);
        }

        [HttpDelete]
        [Route("CVBank/{id:int}")]
        public IActionResult DeleteCVBank(int id)
        {
            var result = DbClientFactory<CVBankClient>.Instance.DeleteCVBank(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpPatch]
        [Route("CVBank/{id}")]
        public IActionResult ModifyCVBank(int id, [FromBody]JsonPatchDocument<CVBankPutModel> value)
        {
            var result = DbClientFactory<CVBankClient>.Instance.PatchCVBank(appSettings.Value.ConnectionString, id, value);
            CVBankSaveResponseModel res = new CVBankSaveResponseModel();
            res.CVBankId = result.CVBankId;
            res.ReferenceNumber = result.ReferenceNumber;
            return Ok(res);
        }

        [HttpGet]
        [Route("CVBank/Home/List/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetAllCVBank(int pageNumber, int pageSize)
        {
            try
            {
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var paramCandidateName = parameters["CandidateName"];
                var paramYearsofExperience = parameters["YearsofExperience"];
                var paramSpecialization = parameters["Specialization"];
                var paramCountry = parameters["CountryofResidence"];
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                DateTime? paramDateFrom = string.IsNullOrEmpty(parameters["DateFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["DateFrom"]);
                DateTime? paramDateTo = string.IsNullOrEmpty(parameters["DateTo"]) ? (DateTime?)null : DateTime.Parse(parameters["DateTo"]);
                var paramSmartSearch = parameters["SmartSearch"];
                var response = DbClientFactory<CVBankClient>.Instance.GetCVBank(appSettings.Value.ConnectionString, pageNumber, pageSize, paramCandidateName, paramYearsofExperience, paramSpecialization, paramCountry, paramDateFrom, paramDateTo, paramSmartSearch, lang);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("CVBank/Report")]
        public async Task<IActionResult> Export([FromBody]CVBankReportRequestModel report, string type)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<CVBankReportClient>.Instance.GetReportExportList(appSettings.Value.ConnectionString, report, lang);
                return await this.Export(type, result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
    }
}
