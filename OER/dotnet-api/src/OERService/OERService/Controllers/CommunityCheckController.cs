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
    public class CommunityCheckController : ControllerBase
    {
        internal DataAccessHelper _DataHelper = null;

		private readonly IConfiguration _configuration;
		private readonly IConverter _converter;
        public CommunityCheckController(IConfiguration configuration, IConverter converter)
        {
            _configuration = configuration;
            _converter = converter;
        }
        /// <summary>
        /// Approve or reject content under community check
        /// </summary>
        /// <param name="contentUpdate">Update object</param>
        /// <returns>True/False</returns>
        [HttpPut("UpdateContentStatus")]
        public async Task<IActionResult> UpdateContentStatus(CommunityContentStatus contentUpdate)
        {
            try
            {

                CommunityCheckDataAccess communityCheckDataAccess = new CommunityCheckDataAccess(_configuration, _converter);

                DatabaseResponse response = await communityCheckDataAccess.UpdateContentStatus(contentUpdate);

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
                else if (response.ResponseCode == (int)DbReturnValue.Approved)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.Approved),
                        ReturnedObject = response.Results
                    });
                }
                else if (response.ResponseCode == (int)DbReturnValue.Rejected)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.Rejected),
                        ReturnedObject = response.Results
                    });
                }

                else if (response.ResponseCode == (int)DbReturnValue.RejectedAndSentToDrafts)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.RejectedAndSentToDrafts),
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
		/// Get a list of content to approve or reject
		/// </summary>
		/// <param name="userId">User ID</param>
		/// <param name="pageNumber">Page Number</param>
		/// <param name="pageSize">Page Size</param>
		/// <param name="categoryId">category Id</param>
		/// <returns>List of content</returns>
		[HttpGet("GetContentApproval/{userId}/{pageNumber}/{pageSize}/{categoryId}")]
        public async Task<IActionResult> GetContentApproval(int userId, int pageNumber, int pageSize, int categoryId)
        {
            try
            {
                CommunityCheckDataAccess communityCheckDataAccess = new CommunityCheckDataAccess(_configuration, _converter);

                DatabaseResponse response = await communityCheckDataAccess.GetContentApproval(userId, pageNumber, pageSize, categoryId);
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
        /// List of approved conent under community check
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>List of content</returns>
        [HttpGet("GetApprovedListByUser/{userId}/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetApprovedListByUser(int userId, int pageNumber, int pageSize)
        {
            try
            {
                CommunityCheckDataAccess communityCheckDataAccess = new CommunityCheckDataAccess(_configuration, _converter);

                DatabaseResponse response = await communityCheckDataAccess.GetApprovedListByUser(userId, pageNumber, pageSize);
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