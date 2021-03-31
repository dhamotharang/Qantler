using RulersCourt.Models.Photographer;
using RulersCourt.Translators.Protocol.Media.Photographer;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Protocol.Media.Photographer
{
    public class PhotographerCommunicationHistoryClient
    {
        public List<PhotographerCommunicationHistory> PhotographerCommunicationHistoryByPhotographerID(string connString, int photographerID, string lang)
        {
            SqlParameter[] contractIDParam = {
                new SqlParameter("@P_PhotographerID", photographerID),
                new SqlParameter("@P_Language", lang)
            };
            List<PhotographerCommunicationHistory> communicationHistories = new List<PhotographerCommunicationHistory>();
            communicationHistories = SqlHelper.ExecuteProcedureReturnData<List<PhotographerCommunicationHistory>>(connString, "Get_MediaNewPhotographerRequestHistoryByID", r => r.TranslateAsPhotographerCommunicationHistoryList(), contractIDParam);
            return communicationHistories;
        }
    }
}
