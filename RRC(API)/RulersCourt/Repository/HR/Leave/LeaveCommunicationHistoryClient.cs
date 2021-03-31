using RulersCourt.Models.LeaveRequest;
using RulersCourt.Translators.LeaveRequest;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Leave
{
    public class LeaveCommunicationHistoryClient
    {
        public List<LeaveCommunicationHistory> LeaveCommunicationHistoryByLeaveID(string connString, int leaveID, string lang)
        {
            SqlParameter[] contractIDParam = {
                new SqlParameter("@P_LeaveID", leaveID),
                new SqlParameter("@P_Language", lang),
            };
            List<LeaveCommunicationHistory> communicationHistories = new List<LeaveCommunicationHistory>();
            communicationHistories = SqlHelper.ExecuteProcedureReturnData<List<LeaveCommunicationHistory>>(connString, "Get_LeaveCommunicationHistory", r => r.TranslateAsLeaveCommunicationHistoryList(), contractIDParam);
            return communicationHistories;
        }
    }
}
