using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Photographer;
using RulersCourt.Repository.Protocol.Media.Photographer;
using System;
using Workflow.Interface;

namespace RulersCourt.Controllers
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class PhotographerController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IWorkflowClient workflow;

        public PhotographerController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app)
        {
            appSettings = app;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("Photographer/{id}")]
        public IActionResult GetPhotographerByID(int id)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<PhotographerClient>.Instance.GetPhotographerByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("Photographer")]
        public IActionResult SaveCircular([FromBody]PhotographerPostModel photographer)
        {
            PhotographerSaveResponseModel response = new PhotographerSaveResponseModel();
            var result = DbClientFactory<PhotographerClient>.Instance.PostPhotographer(appSettings.Value.ConnectionString, photographer);
            PhotographerSaveResponseModel res = new PhotographerSaveResponseModel();
            res.PhotoGrapherID = result.PhotoGrapherID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new PhotoGrapherGetWorkFlowModel().GetDiwanIdentityWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = result.PhotoGrapherID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPut]
        [Route("Photographer")]
        public IActionResult UpdatePhotographer([FromBody]PhotographerPutModel photographer)
        {
            var result = DbClientFactory<PhotographerClient>.Instance.PutPhotographer(appSettings.Value.ConnectionString, photographer);
            PhotographerSaveResponseModel res = new PhotographerSaveResponseModel();
            res.PhotoGrapherID = result.PhotoGrapherID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new PhotoGrapherGetWorkFlowModel().GetDiwanIdentityWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = result.PhotoGrapherID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpDelete]
        [Route("Photographer/{id:int}")]
        public IActionResult DeletePhotographer(int id)
        {
            var result = DbClientFactory<PhotographerClient>.Instance.DeletePhotoGraph(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpPatch]
        [Route("Photographer/{id}")]
        public IActionResult ModifyPhotographer(int id, [FromBody]JsonPatchDocument<PhotographerPutModel> value)
        {
            var result = DbClientFactory<PhotographerClient>.Instance.PatchPhotographer(appSettings.Value.ConnectionString, id, value);
            PhotographerSaveResponseModel res = new PhotographerSaveResponseModel();
            res.PhotoGrapherID = result.PhotoGrapherID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new PhotoGrapherGetWorkFlowModel().GetDiwanIdentityWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = result.PhotoGrapherID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPost]
        [Route("Photographer/CommunicationChat")]
        public IActionResult CommunicationChat([FromBody]PhotographerCommunicationHistory value)
        {
            var result = DbClientFactory<PhotographerClient>.Instance.SaveCommunicationChat(appSettings.Value.ConnectionString, value);
            return Ok(result);
        }
    }
}