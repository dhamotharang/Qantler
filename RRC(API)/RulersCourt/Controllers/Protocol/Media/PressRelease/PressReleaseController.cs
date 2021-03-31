using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.PressRelease;
using RulersCourt.Repository.Protocol.Media.PressRelease;
using System;
using Workflow.Interface;

namespace RulersCourt.Controllers
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Route("api/")]
    [Authorize]
    public class PressReleaseController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IWorkflowClient workflow;

        public PressReleaseController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app)
        {
            appSettings = app;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("PressRelease/{id}")]
        public IActionResult GetPressReleaseByID(int id)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<PressReleaseClient>.Instance.GetPressReleaseByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("PressRelease")]
        public IActionResult SavePressRelease([FromBody]PressReleasePostModel pressRelease)
        {
            var result = DbClientFactory<PressReleaseClient>.Instance.PostPressRelease(appSettings.Value.ConnectionString, pressRelease);
            PressReleaseSaveResponseModel res = new PressReleaseSaveResponseModel();
            res.PressReleaseID = result.PressReleaseID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new PressReleaseGetWorkFlowModel().GetDiwanIdentityWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = result.PressReleaseID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPut]
        [Route("PressRelease")]
        public IActionResult UpdatePressRelease([FromBody]PressReleasePutModel pressRelease)
        {
            var result = DbClientFactory<PressReleaseClient>.Instance.PutPressRelease(appSettings.Value.ConnectionString, pressRelease);
            PressReleaseSaveResponseModel res = new PressReleaseSaveResponseModel();
            res.PressReleaseID = result.PressReleaseID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new PressReleaseGetWorkFlowModel().GetDiwanIdentityWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = result.PressReleaseID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpDelete]
        [Route("PressRelease/{id:int}")]
        public IActionResult DeletePressRelease(int id)
        {
            var result = DbClientFactory<PressReleaseClient>.Instance.DeletePressRelease(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpPatch]
        [Route("PressRelease/{id}")]
        public IActionResult ModifyPressRelease(int id, [FromBody]JsonPatchDocument<PressReleasePutModel> value)
        {
            var result = DbClientFactory<PressReleaseClient>.Instance.PatchPressRelease(appSettings.Value.ConnectionString, id, value);
            PressReleaseSaveResponseModel res = new PressReleaseSaveResponseModel();
            res.PressReleaseID = result.PressReleaseID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new PressReleaseGetWorkFlowModel().GetDiwanIdentityWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = result.PressReleaseID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPost]
        [Route("PressRelease/CommunicationChat")]
        public IActionResult CommunicationChat([FromBody]PressReleaseCommunicationHistory value)
        {
            var result = DbClientFactory<PressReleaseClient>.Instance.SaveCommunicationChat(appSettings.Value.ConnectionString, value);
            return Ok(result);
        }
    }
}