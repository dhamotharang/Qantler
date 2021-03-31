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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace OERService.Controllers
{
	//[Authorize]
	[Route("api/[controller]")]
    [ApiController]
    public class QrcController : ControllerBase
    {
		readonly IConfiguration _iconfiguration;
		private readonly IConverter _converter;
        public QrcController(IConfiguration configuration, IConverter converter)
        {
            _iconfiguration = configuration;
            _converter = converter;

        }
        /// <summary>
        /// This will get all records from QRC.
        /// </summary>
        /// <returns>OperationsResponse</returns>
        // GET: api/qrc
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                QrcDataAccess _qrcAccess = new QrcDataAccess(_iconfiguration, _converter);

                DatabaseResponse response = await _qrcAccess.GetQrcies();

                if (response.ResponseCode == 33)
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

        [HttpGet("GetQrcByCategory/{CategoryId}")]
        public async Task<IActionResult> GetQrcByCategory(int CategoryId)
        {
            try
            {
                QrcDataAccess _qrcAccess = new QrcDataAccess(_iconfiguration, _converter);

                DatabaseResponse response = await _qrcAccess.GetQrciesByCategory(CategoryId);

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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>OperationsResponse</returns>
        // GET: api/qrc/5
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
                QrcDataAccess _qrcAccess = new QrcDataAccess(_iconfiguration, _converter);

                DatabaseResponse response = await _qrcAccess.GetQrc(id);

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
        /// This will Create a QRC entry.
        /// </summary>
        /// <param name="qrc">
        /// CreateQRCMaster
        ///Body: 
        ///{
        ///	"Name" : "",
        ///	"Description" : "",
        ///	"CreatedBy" : "1"        
        ///}
        /// </param>
        /// <returns>OperationResponse</returns>
        // POST: api/qrc
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateQrcMaster qrc)
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

                QrcDataAccess _qrcAccess = new QrcDataAccess(_iconfiguration, _converter);

                DatabaseResponse response = await _qrcAccess.CreateQrc(qrc);

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
        /// This will update qrc entry by id and details passed.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="qrc">
        ///Body: 
        ///{
        ///	"Name" : "", 
        ///	"Description" : "",
        ///	"UpdatedBy" : "1" ,
        ///	"Active":true/false
        ///}       
        /// </param>
        /// <returns>OperationsResponse</returns>
        // PUT: api/qrc/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateQrcMaster qrc)
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

                QrcDataAccess _qrcAccess = new QrcDataAccess(_iconfiguration, _converter);

                DatabaseResponse response = await _qrcAccess.UpdateQrc(qrc, id);

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
        /// This will delete a qrc record by the id passed
        /// </summary>
        /// <param name="id"></param>
        /// <returns>OperationResponse</returns>
        // DELETE: api/qrc/5
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
                QrcDataAccess _qrcAccess = new QrcDataAccess(_iconfiguration, _converter);

                DatabaseResponse response = await _qrcAccess.DeleteQrc(id);

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
                else if (response.ResponseCode == (int)DbReturnValue.ActiveTryDelete)
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.ActiveTryDelete));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.ActiveTryDelete),
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


        [HttpDelete("DeleteUnAssignedQrc/{id}")]
        public async Task<IActionResult> DeleteUnAssignedQrc(int id)
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
                QrcDataAccess _qrcAccess = new QrcDataAccess(_iconfiguration, _converter);

                DatabaseResponse response = await _qrcAccess.DeleteUnAssignedQrc(id);

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
                else if (response.ResponseCode == (int)DbReturnValue.ActiveTryDelete)
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.ActiveTryDelete));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.ActiveTryDelete),
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
        /// This will get (GET) QRC Category.
        /// </summary>
        /// <param name="qrcID"></param>
        /// <returns>OperationResponse</returns>
        [HttpGet("GetQRCCategory/{qrcID}")]
        public async Task<IActionResult> GetCategoryByQRC(int qrcID)
        {
            QrcDataAccess _qrcAccess = new QrcDataAccess(_iconfiguration, _converter);
            DatabaseResponse response = await _qrcAccess.GetCategoryByQRC(qrcID);
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

        /// <summary>
        /// This will Update (POST) Content Status.
        /// </summary>
        /// <param name="contentApproveRequest"></param>
        /// <returns>OperationResponse</returns>
        [HttpPost("UpdateContentStatus")]
        public async Task<IActionResult> UpdateContentStatus(ContentApproveRequest contentApproveRequest)
        {
            try
            {
                QrcDataAccess _qrcAccess = new QrcDataAccess(_iconfiguration, _converter);

                DatabaseResponse response = await _qrcAccess.UpdateContentStatus(contentApproveRequest);

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
        /// This will Update (POST) Content Approval.
        /// </summary>
        /// <param name="contentApprovalRequest"></param>
        /// <returns>OperationResponse</returns>
        [HttpPost("GetContent")]
        public async Task<IActionResult> GetContentApproval(ContentApprovalRequest contentApprovalRequest)
        {
            try
            {
            QrcDataAccess _qrcAccess = new QrcDataAccess(_iconfiguration, _converter);

            DatabaseResponse response = await _qrcAccess.GetContentAsync(contentApprovalRequest);

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
                    StatusCode = ((int) ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }

        /// <summary>
        /// This will get (GET) all users in QRC.
        /// </summary>
        /// <param name="qrcId"></param>
        /// <param name="categoryId"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns>OperationResponse</returns>
        [HttpGet]
        [Route("GetUsers/{qrcId}/{categoryId}/{pageNo}/{pageSize}")]
        public async Task<IActionResult> GetAllUserAsync(int qrcId, int categoryId, int pageNo, int pageSize)
        {
            QrcDataAccess _qrcAccess = new QrcDataAccess(_iconfiguration, _converter);
            DatabaseResponse response = await _qrcAccess.GetUsers(qrcId, categoryId, pageNo, pageSize);
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

		/// <summary>
		/// This will fetch (GET) all users to add in QRC.
		/// </summary>
		/// <param name="qrcId"></param>
		/// <param name="pageNo"></param>
		/// <param name="pageSize"></param>
		/// <param name="filterCategoryId"></param>
		/// <param name="categoryId"></param>
		/// <returns>OperationResponse</returns>
		[HttpGet]
        [Route("FetchUsersToAdd/{qrcId}/{categoryId}/{pageNo}/{pageSize}/{filterCategoryId}")]
        public async Task<IActionResult> FetchAllUsers(int qrcId, int categoryId, int pageNo, int pageSize, int filterCategoryId)
        {
            QrcDataAccess _qrcAccess = new QrcDataAccess(_iconfiguration, _converter);
            DatabaseResponse response = await _qrcAccess.GetUsersToAdd(qrcId, categoryId, pageNo, pageSize, filterCategoryId);
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


        /// <summary>
        /// This will add (POST) users in QRC.
        /// </summary>
        /// <param name="objQRCUserMapping"></param>
        /// <returns>OperationResponse</returns>
        [HttpPost("AddQRCUsers")]
        public async Task<IActionResult> AddQRCUserInfo([FromBody] List<QrcUserMapping> objQRCUserMapping)
        {
            try
            {
                if (objQRCUserMapping.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("QRCId", typeof(int));
                    dt.Columns.Add("CategoryId", typeof(int));
                    dt.Columns.Add("UserId", typeof(int));
                    dt.Columns.Add("CreatedBy", typeof(int));
                    DataRow dr = null;
                    foreach (var item in objQRCUserMapping)
                    {
                        dr = dt.NewRow();
                        dr["QRCId"] = item.QRCId;
                        dr["CategoryId"] = item.CategoryId;
                        dr["UserId"] = item.UserId;
                        dr["CreatedBy"] = item.CreatedBy;
                        dt.Rows.Add(dr);
                    }
                    QrcDataAccess _qrcAccess = new QrcDataAccess(_iconfiguration, _converter);
                    DatabaseResponse response = await _qrcAccess.AddQRCUsers(dt,objQRCUserMapping.FirstOrDefault().EmailUrl);


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
                else
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.RecordExists),
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
                    StatusCode = ((int) ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }


        /// <summary>
        /// This will update (PUT) users in QRC.
        /// </summary>
        /// <param name="qRCUserMapping"></param>
        /// <returns>OperationResponse</returns>
        [HttpPut("UpdateQRCUser")]
        public async Task<IActionResult> UpdateQRCUser(QrcUserMapping qRCUserMapping)
        {
            try
            {
                QrcDataAccess _qrcAccess = new QrcDataAccess(_iconfiguration, _converter);
                DatabaseResponse response = await _qrcAccess.UpdateQRCUser(qRCUserMapping);

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
                    StatusCode = ((int) ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }


        /// <summary>
        /// This will get (GET) QRC by user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>OperationResponse</returns>
        [HttpGet]
        [Route("QrcByUser/{userId}")]
        public async Task<IActionResult> GetQrcByUserID(int userId)
        {
            try
            {
                QrcDataAccess _qrcAccess = new QrcDataAccess(_iconfiguration, _converter);

                DatabaseResponse response = await _qrcAccess.GetQrcByUserID(userId);

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
