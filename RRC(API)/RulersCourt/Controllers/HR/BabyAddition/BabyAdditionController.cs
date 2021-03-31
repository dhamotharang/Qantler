using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.BabyAddition;
using RulersCourt.Repository;
using RulersCourt.Repository.BabyAddition;
using System;
using Workflow.Interface;

namespace RulersCourt.Controllers.BabyAddition
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class BabyAdditionController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;
        private readonly IWorkflowClient workflow;

        public BabyAdditionController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
        {
            appSettings = app;
            environment = env;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("BabyAddition/{id}")]
        public IActionResult GetBabyAdditionByID(int id)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<BabyAdditionClient>.Instance.GetBabyAdditionByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("BabyAddition")]
        public IActionResult SaveBabyAddition([FromBody]BabyAdditionPostModel babyAddition)
        {
            BabyAdditionSaveResponseModel response = new BabyAdditionSaveResponseModel();
            var result = DbClientFactory<BabyAdditionClient>.Instance.PostBabyAddition(appSettings.Value.ConnectionString, babyAddition);
            BabyAdditionSaveResponseModel res = new BabyAdditionSaveResponseModel();
            res.BabyAdditionID = result.BabyAdditionID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new BabyAdditionGetWorkflow().GetBabyAdditionWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.BabyAdditionID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPut]
        [Route("BabyAddition")]
        public IActionResult UpdateBabyAddition([FromBody]BabyAdditionPutModel babyAddition)
        {
            var result = DbClientFactory<BabyAdditionClient>.Instance.PutBabyAddition(appSettings.Value.ConnectionString, babyAddition);
            BabyAdditionSaveResponseModel res = new BabyAdditionSaveResponseModel();
            res.BabyAdditionID = result.BabyAdditionID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new BabyAdditionGetWorkflow().GetBabyAdditionWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.BabyAdditionID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPatch]
        [Route("BabyAddition/{id}")]
        public IActionResult ModifyBabyAddition(int id, [FromBody]JsonPatchDocument<BabyAdditionPutModel> value)
        {
            var result = DbClientFactory<BabyAdditionClient>.Instance.PatchBabyAddition(appSettings.Value.ConnectionString, id, value);
            BabyAdditionSaveResponseModel res = new BabyAdditionSaveResponseModel();
            res.BabyAdditionID = result.BabyAdditionID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new BabyAdditionGetWorkflow().GetBabyAdditionWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.BabyAdditionID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }
    }
}