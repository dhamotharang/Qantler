using RulersCourt.Models.Protocol.Media.Campaign;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.Media.Campaign
{
    public static class CampaignSaveResponseTranslator
    {
        public static CampaignResponseModel TranslateAsDesignSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var campaign = new CampaignResponseModel();

            if (reader.IsColumnExists("CampaignID"))
                campaign.CampaignID = SqlHelper.GetNullableInt32(reader, "CampaignID");

            if (reader.IsColumnExists("ReferenceNumber"))
                campaign.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                campaign.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                campaign.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return campaign;
        }

        public static CampaignResponseModel TranslateAsCampaignSaveResponseList(this SqlDataReader reader)
        {
            var campaignSaveResponse = new CampaignResponseModel();
            while (reader.Read())
            {
                campaignSaveResponse = TranslateAsDesignSaveResponse(reader, true);
            }

            return campaignSaveResponse;
        }
    }
}