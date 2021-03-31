using RulersCourt.Models.HR.Training;
using RulersCourt.Translators.HR.Training;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Training
{
    public class TrainingCommunicationHistoryClient
    {
        public List<TrainingCommunicationHistory> TrainingCommunicationHistoryByLeaveID(string connString, int trainingID, string lang)
        {
            SqlParameter[] param = {new SqlParameter("@P_TrainingID", trainingID),
                new SqlParameter("@P_Language", lang)
            };
            List<TrainingCommunicationHistory> communicationHistories = new List<TrainingCommunicationHistory>();
            communicationHistories = SqlHelper.ExecuteProcedureReturnData<List<TrainingCommunicationHistory>>(connString, "Get_TrainingCommunicationHistory",  r => r.TranslateAsTrainingCommunicationHistoryList(), param);
            return communicationHistories;
        }
    }
}
