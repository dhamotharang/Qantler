using RulersCourt.Models.DutyTask;
using RulersCourt.Translators.DutyTask;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RulersCourt.Repository.DutyTask
{
    public class DutyTaskLinkToMeetingClient
    {
        public List<DutyTaskMeetingReferenceNumberModel> GetLinkToMeeting(string connString, int? serviceID)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_TaskID", serviceID),
                new SqlParameter("@P_Type", 3)
            };

            return SqlHelper.ExecuteProcedureReturnData<List<DutyTaskMeetingReferenceNumberModel>>(connString, "Get_DutyTaskLinkToLetterAndMemoAndMeeting", r => r.TranslateAsGetLinkToMeetingList(), param);
        }

        public string PostLinkToMeeting(string connString, List<DutyTaskMeetingReferenceNumberModel> datas, int? serviceID)
        {
            string result = string.Empty;
            if (datas.Count != 0)
            {
                SqlParameter[] parama = {
                    new SqlParameter("@P_Type", "Delete"),
                    new SqlParameter("@P_TaskID", serviceID),
                    new SqlParameter("@P_ReferenceNumber", string.Join(",", from item in datas select item.MeetingReferenceNumber))
                };

                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_DutyTaskLinkToMeeting", parama);
            }
            else
            {
                SqlParameter[] parama1 = {
                    new SqlParameter("@P_Type", "Delete"),
                    new SqlParameter("@P_TaskID", serviceID)
                };
                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_DutyTaskLinkToMeeting", parama1);
            }

            foreach (DutyTaskMeetingReferenceNumberModel data in datas)
            {
                SqlParameter[] param = {
                    new SqlParameter("@P_Type", "Add"),
                    new SqlParameter("@P_TaskID", serviceID),
                    new SqlParameter("@P_ReferenceNumber", data.MeetingReferenceNumber)
                };
                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_DutyTaskLinkToMeeting", param);
            }

            return result;
        }
    }
}
