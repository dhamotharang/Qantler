using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Vehicle.CarCompany;
using RulersCourt.Repository.Vehicle;
using System;
using System.Threading.Tasks;
using System.Web;

namespace RulersCourt.Controllers.Vehicle
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class VehicleCarCompanyController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;

        public VehicleCarCompanyController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            environment = env;
            appSettings = app;
        }

        [HttpGet]
        [Route("VehicleCarCompanyList")]
        public IActionResult GetCarCompanyDetailsList()
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];

            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<CarCompanyClient>.Instance.GetCarCompanyList(appSettings.Value.ConnectionString, paramUserID, lang);
            return Ok(result);
        }

        [HttpGet]
        [Route("VehicleCarCompany/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetCarCompanyDetails(int pageNumber, int pageSize)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            DateTime? paramCreatedDateFrom = string.IsNullOrEmpty(parameters["CreatedDateFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["CreatedDateFrom"]);
            DateTime? paramCreatedDateTo = string.IsNullOrEmpty(parameters["CreatedDateTo"]) ? (DateTime?)null : DateTime.Parse(parameters["CreatedDateTo"]);
            var paramUserID = parameters["UserID"];
            var paramSmartSearch = parameters["SmartSearch"];
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<CarCompanyClient>.Instance.GetCarCompany(appSettings.Value.ConnectionString, pageNumber, pageSize, paramUserID, paramCreatedDateFrom, paramCreatedDateTo, lang, paramSmartSearch);
            return Ok(result);
        }

        [HttpGet]
        [Route("VehicleCarCompany/{id}")]
        public IActionResult GetCarCompanyDetailsByID(int id)
        {
            int userID = int.Parse(Request.Query["UserID"]);
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<CarCompanyClient>.Instance.GetCarCompanyByID(appSettings.Value.ConnectionString, id, userID, lang);

            return Ok(result);
        }

        [HttpPost]
        [Route("VehicleCarCompany")]
        public IActionResult SaveCarCompany([FromBody]CarCompanyPostModel carCompany)
        {
            var result = DbClientFactory<CarCompanyClient>.Instance.PostContact(appSettings.Value.ConnectionString, carCompany);

            return Ok(result);
        }

        [HttpPut]
        [Route("VehicleCarCompany")]
        public IActionResult UpdateCarCompany([FromBody]CarCompanyPutModel carCompany)
        {
            var result = DbClientFactory<CarCompanyClient>.Instance.PutCarCompany(appSettings.Value.ConnectionString, carCompany);

            return Ok(result);
        }

        [HttpDelete]
        [Route("VehicleCarCompany/{id:int}")]
        public IActionResult DeleteCarCompany(int id)
        {
            var result = DbClientFactory<CarCompanyClient>.Instance.DeleteCarCompany(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpPatch]
        [Route("VehicleCarCompany/{id}")]
        public IActionResult ModifyCarCompany(int id, [FromBody]JsonPatchDocument<CarCompanyPutModel> value)
        {
            var result = DbClientFactory<CarCompanyClient>.Instance.PatchCarCompany(appSettings.Value.ConnectionString, id, value);

            return Ok(result);
        }

        [HttpGet]
        [Route("VehicleCarCompany/export")]
        public async Task<IActionResult> Export()
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var parameCreatedDateTo = parameters["CreatedDateTo"];
            var parameCreatedDateFrom = parameters["CreatedDateFrom"];
            var paramSmartSearch = parameters["SmartSearch"];
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<CarCompanyClient>.Instance.GetCarCompanyReportExportList(appSettings.Value.ConnectionString, lang, parameCreatedDateTo, parameCreatedDateFrom, paramSmartSearch);
            return await this.Export("Excel", result);
        }
    }
}
