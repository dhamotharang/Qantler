using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.CitizenAffair;
using RulersCourt.Repository;
using RulersCourt.Repository.CAComplaintSuggestions;
using System;
using Workflow.Interface;

namespace RulersCourt.Controllers
{
    [EnableCors("AllowOrigin")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/")]
    public class CAComplaintSuggestionsController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IWorkflowClient workflow;

        public CAComplaintSuggestionsController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app)
        {
            appSettings = app;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("CAComplaintSuggestion/{id}")]
        public IActionResult GetCAComplaintSuggestionsByID(int id)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<CAComplaintSuggestionsClient>.Instance.GetCAComplaintSuggestionsByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("CAComplaintSuggestion")]
        public IActionResult SaveCAComplaintSuggestions([FromBody]CAComplaintSuggestionsPostModel cAComplaintSuggestions)
        {
            CAComplaintSuggestionsSaveResponseModel response = new CAComplaintSuggestionsSaveResponseModel();
            var result = DbClientFactory<CAComplaintSuggestionsClient>.Instance.PostCAComplaintSuggestions(appSettings.Value.ConnectionString, cAComplaintSuggestions);
            CAComplaintSuggestionsSaveResponseModel res = new CAComplaintSuggestionsSaveResponseModel();
            res.CAComplaintSuggestionsID = result.CAComplaintSuggestionsID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new CAComplaintSuggestionsGetWorkflow().GetCAComplaintSuggestionsWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.CAComplaintSuggestionsID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPut]
        [Route("CAComplaintSuggestion")]
        public IActionResult UpdateCitizenAffairComplaintSuggestions([FromBody]CAComplaintSuggestionsPutModel cAComplaintSuggestions)
        {
            var result = DbClientFactory<CAComplaintSuggestionsClient>.Instance.PutCAComplaintSuggestions(appSettings.Value.ConnectionString, cAComplaintSuggestions);
            CAComplaintSuggestionsSaveResponseModel res = new CAComplaintSuggestionsSaveResponseModel();
            res.CAComplaintSuggestionsID = result.CAComplaintSuggestionsID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new CAComplaintSuggestionsGetWorkflow().GetCAComplaintSuggestionsWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.CAComplaintSuggestionsID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPatch]
        [Route("CAComplaintSuggestion/{id}")]
        public IActionResult ModifyCAComplaintSuggestions(int id, [FromBody]JsonPatchDocument<CAComplaintSuggestionsPutModel> value)
        {
            var result = DbClientFactory<CAComplaintSuggestionsClient>.Instance.PatchCAComplaintSuggestions(appSettings.Value.ConnectionString, id, value);
            CAComplaintSuggestionsSaveResponseModel res = new CAComplaintSuggestionsSaveResponseModel();
            res.CAComplaintSuggestionsID = result.CAComplaintSuggestionsID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new CAComplaintSuggestionsGetWorkflow().GetCAComplaintSuggestionsWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.CAComplaintSuggestionsID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }
    }
}