using RulersCourt.Models.PressRelease;
using RulersCourt.Translators.Protocol.PressRelease;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Protocol.Media.PressRelease
{
    public class PressReleaseCommunicationHistoryClient
    {
        public List<PressReleaseCommunicationHistory> PressReleaseCommunicationHistoryByPhotographerID(string connString, int pressReleaseID, string lang)
        {
            SqlParameter[] contractIDParam = {
                new SqlParameter("@P_PressReleaseID", pressReleaseID),
                new SqlParameter("@P_Language", lang),
            };
            List<PressReleaseCommunicationHistory> communicationHistories = new List<PressReleaseCommunicationHistory>();
            communicationHistories = SqlHelper.ExecuteProcedureReturnData<List<PressReleaseCommunicationHistory>>(connString, "Get_MediaNewPressReleaseRequestHistoryByID", r => r.TranslateAsPressReleaseCommunicationHistoryList(), contractIDParam);
            return communicationHistories;
        }
    }
}
