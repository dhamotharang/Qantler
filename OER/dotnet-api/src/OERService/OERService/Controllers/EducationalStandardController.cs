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
	public class EducationalStandardController : ControllerBase
	{
		internal DataAccessHelper _DataHelper = null;

		private readonly IConfiguration _configuration;

		/// <summary>
		/// Constructor setting configuration
		/// </summary>
		/// <param name="configuration"></param>
		public EducationalStandardController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		/// <summary>
		/// Get educational standard by Id
		/// </summary>
		/// <param name="Id">ID</param>
		/// <returns>Object of eductional standard</returns>
		[HttpGet("GetById")]
		public async Task<IActionResult> GetByIdAsync(int Id)
		{
			try
			{

				EduStandardDataAccess _EduStandardAccess = new EduStandardDataAccess(_configuration);

				DatabaseResponse response = await _EduStandardAccess.GetByIdAsync(Id);

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
		/// Get all list of eductional standard.
		/// </summary>
		/// <returns>List of eductional standard</returns>
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{

				EduStandardDataAccess _EduStandardAccess = new EduStandardDataAccess(_configuration);

				DatabaseResponse response = await _EduStandardAccess.GetAsync();

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
		/// Educational Standard Update.
		/// </summary>
		/// <param name="educationalStandardUpdate">Update object</param>
		/// <returns>True false status</returns>
		[HttpPut]
		public async Task<IActionResult> Put(EducationalStandardUpdate educationalStandardUpdate)
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

				EduStandardDataAccess _EduStandardAccess = new EduStandardDataAccess(_configuration);

				DatabaseResponse response = await _EduStandardAccess.PutAsync(educationalStandardUpdate);

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
		/// Create educational record
		/// </summary>
		/// <param name="educationalStandardCreate">object</param>
		/// <returns>true false status</returns>
		[HttpPost]
		public async Task<IActionResult> Post(EducationalStandardCreate educationalStandardCreate)
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
				EduStandardDataAccess _EduStandardAccess = new EduStandardDataAccess(_configuration);

				DatabaseResponse response = await _EduStandardAccess.PostAsync(educationalStandardCreate);

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
		/// Delete educational standard.
		/// </summary>
		/// <param name="id">Education Standard ID</param>
		/// <returns>true false object</returns>
		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{

				EduStandardDataAccess _EduStandardAccess = new EduStandardDataAccess(_configuration);

				DatabaseResponse response = await _EduStandardAccess.DeleteAsync(id);

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
				else if (response.ResponseCode == (int)DbReturnValue.ActiveTryNotDelete)
				{
					Log.Error(EnumExtensions.GetDescription(DbReturnValue.ActiveTryNotDelete));

					return Ok(new OperationResponse
					{
						HasSucceeded = false,
						IsDomainValidationErrors = false,
						Message = EnumExtensions.GetDescription(DbReturnValue.ActiveTryNotDelete),
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
	}
}