using RulersCourt.Models;
using RulersCourt.Models.Campaign;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.Media.Campaign
{
    public static class CampaignCoomunicationHistoryLog
    {
        public static CampaignCommunicationHistory TranslateAsCampaignHistoryLog(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var campaignCommunicationHistory = new CampaignCommunicationHistory();

            string photoGuid = string.Empty, photoName = string.Empty;

            if (reader.IsColumnExists("PhotoGuid"))
                photoGuid = SqlHelper.GetNullableString(reader, "PhotoGuid");

            if (reader.IsColumnExists("PhotoName"))
                photoName = SqlHelper.GetNullableString(reader, "PhotoName");

            if (!string.IsNullOrEmpty(photoGuid) && !string.IsNullOrEmpty(photoName))
            {
                campaignCommunicationHistory.Photo = photoGuid + "|" + photoName;
            }

            if (reader.IsColumnExists("CommunicationID"))
                campaignCommunicationHistory.CommunicationID = SqlHelper.GetNullableInt32(reader, "CommunicationID");

            if (reader.IsColumnExists("CampaignID"))
                campaignCommunicationHistory.CampaignID = SqlHelper.GetNullableInt32(reader, "CampaignID");

            if (reader.IsColumnExists("ParentCommunicationID"))
                campaignCommunicationHistory.ParentCommunicationID = SqlHelper.GetNullableInt32(reader, "ParentCommunicationID");

            if (reader.IsColumnExists("Action"))
                campaignCommunicationHistory.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Message"))
                campaignCommunicationHistory.Message = SqlHelper.GetNullableString(reader, "Message");

            if (reader.IsColumnExists("CreatedBy"))
                campaignCommunicationHistory.CreatedBy = SqlHelper.GetNullableString(reader, "CreatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                campaignCommunicationHistory.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            return campaignCommunicationHistory;
        }

        public static List<CampaignCommunicationHistory> TranslateAsCampaignHistoryLogList(this SqlDataReader reader)
        {
            var campaignCommunicationHistoryList = new List<CampaignCommunicationHistory>();
            while (reader.Read())
            {
                campaignCommunicationHistoryList.Add(TranslateAsCampaignHistoryLog(reader, true));
            }

            return campaignCommunicationHistoryList;
        }
    }
}
