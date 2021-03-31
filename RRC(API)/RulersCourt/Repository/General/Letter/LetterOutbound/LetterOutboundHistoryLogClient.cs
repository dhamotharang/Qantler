using RulersCourt.Models.Letter;
using RulersCourt.Translators.Letter;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class LetterOutboundHistoryLogClient
    {
        public List<LetterOutboundHistoryLogModel> LetterHistoryLogByLetterID(string connString, int letterID, string lang)
        {
            SqlParameter[] contractIDParam = {
                new SqlParameter("@P_LetterID", letterID),
                new SqlParameter("@P_Language", lang),
            };

            List<LetterOutboundHistoryLogModel> letterDetails = new List<LetterOutboundHistoryLogModel>();

            letterDetails = SqlHelper.ExecuteProcedureReturnData<List<LetterOutboundHistoryLogModel>>(connString, "Get_LetterOutboundHistoryByID", r => r.TranslateAsLetterOutboundHistoryLogList(), contractIDParam);

            return letterDetails;
        }
    }
}
