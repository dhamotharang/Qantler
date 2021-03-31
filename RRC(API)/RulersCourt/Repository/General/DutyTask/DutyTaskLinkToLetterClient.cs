using RulersCourt.Models.DutyTask;
using RulersCourt.Translators.DutyTask;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RulersCourt.Repository.DutyTask
{
    public class DutyTaskLinkToLetterClient
    {
        public List<DutyTaskLetterReferenceNumberModel> GetLinkToLetter(string connString, int? serviceID)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_TaskID", serviceID),
                new SqlParameter("@P_Type", 1)
            };

            return SqlHelper.ExecuteProcedureReturnData<List<DutyTaskLetterReferenceNumberModel>>(connString, "Get_DutyTaskLinkToLetterAndMemoAndMeeting", r => r.TranslateAsGetLinkToLetterList(), param);
        }

        public string PostLinkToMemo(string connString, List<DutyTaskLetterReferenceNumberModel> datas, int? serviceID)
        {
            string result = string.Empty;
            if (datas.Count != 0)
            {
                SqlParameter[] parama = {
                    new SqlParameter("@P_Type", "Delete"),
                    new SqlParameter("@P_TaskID", serviceID),
                    new SqlParameter("@P_ReferenceNumber", string.Join(",", from item in datas select item.LetterReferenceNumber))
                };

                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_DutyTaskLinkToLetter", parama);
            }
            else
            {
                SqlParameter[] parama1 = {
                    new SqlParameter("@P_Type", "Delete"),
                    new SqlParameter("@P_TaskID", serviceID)
                };
                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_DutyTaskLinkToLetter", parama1);
            }

            foreach (DutyTaskLetterReferenceNumberModel data in datas)
            {
                SqlParameter[] param = {
                    new SqlParameter("@P_Type", "Add"),
                    new SqlParameter("@P_TaskID", serviceID),
                    new SqlParameter("@P_ReferenceNumber", data.LetterReferenceNumber)
                };
                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_DutyTaskLinkToLetter", param);
            }

            return result;
        }
    }
}
