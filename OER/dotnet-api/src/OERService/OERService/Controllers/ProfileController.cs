using Core.Enums;
using Core.Extensions;
using Core.Helpers;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using OERService.DataAccess;
using OERService.Models;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OERService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        readonly IConfiguration _iconfiguration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProfileController(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _iconfiguration = configuration;

            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        /// This will return user profile details for given email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>OperationsResponse</returns>
        [HttpGet("{email}")]
        public async Task<IActionResult> GetById([FromRoute] string email)
        {
            try
            {
                try
                {
                    MailAddress mail = new MailAddress(email);
                    Log.Information(mail.User);
                }
                catch
                {
                    Log.Error(StatusMessages.DomainValidationError);
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = true,
                        Message = EnumExtensions.GetDescription(CommonErrors.InvalidEmail),
                    });
                }

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
                ProfileDataAccess _profileAccess = new ProfileDataAccess(_iconfiguration);

                DatabaseResponse response = await _profileAccess.GetUserProfile(email);

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

        [HttpGet("GetUserById/{Id}")]
        public async Task<IActionResult> GetUserById([FromRoute] int Id)
        {
            try
            {

                ProfileDataAccess _profileAccess = new ProfileDataAccess(_iconfiguration);

                DatabaseResponse response = await _profileAccess.GetUserById(Id);

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
        /// This will create a user profile
        /// </summary>
        /// <param name="profile"></param>
        /// <returns>OperationsResponse</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserProfileRequest profile)
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

                ProfileDataAccess _profileAccess = new ProfileDataAccess(_iconfiguration);

                DatabaseResponse response = await _profileAccess.CreateProfile(profile);

                if (response.ResponseCode == (int)DbReturnValue.CreateSuccess)
                {
                    DatabaseResponse userResponse = await _profileAccess.GetUserProfileById(((UserMaster)response.Results).Id);
                    if (userResponse.ResponseCode == (int)DbReturnValue.RecordExists)
                    {
                        return Ok(new OperationResponse
                        {
                            HasSucceeded = true,
                            IsDomainValidationErrors = false,
                            Message = EnumExtensions.GetDescription(DbReturnValue.CreateSuccess),
                            ReturnedObject = userResponse.Results
                        });

                    }

                    else
                    {
                        return Ok(new OperationResponse
                        {
                            HasSucceeded = true,
                            IsDomainValidationErrors = false,
                            Message = EnumExtensions.GetDescription(DbReturnValue.CreationFailed)

                        });
                    }

                }

                else if (response.ResponseCode == (int)DbReturnValue.EmailExists)
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.EmailExists));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.EmailExists),
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
                        Message = EnumExtensions.GetDescription(DbReturnValue.CreationFailed),
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
        [HttpPut("UpdateLastLogin/{userId}")]
        public async Task<IActionResult> UpdateLastLogin(int userId)
        {
            try
            {
                ProfileDataAccess _profileAccess = new ProfileDataAccess(_iconfiguration);

                DatabaseResponse response = await _profileAccess.UpdateLastLogin(userId);

                if (response.ResponseCode == (int)DbReturnValue.UpdateSuccess)
                {

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.UpdateSuccess),
                        ReturnedObject = null
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
        [HttpPut("UpdateTheme")]
        public async Task<IActionResult> UpdateTheme(int UserId, string Theme)
        {
            try
            {

                ProfileDataAccess _profileAccess = new ProfileDataAccess(_iconfiguration);

                DatabaseResponse response = await _profileAccess.UpdateTheme(UserId, Theme);

                if (response.ResponseCode == (int)DbReturnValue.UpdateSuccess)
                {

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.UpdateSuccess),
                        ReturnedObject = null
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
                        ReturnedObject = null
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
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateUserProfileRequest profile)
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

                ProfileDataAccess _profileAccess = new ProfileDataAccess(_iconfiguration);

                DatabaseResponse response = await _profileAccess.UpdateProfile(profile);

                if (response.ResponseCode == (int)DbReturnValue.UpdateSuccess)
                {
                    DatabaseResponse userResponse = await _profileAccess.GetUserProfileById(((UserMaster)response.Results).Id);
                    if (userResponse.ResponseCode == (int)DbReturnValue.RecordExists)
                    {
                        return Ok(new OperationResponse
                        {
                            HasSucceeded = true,
                            IsDomainValidationErrors = false,
                            Message = EnumExtensions.GetDescription(DbReturnValue.UpdateSuccess),
                            ReturnedObject = userResponse.Results
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
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.CreationFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.CreationFailed),
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
        [HttpPost("DeActiveUserProfiles")]
        public async Task<IActionResult> UpdateUserProfileStatusAll(Boolean Status)
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
                ProfileDataAccess _profileAccess = new ProfileDataAccess(_iconfiguration);

                DatabaseResponse response = await _profileAccess.UpdateUserProfileStatusAll(Status);

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
                        Message = EnumExtensions.GetDescription(DbReturnValue.UpdationFailed),
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
        [HttpPost("DeActiveProfile")]
        public async Task<IActionResult> UpdateUserProfileStatus(int UserId, Boolean Status)
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
                ProfileDataAccess _profileAccess = new ProfileDataAccess(_iconfiguration);

                DatabaseResponse response = await _profileAccess.UpdateUserProfileStatus(UserId, Status);

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
                        Message = EnumExtensions.GetDescription(DbReturnValue.UpdationFailed),
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
        [HttpGet("GetEmailNotificationStatus")]
        public async Task<IActionResult> GetEmailNotificationStatus(int UserID)
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
                ProfileDataAccess _profileAccess = new ProfileDataAccess(_iconfiguration);

                DatabaseResponse response = await _profileAccess.GetEmailNotificationStatus(UserID);

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

        [HttpPut("UpdateEmailNotification")]
        public async Task<IActionResult> UpdateEmailNotification([FromBody] UserNotification userNotification)
        {
            try
            {


                ProfileDataAccess _profileAccess = new ProfileDataAccess(_iconfiguration);

                DatabaseResponse response = await _profileAccess.UpdateEmailNotification(userNotification);

                if (response.ResponseCode == (int)DbReturnValue.UpdateSuccess)
                {

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.UpdateSuccess),
                        ReturnedObject = response.ResponseCode
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
        /// This will create a user profile
        /// </summary>
        /// <param name="profile"></param>
        /// <returns>OperationsResponse</returns>
        [HttpPost("CreateInitialProfile")]
        public async Task<IActionResult> CreateInitialProfile([FromBody] CreateInitialProfileRequest profile)
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

                ProfileDataAccess _profileAccess = new ProfileDataAccess(_iconfiguration);

                DatabaseResponse response = await _profileAccess.CreateInitialProfile(profile);

                if (response.ResponseCode == (int)DbReturnValue.CreateSuccess)
                {
                    DatabaseResponse userResponse = await _profileAccess.GetUserProfileById(((UserMaster)response.Results).Id);
                    if (userResponse.ResponseCode == (int)DbReturnValue.RecordExists)
                    {
                        return Ok(new OperationResponse
                        {
                            HasSucceeded = true,
                            IsDomainValidationErrors = false,
                            Message = EnumExtensions.GetDescription(DbReturnValue.CreateSuccess),
                            ReturnedObject = userResponse.Results
                        });

                    }

                    else
                    {
                        return Ok(new OperationResponse
                        {
                            HasSucceeded = true,
                            IsDomainValidationErrors = false,
                            Message = EnumExtensions.GetDescription(DbReturnValue.CreationFailed)

                        });
                    }

                }

                else if (response.ResponseCode == (int)DbReturnValue.EmailExists)
                {

                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.EmailExists));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.EmailExists),
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
                        Message = EnumExtensions.GetDescription(DbReturnValue.CreationFailed),
                        // ReturnedObject = response.Results
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
        /// This will update IsContributor/IsAdmin roles of User refered by the userID
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="IsContributor"></param>
        /// <param name="IsAdmin">0/1</param>
        /// <param name="portalLanguageId"></param>
        /// <returns></returns>
        [HttpPost("UpdateRole")]
        public async Task<IActionResult> UpdateRole(int userId, bool? IsContributor = null, bool? IsAdmin = null, int? portalLanguageId = null)
        {
            try
            {

                ProfileDataAccess _profileAccess = new ProfileDataAccess(_iconfiguration);

                DatabaseResponse response = await _profileAccess.UpdateUserRole(userId, IsContributor, IsAdmin, portalLanguageId);

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
                else if (response.ResponseCode == (int)DbReturnValue.UpdationFailed)
                {
                    Log.Warning(EnumExtensions.GetDescription(DbReturnValue.UpdationFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.UpdationFailed),
                        ReturnedObject = response.Results
                    });
                }

                else
                {
                    Log.Warning(EnumExtensions.GetDescription(DbReturnValue.NotExists));

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
        /// This will Get User Bookrmarked Content
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns>OperationsResponse</returns>
        [HttpGet("GetUserBookmarkedContent/{UserId}")]
        public async Task<IActionResult> GetUserBookmarkedContent(int UserId)
        {
            try
            {

                ProfileDataAccess _profileAccess = new ProfileDataAccess(_iconfiguration);

                DatabaseResponse response = await _profileAccess.GetUserBookmarkedContent(UserId);

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
        /// This will Get User Bookmarked Content By ContentID
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ContentId"></param>
        /// <param name="ContentType"></param>
        /// <returns>OperationsResponse</returns>
        [HttpGet("GetUserFavouritesByContentID/{UserId}/{ContentId}/{ContentType}")]
        public async Task<IActionResult> GetUserFavouritesByContentID(int UserId, int ContentId, int ContentType)
        {
            try
            {

                ProfileDataAccess _profileAccess = new ProfileDataAccess(_iconfiguration);

                DatabaseResponse response = await _profileAccess.GetUserFavouritesByContentID(UserId, ContentId, ContentType);

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
        /// This will Add User Bookrmarked Content
        /// </summary>
        /// <param name="userBookmarks"></param>
        /// <returns>OperationsResponse</returns>
        [HttpPost("AddUserBookmarkedContent")]
        public async Task<IActionResult> AddUserBookmarkedContent([FromBody] AddUserBookMarks userBookmarks)
        {
            try
            {

                ProfileDataAccess _profileAccess = new ProfileDataAccess(_iconfiguration);

                DatabaseResponse response = await _profileAccess.AddUserBookmarkedContent(userBookmarks);

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
                else if (response.ResponseCode == (int)DbReturnValue.CreationFailed)
                {
                    Log.Warning(EnumExtensions.GetDescription(DbReturnValue.CreationFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.CreationFailed),
                        ReturnedObject = response.Results
                    });
                }

                else if (response.ResponseCode == (int)DbReturnValue.RecordExists)
                {
                    Log.Warning(EnumExtensions.GetDescription(DbReturnValue.RecordExists));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.RecordExists),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Warning(EnumExtensions.GetDescription(DbReturnValue.NotExists));

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
        /// This will Delete User Bookrmarked Content
        /// </summary>
        /// <param name="userBookmarks"></param>
        /// <returns>OperationsResponse</returns>
        [HttpDelete("DeleteUserBookmarkedContent")]
        public async Task<IActionResult> DeleteUserBookmarkedContent([FromBody] DeleteUserBookMarks userBookmarks)
        {
            try
            {

                ProfileDataAccess _profileAccess = new ProfileDataAccess(_iconfiguration);

                DatabaseResponse response = await _profileAccess.DeleteUserBookmarkedContent(userBookmarks);

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
                else if (response.ResponseCode == (int)DbReturnValue.DeleteFailed)
                {
                    Log.Warning(EnumExtensions.GetDescription(DbReturnValue.DeleteFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.DeleteFailed),
                        ReturnedObject = response.Results
                    });
                }

                else
                {
                    Log.Warning(EnumExtensions.GetDescription(DbReturnValue.NotExists));

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

        [Authorize]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> PostResetPassword()
        {
            try
            {
                var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(accessToken) as JwtSecurityToken;
                var userid = tokenS.Claims.First(claim => claim.Type == "sub").Value;
                var client = new HttpClient();

                var clients = new RestSharp.RestClient(_iconfiguration.GetValue<string>("Jwt:Authority") + "/protocol/openid-connect/token");
                var request = new RestSharp.RestRequest(RestSharp.Method.POST);
                string adminUsername = _iconfiguration.GetValue<string>("Jwt:AdminUsername");
                string adminPassword = _iconfiguration.GetValue<string>("Jwt:AdminPassword");
                string clientId = _iconfiguration.GetValue<string>("Jwt:ClientId");
                string clientSecret = _iconfiguration.GetValue<string>("Jwt:ClientSecret");
                request.AddParameter("application/x-www-form-urlencoded", "grant_type=password&username=" + adminUsername + "&password=" + adminPassword + "&client_id=" + clientId + "&client_secret=" + clientSecret, RestSharp.ParameterType.RequestBody);
                RestSharp.IRestResponse response = clients.Execute(request);
                dynamic resp = JObject.Parse(response.Content);
                var token = resp.access_token;

                var url = _iconfiguration.GetValue<string>("Jwt:AdminAuthority") + @"/users/" + userid + @"/execute-actions-email";
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                var content = new StringContent("[\"UPDATE_PASSWORD\"]", Encoding.UTF8, "application/json");
                var result = await client.PutAsync(url, content);

                if (result.IsSuccessStatusCode)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        ReturnedObject = result
                    });
                }
                else
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        ReturnedObject = result
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}