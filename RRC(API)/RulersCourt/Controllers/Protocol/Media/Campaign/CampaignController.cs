using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Campaign;
using RulersCourt.Repository.Protocol.Media.Campaign;
using System;
using Workflow.Interface;

namespace RulersCourt.Controllers
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class CampaignController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IWorkflowClient workflow;

        public CampaignController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app)
        {
            appSettings = app;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("Campaign/{id}")]
        public IActionResult GetCampaignByID(int id)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<CampaignClient>.Instance.GetCampaignRequestByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("Campaign")]
        public IActionResult SaveCircular([FromBody]CampaignPostModel campaign)
        {
            var result = DbClientFactory<CampaignClient>.Instance.PostCampaign(appSettings.Value.ConnectionString, campaign);
            CampaignSaveResponseModel res = new CampaignSaveResponseModel();
            res.CampaignID = result.CampaignID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new CampaignGetWorkFlowModel().GetDiwanIdentityWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.CampaignID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPut]
        [Route("Campaign")]
        public IActionResult UpdateCampaign([FromBody]CampaignPutModel campaign)
        {
            var result = DbClientFactory<CampaignClient>.Instance.PutCampaign(appSettings.Value.ConnectionString, campaign);
            CampaignSaveResponseModel res = new CampaignSaveResponseModel();
            res.CampaignID = result.CampaignID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new CampaignGetWorkFlowModel().GetDiwanIdentityWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.CampaignID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpDelete]
        [Route("Campaign/{id:int}")]
        public IActionResult DeleteCampaign(int id)
        {
            var result = DbClientFactory<CampaignClient>.Instance.DeleteCampaign(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpPatch]
        [Route("Campaign/{id}")]
        public IActionResult ModifyCampaign(int id, [FromBody]JsonPatchDocument<CampaignPutModel> value)
        {
            var result = DbClientFactory<CampaignClient>.Instance.PatchCampaign(appSettings.Value.ConnectionString, id, value);
            CampaignSaveResponseModel res = new CampaignSaveResponseModel();
            res.CampaignID = result.CampaignID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new CampaignGetWorkFlowModel().GetDiwanIdentityWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.CampaignID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPost]
        [Route("Campaign/CommunicationChat")]
        public IActionResult CommunicationChat([FromBody]CampaignCommunicationHistory value)
        {
            var result = DbClientFactory<CampaignClient>.Instance.SaveCommunicationChat(appSettings.Value.ConnectionString, value);
            return Ok(result);
        }
    }
}