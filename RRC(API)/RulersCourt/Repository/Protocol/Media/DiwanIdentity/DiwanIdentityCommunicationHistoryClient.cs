using RulersCourt.Models.DiwanIdentity;
using RulersCourt.Translators.Protocol.Media.DiwanIdentity;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Protocol.Media.DiwanIdentity
{
    public class DiwanIdentityCommunicationHistoryClient
    {
        public List<DiwanIdentityCommunicationHistory> DiwanIdentityCommunicationHistoryByDiwanIdentityID(string connString, int diwanIdentityID, string lang)
        {
            SqlParameter[] contractIDParam = {
                new SqlParameter("@P_DiwanIdentityID", diwanIdentityID),
                new SqlParameter("@P_Language", lang),
            };
            List<DiwanIdentityCommunicationHistory> communicationHistories = new List<DiwanIdentityCommunicationHistory>();
            communicationHistories = SqlHelper.ExecuteProcedureReturnData<List<DiwanIdentityCommunicationHistory>>(connString, "Get_DiwanIdentityCommunicationHistory", r => r.TranslateAsDiwanIdentityCommunicationHistoryList(), contractIDParam);
            return communicationHistories;
        }
    }
}
