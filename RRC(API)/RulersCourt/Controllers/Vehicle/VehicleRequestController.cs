using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Vehicle.TripVehicleIssues;
using RulersCourt.Models.Vehicle.VehicleRequest;
using RulersCourt.Models.Vehicle.Vehicles;
using RulersCourt.Repository;
using RulersCourt.Repository.Vehicle;
using Shark.PdfConvert;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Workflow.Interface;

namespace RulersCourt.Controllers.Vehicle
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class VehicleRequestController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IWorkflowClient workflow;
        private readonly IHostingEnvironment environment;

        public VehicleRequestController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            environment = env;
            appSettings = app;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [Route("VehicleRequest/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetAllVehicleRequest(int pageNumber, int pageSize)
        {
            try
            {
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var paramStatus = parameters["Status"];
                var paramType = parameters["Type"];
                var paramUserID = parameters["UserID"];
                var paramRequestType = parameters["RequestType"];
                var paramRequestor = parameters["Requestor"];
                DateTime? tripDateFrom = string.IsNullOrEmpty(parameters["TripDateFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["TripDateFrom"]);
                DateTime? tripDateTo = string.IsNullOrEmpty(parameters["TripDateTo"]) ? (DateTime?)null : DateTime.Parse(parameters["TripDateTo"]);
                var paramDestination = parameters["Destination"];
                var paramSmartSearch = parameters["SmartSearch"];
                var paramRequestorOfficeDepartment = parameters["RequestorOfficeDepartment"];
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<VehicleRequestClient>.Instance.GetVehicleList(appSettings.Value.ConnectionString, pageNumber, pageSize, paramType, paramUserID, paramStatus, paramRequestType, paramRequestor, tripDateFrom, tripDateTo, paramDestination, paramRequestorOfficeDepartment, paramSmartSearch, lang);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpGet]
        [Route("VehicleRequest/Home/Count/{UserID:int}")]
        public IActionResult GetHomeCount(int userID)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var response = DbClientFactory<VehicleRequestClient>.Instance.GetAllModulesPendingTasksCount(appSettings.Value.ConnectionString, userID, lang);
            return Ok(response);
        }

        [HttpGet]
        [Route("VehicleRequest/{id}")]
        public IActionResult GetVehicleRequestByID(int id)
        {
            try
            {
                int userID = int.Parse(Request.Query["UserID"]);
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<VehicleRequestClient>.Instance.GetVehicleRequestByID(appSettings.Value.ConnectionString, id, userID, lang);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("VehicleRequest")]
        public IActionResult SaveVehicleRequest([FromBody]VehicleRequestPostModel vehicleRequest)
        {
            try
            {
                var result = DbClientFactory<VehicleRequestClient>.Instance.PostVehicleRequest(appSettings.Value.ConnectionString, vehicleRequest);
                VehicleRequestSaveResponseModel res = new VehicleRequestSaveResponseModel();
                res.VehicleReqID = result.VehicleReqID;
                res.ReferenceNumber = result.ReferenceNumber;
                res.Status = result.Status;
                Workflow.WorkflowBO bo = new VehicleRequestWorkflow().GetVehicleWorkflow(result, appSettings.Value.ConnectionString);
                bo.ServiceID = res.VehicleReqID ?? 0;
                workflow.StartWorkflow(bo);
                return Ok(res);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPut]
        [Route("VehicleRequest")]
        public IActionResult UpdateVehicle([FromBody]VehicleRequestPutModel vehicleRequest)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<VehicleRequestClient>.Instance.PutVehicleRequest(appSettings.Value.ConnectionString, vehicleRequest, lang);
            VehicleRequestSaveResponseModel res = new VehicleRequestSaveResponseModel();
            res.VehicleReqID = result.VehicleReqID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new VehicleRequestWorkflow().GetVehicleWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.VehicleReqID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpDelete]
        [Route("VehicleRequest/{id:int}")]
        public IActionResult DeleteVehicle(int id)
        {
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<VehiclesClient>.Instance.DeleteVehicle(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpPatch]
        [Route("VehicleRequest/{id}")]
        public IActionResult ModifyVehicleRequest(int id, [FromBody]JsonPatchDocument<VehicleRequestPutModel> value)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<VehicleRequestClient>.Instance.PatchVehicleRequest(appSettings.Value.ConnectionString, id, value, lang);
                VehicleRequestSaveResponseModel res = new VehicleRequestSaveResponseModel();
                res.VehicleReqID = result.VehicleReqID;
                res.ReferenceNumber = result.ReferenceNumber;
                res.Status = result.Status;
                Workflow.WorkflowBO bo = new VehicleRequestWorkflow().GetVehicleWorkflow(result, appSettings.Value.ConnectionString);
                bo.ServiceID = res.VehicleReqID ?? 0;
                workflow.StartWorkflow(bo);
                return Ok(res);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("VehicleRequest/VehicleIssues/{id}")]
        public IActionResult SaveVehicleIssues(int id, [FromBody]List<TripVehicleIssuesPostModel> vehicleIssues)
        {
            try
            {
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                int paramUserID = parameters["UserID"] is null ? 0 : int.Parse(parameters["UserID"]);
                DbClientFactory<VehicleRequestClient>.Instance.PostVehicleIssues(appSettings.Value.ConnectionString, vehicleIssues, id);
                return Ok(id);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("vehicleRequest/Report")]
        public async Task<IActionResult> Export([FromBody]VehicleRequestReportModel report, string type)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<VehicleRequestClient>.Instance.GetReportExporttList(appSettings.Value.ConnectionString, report, lang);
                return await this.Export(type, result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("vehicleRequest/Document")]
        public IActionResult SaveDocument([FromBody]DocumentPostModel report)
        {
            var result = DbClientFactory<DocumentClient>.Instance.PostDocument(appSettings.Value.ConnectionString, report);
            return Ok(result);
        }

        [HttpPut]
        [Route("vehicleRequest/Document")]
        public IActionResult UpdateDocument([FromBody]DocumentPutModel report)
        {
            var result = DbClientFactory<DocumentClient>.Instance.PutDocument(appSettings.Value.ConnectionString, report);
            return Ok(result);
        }

        [HttpDelete]
        [Route("vehicleRequest/Document/{id:int}")]
        public IActionResult DeleteDocument(int id)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];
            var result = DbClientFactory<DocumentClient>.Instance.DeleteDocument(appSettings.Value.ConnectionString, id, paramUserID);
            return Ok(result);
        }

        [HttpGet]
        [Route("vehicleRequest/Document/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetDocument(int pageNumber, int pageSize)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = int.Parse(parameters["UserID"]);
            var paramType = parameters["Type"];
            var paramCreator = parameters["Creator"];
            var paramSmartSearch = parameters["SmartSearch"];
            var result = DbClientFactory<DocumentClient>.Instance.GetDocument(appSettings.Value.ConnectionString, paramUserID, paramType, pageNumber, pageSize, paramCreator, paramSmartSearch, lang);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPut]
        [Route("VehicleRequestConfirm")]
        public IActionResult GetVehicleRequestConfirm()
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<VehicleRequestClient>.Instance.ConfirmVehicleRequest(appSettings.Value.ConnectionString, 0, lang);
                foreach (var vehicle in result)
                {
                    Workflow.WorkflowBO bo = new VehicleRequestWorkflow().GetVehicleWorkflow(vehicle, appSettings.Value.ConnectionString, true);
                    bo.ServiceID = vehicle.VehicleReqID ?? 0;
                    workflow.StartWorkflow(bo);
                }

                return Ok(true);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("vehicleRequest/preview/{id:int}")]
        public IActionResult PrintPreview(int id)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                int userID = int.Parse(Request.Query["UserID"]);
                int isReturnForm = int.Parse(Request.Query["IsReturnForm"]);
                var result = DbClientFactory<VehicleRequestClient>.Instance.GetVehicleRequestPreviewByID(appSettings.Value.ConnectionString, id, isReturnForm, lang);
                PreviewHtmlToPdf(result, isReturnForm);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private void PreviewHtmlToPdf(VehiclePreviewModel result, int isReturnForm)
        {
            string appPath = Directory.GetCurrentDirectory();
            string downloadPath = appPath + @"\Downloads\";
            string finalPdf = downloadPath + result.ReferenceNumber + ".pdf";
            string htmlString = string.Empty;
            if (isReturnForm == 1)
            {
                htmlString = System.IO.File.ReadAllText(appPath + @"/Templates/ReturnPDFTemplate.html");
            }
            else
            {
                htmlString = System.IO.File.ReadAllText(appPath + @"/Templates/ReleasePDFTemplate.html");
            }

            ArabicConstantModel arabicValues = new ArabicConstantModel();
            htmlString = htmlString.Replace("{{EnRealeaseForm}}", "Release form");
            htmlString = htmlString.Replace("{{ArReturnForm}}", arabicValues.GetReturnForm);
            htmlString = htmlString.Replace("{{ArRealeaseForm}}", arabicValues.GetReleaseForm);
            htmlString = htmlString.Replace("{{VehicleDetails}}", arabicValues.GetVehicleDetails);
            htmlString = htmlString.Replace("{{PlateColorValue}}", result.PlateCode);
            htmlString = htmlString.Replace("{{ArPlateColorlabel}}", arabicValues.GetPlateColor);
            htmlString = htmlString.Replace("{{EnPlateColorlabel}}", "Plate color");
            htmlString = htmlString.Replace("{{PlateNumberValue}}", result.PlateNumber);
            htmlString = htmlString.Replace("{{ArPlateNumberLabel}}", arabicValues.GetPlateNumber);
            htmlString = htmlString.Replace("{{EnPlateNumberLabel}}", "Plate number");
            htmlString = htmlString.Replace("{{YearofManufactureValue}}", result.YearofManufacture);
            htmlString = htmlString.Replace("{{ArYearofManufactureValueLabel}}", arabicValues.GetYearOfManufacture);
            htmlString = htmlString.Replace("{{EnYearofManufactureValueLabel}}", "Year of manufacture");
            htmlString = htmlString.Replace("{{VehicleModelValue}}", result.VehicleModel);
            htmlString = htmlString.Replace("{{ArVehicleModelLabel}}", arabicValues.GetVehicleModel);
            htmlString = htmlString.Replace("{{EnVehicleModelLabel}}", "Vehicle Model");
            htmlString = htmlString.Replace("{{VehicleMakeValue}}", result.VehicleMake);
            htmlString = htmlString.Replace("{{ArReleaseVehicleMakeLabel}}", arabicValues.GetReleaseVehicleMake);
            htmlString = htmlString.Replace("{{ArReturnVehicleMakeLabel}}", arabicValues.GetReturnVehicleMake);
            htmlString = htmlString.Replace("{{EnVehicleMakeLabel}}", "Vehicle Make");
            htmlString = htmlString.Replace("{{VehicleDeliveryData}}", arabicValues.GetVehicleDeliveryData);
            htmlString = htmlString.Replace("{{UserNameValue}}", result.DriverName);
            htmlString = htmlString.Replace("{{ArUserNameLabel}}", arabicValues.GetUserName);
            htmlString = htmlString.Replace("{{EnUserNameLabel}}", "User Name");
            htmlString = htmlString.Replace("{{ReleaseTimeValue}}", result.ReleaseTime);
            htmlString = htmlString.Replace("{{ReleaseMeridiemTimeValue}}", result.ReleaseMeridiem);
            htmlString = htmlString.Replace("{{ArReleaseTimeLabel}}", arabicValues.GetReleaseTime);
            htmlString = htmlString.Replace("{{EnReleaseTimeLabel}}", result.DriverName);
            htmlString = htmlString.Replace("{{ReleaseDateValue}}", result.ReleaseDate);
            htmlString = htmlString.Replace("{{ArReleaseDateLabel}}", arabicValues.GetReleaseDate);
            htmlString = htmlString.Replace("{{EnReleaseDateLabel}}", "Release Date");
            htmlString = htmlString.Replace("{{LastMileageReleaseValue}}", result.LastMileageOnRelease);
            htmlString = htmlString.Replace("{{ArLastMileageReleaseLabel}}", arabicValues.GetReleaseMileage);
            htmlString = htmlString.Replace("{{EnLastMileageReleaseLabel}}", "Last Mileage Reading on Release");
            htmlString = htmlString.Replace("{{ReleaseLocationValue}}", result.ReleaseLocation);
            htmlString = htmlString.Replace("{{ArReleaseLocationLabel}}", arabicValues.GetReleaseLocation);
            htmlString = htmlString.Replace("{{EnReleaseLocationLabel}}", "Release Location");
            htmlString = htmlString.Replace("{{VehicleCondition}}", arabicValues.GetVehicleCondition);
            htmlString = htmlString.Replace("{{VehicleConditionMessage}}", arabicValues.GetVehicleConditionMessage);
            htmlString = htmlString.Replace("{{ReceivedByLabel}}", arabicValues.GetReceivedBy);
            htmlString = htmlString.Replace("{{ReleasedByLabel}}", arabicValues.GetReleasedByLabel);
            htmlString = htmlString.Replace("{{ReceivedByValue}}", result.ReleasedBy);
            htmlString = htmlString.Replace("{{ReleasedByValue}}", result.ReturnedBy);
            htmlString = htmlString.Replace("{{ReturnTimeValue}}", result.ReturnTime);
            htmlString = htmlString.Replace("{{ReturnMeridiemTimeValue}}", result.ReturnMeridiem);
            htmlString = htmlString.Replace("{{ArReturnTimeLabel}}", arabicValues.GetReturnTime);
            htmlString = htmlString.Replace("{{ReturnDateValue}}", result.ReturnDate);
            htmlString = htmlString.Replace("{{ArReturnDateLabel}}", arabicValues.GetReturnDate);
            htmlString = htmlString.Replace("{{LastMileageReturnValue}}", result.LastMileageOnReturn);
            htmlString = htmlString.Replace("{{ArLastMileageReturnLabel}}", arabicValues.GetReturnMileage);
            htmlString = htmlString.Replace("{{ReturnLocationValue}}", result.ReturnLocation);
            htmlString = htmlString.Replace("{{ArReturnLocationLabel}}", arabicValues.GetReturnLocation);
            htmlString = htmlString.Replace("{{ReceivedByLabel}}", arabicValues.GetReceivedBy);
            htmlString = htmlString.Replace("{{ReceivedByValue}}", result.ReturnedBy);
            htmlString = htmlString.Replace("{{ReturnedByLabel}}", arabicValues.GetReturnedByLabel);
            htmlString = htmlString.Replace("{{PersonalObjectiveLabel}}", arabicValues.GetPersonalObjective);
            htmlString = htmlString.Replace("{{NotesLabel}}", arabicValues.GetNotes);
            htmlString = htmlString.Replace("{{PersonalObjectiveValue}}", result.PersonalBelonging);
            htmlString = htmlString.Replace("{{NoteValue}}", result.Note);
            StringBuilder sb = new StringBuilder("<table class='pdftable'> <tbody>");
            int temp = 0;
            for (int i = 0; i < Math.Round(result.TripIssues.Count / 5.0); i++)
            {
                sb.Append("<tr>");
                for (int j = 0; j < 5; j++)
                {
                    if (temp == result.TripIssues.Count)
                        break;

                    if (result.TripIssues[temp].VehicleReqID == null)
                        sb.Append("<td><input type='checkbox' name='' />");
                    else
                        sb.Append("<td><input type='checkbox' name='' checked/>");
                    sb.Append((temp + 1) + "." + result.TripIssues[temp].IssueName);
                    sb.Append("</td>");
                    temp = temp + 1;
                }

                sb.Append("</tr>");
            }

            sb.Append("</tbody></table>");
            htmlString = htmlString.Replace("{{checkbox}}", sb.ToString());
            htmlString = htmlString.Replace("{{CarAttachment1}}", Path.Combine(Directory.GetCurrentDirectory() + @"\Templates\VehicleCarIssueImage1.png"));
            htmlString = htmlString.Replace("{{CarAttachment2}}", Path.Combine(Directory.GetCurrentDirectory() + @"\Templates\VehicleCarIssueImage2.png"));
            htmlString = htmlString.Replace("{{CarAttachment3}}", Path.Combine(Directory.GetCurrentDirectory() + @"\Templates\VehicleCarIssueImage3.png"));
            htmlString = htmlString.Replace("{{CarAttachment4}}", Path.Combine(Directory.GetCurrentDirectory() + @"\Templates\VehicleCarIssueImage4.png"));
            htmlString = htmlString.Replace("{{CarAttachment5}}", Path.Combine(Directory.GetCurrentDirectory() + @"\Templates\VehicleCarIssueImage5.png"));
            PdfConvert.Convert(new PdfConversionSettings
            {
                Content = htmlString,
                OutputPath = finalPdf
            });
        }
    }
}
