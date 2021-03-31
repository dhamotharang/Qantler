using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Vehicle.Vehicles;
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
    public class VehicleManagementController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;

        public VehicleManagementController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            environment = env;
            appSettings = app;
        }

        [HttpGet]
        [Route("VehicleManagement/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetAllVehicles(int pageNumber, int pageSize)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramPlateNumber = parameters["PlateNumber"];
            var paramUserID = parameters["UserID"];
            var paramPlateColor = parameters["PlateColor"];
            var paramDepartmentOffice = parameters["DepartmentOffice"];
            var paramSmartSearch = parameters["SmartSearch"];
            var paramAlternativeVehicle = parameters["IsAlternativeVehicle"];

            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<VehiclesClient>.Instance.GetVehicleManagement(appSettings.Value.ConnectionString, pageNumber, pageSize, paramUserID, paramPlateNumber, paramPlateColor, lang, paramSmartSearch, paramDepartmentOffice, paramAlternativeVehicle);
            return Ok(result);
        }

        [HttpGet]
        [Route("VehicleManagement/{id}")]
        public IActionResult GetVehicleDetailsByID(int id)
        {
            int userID = int.Parse(Request.Query["UserID"]);
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<VehiclesClient>.Instance.GetVehicleByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("VehicleManagement")]
        public IActionResult SaveVehicle([FromBody]VehiclePostModel vehicle)
        {
            var result = DbClientFactory<VehiclesClient>.Instance.PostVehicle(appSettings.Value.ConnectionString, vehicle);
            return Ok(result);
        }

        [HttpPut]
        [Route("VehicleManagement")]
        public IActionResult UpdateVehicle([FromBody]VehiclePutModel vehicle)
        {
            try
            {
                var result = DbClientFactory<VehiclesClient>.Instance.PutVehicle(appSettings.Value.ConnectionString, vehicle);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpDelete]
        [Route("VehicleManagement/{id:int}")]
        public IActionResult DeleteVehicle(int id)
        {
            var result = DbClientFactory<VehiclesClient>.Instance.DeleteVehicle(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpPatch]
        [Route("VehicleManagement/{id}")]
        public IActionResult ModifyVehicle(int id, [FromBody]JsonPatchDocument<VehiclePutModel> value)
        {
            var result = DbClientFactory<VehiclesClient>.Instance.PatchVehicle(appSettings.Value.ConnectionString, id, value);
            return Ok(result);
        }

        [HttpPost]
        [Route("VehicleManagementLogService")]
        public IActionResult SaveVehicleLogService([FromBody]VehicleLogServicePost vehicle)
        {
            try
            {
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var result = DbClientFactory<VehiclesClient>.Instance.PostLogaServiceVehicle(appSettings.Value.ConnectionString, vehicle);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpGet]
        [Route("VehicleManagement/export")]
        public async Task<IActionResult> Export()
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramPlateNumber = parameters["PlateNumber"];
            var paramUserID = parameters["UserID"];
            var paramPlateColor = parameters["PlateColor"];
            var paramAlternativeVehicle = parameters["IsAlternativeVehicle"];

            var paramDepartmentOffice = parameters["DepartmentOffice"];
            var paramSmartSearch = parameters["SmartSearch"];
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<VehiclesClient>.Instance.GetVehicleReportExportList(appSettings.Value.ConnectionString, paramUserID, paramPlateNumber, paramPlateColor, lang, paramSmartSearch, paramDepartmentOffice, paramAlternativeVehicle);
            return await this.Export("Excel", result);
        }

        [HttpGet]
        [Route("VehicleManagementAllPlateNumber")]
        public IActionResult GetAllPlateNumberPlateColor()
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<VehiclesClient>.Instance.GetVehicleManagementAllPlateNumber(appSettings.Value.ConnectionString, paramUserID, lang);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetVehicleManagementLogService/{id:int}")]
        public IActionResult GetVehicleLogService(int id)
        {
            try
            {
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var paramType = Convert.ToInt32(parameters["Type"]);
                var result = DbClientFactory<VehiclesClient>.Instance.GetLogaServiceVehicle(appSettings.Value.ConnectionString, id, paramType);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpGet]
        [Route("GetVehicleManagementLogServiceReport/{id:int}")]
        public async Task<IActionResult> GetVehicleLogServiceReport(int id)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<VehiclesClient>.Instance.GetLogaServiceVehicleReport(appSettings.Value.ConnectionString, id, lang);
                return await this.Export("Excel", result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
    }
}
