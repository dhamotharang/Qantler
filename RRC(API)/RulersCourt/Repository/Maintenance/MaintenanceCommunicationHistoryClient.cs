using RulersCourt.Models.Maintenance;
using RulersCourt.Translators.Maintenance;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Maintenance
{
    public class MaintenanceCommunicationHistoryClient
    {
        public List<MaintenanceCommunicationHistory> MaintenanceCommunicationHistoryByID(string connString, int maintenanceID, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_MaintenanceID", maintenanceID),
                new SqlParameter("@P_Language", lang)
            };
            List<MaintenanceCommunicationHistory> communicationHistories = new List<MaintenanceCommunicationHistory>();
            communicationHistories = SqlHelper.ExecuteProcedureReturnData<List<MaintenanceCommunicationHistory>>(connString, "Get_MaintenanceCommunicationHistory", r => r.TranslateAsMaintenanceCommunicationHistoryList(), param);
            return communicationHistories;
        }
    }
}
