using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Core.Helpers;
using Microsoft.Extensions.Configuration;
using Core.Models;
using Core.Enums;
using OERService.Models;
using Serilog;
using OERService.Helpers;
using OERService.Controllers;
using DinkToPdf.Contracts;
using System.Text;

namespace OERService.DataAccess
{
    public class QrcDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

		private readonly IConfiguration _configuration;
		private readonly IConverter _converter;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="converter"></param>
        public QrcDataAccess(IConfiguration configuration, IConverter converter)
        {
            _configuration = configuration;
            _converter = converter;
        }

        public async Task<DatabaseResponse> CreateQrc(CreateQrcMaster qrcMaster)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Name",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Description",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CreatedBy",  SqlDbType.Int ),
                    new SqlParameter( "@CategoryIds",  SqlDbType.NVarChar )
                };

                parameters[0].Value = qrcMaster.Name;
                parameters[1].Value = qrcMaster.Description;
                parameters[2].Value = qrcMaster.CreatedBy;
                parameters[3].Value = qrcMaster.CategoryIds;
                _DataHelper = new DataAccessHelper("CreateQrc", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<QrcMaster> qrcs = new List<QrcMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    qrcs = (from model in dt.AsEnumerable()
                            select new QrcMaster()
                            {
                                Id = model.Field<int>("Id"),
                                Name = model.Field<string>("Name"),
                                Description = model.Field<string>("Description"),
                                CreatedBy = model.Field<string>("CreatedBy"),
                                CreatedOn = model.Field<DateTime>("CreatedOn"),
                                UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                UpdatedBy = model.Field<string>("UpdatedBy"),
                                Active = model.Field<bool>("Active")

                            }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = qrcs };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));
                throw;
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> GetQrciesByCategory(int categoryId)
        {
            try
            {
                SqlParameter[] parameters =
            {
                    new SqlParameter( "@Id",  SqlDbType.Int )

                };

                parameters[0].Value = categoryId;
                _DataHelper = new DataAccessHelper("GetQRCbyCategoryId", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<QrcMaster> qrcs = new List<QrcMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    qrcs = (from model in dt.AsEnumerable()
                            select new QrcMaster()
                            {
                                Id = model.Field<int>("Id"),
                                Name = model.Field<string>("Name"),
                                Description = model.Field<string>("Description"),
                                CreatedBy = model.Field<string>("CreatedBy"),
                                CreatedOn = model.Field<DateTime>("CreatedOn"),
                                UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                UpdatedBy = model.Field<string>("UpdatedBy"),
                                Active = model.Field<bool>("Active")

                            }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = qrcs };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));
                throw;
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> GetQrcies()
        {
            try
            {
                _DataHelper = new DataAccessHelper("GetQrc", _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<QrcMaster> qrcs = new List<QrcMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    qrcs = (from model in dt.AsEnumerable()
                            select new QrcMaster()
                            {
                                Id = model.Field<int>("Id"),
                                Name = model.Field<string>("Name"),
                                Description = model.Field<string>("Description"),
                                CreatedBy = model.Field<string>("CreatedBy"),
                                CreatedOn = model.Field<DateTime>("CreatedOn"),
                                UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                UpdatedBy = model.Field<string>("UpdatedBy"),
                                Active = model.Field<bool>("Active")

                            }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = qrcs };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));
                throw;
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> GetQrcByUserID(int userId)
        {
            try
            {
                SqlParameter[] parameters =
             {
                    new SqlParameter( "@UserID",  SqlDbType.Int )

                };

                parameters[0].Value = userId;
                _DataHelper = new DataAccessHelper("sps_QRCByUserID", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<QrcMaster> qrcs = new List<QrcMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    qrcs = (from model in dt.AsEnumerable()
                            select new QrcMaster()
                            {
                                Id = model.Field<int>("Id"),
                                Name = model.Field<string>("Name"),
                                Description = model.Field<string>("Description"),
                                CreatedBy = model.Field<string>("CreatedBy"),
                                CreatedOn = model.Field<DateTime>("CreatedOn"),
                                UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                UpdatedBy = model.Field<string>("UpdatedBy"),
                                Active = model.Field<bool>("Active"),
                                CategoryId = model.Field<int?>("CategoryId"),
                                CategoryName = model.Field<string>("CategoryName"),
                                CategoryNameAr = model.Field<string>("CategoryNameAr")


                            }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = qrcs };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));
                throw;
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> UpdateContentStatus(ContentApproveRequest contentApproveRequest)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@ContentApprovalId",  SqlDbType.Int ),
                    new SqlParameter( "@ContentType",  SqlDbType.Int ),
                    new SqlParameter( "@Status",  SqlDbType.Int ),
                    new SqlParameter( "@Comment",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CreatedBy",  SqlDbType.Int ),
                      new SqlParameter( "@ContentId",  SqlDbType.Int ),
                      new SqlParameter( "@EmailUrl",  SqlDbType.NVarChar )

                };

                parameters[0].Value = contentApproveRequest.ContentApprovalId;
                parameters[1].Value = contentApproveRequest.ContentType;
                parameters[2].Value = contentApproveRequest.Status;
                parameters[3].Value = contentApproveRequest.Comment;
                parameters[4].Value = contentApproveRequest.CreatedBy;
                parameters[5].Value = contentApproveRequest.ContentId;
                parameters[6].Value = contentApproveRequest.EmailUrl;
                _DataHelper = new DataAccessHelper("spi_ContentApproval", parameters, _configuration);

                DataSet dt = new DataSet();

                int result = await _DataHelper.RunAsync(dt);
                UserEmail userEmail = null;
                if (dt.Tables[0] != null && dt.Tables[0].Rows.Count > 0 && dt.Tables[0].Rows.Count > 0)
                {
                        int IsPublish = Convert.ToInt32(dt.Tables[0].Rows[0]["PublishContent"]);
                        int portalLanguageId = Convert.ToInt32(dt.Tables[0].Rows[0]["PortalLanguageId"]);
                        string text = string.Empty;
                        string buttonText = string.Empty;
                        if (IsPublish == 1)
                        {
                            //sent email for published
                            if (contentApproveRequest.ContentType == 1)
                            {
                                ContentMediaController controller = new ContentMediaController(_configuration, _converter);
                                await controller.GeneratePdf(contentApproveRequest.ContentId, 1);
                            }
                            else
                            {
                                ContentMediaController controller = new ContentMediaController(_configuration, _converter);
                                await controller.GeneratePdf(contentApproveRequest.ContentId, 2);
                            }
                            userEmail = new UserEmail();
                           

                            userEmail.Email = Convert.ToString(dt.Tables[0].Rows[0]["Email"]);

                            if (portalLanguageId == 2)
                            {
                                text = "Your Content  has been published. Click below button to view";
                                buttonText = "View Content";
                                userEmail.Subject = "Your course or resource has been published.";
                            }
                            else
                            {
                                text = "تم نشر المصدر المحتوى الخاص بك، يمكنك الضغط على الرابط أدناه للعرض";
                                buttonText = "عرض المحتوى";
                                userEmail.Subject = "تم نشر المحتوى";
                            }
                            userEmail.Body = Emailer.CreateEmailBody(Convert.ToString(dt.Tables[0].Rows[0]["UserName"]), 
                                                        contentApproveRequest.EmailUrl, text, buttonText, dt.Tables[0].Rows[0]["PortalLanguageId"]==null?
                                                        0: Convert.ToInt32(dt.Tables[0].Rows[0]["PortalLanguageId"]),_configuration);
                            await Emailer.SendEmailAsync(userEmail,_configuration);
                       
                    }
                        else if(IsPublish == 0)
                        {
                            //sent email for published
                            userEmail = new UserEmail();
                            userEmail.Email = Convert.ToString(dt.Tables[0].Rows[0]["Email"]);
                            if (portalLanguageId == 2)
                            {
                                if(dt.Tables[1] != null && dt.Tables[1].Rows.Count > 0)
                                {
                                    text = "Your Content  has been rejected. Click below button to view. Reason(s) : ";
                                    StringBuilder builder = new StringBuilder();
                                    builder.Append("<br/>");
                                    builder.Append("<ol>");
                                    foreach(string reason in dt.Tables[1].Rows[0]["Comments"].ToString().Remove(dt.Tables[1].Rows[0]["Comments"].ToString().Length-2).Split(";;"))
                                    {
                                        builder.Append("<li>"+reason+"</li>");
                                    }
                                    builder.Append("</ol>");
                                    text += builder.ToString();
                                }
                                else
                                {
                                    text = "Your Content  has been rejected. Click below button to view. Reason : " + contentApproveRequest.Comment;
                                }
                                
                                buttonText = "View Content";
                                userEmail.Subject = "Your course or resource has been rejected.";
                            }
                            else
                            {
                                if (dt.Tables[1] != null && dt.Tables[1].Rows.Count > 0)
                                {
                                    text = "بعد المراجعة، تم رفض المحتوى المقدم من قبلك خلال عملية التحقق من الجودة.يمكنك الضغط على الرابط أدناه لعرض المحتوى. "  + "اللأسباب:";
                                    StringBuilder builder = new StringBuilder();
                                    builder.Append("<br/>");
                                    builder.Append("<ol>");
                                    foreach (string reason in dt.Tables[1].Rows[0]["Comments"].ToString().Remove(dt.Tables[1].Rows[0]["Comments"].ToString().Length - 2).Split(";;"))
                                    {
                                        builder.Append("<li>" + reason + "</li>");
                                    }
                                    builder.Append("</ol>");
                                    text += builder.ToString();
                                }
                                else
                                {
                                    text = "بعد المراجعة، تم رفض المحتوى المقدم من قبلك خلال عملية التحقق من الجودة.يمكنك الضغط على الرابط أدناه لعرض المحتوى .<br><br>الأسباب:" + contentApproveRequest.Comment + "<br/>";
                                }
                                
                                buttonText = "عرض المحتوى";
                                userEmail.Subject = "تم رفض المحتوى.";
                            }
                            userEmail.Body = Emailer.CreateEmailBody(Convert.ToString(dt.Tables[0].Rows[0]["UserName"]), contentApproveRequest.EmailUrl, text, buttonText, dt.Tables[0].Rows[0]["PortalLanguageId"] == null ? 0 : Convert.ToInt32(dt.Tables[0].Rows[0]["PortalLanguageId"]),_configuration);
                            await Emailer.SendEmailAsync(userEmail,_configuration);
                        }
                    }
                return new DatabaseResponse { ResponseCode = result, Results = null };
     
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));
                throw;
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> GetContentAsync(ContentApprovalRequest contentApprovalRequest)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@QrcId",  SqlDbType.Int ),
                    new SqlParameter( "@CategoryID",  SqlDbType.Int ),
                    new SqlParameter( "@UserId",  SqlDbType.Int ),
                    new SqlParameter( "@PageNo",  SqlDbType.Int ),
                    new SqlParameter( "@PageSize",  SqlDbType.Int )

                };

                parameters[0].Value = contentApprovalRequest.QrcId;
                parameters[1].Value = contentApprovalRequest.CategoryId;
                parameters[2].Value = contentApprovalRequest.UserId;
                parameters[3].Value = contentApprovalRequest.PageNo;
                parameters[4].Value = contentApprovalRequest.PageSize;
                _DataHelper = new DataAccessHelper("sps_ContentForApproval", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<QrcManagement> content = new List<QrcManagement>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    content = (from model in dt.AsEnumerable()
                            select new QrcManagement()
                            {
                                RowNumber = model.Field<Int64>("RowNumber"),
                                ContentId = model.Field<int>("ContentId"),
                                ContentApprovalId = model.Field<int>("ContentApprovalId"),
                                Title = model.Field<string>("Title"),
                                CreatedOn = model.Field<DateTime>("CreatedOn"),
                                ContentType = model.Field<int>("ContentType"),
                                CategoryId = model.Field<int>("CategoryId"),
                                TotalRows = model.Field<Int64>("TotalRows")

                            }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = content };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));
                throw;
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> GetQrc(int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("GetQrcById", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<QrcMaster> qrcs = new List<QrcMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    qrcs = (from model in dt.AsEnumerable()
                            select new QrcMaster()
                            {
                                Id = model.Field<int>("Id"),
                                Name = model.Field<string>("Name"),
                                Description = model.Field<string>("Description"),
                                CreatedBy = model.Field<string>("CreatedBy"),
                                CreatedOn = model.Field<DateTime>("CreatedOn"),
                                UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                UpdatedBy = model.Field<string>("UpdatedBy"),
                                Active = model.Field<bool>("Active")

                            }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = qrcs };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));
                throw;
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> GetCategoryByQRC(int qrcID)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@QrcID",  SqlDbType.Int )

                };

                parameters[0].Value = qrcID;

                _DataHelper = new DataAccessHelper("sps_GetCategoryByQrcId", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<CategoryMaster> categories = new List<CategoryMaster>();
                if (dt != null && dt.Rows.Count > 0)
                {

                    categories = (from model in dt.AsEnumerable()
                                  select new CategoryMaster()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Name = model.Field<string>("Name"),
                                      //CreatedBy = model.Field<string>("CreatedBy"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                     // UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                     // UpdatedBy = model.Field<string>("UpdatedBy"),
                                    //  Active = model.Field<bool>("Active")

                                  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = categories };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));
                throw;
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> DeleteUnAssignedQrc(int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("spd_UnAssignedQRC", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);


                return new DatabaseResponse { ResponseCode = result, Results = null };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));
                throw;
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> DeleteQrc(int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("DeleteQrc", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<QrcMaster> qrcs = new List<QrcMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    qrcs = (from model in dt.AsEnumerable()
                            select new QrcMaster()
                            {
                                Id = model.Field<int>("Id"),
                                Name = model.Field<string>("Name"),
                                Description = model.Field<string>("Description"),
                                CreatedBy = model.Field<string>("CreatedBy"),
                                CreatedOn = model.Field<DateTime>("CreatedOn"),
                                UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                UpdatedBy = model.Field<string>("UpdatedBy"),
                                Active = model.Field<bool>("Active")
                            }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = qrcs };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));
                throw;
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> UpdateQRCUser(QrcUserMapping qRCUserMapping)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@QrcID",  SqlDbType.Int ),
                    new SqlParameter( "@CategoryID",  SqlDbType.Int ),
                    new SqlParameter( "@UserID",  SqlDbType.Int )

                };

                parameters[0].Value = qRCUserMapping.QRCId;
                parameters[1].Value = qRCUserMapping.CategoryId;
                parameters[2].Value = qRCUserMapping.UserId;
                _DataHelper = new DataAccessHelper("spu_QRCUserMapping", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<QrcMaster> qrcs = new List<QrcMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    qrcs = (from model in dt.AsEnumerable()
                            select new QrcMaster()
                            {
                                Id = model.Field<int>("Id"),
                                Name = model.Field<string>("Name"),
                                Description = model.Field<string>("Description"),
                                CreatedBy = model.Field<string>("CreatedBy"),
                                CreatedOn = model.Field<DateTime>("CreatedOn"),
                                UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                UpdatedBy = model.Field<string>("UpdatedBy"),
                                Active = model.Field<bool>("Active")
                            }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = qrcs };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));
                throw;
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> AddQRCUsers(DataTable dt,string EmailUrl)
        {

            try
            {

                SqlParameter[] parameters =
            {
                    new SqlParameter( "@QRCUserDetails",  SqlDbType.Structured )

                };

                parameters[0].Value = dt;


                _DataHelper = new DataAccessHelper("USP_Insert_QRCUsers", parameters, _configuration);

                DataTable dt1 = new DataTable();
                int result = await _DataHelper.RunAsync(dt1);
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    var users = (from model in dt1.AsEnumerable()
                            select new UserMaster()
                            {
                                Id = model.Field<int>("Id"),
                                Email = model.Field<string>("Email"),
                                FirstName = model.Field<string>("FirstName"),
                                LastName = model.Field<string>("LastName"),
                                PortalLanguageId = model.Field<int?>("PortalLanguageId")
                            }).ToList();
                    foreach (UserMaster item in users)
                    {
                        UserEmail userEmail = new UserEmail();
                        userEmail.Email = item.Email;
                        int? portalLanguageId = item.PortalLanguageId;
                        string text = string.Empty;
                        string buttonText = string.Empty;
                        if (portalLanguageId == 2)
                        {
                            text = "Now you can create review the courses and resources.";
                            buttonText = "Review Content";
                            userEmail.Subject = "Reviewer Access provided";
                        }
                        else
                        {
                            text = "الآن يمكنك مراجعة المصادر والمساقات التعليمية.";
                            buttonText = "منح صلاحية المراجعة.";
                            userEmail.Subject = "منح صلاحية المراجعة.";
                        }
                        

                        userEmail.Body = Emailer.CreateEmailBody(item.FirstName +' ' +item.LastName, EmailUrl, text, buttonText, portalLanguageId,_configuration);
                        
                        await Emailer.SendEmailAsync(userEmail,_configuration);
                    }
                }
                return new DatabaseResponse { ResponseCode = result, Results = null };
            }
            catch (Exception ex)
            {
				Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Medium));
				throw;
			}
        }
        public async Task<DatabaseResponse> UpdateQrc(UpdateQrcMaster qrcMaster, int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Int ),
                    new SqlParameter( "@Name",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Description",  SqlDbType.NVarChar ),
                    new SqlParameter( "@UpdatedBy",  SqlDbType.Int ),
                    new SqlParameter( "@Active",  SqlDbType.Bit ),
                    new SqlParameter( "@CategoryIds",  SqlDbType.NVarChar )

                };


                parameters[0].Value = id;
                parameters[1].Value = qrcMaster.Name;
                parameters[2].Value = qrcMaster.Description;
                parameters[3].Value = qrcMaster.UpdatedBy;
                parameters[4].Value = qrcMaster.Active;
                parameters[5].Value = qrcMaster.CategoryIds;
                _DataHelper = new DataAccessHelper("UpdateQrc", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<QrcMaster> qrcs = new List<QrcMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    qrcs = (from model in dt.AsEnumerable()
                            select new QrcMaster()
                            {
                                Id = model.Field<int>("Id"),
                                Name = model.Field<string>("Name"),
                                Description = model.Field<string>("Description"),
                                CreatedBy = model.Field<string>("CreatedBy"),
                                CreatedOn = model.Field<DateTime>("CreatedOn"),
                                UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                UpdatedBy = model.Field<string>("UpdatedBy"),
                                Active = model.Field<bool>("Active")

                            }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = qrcs };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));
                throw;
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> GetUsers(int qrcId, int categoryId,int pageNo,int pageSize)
        {
            try
            {
                SqlParameter[] parameters =
            {
                    new SqlParameter( "@QrcId",  SqlDbType.Int ),
                    new SqlParameter( "@Category",  SqlDbType.Int ),
                    new SqlParameter( "@PageNo",  SqlDbType.Int ),
                    new SqlParameter( "@PageSize",  SqlDbType.Int )

                };


                parameters[0].Value = qrcId;
                parameters[1].Value = categoryId;
                parameters[2].Value = pageNo;
                parameters[3].Value = pageSize;

                _DataHelper = new DataAccessHelper("sps_GetUserForQRC",parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<QrcUsers> users = new List<QrcUsers>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    users = (from model in dt.AsEnumerable()
                             select new QrcUsers()
                             {
                                 UserId = model.Field<int>("UserId"),
                                 UserName = model.Field<string>("UserName"),
                                 ResourceContributed = model.Field<int>("ResourceContributed"),
                                 CourseCreated = model.Field<int>("CourseCreated"),
                                 CurrentQRCS = model.Field<int>("CurrentQRCS"),
                                 NoOfReviews = model.Field<int>("NoOfReviews"),
                                 TotalRows = model.Field<Int64>("TotalRows"),
                                 Rownumber = model.Field<Int64>("Rownumber"),
                                 Email = model.Field<string>("Email"),
                                 Photo = model.Field<string>("Photo"),

                             }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = users };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));
				throw;
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }

        public async Task<DatabaseResponse> GetUsersToAdd(int qrcId, int categoryId, int pageNo, int pageSize, int filterCategoryId)
        {
            try
            {
                SqlParameter[] parameters =
            {
                    new SqlParameter( "@QrcId",  SqlDbType.Int ),
                    new SqlParameter( "@Category",  SqlDbType.Int ),
                    new SqlParameter( "@PageNo",  SqlDbType.Int ),
                    new SqlParameter( "@PageSize",  SqlDbType.Int ),
                    new SqlParameter( "@FilterCategoryId",  SqlDbType.Int )

                };


                parameters[0].Value = qrcId;
                parameters[1].Value = categoryId;
                parameters[2].Value = pageNo;
                parameters[3].Value = pageSize;
                parameters[4].Value = filterCategoryId;

                _DataHelper = new DataAccessHelper("sps_GetUserNotInQRC", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<QrcUsers> users = new List<QrcUsers>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    users = (from model in dt.AsEnumerable()
                             select new QrcUsers()
                             {
                                 UserId = model.Field<int>("UserId"),
                                 UserName = model.Field<string>("UserName"),
                                 ResourceContributed = model.Field<int>("ResourceContributed"),
                                 CourseCreated = model.Field<int>("CourseCreated"),
                                 CurrentQRCS = model.Field<int>("CurrentQRCS"),
                                 NoOfReviews = model.Field<int>("NoOfReviews"),
                                 TotalRows = model.Field<Int64>("TotalRows"),
                                 Rownumber = model.Field<Int64>("Rownumber"),
                                 Email = model.Field<string>("Email"),
                                 Photo = model.Field<string>("Photo"),


                             }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = users };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));
                throw;
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }
    }
}
