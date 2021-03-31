using RulersCourt.Models.DutyTasks;
using RulersCourt.Translators.DutyTask;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.DutyTask
{
    public class DutyTaskTaggedUserIDClient
    {
        public List<DutyTaskTaggedUserIDModel> DutyTaskCommunicationHistoryTaggedUserIdByTaskID(string connString, int? communicationID)
        {
            SqlParameter contractIDParam = new SqlParameter("@P_communicationID", communicationID);
            List<DutyTaskTaggedUserIDModel> taggedUserID = new List<DutyTaskTaggedUserIDModel>();
            taggedUserID = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskTaggedUserIDModel>>(connString, "Get_DutyTaskTaggedUserID", r => r.TranslateAsTaggedUserID(), contractIDParam);
            return taggedUserID;
        }
    }
}
