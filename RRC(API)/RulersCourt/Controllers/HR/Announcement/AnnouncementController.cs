using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Announcement;
using RulersCourt.Repository;
using RulersCourt.Repository.Announcement;
using System;
using Workflow.Interface;

namespace RulersCourt.Controllers.Announcement
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class AnnouncementController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;
        private readonly IWorkflowClient workflow;

        public AnnouncementController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            appSettings = app;
            environment = env;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("Announcement/{id}")]
        public IActionResult GetAnnouncementByID(int id)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<AnnouncementClient>.Instance.GetAnnouncementByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("Announcement")]
        public IActionResult SaveAnnouncement([FromBody]AnnouncementPostModel announcement)
        {
            var result = DbClientFactory<AnnouncementClient>.Instance.PostAnnouncement(appSettings.Value.ConnectionString, announcement);
            AnnouncementSaveResponseModel res = new AnnouncementSaveResponseModel();
            res.AnnouncementID = result.AnnouncementID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new AnnouncementGetWorkflow().GetAnnouncementWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.AnnouncementID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPut]
        [Route("Announcement")]
        public IActionResult UpdateAnnouncement([FromBody]AnnouncementPutModel announcement)
        {
            var result = DbClientFactory<AnnouncementClient>.Instance.PutAnnouncement(appSettings.Value.ConnectionString, announcement);
            AnnouncementSaveResponseModel res = new AnnouncementSaveResponseModel();
            res.AnnouncementID = result.AnnouncementID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new AnnouncementGetWorkflow().GetAnnouncementWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.AnnouncementID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPatch]
        [Route("Announcement/{id}")]
        public IActionResult ModifyAnnouncement(int id, [FromBody]JsonPatchDocument<AnnouncementPutModel> value)
        {
            var result = DbClientFactory<AnnouncementClient>.Instance.PatchAnnouncement(appSettings.Value.ConnectionString, id, value);
            AnnouncementSaveResponseModel res = new AnnouncementSaveResponseModel();
            res.AnnouncementID = result.AnnouncementID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new AnnouncementGetWorkflow().GetAnnouncementWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.AnnouncementID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }
    }
}