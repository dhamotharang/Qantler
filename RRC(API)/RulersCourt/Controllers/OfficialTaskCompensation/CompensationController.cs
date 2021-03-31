using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.OfficalTaskCompensation.Compensation;
using RulersCourt.Repository.OfficalTaskCompensation.Compensation;
using Shark.PdfConvert;
using System;
using System.IO;
using Workflow.Interface;

namespace RulersCourt.Controllers.OfficalTaskCompensation
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class CompensationController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IWorkflowClient workflow;
        private readonly IHostingEnvironment environment;

        public CompensationController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            environment = env;
            appSettings = app;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("Compensation/{id}")]
        public IActionResult GetCompensationByID(int id)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                int userID = int.Parse(Request.Query["UserID"]);
                var result = DbClientFactory<CompensationClient>.Instance.GetCompensationByID(appSettings.Value.ConnectionString, id, userID, lang);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("Compensation")]
        public IActionResult SaveCompensation([FromBody]CompensationPostModel compensation)
        {
            var result = DbClientFactory<CompensationClient>.Instance.PostCompensation(appSettings.Value.ConnectionString, compensation);
            CompensationSaveResponseModel res = new CompensationSaveResponseModel();
            res.CompensationID = result.CompensationID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new CompensationGetWorkflow().GetCompensationWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.CompensationID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPut]
        [Route("Compensation")]
        public IActionResult UpdateCompensation([FromBody]CompensationPutModel compensation)
        {
            var result = DbClientFactory<CompensationClient>.Instance.PutCompensation(appSettings.Value.ConnectionString, compensation);
            CompensationSaveResponseModel res = new CompensationSaveResponseModel();
            res.CompensationID = result.CompensationID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new CompensationGetWorkflow().GetCompensationWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.CompensationID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPatch]
        [Route("Compensation/{id}")]
        public IActionResult ModifyCompensation(int id, [FromBody]JsonPatchDocument<CompensationPutModel> value)
        {
            var result = DbClientFactory<CompensationClient>.Instance.PatchCompensation(appSettings.Value.ConnectionString, id, value);
            CompensationSaveResponseModel res = new CompensationSaveResponseModel();
            res.CompensationID = result.CompensationID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            res.HRManagerUserID = result.HRManagerUserID;
            Workflow.WorkflowBO bo = new CompensationGetWorkflow().GetCompensationWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.CompensationID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPost]
        [Route("Compensation/CommunicationChat")]
        public IActionResult CommunicationChat([FromBody]CompensationCommunicationHistoryModel value)
        {
            var result = DbClientFactory<CompensationClient>.Instance.SaveCommunicationChat(appSettings.Value.ConnectionString, value);

            return Ok(result);
        }

        [HttpPost]
        [Route("Compensation/GenerateAdministrativeDecision/{id:int}")]
        public IActionResult PrintPreview(int id)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<CompensationClient>.Instance.GetCompensationPreview(appSettings.Value.ConnectionString, id, lang);
                PreviewPDF(result);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private void PreviewPDF(CompensationPreviewModel result)
        {
            string appPath = Directory.GetCurrentDirectory();
            string downloadPath = appPath + @"\Downloads\";
            string finalPdf = downloadPath + result.CompensationReferenceNo + ".pdf";
            string htmlString = System.IO.File.ReadAllText(appPath + @"/Templates/GeneralAdministrative.html");

            ArabicConstantModel arabicValues = new ArabicConstantModel();
            htmlString = htmlString.Replace("{{OfficialTaskReferenceNo}}", result.OfficialTaskReferenceNo);
            htmlString = htmlString.Replace("{{StartDateYear}}", result.StartDate.Value.ToString("yyyy").ToString());
            htmlString = htmlString.Replace("{{NoOfDays}}", result.NoOfDays.ToString());
            htmlString = htmlString.Replace("{{StartDate}}", result.StartDate.Value.ToString("dd/MM/yyyy").Replace("-", "/"));
            htmlString = htmlString.Replace("{{Day}}", arabicValues.GetArabicDay(result.StartDate.Value.DayOfWeek.ToString()));
            htmlString = htmlString.Replace("{{OfficialTaskDescription}}", result.OfficialTaskDescription);

            // htmlString = htmlString.Replace("{{AssigneeEmployeeID}}", result.AssigneeEmployeeID);
            // htmlString = htmlString.Replace("{{AssigneeName}}", result.AssigneeName);
            htmlString = htmlString.Replace("{{EmployeeDetails}}", result.EmployeeDetails);
            htmlString = htmlString.Replace("{{OfficialTaskCreatorName}}", result.OfficialTaskCreatorName);
            htmlString = htmlString.Replace("{{OfficialTaskCreatorDesignation}}", result.OfficialTaskCreatorDesignation);
            htmlString = htmlString.Replace("{{CreatedDate}}", DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/"));

            PdfConvert.Convert(new PdfConversionSettings
            {
                Content = htmlString,
                OutputPath = finalPdf
            });
        }
    }
}
