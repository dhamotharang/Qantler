using RulersCourt.Models.BabyAddition;
using RulersCourt.Translators.BabyAddition;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RulersCourt.Repository.BabyAddition
{
    public class BabyAdditionAttachmentClient
    {
        public List<BabyAdditionAttachmentGetModel> GetAttachmentById(string connString, int? serviceID, string type)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_ServiceID", serviceID),
                new SqlParameter("@Type", type)
            };

            return SqlHelper.ExecuteProcedureReturnData<List<BabyAdditionAttachmentGetModel>>(connString, "Get_AttachmentByID", r => r.TranslateAsBabyAdditionAttachmentList(), param);
        }

        public string PostAttachments(string connString, string type, List<BabyAdditionAttachmentGetModel> datas, int? serviceID)
        {
            string result = string.Empty;
            if (datas.Count != 0)
            {
                SqlParameter[] param = {
                    new SqlParameter("@P_Type", "Delete"),
                    new SqlParameter("@P_ServiceType", type),
                    new SqlParameter("@P_ServiceID", serviceID),
                    new SqlParameter("@P_AttachmentGuid", string.Join(",", from item in datas select item.AttachmentGuid)),
                    new SqlParameter("@P_AttachmentsName", string.Join(",", from item in datas select item.AttachmentsName))
                };

                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_Attachments", param);
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

            foreach (BabyAdditionAttachmentGetModel data in datas)
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