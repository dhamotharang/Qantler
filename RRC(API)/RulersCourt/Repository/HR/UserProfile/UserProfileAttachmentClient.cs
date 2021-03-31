using RulersCourt.Models.UserProfile;
using RulersCourt.Translators.UserProfile;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RulersCourt.Repository.UserProfile
{
    public class UserProfileAttachmentClient
    {
        public List<UserProfileAttachmentGetModel> GetUserProfileAttachmentById(string connString, int? serviceID, string type)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_ServiceID", serviceID),
                new SqlParameter("@Type", type)
            };

            return SqlHelper.ExecuteProcedureReturnData<List<UserProfileAttachmentGetModel>>(connString, "Get_AttachmentByID", r => r.TranslateAsUserProfileAttachmentList(), param);
        }

        public string PostUserProfileAttachments(string connString, string type, List<UserProfileAttachmentGetModel> datas, int? serviceID, string referenceNumber)
        {
            string result = string.Empty;
            if (datas.Count != 0)
            {
                SqlParameter[] parama = {
                    new SqlParameter("@P_Type", "Delete"),
                    new SqlParameter("@P_ServiceType", type),
                    new SqlParameter("@P_ServiceID", serviceID),
                    new SqlParameter("@P_AttachmentGuid", string.Join(",", from item in datas select item.AttachmentGuid)),
                    new SqlParameter("@P_AttachmentsName", string.Join(",", from item in datas select item.AttachmentsName))
                };
                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_Attachments", parama);
            }
            else
            {
                SqlParameter[] parama1 = {
                    new SqlParameter("@P_Type", "Delete"),
                    new SqlParameter("@P_ServiceType", type),
                    new SqlParameter("@P_ServiceID", serviceID)
                };
                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_Attachments", parama1);
            }

            foreach (UserProfileAttachmentGetModel data in datas)
            {
                SqlParameter[] param = {
                    new SqlParameter("@P_Type", data.AttachmentType),
                    new SqlParameter("@P_ServiceID", serviceID),
                    new SqlParameter("@P_AttachmentGuid", data.AttachmentGuid),
                    new SqlParameter("@P_ServiceType", "UserProfile"),
                    new SqlParameter("@P_AttachmentsName", data.AttachmentsName)
                };
                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_Attachments", param);
            }

            return result;
        }
    }
}
