using RulersCourt.Models.Meeting;
using RulersCourt.Translators.Meeting;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RulersCourt.Repository.Meeting
{
    public class MeetingAttachmentClient
    {
        public List<MeetingAttachmentGetModel> GetMeetingAttachmentById(string connString, int? serviceID, string type)
        {
            SqlParameter[] param = { new SqlParameter("@P_ServiceID", serviceID),
                                     new SqlParameter("@Type", type)
            };

            return SqlHelper.ExecuteProcedureReturnData<List<MeetingAttachmentGetModel>>(connString, "Get_AttachmentByID", r => r.TranslateAsMeetingAttachmentList(), param);
        }

        public string PostMeetingAttachments(string connString, string type, List<MeetingAttachmentGetModel> datas, int? serviceID)
        {
            string result = string.Empty;
            if (datas.Count != 0)
            {
                SqlParameter[] parama = { new SqlParameter("@P_Type", "Delete"),
                                         new SqlParameter("@P_ServiceID", serviceID), new SqlParameter("@P_AttachmentGuid", string.Join(",", from item in datas select item.AttachmentGuid)) };

                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_Attachments", parama);
            }

            foreach (MeetingAttachmentGetModel data in datas)
            {
                SqlParameter[] param = { new SqlParameter("@P_Type", type),
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