using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.DiwanIdentity;
using RulersCourt.Repository;
using RulersCourt.Repository.Protocol.Media.DiwanIdentity;
using System;
using Workflow.Interface;

namespace RulersCourt.Controllers
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class DiwanIdentityController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;
        private readonly IWorkflowClient workflow;

        public DiwanIdentityController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            appSettings = app;
            environment = env;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("DiwanIdentity/{id}")]
        public IActionResult GetDiwanIdentityByID(int id)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<DiwanIdentityClient>.Instance.GetDiwanIdentityByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("DiwanIdentity")]
        public IActionResult SaveDiwanIdentity([FromBody]DiwanIdentityPostModel diwanIdentity)
        {
            var result = DbClientFactory<DiwanIdentityClient>.Instance.PostDiwanIdentity(appSettings.Value.ConnectionString, diwanIdentity);
            DiwanIdentitySaveResponseModel res = new DiwanIdentitySaveResponseModel();
            res.DiwanIdentityID = result.DiwanIdentityID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new DiwanIdentityGetWorkflow().GetDiwanIdentityWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = result.DiwanIdentityID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPut]
        [Route("DiwanIdentity")]
        public IActionResult UpdateDiwanIdentity([FromBody]DiwanIdentityPutModel diwanIdentity)
        {
            var result = DbClientFactory<DiwanIdentityClient>.Instance.PutDiwanIdentity(appSettings.Value.ConnectionString, diwanIdentity);
            DiwanIdentitySaveResponseModel res = new DiwanIdentitySaveResponseModel();
            res.DiwanIdentityID = result.DiwanIdentityID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new DiwanIdentityGetWorkflow().GetDiwanIdentityWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = result.DiwanIdentityID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPatch]
        [Route("DiwanIdentity/{id}")]
        public IActionResult ModifyDiwanIdentity(int id, [FromBody]JsonPatchDocument<DiwanIdentityPutModel> value)
        {
            var result = DbClientFactory<DiwanIdentityClient>.Instance.PatchDiwanIdentity(appSettings.Value.ConnectionString, id, value);
            DiwanIdentitySaveResponseModel res = new DiwanIdentitySaveResponseModel();
            res.DiwanIdentityID = result.DiwanIdentityID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new DiwanIdentityGetWorkflow().GetDiwanIdentityWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = result.DiwanIdentityID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPost]
        [Route("DiwanIdentity/CommunicationChat")]
        public IActionResult CommunicationChat([FromBody]DiwanIdentityCommunicationHistory value)
        {
            var result = DbClientFactory<DiwanIdentityClient>.Instance.SaveCommunicationChat(appSettings.Value.ConnectionString, value);
            return Ok(result);
        }
    }
}