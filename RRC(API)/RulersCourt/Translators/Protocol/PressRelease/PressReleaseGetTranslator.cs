using RulersCourt.Models.PressRelease;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.PressRelease
{
    public static class PressReleaseGetTranslator
    {
        public static PressReleaseGetModel TranslateAsPhotographer(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var photographerModel = new PressReleaseGetModel();

            if (reader.IsColumnExists("PressReleaseID"))
                photographerModel.PressReleaseID = SqlHelper.GetNullableInt32(reader, "PressReleaseID");

            if (reader.IsColumnExists("ReferenceNumber"))
                photographerModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("SourceOU"))
                photographerModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                photographerModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("Date"))
                photographerModel.Date = SqlHelper.GetDateTime(reader, "Date");

            if (reader.IsColumnExists("Location"))
                photographerModel.Location = SqlHelper.GetNullableString(reader, "Location");

            if (reader.IsColumnExists("Subject"))
                photographerModel.Subject = SqlHelper.GetNullableString(reader, "Subject");

            if (reader.IsColumnExists("Type"))
                photographerModel.Type = SqlHelper.GetNullableString(reader, "Type");

            if (reader.IsColumnExists("AttendedBy"))
                photographerModel.AttendedBy = SqlHelper.GetNullableString(reader, "AttendedBy");

            if (reader.IsColumnExists("Partners"))
                photographerModel.Partners = SqlHelper.GetNullableString(reader, "Partners");

            if (reader.IsColumnExists("EventName"))
                photographerModel.EventName = SqlHelper.GetNullableString(reader, "EventName");

            if (reader.IsColumnExists("CreatedBy"))
                photographerModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                photographerModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("Status"))
                photographerModel.Status = SqlHelper.GetNullableInt32(reader, "Status");

            if (reader.IsColumnExists("CreatedDateTime"))
                photographerModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                photographerModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("ApproverDepartmentID"))
                photographerModel.ApproverDepartmentID = SqlHelper.GetNullableInt32(reader, "ApproverDepartmentID");

            if (reader.IsColumnExists("ApproverID"))
                photographerModel.ApproverID = SqlHelper.GetNullableInt32(reader, "ApproverID");

            return photographerModel;
        }

        public static List<PressReleaseGetModel> TranslateAsPressReleaseList(this SqlDataReader reader)
        {
            var photographerList = new List<PressReleaseGetModel>();
            while (reader.Read())
            {
                photographerList.Add(TranslateAsPhotographer(reader, true));
            }

            return photographerList;
        }
    }
}
