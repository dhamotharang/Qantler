using RulersCourt.Models.PressRelease;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.Media.PressRelease
{
    public static class PressReleasePutTranslator
    {
        public static PressReleasePutModel TranslateAsPutPressRelease(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var pressReleaseModel = new PressReleasePutModel();

            if (reader.IsColumnExists("PressReleaseID"))
                pressReleaseModel.PressReleaseID = SqlHelper.GetNullableInt32(reader, "PressReleaseID");

            if (reader.IsColumnExists("Date"))
                pressReleaseModel.Date = SqlHelper.GetDateTime(reader, "Date");

            if (reader.IsColumnExists("SourceOU"))
                pressReleaseModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                pressReleaseModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("Subject"))
                pressReleaseModel.Subject = SqlHelper.GetNullableString(reader, "Subject");

            if (reader.IsColumnExists("Type"))
                pressReleaseModel.Type = SqlHelper.GetNullableString(reader, "Type");

            if (reader.IsColumnExists("Location"))
                pressReleaseModel.Location = SqlHelper.GetNullableString(reader, "Location");

            if (reader.IsColumnExists("EventName"))
                pressReleaseModel.EventName = SqlHelper.GetNullableString(reader, "EventName");

            if (reader.IsColumnExists("AttendedBy"))
                pressReleaseModel.AttendedBy = SqlHelper.GetNullableString(reader, "AttendedBy");

            if (reader.IsColumnExists("Partners"))
                pressReleaseModel.Partners = SqlHelper.GetNullableString(reader, "Partners");

            if (reader.IsColumnExists("UpdatedBy"))
                pressReleaseModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                pressReleaseModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Action"))
                pressReleaseModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                pressReleaseModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("ApproverID"))
                pressReleaseModel.ApproverID = SqlHelper.GetNullableInt32(reader, "ApproverID");

            if (reader.IsColumnExists("ApproverDepartmentID"))
                pressReleaseModel.ApproverDepartmentID = SqlHelper.GetNullableInt32(reader, "ApproverDepartmentID");

            return pressReleaseModel;
        }

        public static List<PressReleasePutModel> TranslateAsPutPressReleasesList(this SqlDataReader reader)
        {
            var pressReleaseModel = new List<PressReleasePutModel>();
            while (reader.Read())
            {
                pressReleaseModel.Add(TranslateAsPutPressRelease(reader, true));
            }

            return pressReleaseModel;
        }
    }
}