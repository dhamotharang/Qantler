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

    public class CourseController : ControllerBase
    {
		readonly IConfiguration _iconfiguration;
        public CourseController(IConfiguration configuration)
        {
            _iconfiguration = configuration;
        }
        /// <summary>
        /// This will get all records from Course Master.
        /// </summary>
        /// <returns></returns>
        // GET: api/Courses
        // [Authorize]
        [HttpGet("{pageNo}/{pageSize}")]
        public async Task<IActionResult> Get(int pageNo, int pageSize)
        {
            try
            {

                CourseDataAccess _courseAccess = new CourseDataAccess(_iconfiguration);
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var paramIsSearch = parameters["search"];
                var paramSortType = parameters["sortType"];
                var paramSortfield = string.IsNullOrEmpty(parameters["sortField"]) ? 0 : int.Parse(parameters["sortField"]);


                DatabaseResponse response = await _courseAccess.GetAllCourse(pageNo, pageSize, paramIsSearch, paramSortType, paramSortfield);

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
        /// Search the course list based upon keywords.
        /// </summary>
        /// <param name="keyword">Search Keyword</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>List of Coruse</returns>
        [HttpGet("SearchCourses/{keyword}/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> SearchCourses(string keyword, int pageNumber, int pageSize)
        {
            try
            {

                CourseDataAccess _courseAccess = new CourseDataAccess(_iconfiguration);

                DatabaseResponse response = await _courseAccess.SearchCourses(keyword, pageNumber,  pageSize);

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
        /// Get a course referenced by the Id passed
        /// </summary>
        /// <param name="id"></param>
        /// <returns>OperationsResponse</returns>
        // GET: api/Courses/5
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
                CourseDataAccess _courseAccess = new CourseDataAccess(_iconfiguration);

                DatabaseResponse response = await _courseAccess.GetCourse(id);

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
        /// This will Create a CourseMaster entry.
        /// </summary>
        /// <param name="course">
        /// CreateCourseMaster
        ///Body: 
        ///{ 
        ///  "title": "Course8_updated",
        ///  "categoryId": 1,
        ///  "subCategoryId": 1,
        ///  "thumbnail": "Not yet uploaded",
        ///  "courseDescription": "This description is updated",
        ///  "keywords": "No keywords are given",
        ///  "courseContent": "No content given",  
        ///  "materialTypeId": 2,
        ///  "copyRightId": 2,
        ///  "isDraft": false,
        ///  "createdBy": 1,
        ///  "references": [
        ///  { "urlReference": "ref1" }, 
        ///  { "urlReference": "ref2" },
        ///  { "urlReference": "ref3" }],
        ///  "courseFiles": [
        ///  {"associatedFile": "file1"},
        ///  {"associatedFile": "file2"}]}}
        /// </param>
        /// <returns>OperationResponse</returns>
        // POST: api/Courses
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCourseRequest course)
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

                CourseDataAccess _courseAccess = new CourseDataAccess(_iconfiguration);

                DatabaseResponse response = await _courseAccess.CreateCourse(course);

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
        /// Share content information to create
        /// </summary>
        /// <param name="sharedContentInfoCreate">object</param>
        /// <returns></returns>
        [HttpPost("SharedContent")]
        public async Task<IActionResult> SharedContent([FromBody] SharedContentInfoCreate sharedContentInfoCreate)
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

                CourseDataAccess _courseAccess = new CourseDataAccess(_iconfiguration);

                DatabaseResponse response = await _courseAccess.SharedContent(sharedContentInfoCreate);

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
        /// Download content for course
        /// </summary>
        /// <param name="downloadContentInfoCreate">object</param>
        /// <returns></returns>
        [HttpPost("DownloadContentForCourse")]
        public async Task<IActionResult> DownloadContent([FromBody] DownloadContentInfoCreate downloadContentInfoCreate)
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

                CourseDataAccess _courseAccess = new CourseDataAccess(_iconfiguration);

                DatabaseResponse response = await _courseAccess.DownloadContent(downloadContentInfoCreate);

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
        /// This will update CourseMaster entry by id and details passed.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="course">
        ///Body: 
        ///{   
        ///{  "id":8,
        ///  "title": "Course8_updated",
        ///  "categoryId": 1,
        ///  "subCategoryId": 1,
        ///  "thumbnail": "Not yet uploaded",
        ///  "courseDescription": "This description is updated",
        ///  "keywords": "No keywords are given",
        ///  "courseContent": "No content given",  
        ///  "materialTypeId": 2,
        ///  "copyRightId": 2,
        ///  "isDraft": false,
        ///  "createdBy": 1,
        ///  "references": [
        ///  { "urlReference": "ref1" }, 
        ///  { "urlReference": "ref2" },
        ///  { "urlReference": "ref3" }],
        ///  "courseFiles": [
        ///  {"associatedFile": "file1"},
        ///  {"associatedFile": "file2"}]}}
        /// </param>
        /// <returns>OperationsResponse</returns>
        // PUT: api/Courses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateCourseRequest course)
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

                CourseDataAccess _courseAccess = new CourseDataAccess(_iconfiguration);

                DatabaseResponse response = await _courseAccess.UpdateCourse(course, id);

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
        /// Widthdrawal the content resource or course
        /// </summary>
        /// <param name="contentId">Resource or course ID</param>
        /// <param name="contentTypeId">1=Course and 2= resource</param>
        /// <returns></returns>
        [HttpPut("ContentWithdrawal")]
        public async Task<IActionResult> ContentWithdrawal(int contentId, int contentTypeId)
        {
            try
            {
             
                CourseDataAccess _courseAccess = new CourseDataAccess(_iconfiguration);

                DatabaseResponse response = await _courseAccess.ContentWithdrawal(contentId, contentTypeId);

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
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.NotWidthdrawal));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.NotWidthdrawal),
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
        /// This will delete a course record by the id passed
        /// </summary>
        /// <param name="id"></param>
        /// <returns>OperationResponse</returns>
        // DELETE: api/Courses/5
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
                CourseDataAccess _courseAccess = new CourseDataAccess(_iconfiguration);

                DatabaseResponse response = await _courseAccess.DeleteCourse(id);

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
        /// This will approve a course
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ApproveCourse/{id}/{userId}")]
        public async Task<IActionResult> ApproveCourse(decimal id, int userId)
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
                CourseDataAccess _courseAccess = new CourseDataAccess(_iconfiguration);

                DatabaseResponse response = await _courseAccess.ApproveCourse(id, userId);

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
            /// Course Enrollment status
            /// </summary>
            /// <param name="courseEnrollmentCreate"></param>
            /// <returns></returns>
        [HttpPost("CourseEnrolledStatus")]
        public async Task<IActionResult> CourseEnrolledStatus(CourseEnrollmentCreate courseEnrollmentCreate)
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
                CourseDataAccess _courseAccess = new CourseDataAccess(_iconfiguration);

                DatabaseResponse response = await _courseAccess.CourseEnrolledStatus(courseEnrollmentCreate);

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
        /// Enroll the course
        /// </summary>
        /// <param name="courseEnrollmentCreate">object</param>
        /// <returns></returns>
        [HttpPost("CourseEnrollment")]
        public async Task<IActionResult> CourseEnrollment(CourseEnrollmentCreate courseEnrollmentCreate)
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

                CourseDataAccess _courseAccess = new CourseDataAccess(_iconfiguration);

                DatabaseResponse response = await _courseAccess.CourseEnrollment(courseEnrollmentCreate);

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
        /// Create comment on course
        /// </summary>
        /// <param name="courseComment">Course Comment object</param>
        /// <returns>true/false status</returns>
        [HttpPost("CommentOnCourse")]
        public async Task<IActionResult> CommentOnCourse([FromBody] CourseCommentRequest courseComment)
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

                CourseDataAccess _courseAccess = new CourseDataAccess(_iconfiguration);

                DatabaseResponse response = await _courseAccess.CommentOnCourse(courseComment);

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
        /// Update course comment
        /// </summary>
        /// <param name="courseCommentUpdate">object course comment update</param>
        /// <returns>true false status</returns>
        [HttpPost("UpdateCourseComment")]
        public async Task<IActionResult> UpdateCourseComment([FromBody] CourseCommentUpdateRequest courseCommentUpdate)
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

                CourseDataAccess _courseAccess = new CourseDataAccess(_iconfiguration);

                DatabaseResponse response = await _courseAccess.UpdateCourseComment(courseCommentUpdate);

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

		///// <summary>
		///// DELETE: DeleteCourseComment/1/1
		///// </summary>
		///// <param name="id"></param>
		///// <returns></returns>
		[HttpDelete("DeleteCourseComment/{id}/{requestedBy}")]
        public async Task<IActionResult> DeleteCourseComment(decimal id, int requestedBy)
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
                CourseDataAccess _courseAccess = new CourseDataAccess(_iconfiguration);

                DatabaseResponse response = await _courseAccess.DeleteCourseComment(id, requestedBy);

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
        /// Hide a course comment. Can only be done by the resource owner
        /// </summary>
        /// <param name="id"></param>
        /// <param name="courseId"></param>
        /// <param name="requestedBy"></param>
        /// <returns></returns>
        [HttpGet("HideCourseComment/{id}/{courseId}/{requestedBy}")]
        public async Task<IActionResult> HideCourseComment(decimal id, decimal courseId, int requestedBy)
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
                CourseDataAccess _courseAccess = new CourseDataAccess(_iconfiguration);

                DatabaseResponse response = await _courseAccess.HideCourseCommentByAuthor(id, courseId, requestedBy);

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
        [HttpPost("ReportCourse")]
        public async Task<IActionResult> ReportCourse(CourseReportAbuseWithComment request)
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

                CourseDataAccess _courseAccess = new CourseDataAccess(_iconfiguration);

                DatabaseResponse response = await _courseAccess.ReportCourseWithComment(request);

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
                else if (response.ResponseCode == (int)DbReturnValue.RecordExists)
                {
                    Log.Error(EnumExtensions.GetDescription(CommonErrors.AlreadyReportedCourse));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.AlreadyReportedCourse),
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
        /// Submit user answers
        /// </summary>
        /// <param name="answerOptions">user answer options</param>
        /// <returns></returns>
        [HttpPost("SubmitUserAnswers")]
        public async Task<IActionResult> SubmitUserAnswers([FromBody] UserAnswersOptions answerOptions)
        {
            try
            {


                CourseDataAccess _courseAccess = new CourseDataAccess(_iconfiguration);

                DatabaseResponse response = await _courseAccess.SubmitUserAnswers(answerOptions);

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
        /// Get course test based upon course ID
        /// </summary>
        /// <param name="courseId">Course ID</param>
        /// <returns></returns>
        [HttpGet("GetCourseTest/{courseId}")]
        public async Task<IActionResult> GetCourseTestById(int courseId)
        {
            try
            {

                CourseDataAccess _courseAccess = new CourseDataAccess(_iconfiguration);

                DatabaseResponse response = await _courseAccess.GetCourseTestById(courseId);

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
        /// Rate a resource with comment
        /// </summary>
        /// <param name="request">
        /// 
        /// </param>
        /// <returns></returns>
        /// 
        [HttpPost("RateCourse")]
        public async Task<IActionResult> RateCourse(CourseRatingRequest request)
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

                CourseDataAccess _courseAccess = new CourseDataAccess(_iconfiguration);

                DatabaseResponse response = await _courseAccess.RateCourse(request);

                if (response.ResponseCode == (int)DbReturnValue.CreateSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.RatingCourseSucess),
                        ReturnedObject = response.Results
                    });
                }
                else if (response.ResponseCode == (int)DbReturnValue.CreationFailed)
                {
                    Log.Error(EnumExtensions.GetDescription(CommonErrors.RatingCourseFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.RatingCourseFailed),
                        ReturnedObject = response.Results
                    });
                }

                else
                {
                    Log.Error(EnumExtensions.GetDescription(CommonErrors.AlreadyRatedCourse));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.AlreadyRatedCourse),
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
        /// Report the course comment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("ReportCourseComment")]
        public async Task<IActionResult> ReportCourseComment(CourseCommentReportAbuseWithComment request)
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

                CourseDataAccess _courseAccess = new CourseDataAccess(_iconfiguration);

                DatabaseResponse response = await _courseAccess.ReportCourseCommentWithComment(request);

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
                else if (response.ResponseCode == (int)DbReturnValue.RecordExists)
                {
                    Log.Error(EnumExtensions.GetDescription(CommonErrors.AlreadyReportedCourse));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.AlreadyReportedCourse),
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


    }
}

