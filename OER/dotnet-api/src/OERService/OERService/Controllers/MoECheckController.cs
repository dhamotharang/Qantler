using Core.Enums;
using Core.Extensions;
using Core.Helpers;
using Core.Models;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OERService.DataAccess;
using OERService.Models;
using Serilog;
using System;
using System.Threading.Tasks;

namespace OERService.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
    [ApiController]
    public class MoECheckController : ControllerBase
    {
        internal DataAccessHelper _DataHelper = null;

		private readonly IConfiguration _configuration;
		private readonly IConverter _converter;
        public MoECheckController(IConfiguration configuration,IConverter converter)
        {
            _configuration = configuration;
            _converter = converter;
        }
        /// <summary>
        /// MOE check conent status update 
        /// </summary>
        /// <param name="contentUpdate">Update status</param>
        /// <returns>Success/Failed</returns>
        [HttpPut("MoEUpdateContentStatus")]
        public async Task<IActionResult> UpdateContentStatus(MoEContentStatus contentUpdate)
        {
            try
            {

                MoECheckDataAccess moeCheckDataAccess = new MoECheckDataAccess(_configuration,_converter);

                DatabaseResponse response = await moeCheckDataAccess.UpdateContentStatus(contentUpdate);

                if (response.ResponseCode == (int)DbReturnValue.Published)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.Published),
                        ReturnedObject = response.Results
                    });
                }
                else if (response.ResponseCode == (int)DbReturnValue.Rejected)
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.Rejected));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.Rejected),
                        ReturnedObject = response.Results
                    });
                }
                else if (response.ResponseCode == (int)DbReturnValue.ReviewedByOtherUsers)
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.ReviewedByOtherUsers));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.ReviewedByOtherUsers),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.ActionAlreadyTaken),
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
		/// Get list of content for MOE Check approval.
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="pageNumber"></param>
		/// <param name="pageSize"></param>
		/// <param name="categoryId"></param>
		/// <returns></returns>
		[HttpGet("GetMoEContentApproval/{userId}/{pageNumber}/{pageSize}/{categoryId}")]
        public async Task<IActionResult> GetContentApproval(int userId, int pageNumber, int pageSize, int categoryId)
        {
            try
            {
                MoECheckDataAccess moeCheckDataAccess = new MoECheckDataAccess(_configuration, _converter);

                DatabaseResponse response = await moeCheckDataAccess.GetContentApproval(userId, pageNumber, pageSize, categoryId);
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
        /// Get list of approved list by user ID
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>List of content</returns>
        [HttpGet("GetMoEApprovedListByUser/{userId}/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetApprovedListByUser(int userId, int pageNumber, int pageSize)
        {
            try
            {
                MoECheckDataAccess moeCheckDataAccess = new MoECheckDataAccess(_configuration, _converter);

                DatabaseResponse response = await moeCheckDataAccess.GetApprovedListByUser(userId, pageNumber, pageSize);
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