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
	public class WcmController : ControllerBase
	{
		readonly IConfiguration _iconfiguration;
		public WcmController(IConfiguration configuration)
		{
			_iconfiguration = configuration;
		}
		/// <summary>
		/// WCM Update request
		/// </summary>
		/// <param name="webContentPages"></param>
		/// <returns>OperationResponse</returns>
		[HttpPut]
		public async Task<IActionResult> Put(WebContentPages webContentPages)
		{
			try
			{

				WcmDataAccess _wcmAccess = new WcmDataAccess(_iconfiguration);

				DatabaseResponse response = await _wcmAccess.UpdatePageContent(webContentPages);

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
					Log.Error(EnumExtensions.GetDescription(DbReturnValue.RecordExists));

					return Ok(new OperationResponse
					{
						HasSucceeded = false,
						IsDomainValidationErrors = false,
						Message = EnumExtensions.GetDescription(DbReturnValue.RecordExists)

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
		/// WCM Add (POST) Page Content request
		/// </summary>
		/// <param name="webContentPages"></param>
		/// <returns>OperationResponse</returns>
		[HttpPost]
		public async Task<IActionResult> Post(WebContentPages webContentPages)
		{
			try
			{

				WcmDataAccess _wcmAccess = new WcmDataAccess(_iconfiguration);

				DatabaseResponse response = await _wcmAccess.AddPageContent(webContentPages);

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
					Log.Error(EnumExtensions.GetDescription(DbReturnValue.RecordExists));

					return Ok(new OperationResponse
					{
						HasSucceeded = false,
						IsDomainValidationErrors = false,
						Message = EnumExtensions.GetDescription(DbReturnValue.RecordExists)

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
		/// WCM GET Page ContentBy ID
		/// </summary>
		/// <param name="pageId"></param>
		/// <returns>OperationResponse</returns>
        [AllowAnonymous]
		[HttpGet("GetPageContent/{pageId}")]
		public async Task<IActionResult> GetPageContentById(int pageId)
		{
			try
			{
				WcmDataAccess _wcmAccess = new WcmDataAccess(_iconfiguration);

				DatabaseResponse response = await _wcmAccess.GetPageContentById(pageId);

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
        /// WCM GET All Page Content
        /// </summary>
        /// <returns>OperationResponse</returns>
		[AllowAnonymous]
		[HttpGet("GetAllPageContents")]
		public async Task<IActionResult> GetAllPageContents()
		{
			try
			{
				WcmDataAccess _wcmAccess = new WcmDataAccess(_iconfiguration);

				DatabaseResponse response = await _wcmAccess.GetPageContents();

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
		/// WCM GET All Pages
		/// </summary>
		/// <returns>OperationResponse</returns>
		[AllowAnonymous]
		[HttpGet("GetPages/{categoryId}")]
		public async Task<IActionResult> GetPages(int categoryId)
		{
			try
			{

				WcmDataAccess _wcmAccess = new WcmDataAccess(_iconfiguration);

				DatabaseResponse response = await _wcmAccess.GetPages(categoryId);

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