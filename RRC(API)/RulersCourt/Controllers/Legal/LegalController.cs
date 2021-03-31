using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Legal;
using RulersCourt.Repository.Legal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Workflow.Interface;

namespace RulersCourt.Controllers
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class LegalController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IWorkflowClient workflow;
        private readonly IHostingEnvironment environment;

        public LegalController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            environment = env;
            appSettings = app;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("Legal/{id}")]
        public IActionResult GetLegalByID(int id)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<LegalClient>.Instance.GetLegalByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("Legal")]
        public IActionResult SaveLegal([FromBody]LegalPostModel legal)
        {
            var result = DbClientFactory<LegalClient>.Instance.PostLegal(appSettings.Value.ConnectionString, legal);
            LegalSaveResponseModel res = new LegalSaveResponseModel();
            res.LegalID = result.LegalID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new LegalGetWorkflow().GetLegalWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.LegalID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPut]
        [Route("Legal")]
        public IActionResult UpdateLegal([FromBody]LegalPutModel legal)
        {
            var result = DbClientFactory<LegalClient>.Instance.PutLegal(appSettings.Value.ConnectionString, legal);
            LegalSaveResponseModel res = new LegalSaveResponseModel();
            res.LegalID = result.LegalID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new LegalGetWorkflow().GetLegalWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.LegalID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPatch]
        [Route("Legal/{id}")]
        public IActionResult ModifyLegal(int id, [FromBody]JsonPatchDocument<LegalPutModel> value)
        {
            var result = DbClientFactory<LegalClient>.Instance.PatchLegal(appSettings.Value.ConnectionString, id, value);
            LegalSaveResponseModel res = new LegalSaveResponseModel();
            res.LegalID = result.LegalID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new LegalGetWorkflow().GetLegalWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.LegalID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPost]
        [Route("Legal/Report")]
        public async Task<IActionResult> Export([FromBody]LegalReportRequestModel report, string type)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<LegalReportClient>.Instance.GetReportExporttList(appSettings.Value.ConnectionString, report, lang);
                return await this.Export(type, result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("Legal/CommunicationChat")]
        public IActionResult CommunicationChat([FromBody]LegalCommunicationHistory value)
        {
            var result = DbClientFactory<LegalClient>.Instance.SaveCommunicationChat(appSettings.Value.ConnectionString, value);
            return Ok(result);
        }

        [HttpPost]
        [Route("Legal/Labels/{id}")]
        public IActionResult SaveLabel(int id, [FromBody]List<LegalKeywordsModel> legal)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int paramUserID = parameters["UserID"] is null ? 0 : int.Parse(parameters["UserID"]);
            DbClientFactory<LegalClient>.Instance.Postlabel(appSettings.Value.ConnectionString, legal, id, paramUserID);
            return Ok(id);
        }
    }
}