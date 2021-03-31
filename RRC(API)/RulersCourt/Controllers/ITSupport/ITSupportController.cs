using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.ITSupport;
using RulersCourt.Repository;
using RulersCourt.Repository.ITSupportClient;
using System;
using Workflow.Interface;

namespace RulersCourt.Controllers.ITSupport
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class ITSupportController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;
        private readonly IWorkflowClient workflow;

        public ITSupportController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
        {
            appSettings = app;
            environment = env;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("ITSupport/{id}")]
        public IActionResult GetITSupportByID(int id)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<ITSupportClient>.Instance.GetITSupportID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("ITSupport")]
        public IActionResult SaveITSupport([FromBody]ITSupportPostModel itSupport)
        {
            ITSupportSaveResponseModel response = new ITSupportSaveResponseModel();
            var result = DbClientFactory<ITSupportClient>.Instance.PostITSupport(appSettings.Value.ConnectionString, itSupport);
            response.ITSupportID = result.ITSupportID;
            response.ReferenceNumber = result.ReferenceNumber;
            response.Status = result.Status;
            Workflow.WorkflowBO bo = new ITSupportWorkFlow().GetITSupportWorkflow(result, appSettings.Value.ConnectionString);
            return Ok(response);
        }

        [HttpPut]
        [Route("ITSupport")]
        public IActionResult UpdateITSupport([FromBody]ITSupportPutModel itSupport)
        {
            ITSupportSaveResponseModel response = new ITSupportSaveResponseModel();
            var result = DbClientFactory<ITSupportClient>.Instance.PutITSupport(appSettings.Value.ConnectionString, itSupport);
            response.ITSupportID = result.ITSupportID;
            response.ReferenceNumber = result.ReferenceNumber;
            return Ok(response);
        }

        [HttpDelete]
        [Route("ITSupport/{id:int}")]
        public IActionResult DeleteITSupport(int id)
        {
            return Ok();
        }

        [HttpPatch]
        [Route("ITSupport/{id}")]
        public IActionResult ModifyITSupport(int id, [FromBody]JsonPatchDocument<ITSupportPutModel> value)
        {
            var result = DbClientFactory<ITSupportClient>.Instance.PatchITSupport(appSettings.Value.ConnectionString, id, value);
            ITSupportSaveResponseModel response = new ITSupportSaveResponseModel();
            response.ITSupportID = result.ITSupportID;
            response.ReferenceNumber = result.ReferenceNumber;
            response.Status = result.Status;
            Workflow.WorkflowBO bo = new ITSupportWorkFlow().GetITSupportWorkflow(result, appSettings.Value.ConnectionString);
            return Ok(response);
        }

        [HttpGet]
        [Route("ITSupport/SyncDate")]
        public IActionResult GetITSupportLastSync()
        {
            var result = DbClientFactory<ITSupportClient>.Instance.GetITSupportLastSync(appSettings.Value.ConnectionString);
            return Ok(result);
        }
    }
}