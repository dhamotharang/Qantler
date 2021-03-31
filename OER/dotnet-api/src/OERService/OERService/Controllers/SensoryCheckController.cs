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

namespace OERService.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
    [ApiController]
    public class SensoryCheckController : ControllerBase
    {
        internal DataAccessHelper _DataHelper = null;

		private readonly IConfiguration _configuration;
        public SensoryCheckController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// This will Update [PUT] content status.
        /// </summary>
        /// <param name="contentUpdate"></param>
        /// <returns>OperationResponse</returns>
        [HttpPut("UpdateContentStatus")]
        public async Task<IActionResult> UpdateContentStatus(SensoryContentStatus contentUpdate)
        {
            try
            {

                SensoryCheckDataAccess sensoryCheckDataAccess = new SensoryCheckDataAccess(_configuration);

                DatabaseResponse response = await sensoryCheckDataAccess.UpdateContentStatus(contentUpdate);

                if (response.ResponseCode == (int)DbReturnValue.Approved)
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
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.ActionAlreadyTaken));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.ActionAlreadyTaken)

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
		/// This will GET content approval.
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="pageNumber"></param>
		/// <param name="pageSize"></param>
		/// <param name="categoryId"></param>
		/// <returns>OperationResponse</returns>
		[HttpGet("GetContentApproval/{userId}/{pageNumber}/{pageSize}/{categoryId}")]
        public async Task<IActionResult> GetContentList(int userId, int pageNumber, int pageSize, int categoryId)
        {
            try
            {

                SensoryCheckDataAccess sensoryCheckDataAccess = new SensoryCheckDataAccess(_configuration);

                DatabaseResponse response = await sensoryCheckDataAccess.GetContentList(userId, pageNumber, pageSize, categoryId);
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