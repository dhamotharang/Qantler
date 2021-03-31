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

namespace OERService.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
    [ApiController]
    public class ElasticController : ControllerBase
    {

		readonly IConfiguration _iconfiguration;
        public ElasticController(IConfiguration configuration)
        {
            _iconfiguration = configuration;
        }
        /// <summary>
        /// This will Get Search Results from Elastic.
        /// </summary>
        /// <returns>OperationResponse</returns>
        // POST: api/Categories
		[AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ElasticModel elasticModel)
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

                ElasticDataAccess _elasticAccess = new ElasticDataAccess(_iconfiguration);

                DatabaseResponse response = await _elasticAccess.ElasticSearch(elasticModel.searchString,elasticModel.partialURL);

                if (response.ResponseCode == (int)DbReturnValue.SearchSuccessful)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.SearchSuccessful),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.SearchUnsuccessful));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.SearchUnsuccessful),
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