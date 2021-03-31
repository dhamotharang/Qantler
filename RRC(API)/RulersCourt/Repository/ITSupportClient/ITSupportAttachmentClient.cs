using RulersCourt.Models.ITSupport;
using RulersCourt.Translators.ITSupportTranslators;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RulersCourt.Repository.ITSupportClient
{
    public class ITSupportAttachmentClient
    {
        public List<ITSupportAttachmentGetModel> GetITSupportAttachmentById(string connString, int? serviceID, string type)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_ServiceID", serviceID),
                new SqlParameter("@Type", type)
            };

            return SqlHelper.ExecuteProcedureReturnData<List<ITSupportAttachmentGetModel>>(connString, "Get_AttachmentByID", r => r.TranslateAsITSupportAttachmentList(), param);
        }

        public string PostITSupportAttachments(string connString, string type, List<ITSupportAttachmentGetModel> datas, int? serviceID)
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

            foreach (ITSupportAttachmentGetModel data in datas)
            {
                SqlParameter[] param = {
                    new SqlParameter("@P_Type", type),
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
