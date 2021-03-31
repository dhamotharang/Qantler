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
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OERService.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
    [ApiController] 

    public class ResourceController : ControllerBase
    {
		readonly IConfiguration _iconfiguration;
        public ResourceController(IConfiguration configuration)
        {
            _iconfiguration = configuration;
        }

        /// <summary>
        /// This will GET resource by CourseID.
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns>OperationResponse</returns>
        [HttpGet("GetResourceByCourseId/{courseId}")]
        public async Task<IActionResult> GetResourceByCourseId(int courseId)
        {
            try
            {

                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.GetResourceByCourseId(courseId);

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
        /// This will get all records from Resource Master.
        /// </summary>
        /// <returns></returns>
        // GET: api/Resources
        // [Authorize]
        [HttpGet("{pageNo}/{pageSize}")]
        public async Task<IActionResult> Get(int pageNo, int pageSize)
        {
            try
            {

                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var paramIsSearch = parameters["search"];
                var paramAscDescNo = parameters["ascDescNo"];
                var paramColumnNo = string.IsNullOrEmpty(parameters["columnNo"]) ? 0 : int.Parse(parameters["columnNo"]);

                DatabaseResponse response = await _resourceAccess.GetAllResource(pageNo, pageSize , paramIsSearch, paramAscDescNo, paramColumnNo);

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
        /// This will Download [POST] Cntent for Resource.
        /// </summary>
        /// <returns>OperationResponse</returns>
        [HttpPost("DownloadContentForResource")]
        public async Task<IActionResult> DownloadContent([FromBody] DownloadContentInfoCreateResource downloadContentInfoCreate)
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

                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.DownloadContent(downloadContentInfoCreate);

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
        /// This will GET Resource Master Data.
        /// </summary>
        /// <returns>OperationResponse</returns>
		[AllowAnonymous]
        [HttpGet("ResourceMasterData")]
        public async Task<IActionResult> GetResourceMasterData()
        {
            try
            {

                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.GetResourceMaster();

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
        /// Get a resource referenced by the Id passed
        /// </summary>
        /// <param name="id"></param>
        /// <returns>OperationsResponse</returns>
        // GET: api/Resources/5
		[AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
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
                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.GetResource(id);

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
        /// This will GET Remix Version.
        /// </summary>
        /// <param name="ResourceRemixedID"></param>
        /// <returns>OperationResponse</returns>
        [HttpGet("GetRemixVersion/{ResourceRemixedID}")]
        public async Task<IActionResult> GetRemixVersion(decimal ResourceRemixedID)
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
                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.GetRemixVersion(ResourceRemixedID);

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
        /// This will Create a ResourceMaster entry.
        /// </summary>
        /// <param name="resource">
        /// CreateResourceMaster
        ///Body: 
        ///{ 
        ///  "title": "Resource8_updated",
        ///  "categoryId": 1,
        ///  "subCategoryId": 1,
        ///  "thumbnail": "Not yet uploaded",
        ///  "resourceDescription": "This description is updated",
        ///  "keywords": "No keywords are given",
        ///  "resourceContent": "No content given",  
        ///  "materialTypeId": 2,
        ///  "copyRightId": 2,
        ///  "isDraft": false,
        ///  "createdBy": 1,
        ///  "references": [
        ///  { "urlReference": "ref1" }, 
        ///  { "urlReference": "ref2" },
        ///  { "urlReference": "ref3" }],
        ///  "resourceFiles": [
        ///  {"associatedFile": "file1"},
        ///  {"associatedFile": "file2"}]}}
        /// </param>
        /// <returns>OperationResponse</returns>
        // POST: api/Resources
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateResourceRequest resource)
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

                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.CreateResource(resource);
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.CreateSuccess),
                        ReturnedObject = response.Results
                    });
        }
        /// <summary>
        /// This will update ResourceMaster entry by id and details passed.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource">
        ///Body: 
        ///{   
        ///{  "id":8,
        ///  "title": "Resource8_updated",
        ///  "categoryId": 1,
        ///  "subCategoryId": 1,
        ///  "thumbnail": "Not yet uploaded",
        ///  "resourceDescription": "This description is updated",
        ///  "keywords": "No keywords are given",
        ///  "resourceContent": "No content given",  
        ///  "materialTypeId": 2,
        ///  "copyRightId": 2,
        ///  "isDraft": false,
        ///  "createdBy": 1,
        ///  "references": [
        ///  { "urlReference": "ref1" }, 
        ///  { "urlReference": "ref2" },
        ///  { "urlReference": "ref3" }],
        ///  "resourceFiles": [
        ///  {"associatedFile": "file1"},
        ///  {"associatedFile": "file2"}]}}
        /// </param>
        /// <returns>OperationsResponse</returns>
        // PUT: api/Resources/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateResourceRequest resource)
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

                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.UpdateResource(resource, id);

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.UpdateSuccess),
                        ReturnedObject = response.Results
                    });
              
        }
        /// <summary>
        /// This will delete a resource record by the id passed
        /// </summary>
        /// <param name="id"></param>
        /// <returns>OperationResponse</returns>
        // DELETE: api/Resources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
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
                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.DeleteResource(id);

                if (response.ResponseCode == (int)DbReturnValue.DeleteSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.DeleteSuccess)
                      
                    });
                }
                else if (response.ResponseCode == (int)DbReturnValue.ActiveTryDelete)
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.ActiveTryDelete));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.ActiveTryDelete)
                        
                    });
                }

                else if (response.ResponseCode == (int)DbReturnValue.DeleteFailed)
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.DeleteFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.DeleteFailed)
                        
                    });
                }
                else if (response.ResponseCode == (int)DbReturnValue.ResourceReferred)
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.ResourceReferred));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.ResourceReferred)

                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.NotExists));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.NotExists)
                       
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
        /// This will approve a resource
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]     
        [Route("ApproveResource/{id}/{userId}")]
        public async Task<IActionResult> ApproveResource(decimal id, int userId)
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
                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.ApproveResource(id, userId);

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
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.AlreadyApproved));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.AlreadyApproved),
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
        /// This will Add (POST) comment on resource.
        /// </summary>
        /// <param name="resourceComment"></param>
        /// <returns>OperationResponse</returns>
        [HttpPost("CommentOnResource")]
        public async Task<IActionResult> CommentOnResource([FromBody] ResourceCommentRequest resourceComment)
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

                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.CommentOnResource(resourceComment);

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
        /// This will Update (POST) comment on resource.
        /// </summary>
        /// <param name="resourceUpdate"></param>
        /// <returns>OperationResponse</returns>
        [HttpPost("UpdateResourceComment")]
        public async Task<IActionResult> UpdateResourceComment([FromBody] ResourceCommentUpdateRequest resourceUpdate)
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

                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.UpdateResourceComment(resourceUpdate);

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
        /// This will Report (POST) comment on resource.
        /// </summary>
        /// <param name="resourceCommentReport"></param>
        /// <returns>OperationResponse</returns>
        [HttpPost("ReportResourceComment")]
        public async Task<IActionResult> ReportResourceComment(ResourceCommentReportAbuseWithComment resourceCommentReport)
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
                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.ReportResourceCommentWithComment(resourceCommentReport);

                if (response.ResponseCode == (int)DbReturnValue.CreateSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.AbuseReported),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(CommonErrors.AbuseReportFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.AbuseReportFailed),
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
        /// This will delete (DELETE) comment on resource.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="requestedBy"></param>
        /// <returns>OperationResponse</returns>
        [HttpDelete("DeleteResourceComment/{id}/{requestedBy}")]
        public async Task<IActionResult> DeleteResourceComment(decimal id, int requestedBy)
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
                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.DeleteResourceComment(id,requestedBy);

                if (response.ResponseCode == (int)DbReturnValue.DeleteSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.DeleteSuccess)

                    });
                }               

                else if (response.ResponseCode == (int)DbReturnValue.DeleteFailed)
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.DeleteFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.DeleteFailed)

                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.NotExists));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.NotExists)

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
       /// Hide a resource comment. Can only be done by the resource owner
       /// </summary>
       /// <param name="id"></param>
       /// <param name="resourceId"></param>
       /// <param name="requestedBy"></param>
       /// <returns></returns>
        [HttpGet("HideResourceComment/{id}/{resourceId}/{requestedBy}")]
        public async Task<IActionResult> HideResourceComment(decimal id,decimal resourceId, int requestedBy)
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
                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.HideResourceCommentByAuthor(id,resourceId,requestedBy);

                if (response.ResponseCode == (int)DbReturnValue.UpdateSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.CommentHide),
                        ReturnedObject = response.Results
                    });
                }
                else if (response.ResponseCode == (int)DbReturnValue.UpdationFailed)
                {
                    Log.Error(EnumExtensions.GetDescription(CommonErrors.CommentHideFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.CommentHideFailed),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.ReasouceOnlyHiddenByAuthor));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.ReasouceOnlyHiddenByAuthor),
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
        /// This will report a resource with comment and reason/s to report
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("ReportResource")]
        public async Task<IActionResult> ReportResource(ResourceReportAbuseWithComment request)
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
                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.ReportResourceWithComment(request);

                if (response.ResponseCode == (int)DbReturnValue.CreateSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.AbuseReported),
                        ReturnedObject = response.Results
                    });
                }
                else if(response.ResponseCode == (int)DbReturnValue.RecordExists)
                {
                    Log.Error(EnumExtensions.GetDescription(CommonErrors.AlreadyReportedResource));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.AlreadyReportedResource),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(CommonErrors.AbuseReportFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.AbuseReportFailed),
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
        /// Rate a resource with comment
        /// </summary>
        /// <param name="request">
        /// 
        /// </param>
        /// <returns></returns>
        /// 
        [HttpPost("RateResource")]
        public async Task<IActionResult> RateResource(ResourceRatingRequest request)
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
                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.RateResource(request);

                if (response.ResponseCode == (int)DbReturnValue.CreateSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.RatingSucess),
                        ReturnedObject = response.Results
                    });
                }
                else if(response.ResponseCode == (int)DbReturnValue.CreationFailed)
                {
                    Log.Error(EnumExtensions.GetDescription(CommonErrors.RatingFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.RatingFailed),
                        ReturnedObject = response.Results
                    });
                }

                else
                {
                    Log.Error(EnumExtensions.GetDescription(CommonErrors.AlreadyRatedResource));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.AlreadyRatedResource),
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
        /// This will Add (POST) resource alignment rating.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>OperationResponse</returns>
        [HttpPost("ResourceAlignmentRating")]
        public async Task<IActionResult> ResourceAlignmentRating(ResourceAlignmentRatingRequest request)
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
                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.ResourceAlignmentRating(request);

                if (response.ResponseCode == (int)DbReturnValue.CreateSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.RatingSucess),
                        ReturnedObject = response.Results
                    });
                }
                else if (response.ResponseCode == (int)DbReturnValue.CreationFailed)
                {
                    Log.Error(EnumExtensions.GetDescription(CommonErrors.RatingFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.RatingFailed),
                        ReturnedObject = response.Results
                    });
                }

                else
                {
                    Log.Error(EnumExtensions.GetDescription(CommonErrors.AlreadyRatedResource));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.AlreadyRatedResource),
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
        /// This will get (GET) resources by keyword search.
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>OperationResponse</returns>
        [HttpGet("SearchResources/{keyword}/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> SearchResources(string keyword, int pageNumber, int pageSize)
        {
            try
            {
                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.SearchResources(keyword, pageNumber, pageSize);

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
        /// This will Update (POST) Ratings by Content.
        /// </summary>
        /// <param name="ratingRequest"></param>
        /// <returns>OperationResponse</returns>
		[AllowAnonymous]
        [HttpPost("RatingsByContent")]
        public async Task<IActionResult> RatingsByContent([FromBody] List<RatingRequest> ratingRequest)
        {
            try
            {

                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.GetRatingsByContent(ratingRequest);

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