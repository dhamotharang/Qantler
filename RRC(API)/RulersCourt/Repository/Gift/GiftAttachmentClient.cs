using RulersCourt.Models.Gift;
using RulersCourt.Translators.Gift;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RulersCourt.Repository.Gift
{
    public class GiftAttachmentClient
    {
        public List<GiftAttachmentGetModel> GetAttachmentById(string connString, int? serviceID, string type)
        {
            SqlParameter[] param = { new SqlParameter("@P_ServiceID", serviceID),
                                     new SqlParameter("@Type", type)
            };

            return SqlHelper.ExecuteProcedureReturnData<List<GiftAttachmentGetModel>>(connString, "Get_AttachmentByID", r => r.TranslateAsGetGiftAttachmentList(), param);
        }

        public string GiftPostAttachments(string connString, string type, List<GiftAttachmentGetModel> datas, int? serviceID)
        {
            string result = string.Empty;
            if (datas.Count != 0)
            {
                SqlParameter[] parama = {
                    new SqlParameter("@P_Type", "Delete"),
                    new SqlParameter("@P_ServiceID", serviceID),
                    new SqlParameter("@P_AttachmentGuid", string.Join(",", from item in datas select item.AttachmentGuid))
                 };

                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_Attachments", parama);
            }

            foreach (GiftAttachmentGetModel data in datas)
            {
                SqlParameter[] param = {
                    new SqlParameter("@P_Type", type),
                    new SqlParameter("@P_ServiceType", "Gift"),
                    new SqlParameter("@P_ServiceID", serviceID),
                    new SqlParameter("@P_AttachmentGuid", data.AttachmentGuid),
                    new SqlParameter("@P_AttachmentsName", data.AttachmentsName)
                 };

                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_Attachments", param);
            }

            return result;
        }
    }
}