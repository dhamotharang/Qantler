using RulersCourt.Models.DutyTask;
using RulersCourt.Translators.DutyTask;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RulersCourt.Repository.DutyTask
{
    public class DutyTaskLinkToMemoClient
    {
        public List<DutyTaskMemoReferenceNumberModel> GetLinkToMemo(string connString, int? serviceID)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_TaskID", serviceID),
                new SqlParameter("@P_Type", 2)
            };

            return SqlHelper.ExecuteProcedureReturnData<List<DutyTaskMemoReferenceNumberModel>>(connString, "Get_DutyTaskLinkToLetterAndMemoAndMeeting", r => r.TranslateAsGetLinkToMemoList(), param);
        }

        public string PostLinkToMemo(string connString, List<DutyTaskMemoReferenceNumberModel> datas, int? serviceID)
        {
            string result = string.Empty;
            if (datas.Count != 0)
            {
                SqlParameter[] parama = {
                    new SqlParameter("@P_Type", "Delete"),
                    new SqlParameter("@P_TaskID", serviceID),
                    new SqlParameter("@P_ReferenceNumber", string.Join(",", from item in datas select item.MemoReferenceNumber))
                };

                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_DutyTaskLinkToMemo", parama);
            }
            else
            {
                SqlParameter[] parama1 = {
                    new SqlParameter("@P_Type", "Delete"),
                    new SqlParameter("@P_TaskID", serviceID)
                };
                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_DutyTaskLinkToMemo", parama1);
            }

            foreach (DutyTaskMemoReferenceNumberModel data in datas)
            {
                SqlParameter[] param = {
                    new SqlParameter("@P_Type", "Add"),
                    new SqlParameter("@P_TaskID", serviceID),
                    new SqlParameter("@P_ReferenceNumber", data.MemoReferenceNumber)
                };
                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_DutyTaskLinkToMemo", param);
            }

            return result;
        }
    }
}
