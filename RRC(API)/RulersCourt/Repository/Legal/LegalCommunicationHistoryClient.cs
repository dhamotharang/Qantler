using RulersCourt.Models.Legal;
using RulersCourt.Translators.Legal;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Legal
{
    public class LegalCommunicationHistoryClient
    {
        public List<LegalCommunicationHistory> LegalCommunicationHistoryByLegalID(string connString, int legalID, string lang)
        {
            SqlParameter[] contactIDParam = {
                new SqlParameter("@P_LegalID", legalID),
                new SqlParameter("@P_Language", lang)
            };
            List<LegalCommunicationHistory> communicationHistories = new List<LegalCommunicationHistory>();
            communicationHistories = SqlHelper.ExecuteProcedureReturnData<List<LegalCommunicationHistory>>(connString, "Get_LegalCommunicationHistory", r => r.TranslateAsLegalCommunicationHistoryList(), contactIDParam);
            return communicationHistories;
        }
    }
}
