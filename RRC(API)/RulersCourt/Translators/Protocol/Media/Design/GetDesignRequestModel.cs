using RulersCourt.Models.Design;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.Media.Design
{
    public static class GetDesignRequestModel
    {
        public static DesignGetModel TranslateAsDesignGetbyID(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var designModel = new DesignGetModel();

            if (reader.IsColumnExists("DesignId"))
                designModel.DesignId = SqlHelper.GetNullableInt32(reader, "DesignId");

            if (reader.IsColumnExists("ReferenceNumber"))
                designModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("SourceOU"))
                designModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                designModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("Status"))
                designModel.Status = SqlHelper.GetNullableInt32(reader, "Status");

            if (reader.IsColumnExists("Title"))
                designModel.Title = SqlHelper.GetNullableString(reader, "Title");

            if (reader.IsColumnExists("DeliverableDate"))
                designModel.DeliverableDate = SqlHelper.GetDateTime(reader, "DeliverableDate");

            if (reader.IsColumnExists("TypeofDesignRequired"))
                designModel.TypeofDesignRequired = SqlHelper.GetNullableString(reader, "TypeofDesignRequired");

            if (reader.IsColumnExists("Date"))
                designModel.Date = SqlHelper.GetDateTime(reader, "Date");

            if (reader.IsColumnExists("DiwansRole"))
                designModel.DiwansRole = SqlHelper.GetNullableString(reader, "DiwansRole");

            if (reader.IsColumnExists("Languages"))
                designModel.Languages = SqlHelper.GetNullableInt32(reader, "Languages");

            if (reader.IsColumnExists("OtherParties"))
                designModel.OtherParties = SqlHelper.GetNullableString(reader, "OtherParties");

            if (reader.IsColumnExists("TargetGroup"))
                designModel.TargetGroup = SqlHelper.GetNullableString(reader, "TargetGroup");

            if (reader.IsColumnExists("InitiativeProjectActivity"))
                designModel.InitiativeProjectActivity = SqlHelper.GetNullableString(reader, "InitiativeProjectActivity");

            if (reader.IsColumnExists("MainObjective"))
                designModel.MainObjective = SqlHelper.GetNullableString(reader, "MainObjective");

            if (reader.IsColumnExists("StrategicObjective"))
                designModel.StrategicObjective = SqlHelper.GetNullableString(reader, "StrategicObjective");

            if (reader.IsColumnExists("GeneralObjective"))
                designModel.GeneralObjective = SqlHelper.GetNullableString(reader, "GeneralObjective");

            if (reader.IsColumnExists("CreatedBy"))
                designModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                designModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                designModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                designModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("ApproverID"))
                designModel.ApproverID = SqlHelper.GetNullableInt32(reader, "ApproverID");

            if (reader.IsColumnExists("ApproverDepartmentID"))
                designModel.ApproverDepartmentID = SqlHelper.GetNullableInt32(reader, "ApproverDepartmentID");

            return designModel;
        }

        public static List<DesignGetModel> TranslateAsDesignList(this SqlDataReader reader)
        {
            var babyAdditionList = new List<DesignGetModel>();
            while (reader.Read())
            {
                babyAdditionList.Add(TranslateAsDesignGetbyID(reader, true));
            }

            return babyAdditionList;
        }
    }
}