using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Photo;
using RulersCourt.Repository.Protocol.Media.Photo;
using RulersCourt.Repository.Protocol.Media.PhotoRequest;
using System;
using Workflow.Interface;

namespace RulersCourt.Controllers
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class PhotoController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;
        private readonly IWorkflowClient workflow;

        public PhotoController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
        {
            appSettings = app;
            environment = env;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("Photo/{id}")]
        public IActionResult GetPhotoByID(int id)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<PhotoRequestClient>.Instance.GetPhotoID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("Photo")]
        public IActionResult SavePhoto([FromBody]PhotoPostModel photoRequest)
        {
            var result = DbClientFactory<PhotoRequestClient>.Instance.PostPhoto(appSettings.Value.ConnectionString, photoRequest);
            PhotoSaveResponseModel response = new PhotoSaveResponseModel();
            response.PhotoID = result.PhotoID;
            response.ReferenceNumber = result.ReferenceNumber;
            response.Status = result.Status;
            Workflow.WorkflowBO bo = new PhotoRequestGetWorkflow().GetPhotoRequestWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = result.PhotoID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(response);
        }

        [HttpPut]
        [Route("Photo")]
        public IActionResult UpdatePhoto([FromBody]PhotoPutModel photo)
        {
            var result = DbClientFactory<PhotoRequestClient>.Instance.PutPhoto(appSettings.Value.ConnectionString, photo);
            PhotoSaveResponseModel response = new PhotoSaveResponseModel();
            response.PhotoID = result.PhotoID;
            response.ReferenceNumber = result.ReferenceNumber;
            response.Status = result.Status;
            Workflow.WorkflowBO bo = new PhotoRequestGetWorkflow().GetPhotoRequestWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = result.PhotoID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(response);
        }

        [HttpDelete]
        [Route("Photo/{id:int}")]
        public IActionResult DeletePhoto(int id)
        {
            var result = DbClientFactory<PhotoRequestClient>.Instance.DeletePhoto(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpPatch]
        [Route("Photo/{id}")]
        public IActionResult ModifyPhoto(int id, [FromBody]JsonPatchDocument<PhotoPutModel> value)
        {
            var result = DbClientFactory<PhotoRequestClient>.Instance.PatchPhoto(appSettings.Value.ConnectionString, id, value);
            PhotoSaveResponseModel response = new PhotoSaveResponseModel();
            response.PhotoID = result.PhotoID;
            response.ReferenceNumber = result.ReferenceNumber;
            response.Status = result.Status;
            Workflow.WorkflowBO bo = new PhotoRequestGetWorkflow().GetPhotoRequestWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = result.PhotoID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(response);
        }

        [HttpPost]
        [Route("Photo/CommunicationChat")]
        public IActionResult CommunicationChat([FromBody]PhotoCommunicationHistory value)
        {
            var result = DbClientFactory<PhotoRequestClient>.Instance.SaveCommunicationChat(appSettings.Value.ConnectionString, value);
            return Ok(result);
        }
    }
}