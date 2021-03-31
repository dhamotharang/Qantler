using RulersCourt.Models.DutyTasks;
using RulersCourt.Translators.DutyTask;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.DutyTask
{
    public class DutyTaskCommunicationHistoryLogClient
    {
        public List<DutyTaskCommunicationHistoryGetModel> DutyTaskCommunicationHistoryBytaskID(string connString, int? taskID, string lang)
        {
            SqlParameter[] contractIDParam = { new SqlParameter("@P_DutyTaskID", taskID),
             new SqlParameter("@P_Language", lang)
            };
            List<DutyTaskCommunicationHistoryGetModel> communicationHistories = new List<DutyTaskCommunicationHistoryGetModel>();
            communicationHistories = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskCommunicationHistoryGetModel>>(connString, "Get_DutyTaskCommunicationHistory", r => r.TranslateAsDutyTaskCommunicationHistoryList(), contractIDParam);
            return communicationHistories;
        }
    }
}
