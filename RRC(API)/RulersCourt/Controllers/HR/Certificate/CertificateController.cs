using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Certificate;
using RulersCourt.Repository.Certificate;
using System;
using Workflow.Interface;

namespace RulersCourt.Controllers
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class CertificateController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IWorkflowClient workflow;
        private readonly IHostingEnvironment environment;

        public CertificateController(IOptions<ConnectionSettingsModel> app, IServiceProvider serviceProvider, IHostingEnvironment env)
            : base(app, env)
        {
            appSettings = app;
            environment = env;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("Certificate/{id}")]
        public IActionResult GetCertificateByID(int id)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<CertificateClient>.Instance.GetCertificateByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("Certificate")]
        public IActionResult SaveCertificate([FromBody]CertificatePostModel certificate)
        {
            var result = DbClientFactory<CertificateClient>.Instance.PostCertificate(appSettings.Value.ConnectionString, certificate);
            CertificateSaveResponseModel res = new CertificateSaveResponseModel();
            res.CertificateId = result.CertificateId;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new GetCertificateWorkflow().GetCertificatesWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.CertificateId ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPut]
        [Route("Certificate")]
        public IActionResult UpdateCertificate([FromBody]CertificatePutModel certificate)
        {
            var result = DbClientFactory<CertificateClient>.Instance.PutCertificate(appSettings.Value.ConnectionString, certificate);
            CertificateSaveResponseModel res = new CertificateSaveResponseModel();
            res.CertificateId = result.CertificateId;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new GetCertificateWorkflow().GetCertificatesWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.CertificateId ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpDelete]
        [Route("Certificate/{id:int}")]
        public IActionResult DeleteCertificate(int id)
        {
            var result = DbClientFactory<CertificateClient>.Instance.DeleteCertificate(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpPatch]
        [Route("Certificate/{id}")]
        public IActionResult ModifyCertificate(int id, [FromBody]JsonPatchDocument<CertificatePutModel> value)
        {
            var result = DbClientFactory<CertificateClient>.Instance.PatchCertificate(appSettings.Value.ConnectionString, id, value);
            CertificateSaveResponseModel res = new CertificateSaveResponseModel();
            res.CertificateId = result.CertificateId;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new GetCertificateWorkflow().GetCertificatesWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.CertificateId ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }
    }
}
