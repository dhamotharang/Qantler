using RulersCourt.Models.OfficalTask;
using RulersCourt.Translators.OfficialTaskCompensation.OfficalTask;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.OfficialTaskCompensation.OfficialTask
{
    public class OfficialTaskCommunicationHistoryClient
    {
        public List<OfficialTaskCommunicationHistoryModel> OfficialTaskCommunicationHistoryByOfficalTaskID(string connString, int officialTaskID, string lang)
        {
            SqlParameter[] officialTaskIDParam = {
                new SqlParameter("@P_OfficialTaskID", officialTaskID),
                new SqlParameter("@P_Language", lang)
            };
            List<OfficialTaskCommunicationHistoryModel> communicationHistories = new List<OfficialTaskCommunicationHistoryModel>();
            communicationHistories = SqlHelper.ExecuteProcedureReturnData<List<OfficialTaskCommunicationHistoryModel>>(connString, "Get_OfficialTaskCommunicationHistory", r => r.TranslateAsOfficialTaskCommunicationHistoryList(), officialTaskIDParam);
            return communicationHistories;
        }
    }
}
