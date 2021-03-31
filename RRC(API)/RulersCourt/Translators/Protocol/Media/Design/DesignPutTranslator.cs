using RulersCourt.Models.Design;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.Media.Design
{
    public static class DesignPutTranslator
    {
        public static DesignPutModel TranslateAsDesignPatch(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var designModel = new DesignPutModel();

            if (reader.IsColumnExists("DesignID"))
                designModel.DesignID = SqlHelper.GetNullableInt32(reader, "DesignID");

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
                designModel.TargetGroups = SqlHelper.GetNullableString(reader, "TargetGroup");

            if (reader.IsColumnExists("InitiativeProjectActivity"))
                designModel.Project = SqlHelper.GetNullableString(reader, "InitiativeProjectActivity");

            if (reader.IsColumnExists("MainObjective"))
                designModel.MainObjective = SqlHelper.GetNullableString(reader, "MainObjective");

            if (reader.IsColumnExists("StrategicObjective"))
                designModel.StrategicObjective = SqlHelper.GetNullableString(reader, "StrategicObjective");

            if (reader.IsColumnExists("GeneralObjective"))
                designModel.GeneralObjective = SqlHelper.GetNullableString(reader, "GeneralObjective");

            if (reader.IsColumnExists("UpdatedBy"))
                designModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                designModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Action"))
                designModel.Action = SqlHelper.GetNullableString(reader, "Action");

            return designModel;
        }

        public static List<DesignPutModel> TranslateAsDesignPatchList(this SqlDataReader reader)
        {
            var babyAdditionList = new List<DesignPutModel>();
            while (reader.Read())
            {
                babyAdditionList.Add(TranslateAsDesignPatch(reader, true));
            }

            return babyAdditionList;
        }
    }
}
