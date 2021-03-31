using RulersCourt.Models;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class MemoHistoryLogClient
    {
        public List<MemoHistoryLogModel> MemoHistoryLogByMemoID(string connString, int memoID, string lang)
        {
            SqlParameter[] contractIDParam = {
                new SqlParameter("@P_MemoID", memoID),
                new SqlParameter("@P_Language", lang)
            };

            List<MemoHistoryLogModel> memoDetails = new List<MemoHistoryLogModel>();

            memoDetails = SqlHelper.ExecuteProcedureReturnData<List<MemoHistoryLogModel>>(connString, "Get_MemoHistoryByID", r => r.TranslateAsMemoHistoryLogList(), contractIDParam);

            return memoDetails;
        }
    }
}
