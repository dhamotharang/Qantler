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
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OERService.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        internal DataAccessHelper _DataHelper = null;

		private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public NotificationsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// Post reply of user notification
        /// </summary>
        /// <param name="queryUpdate">Query update object</param>
        /// <returns></returns>
        [HttpPost("PostReply")]
        public async Task<IActionResult> PostReply([FromBody] QueryUpdate queryUpdate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = true,
                        StatusCode = ((int)ResponseStatus.BadRequest).ToString(),
                        Message = string.Join("; ", ModelState.Values
                                                   .SelectMany(x => x.Errors)
                                                   .Select(x => x.ErrorMessage))
                    });
                }

                NotificationsDataAccess _notificationsDataAccess = new NotificationsDataAccess(_configuration);

                DatabaseResponse response = await _notificationsDataAccess.PostReply(queryUpdate);

                if (response.ResponseCode == (int)DbReturnValue.UpdateSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.UpdateSuccess),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.UpdationFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.AlreadyReplied),
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
        /// Get list of user notifications.
        /// </summary>
        /// <param name="UserId">UserID</param>
        /// <param name="PageNo">Page Number</param>
        /// <param name="PageSize">Page Size</param>
        /// <returns></returns>
        [HttpGet("GetUserNotifications/{UserId}/{PageNo}/{PageSize}")]
        public async Task<IActionResult> GetUserNotifications(int UserId,int PageNo, int PageSize)
        {
            try
            {

                NotificationsDataAccess _notificationsDataAccess = new NotificationsDataAccess(_configuration);

                DatabaseResponse response = await _notificationsDataAccess.GetUserNotifications(UserId, PageNo, PageSize);



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
        /// Get contact us queries
        /// </summary>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns></returns>
        [HttpGet("GetContactUsQueries/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetContactUsQueries(int pageNumber, int pageSize)
        {
            try
            {

                NotificationsDataAccess _notificationsDataAccess = new NotificationsDataAccess(_configuration);
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var paramIsSearch = parameters["search"];
                var paramSortType = parameters["sortType"];
                var paramSortfield = string.IsNullOrEmpty(parameters["sortField"]) ? 0 : int.Parse(parameters["sortField"]);

                DatabaseResponse response = await _notificationsDataAccess.GetContactUsQueries(pageNumber,pageSize, paramIsSearch , paramSortType , paramSortfield);

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
        /// Delete a notification
        /// </summary>
        /// <param name="UserId">User ID</param>
        /// <param name="NotificationId">Notification ID</param>
        /// <returns></returns>
        [HttpDelete("DeleteNotification")]
        public async Task<IActionResult> DeleteNotification(int UserId, int NotificationId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = true,
                        StatusCode = ((int)ResponseStatus.BadRequest).ToString(),
                        Message = string.Join("; ", ModelState.Values
                                                   .SelectMany(x => x.Errors)
                                                   .Select(x => x.ErrorMessage))
                    });
                }

                NotificationsDataAccess _notificationsDataAccess = new NotificationsDataAccess(_configuration);

                DatabaseResponse response = await _notificationsDataAccess.DeleteNotification(UserId, NotificationId);

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
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.DeleteFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.DeleteFailed),
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
        /// Update notifications
        /// </summary>
        /// <param name="UserId">UserID</param>
        /// <param name="NotificationId">NotificationID</param>
        /// <returns></returns>
        [HttpPut("UpdateNotification")]
        public async Task<IActionResult> UpdateNotification(int UserId, int NotificationId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = true,
                        StatusCode = ((int)ResponseStatus.BadRequest).ToString(),
                        Message = string.Join("; ", ModelState.Values
                                                   .SelectMany(x => x.Errors)
                                                   .Select(x => x.ErrorMessage))
                    });
                }

                NotificationsDataAccess _notificationsDataAccess = new NotificationsDataAccess(_configuration);

                DatabaseResponse response = await _notificationsDataAccess.UpdateNotification(UserId, NotificationId);

                if (response.ResponseCode == (int)DbReturnValue.UpdateSuccess)
                {

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.UpdateSuccess),
                        ReturnedObject = response.Results
                    });


                }
                else
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.UpdationFailed)

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
        /// Contact use post
        /// </summary>
        /// <param name="contactUsCreate">Create object</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("ContactUs")]
        public async Task<IActionResult> SaveContactUsForm(ContactUsCreate contactUsCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = true,
                        StatusCode = ((int)ResponseStatus.BadRequest).ToString(),
                        Message = string.Join("; ", ModelState.Values
                                                   .SelectMany(x => x.Errors)
                                                   .Select(x => x.ErrorMessage))
                    });
                }
                NotificationsDataAccess _notificationsDataAccess = new NotificationsDataAccess(_configuration);

                DatabaseResponse response = await _notificationsDataAccess.SaveContactUsForm(contactUsCreate);

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
    }
}