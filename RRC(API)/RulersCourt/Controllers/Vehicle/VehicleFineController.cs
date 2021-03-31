using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Vehicle.VehicleFine;
using RulersCourt.Models.Vehicle.VehicleRequest;
using RulersCourt.Repository.Vehicle;
using System;
using System.Threading.Tasks;
using System.Web;
using Workflow.Interface;

namespace RulersCourt.Controllers.Vehicle
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class VehicleFineController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IWorkflowClient workflow;
        private readonly IHostingEnvironment environment;

        public VehicleFineController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            environment = env;
            appSettings = app;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [Route("FineCarList")]
        public IActionResult GetFineCarList()
        {
            try
            {
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var paramPlateNumber = parameters["PlateNumber"];
                var paramVehicleID = parameters["VehicleID"];
                var paramUserID = parameters["UserID"];
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<CarCompanyClient>.Instance.GetVehicleList(appSettings.Value.ConnectionString, paramUserID, paramPlateNumber, paramVehicleID);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpGet]
        [Route("Fine/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetFine(int pageNumber, int pageSize)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];
            var paramStatus = parameters["Status"];
            var paramIssuedAgainstDepartment = parameters["IssuedAgainstDepartment"];
            var paramIssuedAgainstName = parameters["IssuedAgainstName"];
            DateTime? paramFineDateFrom = string.IsNullOrEmpty(parameters["FineDateFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["FineDateFrom"]);
            DateTime? paramFineDateTo = string.IsNullOrEmpty(parameters["FineDateTo"]) ? (DateTime?)null : DateTime.Parse(parameters["FineDateTo"]);
            var paramPlateNumber = parameters["PlateNumber"];
            var paramSmartSearch = parameters["SmartSearch"];
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<FineClient>.Instance.GetFineCarList(appSettings.Value.ConnectionString, pageNumber, pageSize, paramUserID, paramStatus, paramFineDateFrom, paramFineDateTo, lang, paramSmartSearch, paramPlateNumber, paramIssuedAgainstDepartment, paramIssuedAgainstName);
            return Ok(result);
        }

        [HttpGet]
        [Route("Fine/{id}")]
        public IActionResult GetFineByID(int id)
        {
            int userID = int.Parse(Request.Query["UserID"]);
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<FineClient>.Instance.GetFineCarByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("Fine")]
        public IActionResult SaveFine([FromBody]VehicleFinePostModel fine)
        {
            try
            {
                int userID = int.Parse(Request.Query["UserID"]);
                var result = DbClientFactory<FineClient>.Instance.PostFineCar(appSettings.Value.ConnectionString, fine);
                Workflow.WorkflowBO bo = new VehicleRequestRemainderWorkflow().GetVehicleSubmissionWorkflow(result, appSettings.Value.ConnectionString, userID);
                bo.ServiceID = result.VehicleFineID ?? 0;
                workflow.StartWorkflow(bo);
                return Ok(result.VehicleFineID);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPut]
        [Route("Fine")]
        public IActionResult UpdateFine([FromBody]VehicleFinePutModel fine)
        {
            var result = DbClientFactory<FineClient>.Instance.PutFineCar(appSettings.Value.ConnectionString, fine);
            return Ok(result);
        }

        [HttpDelete]
        [Route("Fine/{id:int}")]
        public IActionResult DeleteFine(int id)
        {
            var result = DbClientFactory<FineClient>.Instance.DeleteFineCar(appSettings.Value.ConnectionString, id);
            return Ok(true);
        }

        [HttpPatch]
        [Route("Fine/{id}")]
        public IActionResult ModifyFine(int id, [FromBody]JsonPatchDocument<VehicleFinePutModel> value)
        {
            var result = DbClientFactory<FineClient>.Instance.PatchFineCar(appSettings.Value.ConnectionString, id, value);
            return Ok(result);
        }

        [HttpGet]
        [Route("Fine/export")]
        public async Task<IActionResult> Export()
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];
            var paramStatus = parameters["Status"];
            var paramIssuedAgainstDepartment = parameters["IssuedAgainstDepartment"];
            var paramIssuedAgainstName = parameters["IssuedAgainstName"];
            DateTime? paramFineDateFrom = string.IsNullOrEmpty(parameters["FineDateFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["FineDateFrom"]);
            DateTime? paramFineDateTo = string.IsNullOrEmpty(parameters["FineDateTo"]) ? (DateTime?)null : DateTime.Parse(parameters["FineDateTo"]);
            var paramPlateNumber = parameters["PlateNumber"];
            var paramSmartSearch = parameters["SmartSearch"];
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parametype = parameters["Type"];
            var result = DbClientFactory<FineClient>.Instance.GetVehicleFineReportExportList(appSettings.Value.ConnectionString, lang, paramStatus, paramFineDateFrom,            paramFineDateTo, paramSmartSearch, paramPlateNumber, paramIssuedAgainstDepartment, paramIssuedAgainstName);
            return await this.Export(parametype, result);
        }

        [HttpPost]
        [Route("SendaRemainder")]
        public IActionResult SendRemainder([FromBody]SendRemainderModel sendRemainder)
        {
            try
            {
                int userID = int.Parse(Request.Query["UserID"]);
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<FineClient>.Instance.GetFineCarByID(appSettings.Value.ConnectionString, sendRemainder.VehicleFineID, userID, lang);
                Workflow.WorkflowBO bo = new VehicleRequestRemainderWorkflow().GetVehicleRemainderWorkflow(result, appSettings.Value.ConnectionString, sendRemainder, userID);
                bo.ServiceID = result.VehicleFineID ?? 0;
                workflow.StartWorkflow(bo);
                return Ok(true);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
    }
}
