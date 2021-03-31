using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Vehicle.Drivers;
using RulersCourt.Repository.Vehicle;
using Shark.PdfConvert;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RulersCourt.Controllers.Vehicle
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Route("api/")]
    public class VehicleDriverController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;

        public VehicleDriverController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            environment = env;
            appSettings = app;
        }

        [HttpGet]
        [Route("VehicleDriver/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetDriver(int pageNumber, int pageSize)
        {
            try
            {
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                DateTime? paramDateRangeFrom = string.IsNullOrEmpty(parameters["DateRangeFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["DateRangeFrom"]);
                DateTime? paramDateRangeTo = string.IsNullOrEmpty(parameters["DateRangeTo"]) ? (DateTime?)null : DateTime.Parse(parameters["DateRangeTo"]);
                var paramUserID = parameters["UserID"];
                var isCalendarView = parameters["IsCalendarView"];
                DateTime? calendarViewDate = string.IsNullOrEmpty(parameters["CalendarViewDate"]) ? (DateTime?)null : DateTime.Parse(parameters["CalendarViewDate"]);
                var paramSmartSearch = parameters["SmartSearch"];
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<DriverClient>.Instance.GetDriver(appSettings.Value.ConnectionString, pageNumber, pageSize, paramUserID, paramDateRangeFrom, paramDateRangeTo, lang, paramSmartSearch, calendarViewDate);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpGet]
        [Route("VehicleDriver/{id}")]
        public IActionResult GetDriverByID(int id)
        {
            try
            {
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                int userID = int.Parse(Request.Query["UserID"]);
                DateTime? paramDateRangeFrom = string.IsNullOrEmpty(parameters["DateRangeFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["DateRangeFrom"]);
                DateTime? paramDateRangeTo = string.IsNullOrEmpty(parameters["DateRangeTo"]) ? (DateTime?)null : DateTime.Parse(parameters["DateRangeTo"]);
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<DriverClient>.Instance.GetDriverByID(appSettings.Value.ConnectionString, id, userID, paramDateRangeFrom, paramDateRangeTo, lang);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("VehicleDriver")]
        public IActionResult SaveDriver([FromBody]DriverPostModel driver)
        {
            var result = DbClientFactory<DriverClient>.Instance.PostDriver(appSettings.Value.ConnectionString, driver);

            return Ok(result);
        }

        [HttpPut]
        [Route("VehicleDriver")]
        public IActionResult UpdateDriver([FromBody]DriverPutModel driver)
        {
            var result = DbClientFactory<DriverClient>.Instance.PutDriver(appSettings.Value.ConnectionString, driver);

            return Ok(result);
        }

        [HttpDelete]
        [Route("VehicleDriver/{id:int}")]
        public IActionResult DeleteDriver(int id)
        {
            var result = DbClientFactory<DriverClient>.Instance.DeleteDriver(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpPatch]
        [Route("VehicleDriver/{id}")]
        public IActionResult ModifyDriver(int id, [FromBody]JsonPatchDocument<DriverPutModel> value)
        {
            var result = DbClientFactory<DriverClient>.Instance.PatchDriver(appSettings.Value.ConnectionString, id, value);
            return Ok(result);
        }

        [HttpGet]
        [Route("Master/VehicleDriver")]
        public IActionResult GetMasterDriver()
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
            bool driver = parameters["Driver"] == null ? false : Convert.ToBoolean(parameters["Driver"]);
            var result = DbClientFactory<DriverClient>.Instance.GetMasterDriver(appSettings.Value.ConnectionString, userID, driver, lang);

            return Ok(result);
        }

        [HttpPost]
        [Route("Master/VehicleDriver")]
        public IActionResult SavemasterDriver([FromBody]DriverBlinding master)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
            var result = DbClientFactory<DriverClient>.Instance.SaveMasterDriver(appSettings.Value.ConnectionString, userID, master);
            return Ok(result);
        }

        [HttpGet]
        [Route("VehicleDriver/export/{id}")]
        public async Task<IActionResult> Export(int id)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = int.Parse(Request.Query["UserID"]);
            DateTime? paramDateRangeFrom = string.IsNullOrEmpty(parameters["DateRangeFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["DateRangeFrom"]);
            DateTime? paramDateRangeTo = string.IsNullOrEmpty(parameters["DateRangeTo"]) ? (DateTime?)null : DateTime.Parse(parameters["DateRangeTo"]);
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<DriverClient>.Instance.GetDriverReportExportList(appSettings.Value.ConnectionString, lang, paramDateRangeFrom, paramDateRangeTo, userID, id);
            return await this.Export("Excel", result);
        }

        [HttpGet]
        [Route("VehicleDriverTrips/{Driver:int}")]
        public IActionResult GetDriverTrip(int driver)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var vehicleId = parameters["VehicelID"].ToString();
            var result = DbClientFactory<DriverClient>.Instance.GetDriverTripList(appSettings.Value.ConnectionString, lang, driver, vehicleId);
            return Ok(result);
        }

        [HttpPost]
        [Route("VehicleDriver/Preview")]
        public IActionResult PrintPreview()
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                DateTime? calendarViewDate = string.IsNullOrEmpty(parameters["CalendarViewDate"]) ? (DateTime?)null : DateTime.Parse(parameters["CalendarViewDate"]);
                int timeZone = string.IsNullOrEmpty(parameters["TimeZone"]) ? 0 : int.Parse(parameters["TimeZone"]);
                var result = DbClientFactory<DriverClient>.Instance.GetDriverPreviewCalender(appSettings.Value.ConnectionString, calendarViewDate, lang);
                PreviewHtmlToPdf(result, calendarViewDate, timeZone);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.StackTrace);
            }
        }

        private void PreviewHtmlToPdf(List<DriverGetModel> result, DateTime? calendarViewDate, int timeZone)
        {
            string appPath = Directory.GetCurrentDirectory();
            string downloadPath = appPath + @"\Downloads\";
            string finalPdf = downloadPath + "PreviewCalender-" + calendarViewDate.Value.ToString("dd-MM-yyyy") + ".pdf";
            string htmlString = string.Empty;
            htmlString = System.IO.File.ReadAllText(appPath + @"/Templates/CalenderPDFTemplate.html");
            ArabicConstantModel arabicValues = new ArabicConstantModel();

            string arMonth = string.Empty;
            if (calendarViewDate.Value.ToString("MMM").ToLower() == "jan")
                arMonth = arabicValues.GetJanuary;
            else if (calendarViewDate.Value.ToString("MMM").ToLower() == "feb")
                arMonth = arabicValues.GetFebruary;
            else if (calendarViewDate.Value.ToString("MMM").ToLower() == "mar")
                arMonth = arabicValues.GetMarch;
            else if (calendarViewDate.Value.ToString("MMM").ToLower() == "apr")
                arMonth = arabicValues.GetApril;
            else if (calendarViewDate.Value.ToString("MMM").ToLower() == "may")
                arMonth = arabicValues.GetMay;
            else if (calendarViewDate.Value.ToString("MMM").ToLower() == "jun")
                arMonth = arabicValues.GetJune;
            else if (calendarViewDate.Value.ToString("MMM").ToLower() == "jul")
                arMonth = arabicValues.GetJuly;
            else if (calendarViewDate.Value.ToString("MMM").ToLower() == "aug")
                arMonth = arabicValues.GetAugust;
            else if (calendarViewDate.Value.ToString("MMM").ToLower() == "sep")
                arMonth = arabicValues.GetSeptember;
            else if (calendarViewDate.Value.ToString("MMM").ToLower() == "feb")
                arMonth = arabicValues.GetFebruary;
            else if (calendarViewDate.Value.ToString("MMM").ToLower() == "oct")
                arMonth = arabicValues.GetOctober;
            else if (calendarViewDate.Value.ToString("MMM").ToLower() == "nov")
                arMonth = arabicValues.GetNovember;
            else if (calendarViewDate.Value.ToString("MMM").ToLower() == "dec")
                arMonth = arabicValues.GetDecember;

            htmlString = htmlString.Replace("{{Date}}", arMonth + calendarViewDate.Value.ToString(" dd,yyyy"));
            htmlString = htmlString.Replace("{{DriverName}}", arabicValues.GetDriverName);
            htmlString = htmlString.Replace("{{AM}}", arabicValues.GetAM);
            htmlString = htmlString.Replace("{{PM}}", arabicValues.GetPM);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < result.Count; i++)
            {
                List<DriverGetTripDaysModel> driverGetTripDays = new List<DriverGetTripDaysModel>();
                for (int j = 0; j < result[i].DriverTrips.Count; j++)
                {
                    result[i].DriverTrips[j].TripPeriodFrom = result[i].DriverTrips[j].TripPeriodFrom.Value.AddMinutes(-timeZone);
                    result[i].DriverTrips[j].TripPeriodTo = result[i].DriverTrips[j].TripPeriodTo.Value.AddMinutes(-timeZone);
                    if (result[i].DriverTrips[j].TripPeriodFrom.Value.Date <= calendarViewDate.Value.Date && result[i].DriverTrips[j].TripPeriodTo.Value.Date >= calendarViewDate.Value.Date)
                    {
                        driverGetTripDays.Add(result[i].DriverTrips[j]);
                    }
                }

                result[i].DriverTrips.Clear();
                result[i].DriverTrips = driverGetTripDays;

                for (int j = 0; j < result[i].DriverTrips.Count; j++)
                {
                        sb.Append("<tr>");
                        if ((i == 0 && j == 0) || (i == 0 ? (true && j == 0) : ((result[i].DriverName != result[i - 1].DriverName) && j == 0)))
                        {
                            sb.Append("<td rowspan='" + result[i].DriverTrips.Count + "'>");
                            sb.Append(result[i].DriverName);
                            sb.Append("</td>");
                        }

                        decimal from = 0;
                        if (calendarViewDate.Value.ToString("dd-MM-yyyy") == result[i].DriverTrips[j].TripPeriodFrom.Value.ToString("dd-MM-yyyy"))
                        {
                        if (int.Parse(result[i].DriverTrips[j].TripPeriodFrom.Value.ToString("mm")) > 0)
                        {
                           int min = int.Parse(result[i].DriverTrips[j].TripPeriodFrom.Value.ToString("mm"));
                           if (min >= 30)
                                from = (int.Parse(result[i].DriverTrips[j].TripPeriodFrom.Value.ToString("HH")) * 2) + 1;
                            else if (min < 30)
                                from = decimal.Parse(result[i].DriverTrips[j].TripPeriodFrom.Value.ToString("HH")) * 2;
                        }
                        else
                        {
                            from = int.Parse(result[i].DriverTrips[j].TripPeriodFrom.Value.ToString("HH")) * 2;
                        }
                        }

                        for (int k = 1; k <= from; k++)
                        {
                            sb.Append("<td class='minsplit'></td>");
                        }

                        decimal to = 48;
                        if (calendarViewDate.Value.ToString("dd-MM-yyyy") == result[i].DriverTrips[j].TripPeriodTo.Value.ToString("dd-MM-yyyy"))
                        {
                        if (int.Parse(result[i].DriverTrips[j].TripPeriodTo.Value.ToString("mm")) > 0)
                        {
                            int min = int.Parse(result[i].DriverTrips[j].TripPeriodTo.Value.ToString("mm"));
                            if (min >= 30)
                                to = (int.Parse(result[i].DriverTrips[j].TripPeriodTo.Value.ToString("HH")) * 2) + 1;
                            else if (min < 30)
                                to = decimal.Parse(result[i].DriverTrips[j].TripPeriodTo.Value.ToString("HH")) * 2;
                        }
                        else
                        {
                            to = int.Parse(result[i].DriverTrips[j].TripPeriodTo.Value.ToString("HH")) * 2;
                        }
                    }

                        sb.Append("<td class='tripTextSize' colspan='" + Math.Abs(to - from) + "'> " + arabicValues.GetCalenderWith + " : " + result[i].DriverTrips[j].With + " , " + arabicValues.GetCalenderTo + " : " + result[i].DriverTrips[j].TO + "</td>");
                        if (to > from)
                        {
                            for (decimal k = to + 1; k <= 48; k++)
                            {
                                sb.Append("<td class='minsplit'></td>");
                            }
                        }

                        sb.Append("</tr>");
                }
            }

            htmlString = htmlString.Replace("{{ListData}}", sb.ToString());
            PdfConvert.Convert(new PdfConversionSettings
            {
                Content = htmlString,
                OutputPath = finalPdf,
                Orientation = PdfPageOrientation.Landscape,
                Size = PdfPageSize.A4
            });
        }
    }
}
