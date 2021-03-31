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
using System.Threading.Tasks;

namespace OERService.DataAccess
{
    public class MoECheckDataAccess
    {
        internal DataAccessHelper _DataHelper = null;
        private readonly IConfiguration _configuration;
        private readonly IConverter _converter;

        public MoECheckDataAccess(IConfiguration configuration, IConverter converter)
        {
            _configuration = configuration;
            _converter = converter;
        }
        public async Task<DatabaseResponse> UpdateContentStatus(MoEContentStatus contentUpdate)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter( "@UserId",  SqlDbType.Int),
                    new SqlParameter( "@ContentId",  SqlDbType.Int),
                    new SqlParameter( "@ContentType",  SqlDbType.Int),
                    new SqlParameter( "@Status",  SqlDbType.Bit),
                    new SqlParameter( "@Comments",  SqlDbType.NVarChar),
                    new SqlParameter( "@EmailUrl",  SqlDbType.NVarChar)
                };

                parameters[0].Value = contentUpdate.UserId;
                parameters[1].Value = contentUpdate.ContentId;
                parameters[2].Value = contentUpdate.ContentType;
                parameters[3].Value = contentUpdate.Status;
                parameters[4].Value = contentUpdate.comments;
                parameters[5].Value = contentUpdate.EmailUrl;

                _DataHelper = new DataAccessHelper("spi_MoEApproveReject", parameters, _configuration);

                DataTable dt = new DataTable();
                int result = await _DataHelper.RunAsync(dt);
                if (dt.Rows.Count > 0)
                {
                    int portalLanguageId = Convert.ToInt32(dt.Rows[0]["PortalLanguageId"]);
                    string text = string.Empty;
                    string buttonText = string.Empty;
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

                        userEmail.Email = Convert.ToString(dt.Rows[0]["Email"]);

                        if (portalLanguageId == 2)
                        {
                            text = "Your Content has been published during expert check.";
                            userEmail.Subject = "Content has been published";
                            buttonText = "View Content";
                        }
                        else
                        {

                            text = "تم نشر المصدر المحتوى الخاص بك، يمكنك الضغط على الرابط أدناه للعرض.";
                            userEmail.Subject = "تم نشر المحتوى .";
                            buttonText = "عرض المحتوى";
                        }
                        userEmail.Body = Emailer.CreateEmailBody(Convert.ToString(dt.Rows[0]["UserName"]), contentUpdate.EmailUrl, text, buttonText, dt.Rows[0]["PortalLanguageId"] == null ? 0 : Convert.ToInt32(dt.Rows[0]["PortalLanguageId"]),_configuration);

                        await Emailer.SendEmailAsync(userEmail, _configuration);
                    }
                    else if (result == (int)DbReturnValue.Rejected)
                    {
                        UserEmail userEmail = new UserEmail();
                        userEmail.Email = Convert.ToString(dt.Rows[0]["Email"]);
                        if (portalLanguageId == 2)
                        {
                            text = "Your Content has been rejected during expert check. Click below button to view. <br/> Reason : " + contentUpdate.comments;
                            buttonText = "View Content";
                            userEmail.Subject = "Content has been rejected";
                        }
                        else
                        {
                            text = "بعد المراجعة، تم رفض المحتوى المقدم من قبلك. يمكنك الضغط على الرابط أدناه لعرض المحتوى.<br><br>الأسباب:" + contentUpdate.comments + "<br/>";
                            buttonText = "عرض المحتوى";
                            userEmail.Subject = "بعد المراجعة، تم رفض المحتوى المقدم من قبلك.";
                        }
                        userEmail.Body = Emailer.CreateEmailBody(Convert.ToString(dt.Rows[0]["UserName"]), contentUpdate.EmailUrl, text, buttonText, dt.Rows[0]["PortalLanguageId"] == null ? 0 : Convert.ToInt32(dt.Rows[0]["PortalLanguageId"]),_configuration);
                        await Emailer.SendEmailAsync(userEmail, _configuration);
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
        public async Task<DatabaseResponse> GetContentApproval(int userId, int pageNumber, int pageSize, int categoryId)
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
                _DataHelper = new DataAccessHelper("sps_GetMoECheckList", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<MoECheck> content = new List<MoECheck>();

                if (dt != null && dt.Rows.Count > 0)
                {
                    content = (from model in dt.AsEnumerable()
                               select new MoECheck()
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
                    new SqlParameter( "@PageNo",  SqlDbType.Int),
                    new SqlParameter( "@PageSize",  SqlDbType.Int),
                    new SqlParameter( "@UserID",  SqlDbType.Int)
                };

                parameters[0].Value = pageNumber;
                parameters[1].Value = pageSize;
                parameters[2].Value = userId;

                _DataHelper = new DataAccessHelper("sps_MoEApproved", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);


                List<MoECheck> content = new List<MoECheck>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    content = (from model in dt.AsEnumerable()
                               select new MoECheck()
                               {
                                   ContentId = model.Field<int>("ContentId"),
                                   ContentType = model.Field<int>("ContentType"),
                                   Title = model.Field<string>("Title"),
                                   Totalrows = model.Field<Int64>("Totalrows"),
                                   Rownumber = model.Field<Int64>("Rownumber"),
                                   ApprovedBy = model.Field<string>("ApprovedBy")
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
