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
    public class VerifierController : ControllerBase
    {
        internal DataAccessHelper _DataHelper = null;

		private readonly IConfiguration _configuration;
        public VerifierController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Verifier Approve/Reject POST request
        /// </summary>
        /// <param name="contentUpdate"></param>
        /// <returns>OperationResponse</returns>
        [HttpPut("UpdateContentStatus")]
        public async Task<IActionResult> UpdateContentStatus(ContentUpdate contentUpdate)
        {
            try
            {
                VerifierDataAccess _verifierDataAccess = new VerifierDataAccess(_configuration);

                DatabaseResponse response = await _verifierDataAccess.UpdateContentStatus(contentUpdate);

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
        /// Verifier GET Approval content request
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>OperationResponse</returns>
        [HttpGet("GetContentApproval/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetContentApproval(int pageNumber, int pageSize)
        {
            try
            {
                VerifierDataAccess _verifierDataAccess = new VerifierDataAccess(_configuration);

                DatabaseResponse response = await _verifierDataAccess.GetContentApproval(pageNumber, pageSize);
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
        /// Verifier GET Report request
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>OperationResponse</returns>
        [HttpGet("GetVerifiersReport/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetVerifiersReport(int pageNumber, int pageSize)
        {
            try
            {
                VerifierDataAccess _verifierDataAccess = new VerifierDataAccess(_configuration);
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var paramIsSearch = parameters["search"];
                var paramSortType = parameters["sortType"];
                var paramSortfield = string.IsNullOrEmpty(parameters["sortField"]) ? 0 : int.Parse(parameters["sortField"]);


                DatabaseResponse response = await _verifierDataAccess.GetVerifiersReport(pageNumber, pageSize , paramIsSearch , paramSortType , paramSortfield);
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