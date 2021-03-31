using RulersCourt.Models.Contact;
using RulersCourt.Translators.Contact;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RulersCourt.Repository.Contact
{
    public class ContactAttachmentClient
    {
        public List<ContactAttachmentGetModel> GetContactAttachmentById(string connString, int? serviceID, string type)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_ServiceID", serviceID),
                new SqlParameter("@Type", type)
            };
            return SqlHelper.ExecuteProcedureReturnData<List<ContactAttachmentGetModel>>(connString, "Get_AttachmentByID", r => r.TranslateAsContactAttachmentList(), param);
        }

        public string PostContactAttachments(string connString, string type, List<ContactAttachmentGetModel> datas, int? serviceID)
        {
            string result = string.Empty;
            if (datas.Count != 0)
            {
                SqlParameter[] param = {
                    new SqlParameter("@P_Type", "Delete"),
                    new SqlParameter("@P_ServiceID", serviceID),
                    new SqlParameter("@P_AttachmentGuid", string.Join(",", from item in datas select item.AttachmentGuid))
                };
                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_Attachments", param);
            }

            foreach (ContactAttachmentGetModel data in datas)
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