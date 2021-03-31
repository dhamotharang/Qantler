using RulersCourt.Models.Campaign;
using RulersCourt.Translators.Protocol.Media.Campaign;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Protocol.Media.Campaign
{
    public class CampaignHistoryLogClient
    {
        public List<CampaignCommunicationHistory> CampaignHistoryLogID(string connString, int campaignID, string lang)
        {
            SqlParameter[] contractIDParam = {
                new SqlParameter("@P_CampaignID", campaignID),
                new SqlParameter("@P_Language", lang)
            };

            List<CampaignCommunicationHistory> circularDetails = new List<CampaignCommunicationHistory>();

            circularDetails = SqlHelper.ExecuteProcedureReturnData<List<CampaignCommunicationHistory>>(connString, "Get_MediaNewCampaignRequestHistoryByID", r => r.TranslateAsCampaignHistoryLogList(), contractIDParam);

            return circularDetails;
        }
    }
}
