using Core.Enums;
using Core.Helpers;
using Core.Models;
using DinkToPdf.Contracts;
using Microsoft.Extensions.Configuration;
using OERService.Controllers;
using OERService.Helpers;
using OERService.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OERService.DataAccess
{
    public class CommunityCheckDataAccess
    {
        internal DataAccessHelper _DataHelper = null;
        private  readonly IConfiguration _configuration;
		private readonly IConverter _converter;

        public CommunityCheckDataAccess(IConfiguration configuration, IConverter converter)
        {
            _configuration = configuration;
            _converter = converter;
        }
        public async Task<DatabaseResponse> UpdateContentStatus(CommunityContentStatus contentUpdate)
        {
            try
            {
                SqlParameter[] parameters =
            {
                    new SqlParameter( "@UserId",  SqlDbType.Int),
                    new SqlParameter( "@ContentId",  SqlDbType.Int),
                    new SqlParameter( "@ContentType",  SqlDbType.Int),
                    new SqlParameter( "@Status",  SqlDbType.Bit),
                    new SqlParameter( "@Comments",  SqlDbType.VarChar)


                };
                parameters[0].Value = contentUpdate.UserId;
                parameters[1].Value = contentUpdate.ContentId;
                parameters[2].Value = contentUpdate.ContentType;
                parameters[3].Value = contentUpdate.Status;
                parameters[4].Value = contentUpdate.comments;

                _DataHelper = new DataAccessHelper("spi_CommunityApproveReject", parameters, _configuration);

                DataSet dt = new DataSet();



                int result = await _DataHelper.RunAsync(dt);
  
                    if (dt.Tables.Count > 0 && dt.Tables[0] != null && dt.Tables[0].Rows.Count > 0)
                    {
                        if (result == (int)DbReturnValue.Published)
                        {
                            if (contentUpdate.ContentType == 1)
                            {
                                ContentMediaController controller = new ContentMediaController(_configuration, _converter);
                                await controller.GeneratePdf(contentUpdate.ContentId, 1);
                            }
                            else
                            {
                                ContentMediaController controller = new ContentMediaController(_configuration, _converter);
                                await controller.GeneratePdf(contentUpdate.ContentId, 2);
                            }
                            UserEmail userEmail = new UserEmail();
                            userEmail.Email = Convert.ToString(dt.Tables[0].Rows[0]["Email"]);
                            int portalLanguageId = Convert.ToInt32(dt.Tables[0].Rows[0]["PortalLanguageId"]);
                            string text = string.Empty;
                            string buttonText = string.Empty;
                            if (portalLanguageId == 2)
                            {
                                text = "Your Content has been published during community check.";
                                buttonText = "View Content";
                                userEmail.Subject = "Content has been published";
                            }
                            else
                            {
                                text = "تم نشر المصدر المحتوى الخاص بك، يمكنك الضغط على الرابط أدناه للعرض";
                                buttonText = "عرض المحتوى";
                                userEmail.Subject = "تم نشر المحتوى";
                            }
                            userEmail.Body = Emailer.CreateEmailBody(Convert.ToString(dt.Tables[0].Rows[0]["UserName"]), contentUpdate.EmailUrl, text, buttonText, dt.Tables[0].Rows[0]["PortalLanguageId"] == null ? 0 : Convert.ToInt32(dt.Tables[0].Rows[0]["PortalLanguageId"]),_configuration);
                            await Emailer.SendEmailAsync(userEmail, _configuration);
                        }
                        else if (result == (int)DbReturnValue.Rejected)
                        {
                            UserEmail userEmail = new UserEmail();
                            int portalLanguageId = Convert.ToInt32(dt.Tables[0].Rows[0]["PortalLanguageId"]);
                            userEmail.Email = Convert.ToString(dt.Tables[0].Rows[0]["Email"]);
                            string text = string.Empty;
                            string buttonText = string.Empty;
                            if (portalLanguageId == 2)
                            {
                                if (dt.Tables[1] != null && dt.Tables[1].Rows.Count > 0)
                                {
                                    text = "Your Content  has been rejected during Community Check. Click below button to view. Reason(s) : ";
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
                                    text = "Your Content has been rejected during community check. Click below button to view. Reason : " + contentUpdate.comments;
                                }

                                buttonText = "View Content";
                                userEmail.Subject = "Your course or resource has been rejected.";


                                userEmail.Body = Emailer.CreateEmailBody(Convert.ToString(dt.Tables[0].Rows[0]["UserName"]), contentUpdate.EmailUrl, text, buttonText, dt.Tables[0].Rows[0]["PortalLanguageId"] == null ? 0 : Convert.ToInt32(dt.Tables[0].Rows[0]["PortalLanguageId"]),_configuration);
                                await Emailer.SendEmailAsync(userEmail, _configuration);
                            }
                            else
                            {
                                if (dt.Tables[1] != null && dt.Tables[1].Rows.Count > 0)
                                {
                                    text = "بعد المراجعة، تم رفض المحتوى المقدم من قبلك يمكنك الضغط على الرابط أدناه لعرض المحتوى .<br><br>";
                                    buttonText = "عرض المحتوى";
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
                                    text = "بعد المراجعة، تم رفض المحتوى المقدم من قبلك يمكنك الضغط على الرابط أدناه لعرض المحتوى .<br><br>" + contentUpdate.comments + ":الأسباب";
                                    buttonText = "عرض المحتوى";
                                }


                                userEmail.Subject = "بعد المراجعة، تم رفض المحتوى المقدم من قبلك.";

                                userEmail.Body = Emailer.CreateEmailBody(Convert.ToString(dt.Tables[0].Rows[0]["UserName"]), contentUpdate.EmailUrl, text, buttonText, dt.Tables[0].Rows[0]["PortalLanguageId"] == null ? 0 : Convert.ToInt32(dt.Tables[0].Rows[0]["PortalLanguageId"]),_configuration);
                                await Emailer.SendEmailAsync(userEmail, _configuration);
                            }

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
        public async Task<DatabaseResponse> GetContentApproval(int userId, int pageNumber, int pageSize,int categoryId)
        {
            try
            {
                SqlParameter[] parameters =
            {
                    new SqlParameter( "@UserId",  SqlDbType.Int),
                    new SqlParameter( "@PageNo",  SqlDbType.Int),
                    new SqlParameter( "@PageSize",  SqlDbType.Int),
                     new SqlParameter( "@CategoryId",  SqlDbType.Int)


                };
                parameters[0].Value = userId;
                parameters[1].Value = pageNumber;
                parameters[2].Value = pageSize;
                parameters[3].Value = categoryId;
                _DataHelper = new DataAccessHelper("sps_GetCommunityCheckList", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);


                List<CommunityCheck> content = new List<CommunityCheck>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    content = (from model in dt.AsEnumerable()
                               select new CommunityCheck()
                               {
                                   ContentId = model.Field<int>("ContentId"),
                                   ContentType = model.Field<int>("ContentType"),
                                   Title = model.Field<string>("Title"),
                                   Totalrows = model.Field<Int64>("Totalrows"),
                                   Rownumber = model.Field<Int64>("Rownumber"),
                                   Category = model.Field<string>("Category")
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

        public async Task<DatabaseResponse> GetApprovedListByUser(int userId, int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter[] parameters =
            {
                    new SqlParameter( "@UserId",  SqlDbType.Int),
                    new SqlParameter( "@PageNo",  SqlDbType.Int),
                    new SqlParameter( "@PageSize",  SqlDbType.Int)


                };
                parameters[0].Value = userId;
                parameters[1].Value = pageNumber;
                parameters[2].Value = pageSize;

                _DataHelper = new DataAccessHelper("sps_CommunityApprovedByUser", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);


                List<CommunityCheck> content = new List<CommunityCheck>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    content = (from model in dt.AsEnumerable()
                               select new CommunityCheck()
                               {
                                   ContentId = model.Field<int>("ContentId"),
                                   ContentType = model.Field<int>("ContentType"),
                                   Title = model.Field<string>("Title"),
                                   Totalrows = model.Field<Int64>("Totalrows"),
                                   Rownumber = model.Field<Int64>("Rownumber")
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
    }
}
