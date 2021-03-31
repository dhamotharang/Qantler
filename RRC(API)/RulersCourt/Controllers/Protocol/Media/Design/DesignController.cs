using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Design;
using RulersCourt.Repository.Protocol.Media.Design;
using System;
using Workflow.Interface;

namespace RulersCourt.Controllers
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class DesignController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IWorkflowClient workflow;

        public DesignController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app)
        {
            appSettings = app;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("Design/{id}")]
        public IActionResult GetDesignByID(int id)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<DesignClient>.Instance.GetDesignRequestByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("Design")]
        public IActionResult SaveCircular([FromBody]DesignPostModel design)
        {
            DesignSaveResponseModel response = new DesignSaveResponseModel();
            var result = DbClientFactory<DesignClient>.Instance.PostDesign(appSettings.Value.ConnectionString, design);
            DesignSaveResponseModel res = new DesignSaveResponseModel();
            res.DesignID = result.DesignID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new DesignRequestGetWorkFlow().GetDiwanIdentityWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = result.DesignID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPut]
        [Route("Design")]
        public IActionResult UpdateDesign([FromBody]DesignPutModel design)
        {
            var result = DbClientFactory<DesignClient>.Instance.PutDesign(appSettings.Value.ConnectionString, design);
            DesignSaveResponseModel res = new DesignSaveResponseModel();
            res.DesignID = result.DesignID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new DesignRequestGetWorkFlow().GetDiwanIdentityWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = result.DesignID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpDelete]
        [Route("Design/{id:int}")]
        public IActionResult DeleteDesign(int id)
        {
            var result = DbClientFactory<DesignClient>.Instance.DeleteDesign(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpPatch]
        [Route("Design/{id}")]
        public IActionResult ModifyDesign(int id, [FromBody]JsonPatchDocument<DesignPutModel> value)
        {
            var result = DbClientFactory<DesignClient>.Instance.PatchDesign(appSettings.Value.ConnectionString, id, value);
            DesignSaveResponseModel res = new DesignSaveResponseModel();
            res.DesignID = result.DesignID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new DesignRequestGetWorkFlow().GetDiwanIdentityWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = result.DesignID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPost]
        [Route("Design/CommunicationChat")]
        public IActionResult CommunicationChat([FromBody]DesignCommunicationHistory value)
        {
            var result = DbClientFactory<DesignClient>.Instance.SaveCommunicationChat(appSettings.Value.ConnectionString, value);
            return Ok(result);
        }
    }
}