using Core.Enums;
using Core.Extensions;
using Core.Helpers;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OERService.DataAccess;
using OERService.Models;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Web;

namespace OERService.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
		readonly IConfiguration _iconfiguration;
        public ReportsController(IConfiguration configuration)
        {
            _iconfiguration = configuration;
        }

        /// <summary>
        /// This will DELETE (PUT) report abuse content.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contentType"></param>
        /// <param name="reason"></param>
        /// <returns>OperationResponse</returns>
        [HttpPut("DeleteAbuseReport")]
        public async Task<IActionResult> DeleteAbuseReport(int id,int contentType,string reason)
        {
            try
            {


                ReportDataAccess _reportAccess = new ReportDataAccess(_iconfiguration);

                DatabaseResponse response = await _reportAccess.DeleteAbuseReport(id,contentType, reason);

                if (response.ResponseCode == (int)DbReturnValue.DeleteSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.DeleteSuccess),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.NotExists));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.NotExists),
                        ReturnedObject = response.Results
                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }

        /// <summary>
        /// This will Add (POST) visiter.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>OperationResponse</returns>
		[AllowAnonymous]
        [HttpPost("AddVisiter")]
        public async Task<IActionResult> AddVisiter(int? userId)
        {
            try
            {


                ReportDataAccess _reportAccess = new ReportDataAccess(_iconfiguration);

                DatabaseResponse response = await _reportAccess.AddVisiter(userId);

                if (response.ResponseCode == (int)DbReturnValue.CreateSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.CreateSuccess),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.CreationFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.RecordExists),
                        ReturnedObject = response.Results
                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }

        /// <summary>
        /// This will get (GET) visiter count.
        /// </summary>
        /// <returns>OperationResponse</returns>
        [HttpGet("GetVisitersCount")]
        public async Task<IActionResult> GetVisitersCount()
        {
            try
            {

                ReportDataAccess _reportAccess = new ReportDataAccess(_iconfiguration);

                DatabaseResponse response = await _reportAccess.GetVisitersCount();

                if (response.ResponseCode == (int)DbReturnValue.RecordExists)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.RecordExists),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.NoRecords));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.NoRecords)

                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }

        /// <summary>
        /// This will get (GET) QRC report.
        /// </summary>
        /// <returns>OperationResponse</returns>
        [HttpGet("GetQrcReport")]
        public async Task<IActionResult> GetQrcReport()
        {
            try
            {

                ReportDataAccess _reportAccess = new ReportDataAccess(_iconfiguration);

                DatabaseResponse response = await _reportAccess.GetQrcReport();

                if (response.ResponseCode == (int)DbReturnValue.RecordExists)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.RecordExists),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.NoRecords));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.NoRecords)

                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }

        /// <summary>
        /// This will get (GET) all users.
        /// </summary>
        /// <returns>OperationResponse</returns>
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {

                ReportDataAccess _reportAccess = new ReportDataAccess(_iconfiguration);

                DatabaseResponse response = await _reportAccess.GetUsers();

                if (response.ResponseCode == (int)DbReturnValue.RecordExists)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.RecordExists),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.NoRecords));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.NoRecords)

                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }
        /// <summary>
        /// This will get (GET) shared content report.
        /// </summary>
        /// <param name="sharedContentInput"></param>
        /// <returns>OperationResponse</returns>
        [HttpPost("GetSharedContentReport")]
        public async Task<IActionResult> GetSharedContentReport(SharedContentInput sharedContentInput)
        {
            try
            {

                ReportDataAccess _reportAccess = new ReportDataAccess(_iconfiguration);

                DatabaseResponse response = await _reportAccess.GetSharedContentReport(sharedContentInput);

                if (response.ResponseCode == (int)DbReturnValue.RecordExists)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.RecordExists),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.NoRecords));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.NoRecords)

                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }

        /// <summary>
        /// This will get (GET) resource by user id.
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns>OperationResponse</returns>
        [HttpGet("GetResourceByUserId/{Userid}")]
        public async Task<IActionResult> GetResourceByUserId(int Userid)
        {
            try
            {

                ReportDataAccess _reportAccess = new ReportDataAccess(_iconfiguration);

                DatabaseResponse response = await _reportAccess.GetResourceByUserID(Userid);

                if (response.ResponseCode == (int)DbReturnValue.RecordExists)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.RecordExists),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.NoRecords));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.NoRecords)

                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }

        /// <summary>
        /// This will get (GET) courses by user id.
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns>OperationResponse</returns>
        [HttpGet("GetCoursesByUserId/{Userid}")]
        public async Task<IActionResult> GetCoursesByUserId(int Userid)
        {
            try
            {

                ReportDataAccess _reportAccess = new ReportDataAccess(_iconfiguration);

                DatabaseResponse response = await _reportAccess.GetCourseByUserId(Userid);

                if (response.ResponseCode == (int)DbReturnValue.RecordExists)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.RecordExists),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.NoRecords));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.NoRecords)

                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }

        /// <summary>
        /// This will get (GET) dashboard report.
        /// </summary>
        /// <returns>OperationResponse</returns>
		[AllowAnonymous]
        [HttpGet("GetDashboardReport")]
        public async Task<IActionResult> GetDashboardData()
        {
            try
            {

                ReportDataAccess _reportAccess = new ReportDataAccess(_iconfiguration);

                DatabaseResponse response = await _reportAccess.GetDashboardReport();

                if (response.ResponseCode == (int)DbReturnValue.RecordExists)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.RecordExists),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.NoRecords));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.NoRecords)

                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }

		/// <summary>
		/// This will get (GET) user dashboard report by user id.
		/// </summary>
		/// <param name="UserId"></param>
		/// <returns>OperationResponse</returns>
		[HttpGet("UserDashboardReport/{UserId}")]
        public async Task<IActionResult> UserDashboardData(int UserId)
        {
            try
            {

                ReportDataAccess _reportAccess = new ReportDataAccess(_iconfiguration);

                DatabaseResponse response = await _reportAccess.UserDashboardData(UserId);

                if (response.ResponseCode == (int)DbReturnValue.RecordExists)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.RecordExists),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.NoRecords));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.NoRecords)

                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }

        /// <summary>
        /// This will get (GET) Content Rejected List.
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns>OperationResponse</returns>
        [HttpGet("ContentRejectedList/{pageNo}/{pageSize}")]
        public async Task<IActionResult> ContentRejectedList(int pageNo, int pageSize)
        {
            try
            {

                ReportDataAccess _reportAccess = new ReportDataAccess(_iconfiguration);
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var paramIsSearch = parameters["search"];
                var paramSortType = parameters["sortType"];
                var paramSortfield = string.IsNullOrEmpty(parameters["sortField"]) ? 0 : int.Parse(parameters["sortField"]);


                DatabaseResponse response = await _reportAccess.ContentRejectedList(pageNo, pageSize, paramIsSearch, paramSortType, paramSortfield);

                if (response.ResponseCode == (int)DbReturnValue.RecordExists)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.RecordExists),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.NoRecords));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.NoRecords)

                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }

        /// <summary>
        /// This will get (GET) User Recommended Content.
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns>OperationResponse</returns>
		[AllowAnonymous]
        [HttpGet("UserRecommendedContent/{UserId}/{pageNo}/{pageSize}")]
        public async Task<IActionResult> UserRecommendedContent(int UserId, int pageNo, int pageSize)
        {
            try
            {

                ReportDataAccess _reportAccess = new ReportDataAccess(_iconfiguration);

                DatabaseResponse response = await _reportAccess.UserRecommendedContent(UserId, pageNo, pageSize);

                if (response.ResponseCode == (int)DbReturnValue.RecordExists)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.RecordExists),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.NoRecords));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.NoRecords)

                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }


        /// <summary>
        /// This will get (GET) Report Abuse Content.
        /// </summary>
        /// <returns>OperationResponse</returns>
        [HttpGet("GetReportAbuseContent")]
        public async Task<IActionResult> GetReportAbuseContent()
        {
            try
            {

                ReportDataAccess _reportAccess = new ReportDataAccess(_iconfiguration);

                DatabaseResponse response = await _reportAccess.GetReportAbuseContent();

                if (response.ResponseCode == (int)DbReturnValue.RecordExists)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.RecordExists),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.NoRecords));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.NoRecords)

                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }
    }
}