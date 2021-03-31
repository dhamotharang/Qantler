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
    public class SensoryCheckDataAccess
    {
        internal DataAccessHelper _DataHelper = null;
		private readonly IConfiguration _configuration;

        public SensoryCheckDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<DatabaseResponse> UpdateContentStatus(SensoryContentStatus contentUpdate)
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

                _DataHelper = new DataAccessHelper("spi_SensoryApproveReject ", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);


                int PortalLanguageId = Convert.ToInt32(dt.Rows[0]["PortalLanguageId"]);
                string text = string.Empty;
                string buttonText = string.Empty;
                if (result == (int)DbReturnValue.Rejected)
                {

                    UserEmail userEmail = new UserEmail();
                    userEmail.Email = Convert.ToString(dt.Rows[0]["Email"]);
                    if (PortalLanguageId == 2)
                    {
                        text = "Your Content has been rejected during sensitivity check. Click below button to view. <br/>Reason : " + contentUpdate.comments;
                        buttonText = "View Content";
                        userEmail.Subject = "Content has been rejected";
                    }
                    else
                    {
                        text = "بعد المراجعة، تم رفض المحتوى المقدم من قبلك.يمكنك الضغط على الرابط أدناه لعرض المحتوى.<br><br>الأسباب:" + contentUpdate.comments+ " <br/>";
                        buttonText = "عرض المحتوى";
                        userEmail.Subject = "تم رفض المحتوى";
                    }
                    userEmail.Body = Emailer.CreateEmailBody(Convert.ToString(dt.Rows[0]["UserName"]), contentUpdate.EmailUrl, text, buttonText, Convert.ToInt32(dt.Rows[0]["PortalLanguageId"]),_configuration);
                    
                    await Emailer.SendEmailAsync(userEmail,_configuration);
                }
                else if (result == (int)DbReturnValue.Approved)
                {

                    UserEmail userEmail = new UserEmail();
                    userEmail.Email = Convert.ToString(dt.Rows[0]["Email"]);
                    if (PortalLanguageId == 2)
                    {
                        text = "Your Content has been approved during sensitivity check. ";
                        buttonText = "View Content";
                        userEmail.Subject = "Content has been approved";
                    }
                    else
                    {

                        text = "تم نشر المصدر المحتوى الخاص بك، يمكنك الضغط على الرابط أدناه للعرض. ";
                        buttonText = "عرض المحتوى";
                        userEmail.Subject = "تم نشر المحتوى .";
                    }
                    userEmail.Body = Emailer.CreateEmailBody(Convert.ToString(dt.Rows[0]["UserName"]), contentUpdate.EmailUrl, text, buttonText, Convert.ToInt32(dt.Rows[0]["PortalLanguageId"]),_configuration);
                    await Emailer.SendEmailAsync(userEmail,_configuration);
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
        public async Task<DatabaseResponse> GetContentList(int userId, int pageNumber, int pageSize, int categoryId)
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
                _DataHelper = new DataAccessHelper("sps_GetSensoryCheckList", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);


                List<SensoryCheck> content = new List<SensoryCheck>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    content = (from model in dt.AsEnumerable()
                               select new SensoryCheck()
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
    }
}
