using RulersCourt.Models.Letter.LetterInbound;
using RulersCourt.Translators.Letter.LetterInbound;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Letter.LetterInbound
{
    public class LetterInboundHistoryLogClient
    {
        public List<LetterInboundHistoryLogModel> LetterInboundHistoryLogByLetterID(string connString, int letterID, string lang)
        {
            SqlParameter[] contractIDParam = {
                new SqlParameter("@P_LetterID", letterID),
                new SqlParameter("@P_Language", lang),
            };

            List<LetterInboundHistoryLogModel> letterDetails = new List<LetterInboundHistoryLogModel>();

            letterDetails = SqlHelper.ExecuteProcedureReturnData<List<LetterInboundHistoryLogModel>>(connString, "Get_LetterInboundHistoryByID", r => r.TranslateAsLetterInboundHistoryLogList(), contractIDParam);

            return letterDetails;
        }
    }
}
