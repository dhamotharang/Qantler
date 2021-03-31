using RulersCourt.Models.Campaign;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.Media.Campaign
{
    public static class CampaignGetTranslator
    {
        public static CampaignGetModel TranslateAsCampaignGetbyID(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var campaignModel = new CampaignGetModel();

            if (reader.IsColumnExists("CampaignID"))
                campaignModel.CampaignID = SqlHelper.GetNullableInt32(reader, "CampaignID");

            if (reader.IsColumnExists("ReferenceNumber"))
                campaignModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("SourceOU"))
                campaignModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                campaignModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("Status"))
                campaignModel.Status = SqlHelper.GetNullableInt32(reader, "Status");

            if (reader.IsColumnExists("Date"))
                campaignModel.Date = SqlHelper.GetDateTime(reader, "Date");

            if (reader.IsColumnExists("CampaignStartDate"))
                campaignModel.CampaignStartDate = SqlHelper.GetDateTime(reader, "CampaignStartDate");

            if (reader.IsColumnExists("CampaignPeriod"))
                campaignModel.CampaignPeriod = SqlHelper.GetNullableString(reader, "CampaignPeriod");

            if (reader.IsColumnExists("Date"))
                campaignModel.Date = SqlHelper.GetDateTime(reader, "Date");

            if (reader.IsColumnExists("DiwansRole"))
                campaignModel.DiwansRole = SqlHelper.GetNullableString(reader, "DiwansRole");

            if (reader.IsColumnExists("Languages"))
                campaignModel.Languages = SqlHelper.GetNullableInt32(reader, "Languages");

            if (reader.IsColumnExists("OtherEntities"))
                campaignModel.OtherEntities = SqlHelper.GetNullableString(reader, "OtherEntities");

            if (reader.IsColumnExists("Location"))
                campaignModel.Location = SqlHelper.GetNullableString(reader, "Location");

            if (reader.IsColumnExists("MainIdea"))
                campaignModel.MainIdea = SqlHelper.GetNullableString(reader, "MainIdea");

            if (reader.IsColumnExists("MediaChannels"))
                campaignModel.MediaChannels = SqlHelper.GetNullableInt32(reader, "MediaChannels");

            if (reader.IsColumnExists("Notes"))
                campaignModel.Notes = SqlHelper.GetNullableString(reader, "Notes");

            if (reader.IsColumnExists("RequestDetails"))
                campaignModel.RequestDetails = SqlHelper.GetNullableString(reader, "RequestDetails");

            if (reader.IsColumnExists("TargetGroup"))
                campaignModel.TargetGroup = SqlHelper.GetNullableString(reader, "TargetGroup");

            if (reader.IsColumnExists("TargetAudience"))
                campaignModel.TargetAudience = SqlHelper.GetNullableString(reader, "TargetAudience");

            if (reader.IsColumnExists("InitiativeProjectActivity"))
                campaignModel.InitiativeProjectActivity = SqlHelper.GetNullableString(reader, "InitiativeProjectActivity");

            if (reader.IsColumnExists("MainObjective"))
                campaignModel.MainObjective = SqlHelper.GetNullableString(reader, "MainObjective");

            if (reader.IsColumnExists("StrategicGoals"))
                campaignModel.StrategicGoals = SqlHelper.GetNullableString(reader, "StrategicGoals");

            if (reader.IsColumnExists("GeneralInformation"))
                campaignModel.GeneralInformation = SqlHelper.GetNullableString(reader, "GeneralInformation");

            if (reader.IsColumnExists("CreatedBy"))
                campaignModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                campaignModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                campaignModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                campaignModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("ApproverID"))
                campaignModel.ApproverID = SqlHelper.GetNullableInt32(reader, "ApproverID");

            if (reader.IsColumnExists("ApproverDepartmentID"))
                campaignModel.ApproverDepartmentID = SqlHelper.GetNullableInt32(reader, "ApproverDepartmentID");

            return campaignModel;
        }

        public static List<CampaignGetModel> TranslateAsCampaignList(this SqlDataReader reader)
        {
            var babyAdditionList = new List<CampaignGetModel>();
            while (reader.Read())
            {
                babyAdditionList.Add(TranslateAsCampaignGetbyID(reader, true));
            }

            return babyAdditionList;
        }
    }
}