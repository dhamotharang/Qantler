using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Contact;
using RulersCourt.Repository.Contact;
using System;
using System.Threading.Tasks;
using System.Web;
using Workflow.Interface;

namespace RulersCourt.Controllers.Contact
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class ContactController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IWorkflowClient workflow;
        private readonly IHostingEnvironment environment;

        public ContactController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            environment = env;
            appSettings = app;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [Route("Contact/{id}")]
        public IActionResult GetContactByID(int id)
        {
            int userID = int.Parse(Request.Query["UserID"]);
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<ContactClient>.Instance.GetContactByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("Contact")]
        public IActionResult SaveContact([FromBody]ContactPostModel contact)
        {
            var result = DbClientFactory<ContactClient>.Instance.PostContact(appSettings.Value.ConnectionString, contact);
            ContactSaveResponseModel res = new ContactSaveResponseModel();
            res.ContactID = result.ContactID;
            res.ReferenceNumber = result.ReferenceNumber;
            return Ok(res);
        }

        [HttpPut]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("Contact")]
        public IActionResult UpdateContact([FromBody]ContactPutModel contact)
        {
            var result = DbClientFactory<ContactClient>.Instance.PutContact(appSettings.Value.ConnectionString, contact);
            ContactSaveResponseModel res = new ContactSaveResponseModel();
            res.ContactID = result.ContactID;
            res.ReferenceNumber = result.ReferenceNumber;
            return Ok(res);
        }

        [HttpDelete]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("Contact/{id:int}")]
        public IActionResult DeleteContact(int id)
        {
            var result = DbClientFactory<ContactClient>.Instance.DeleteContact(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpPatch]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("Contact/{id}")]
        public IActionResult ModifyContact(int id, [FromBody]JsonPatchDocument<ContactPutModel> value)
        {
            var result = DbClientFactory<ContactClient>.Instance.PatchContact(appSettings.Value.ConnectionString, id, value);
            ContactSaveResponseModel res = new ContactSaveResponseModel();
            res.ContactID = result.ContactID;
            res.ReferenceNumber = result.ReferenceNumber;
            return Ok(res);
        }

        [HttpGet]
        [Route("Contact/Home/List/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetAllContact(int pageNumber, int pageSize)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramType = parameters["Type"];
            var paramDepartment = parameters["Department"];
            var paramUserName = parameters["UserName"];
            var paramEntityName = parameters["EntityName"];
            var paramDesignation = parameters["Destination"];
            var paramGovernmentEntity = parameters["GovernmentEntity"];
            var paramEmailId = parameters["EmailId"];
            var paramPhoneNumber = parameters["PhoneNumber"];
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var response = DbClientFactory<ContactClient>.Instance.GetInternalContact(appSettings.Value.ConnectionString, pageNumber, pageSize, paramType, paramDepartment, paramUserName, paramEntityName, paramDesignation, paramEmailId, paramPhoneNumber, paramGovernmentEntity, lang);
            return Ok(response);
        }

        [HttpPost]
        [Route("InternalContact/Report")]
        public async Task<IActionResult> Export([FromBody]InternalContactReportRequestModel report, string type)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<ContactReportClient>.Instance.GetReportExportList(appSettings.Value.ConnectionString, report, lang);
                return await this.Export(type, result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("ExternalContact/Report")]
        public async Task<IActionResult> ExternalExport([FromBody]ExternalContactReportRequestModel report, string type)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<ContactReportClient>.Instance.GetExternalReportExportList(appSettings.Value.ConnectionString, report, lang);
                return await this.Export(type, result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
    }
}