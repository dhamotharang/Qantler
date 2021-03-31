using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.HR.Training;
using RulersCourt.Models.Training;
using RulersCourt.Repository.Training;
using System;
using System.Collections.Generic;
using Workflow.Interface;

namespace RulersCourt.Controllers.Training
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class TrainingController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;
        private readonly IWorkflowClient workflow;

        public TrainingController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
        {
            appSettings = app;
            environment = env;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("Training/{id}")]
        public IActionResult GetTrainingByID(int id)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<TrainingClient>.Instance.GetTrainingByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("Training")]
        public IActionResult SaveTraining([FromBody]TrainingPostModel training)
        {
            TrainingSaveResponseModel response = new TrainingSaveResponseModel();
            var result = DbClientFactory<TrainingClient>.Instance.SaveTraining(appSettings.Value.ConnectionString, training);
            response.TrainingID = result.TrainingID;
            response.ReferenceNumber = result.ReferenceNumber;
            response.Status = result.Status;
            Workflow.WorkflowBO bo = new GetTrainingWorkflow().GetWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = response.TrainingID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(response);
        }

        [HttpPut]
        [Route("Training")]
        public IActionResult UpdateTraining([FromBody]TrainingPutModel training)
        {
            TrainingSaveResponseModel response = new TrainingSaveResponseModel();
            var result = DbClientFactory<TrainingClient>.Instance.PutTraining(appSettings.Value.ConnectionString, training);
            response.TrainingID = result.TrainingID;
            response.ReferenceNumber = result.ReferenceNumber;
            response.Status = result.Status;
            Workflow.WorkflowBO bo = new GetTrainingWorkflow().GetWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = response.TrainingID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(response);
        }

        [HttpDelete]
        [Route("Training/{id:int}")]
        public IActionResult DeleteTraining(int id)
        {
            var result = DbClientFactory<TrainingClient>.Instance.DeleteTraining(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpPut]
        [Route("Training/Attendance/{id:int}")]
        public IActionResult UpdateTrainingCertificate(int id, [FromBody]List<TrainingAttachmentModel> attachment)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<TrainingClient>.Instance.UpdateTrainingCertificate(appSettings.Value.ConnectionString, id, attachment, userID, lang);
            Workflow.WorkflowBO bo = new GetTrainingWorkflow().GetWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = id;
            workflow.StartWorkflow(bo);
            return Ok(result);
        }

        [HttpPatch]
        [Route("Training/{id}")]
        public IActionResult ModifyTraining(int id, [FromBody]JsonPatchDocument<TrainingPutModel> training)
        {
            try
            {
                TrainingSaveResponseModel response = new TrainingSaveResponseModel();
                var result = DbClientFactory<TrainingClient>.Instance.PatchTraining(appSettings.Value.ConnectionString, id, training);
                response.TrainingID = result.TrainingID;
                response.ReferenceNumber = result.ReferenceNumber;
                response.Status = result.Status;
                response.HRManagerUserID = result.HRManagerUserID;
                Workflow.WorkflowBO bo = new GetTrainingWorkflow().GetWorkflow(result, appSettings.Value.ConnectionString);
                bo.ServiceID = response.TrainingID ?? 0;
                workflow.StartWorkflow(bo);
                return Ok(response);
            }
            catch (Exception r)
            {
                return Ok(r.Message);
            }
        }

        [HttpPost]
        [Route("Training/CommunicationChat")]
        public IActionResult CommunicationChat([FromBody]TrainingCommunicationHistory value)
        {
            try
            {
                var result = DbClientFactory<TrainingClient>.Instance.SaveCommunicationChat(appSettings.Value.ConnectionString, value);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }

            return Ok(1);
        }
    }
}