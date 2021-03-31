using RulersCourt.Models.OfficalTaskCompensation.Compensation;
using RulersCourt.Translators.OfficalTaskCompensation.Compensation;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.OfficalTaskCompensation.Compensation
{
    public class CompensationCommunicationHistoryClient
    {
        public List<CompensationCommunicationHistoryModel> CompensationCommunicationHistoryByCompensationID(string connString, int compensationID, string lang)
        {
            SqlParameter[] compensationIDParam = {
                new SqlParameter("@P_CompensationID", compensationID),
                new SqlParameter("@P_Language", lang)
            };
            List<CompensationCommunicationHistoryModel> communicationHistories = new List<CompensationCommunicationHistoryModel>();
            communicationHistories = SqlHelper.ExecuteProcedureReturnData<List<CompensationCommunicationHistoryModel>>(connString, "Get_CompensationCommunicationHistory", r => r.TranslateAsCompensationCommunicationHistoryList(), compensationIDParam);
            return communicationHistories;
        }
    }
}
