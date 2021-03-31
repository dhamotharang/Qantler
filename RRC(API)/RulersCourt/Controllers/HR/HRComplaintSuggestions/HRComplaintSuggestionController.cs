using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.HRComplaintSuggestions;
using RulersCourt.Repository.HRComplaintSuggestions;
using System;
using Workflow.Interface;

namespace RulersCourt.Controllers
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class HRComplaintSuggestionController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IWorkflowClient workflow;

        public HRComplaintSuggestionController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app)
        {
            appSettings = app;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("HRComplaintSuggestion/{id}")]
        public IActionResult GetHRComplaintSuggestionsByID(int id)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<HRComplaintSuggestionsClient>.Instance.GetHRComplaintSuggestionsByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("HRComplaintSuggestion")]
        public IActionResult SaveHRComplaintSuggestions([FromBody]HRComplaintSuggestionsPostModel hrComplaintSuggestions)
        {
            HRComplaintSuggestionsSaveResponseModel response = new HRComplaintSuggestionsSaveResponseModel();
            var result = DbClientFactory<HRComplaintSuggestionsClient>.Instance.PostHRComplaintSuggestions(appSettings.Value.ConnectionString, hrComplaintSuggestions);
            HRComplaintSuggestionsSaveResponseModel res = new HRComplaintSuggestionsSaveResponseModel();
            res.HRComplaintSuggestionsID = result.HRComplaintSuggestionsID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new HRComplaintSuggestionsGetWorkflow().GetHRComplaintSuggestionsWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.HRComplaintSuggestionsID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPut]
        [Route("HRComplaintSuggestion")]
        public IActionResult UpdateHRComplaintSuggestions([FromBody]HRComplaintSuggestionsPutModel hrComplaintSuggestions)
        {
            var result = DbClientFactory<HRComplaintSuggestionsClient>.Instance.PutHRComplaintSuggestions(appSettings.Value.ConnectionString, hrComplaintSuggestions);
            HRComplaintSuggestionsSaveResponseModel res = new HRComplaintSuggestionsSaveResponseModel();
            res.HRComplaintSuggestionsID = result.HRComplaintSuggestionsID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new HRComplaintSuggestionsGetWorkflow().GetHRComplaintSuggestionsWorkflow(result, appSettings.Value.ConnectionString);
            workflow.StartWorkflow(bo);
            bo.ServiceID = res.HRComplaintSuggestionsID ?? 0;
            return Ok(res);
        }

        [HttpPatch]
        [Route("HRComplaintSuggestion/{id}")]
        public IActionResult ModifyHRComplaintSuggestions(int id, [FromBody]JsonPatchDocument<HRComplaintSuggestionsPutModel> value)
        {
            var result = DbClientFactory<HRComplaintSuggestionsClient>.Instance.PatchHRComplaintSuggestions(appSettings.Value.ConnectionString, id, value);
            HRComplaintSuggestionsSaveResponseModel res = new HRComplaintSuggestionsSaveResponseModel();
            res.HRComplaintSuggestionsID = result.HRComplaintSuggestionsID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new HRComplaintSuggestionsGetWorkflow().GetHRComplaintSuggestionsWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.HRComplaintSuggestionsID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }
    }
}