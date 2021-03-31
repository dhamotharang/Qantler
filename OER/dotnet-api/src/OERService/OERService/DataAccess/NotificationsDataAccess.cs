using Core.Enums;
using Core.Helpers;
using Core.Models;
using Microsoft.Extensions.Configuration;
using OERService.Helpers;
using OERService.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OERService.DataAccess
{
    public class NotificationsDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

		private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public NotificationsDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<DatabaseResponse> PostReply(QueryUpdate queryUpdate)
        {
            try
            {
                SqlParameter[] parameters =
            {
                    new SqlParameter( "@Id",  SqlDbType.Int),
                    new SqlParameter( "@RepliedBy",  SqlDbType.Int),
                    new SqlParameter( "@RepliedText",  SqlDbType.NVarChar)

                };

                parameters[0].Value = queryUpdate.QueryId;
                parameters[1].Value = queryUpdate.RepliedBy;
                parameters[2].Value = queryUpdate.ReplyText;

                _DataHelper = new DataAccessHelper("spu_ContactUs", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                if (result == 101)
                {
                    UserEmail userEmail = new UserEmail();
                    userEmail.Email = queryUpdate.Email;
                    string text = queryUpdate.ReplyText;
                    userEmail.Body = Emailer.CreateEmailBodyForReply(queryUpdate.Username, queryUpdate.Url, text,_configuration);
                    userEmail.Subject = "Reply of your query submitted";
                    await Emailer.SendEmailAsync(userEmail,_configuration);
                }

                return new DatabaseResponse { ResponseCode = result };
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
        public async Task<DatabaseResponse> DeleteNotification(int UserId, int NotificationId)
        {
            try
            {
                SqlParameter[] parameters =
            {
                    new SqlParameter( "@UserID",  SqlDbType.Int),
                    new SqlParameter( "@NotificationId",  SqlDbType.Int)
                };

                parameters[0].Value = UserId;
                parameters[1].Value = NotificationId;

                _DataHelper = new DataAccessHelper("spd_Notifications", parameters, _configuration);

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
        public async Task<DatabaseResponse> UpdateNotification(int UserId, int NotificationId)
        {
            try
            {
                SqlParameter[] parameters =
            {
                    new SqlParameter( "@UserID",  SqlDbType.Int),
                    new SqlParameter( "@NotificationId",  SqlDbType.Int)
                };

                parameters[0].Value = UserId;
                parameters[1].Value = NotificationId;

                _DataHelper = new DataAccessHelper("spu_Notifications", parameters, _configuration);

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
        public async Task<DatabaseResponse> GetUserNotifications(int UserId, int PageNo, int PageSize)
        {
            try
            {
                SqlParameter[] parameters =
            {
                    new SqlParameter( "@UserID",  SqlDbType.Int),
                    new SqlParameter( "@PageNo",  SqlDbType.Int),
                    new SqlParameter( "@PageSize",  SqlDbType.Int)
                };

                parameters[0].Value = UserId;
                parameters[1].Value = PageNo;
                parameters[2].Value = PageSize;

                _DataHelper = new DataAccessHelper("sps_Notifications", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);


                List<Notification> userNotification = new List<Notification>();

                if ( ds.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow drItem in ds.Tables[0].Rows)
                    {
                        Notification notification = new Notification();
                        notification.Id = Convert.ToInt32(drItem["Id"]);
                        notification.IsApproved = Convert.ToBoolean(drItem["IsApproved"]);
                        notification.ReferenceId = Convert.ToInt32(drItem["ReferenceId"]);
                        notification.ReferenceTypeId = Convert.ToInt32(drItem["ReferenceTypeId"]);
                        notification.Status = drItem["Status"].ToString() != "" ? Convert.ToInt32(drItem["Status"]) : 1;
                        if ((notification.ReferenceTypeId == 2 || notification.ReferenceTypeId == 1)&&(!notification.IsApproved))
                        {
                                string FilterCond1 = "ReferenceId=" + notification.ReferenceId;
                                List<ReviewerComments> ReviewerComments = new List<ReviewerComments>();
                                foreach (DataRow dritem2 in ds.Tables[0].Select(FilterCond1))
                                {
                                    ReviewerComments objComments = new ReviewerComments();
                                    objComments.Reviewer = Convert.ToString(dritem2["Reviewer"]);
                                    objComments.Reasons = Convert.ToString(dritem2["Comment"]);
                                    ReviewerComments.Add(objComments);
                                }
                                notification.reviewerComments = ReviewerComments;
                        }
                        notification.Total = Convert.ToInt32(drItem["Total"]);
                        notification.Subject = Convert.ToString(drItem["Subject"]);
                        notification.Content = Convert.ToString(drItem["Content"]);
                        notification.MessageType = Convert.ToString(drItem["MessageType"]);
                        notification.CreatedDate = Convert.ToDateTime(drItem["CreatedDate"]);
                        notification.IsRead = Convert.ToBoolean(drItem["IsRead"]);
                        notification.Totalrows = Convert.ToInt32(drItem["Totalrows"]);
                        notification.Rownumber = Convert.ToInt32(drItem["Rownumber"]);
                        notification.Reviewer = Convert.ToString(drItem["Reviewer"]);
                        notification.Comment = Convert.ToString(drItem["Comment"]);
                        notification.EmailUrl = Convert.ToString(drItem["EmailUrl"]);
                        notification.TotalUnRead = Convert.ToInt32(drItem["TotalUnRead"]);
                        userNotification.Add(notification);
                    }
                }

                var un = userNotification.Where(c => c.Status == 1);

                return new DatabaseResponse { ResponseCode = result, Results = un };
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
        public async Task<DatabaseResponse> GetContactUsQueries(int pageNumber, int pageSize, string paramIsSearch, string sortType, int sortField)
        {
            try
            {
                SqlParameter[] parameters =
            {
                    new SqlParameter( "@PageNo",  SqlDbType.Int),
                    new SqlParameter( "@PageSize",  SqlDbType.Int),
                     new SqlParameter( "@Keyword",  SqlDbType.NVarChar ),
                    new SqlParameter( "@SortType",  SqlDbType.NVarChar ),
                    new SqlParameter( "@SortField",  SqlDbType.Int )

                };

                parameters[0].Value = pageNumber;
                parameters[1].Value = pageSize;
                parameters[2].Value = paramIsSearch;
                parameters[3].Value = sortType;
                parameters[4].Value = sortField;

                _DataHelper = new DataAccessHelper("sps_ContactUs", parameters , _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);


                List<ContactUs> contactUs = new List<ContactUs>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    contactUs = (from model in dt.AsEnumerable()
                                 select new ContactUs()
                                 {
                                     Id = model.Field<int>("Id"),
                                     FirstName = model.Field<string>("FirstName"),
                                     LastName = model.Field<string>("LastName"),
                                     Email = model.Field<string>("Email"),
                                     Telephone = model.Field<string>("Telephone"),
                                     Subject = model.Field<string>("Subject"),
                                     Message = model.Field<string>("Message"),
                                     TotalRows = model.Field<Int64>("Totalrows"),
                                     IsReplied = model.Field<int>("IsReplied"),
                                     RepliedText = model.Field<string>("RepliedText"),
                                     RepliedBy = model.Field<string>("RepliedBy"),
                                     RepliedById = model.Field<int?>("RepliedById"),
                                     RepliedOn = model.Field<DateTime?>("RepliedOn")
                                 }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = contactUs };
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
        public async Task<DatabaseResponse> SaveContactUsForm(ContactUsCreate contactUsCreate)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@FirstName",  SqlDbType.NVarChar ),
                    new SqlParameter( "@LastName",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Email",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Telephone",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Subject",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Message",  SqlDbType.NVarChar )

                };

                parameters[0].Value = contactUsCreate.FirstName;
                parameters[1].Value = contactUsCreate.LastName;
                parameters[2].Value = contactUsCreate.Email;
                parameters[3].Value = contactUsCreate.Telephone;
                parameters[4].Value = contactUsCreate.Subject;
                parameters[5].Value = contactUsCreate.Message;
                _DataHelper = new DataAccessHelper("spi_ContactUs", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);


                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        UserEmail userEmail = new UserEmail();
                        userEmail.Email = Convert.ToString(item["Email"]);
                        string text = contactUsCreate.Message;
                        string buttonText = "Review Content";
                        userEmail.Body = Emailer.CreateEmailBody(Convert.ToString(item["UserName"]), contactUsCreate.Url, text, buttonText,_configuration);
                        userEmail.Subject = "Query :" + contactUsCreate.Subject;
                        await Emailer.SendEmailAsync(userEmail,_configuration);
                    }

                }

                return new DatabaseResponse { ResponseCode = result };
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
