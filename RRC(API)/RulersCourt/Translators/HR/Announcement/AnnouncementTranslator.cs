using RulersCourt.Models.Announcement;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Announcement
{
    public static class AnnouncementTranslator
    {
        public static AnnouncementGetModel TranslateAsAnnouncement(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var announcementModel = new AnnouncementGetModel();

            if (reader.IsColumnExists("AnnouncementID"))
                announcementModel.AnnouncementID = SqlHelper.GetNullableInt32(reader, "AnnouncementID");

            if (reader.IsColumnExists("ReferenceNumber"))
                announcementModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("SourceOU"))
                announcementModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                announcementModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("AnnouncementType"))
                announcementModel.AnnouncementType = SqlHelper.GetNullableString(reader, "AnnouncementType");

            if (reader.IsColumnExists("AnnouncementDescription"))
                announcementModel.AnnouncementDescription = SqlHelper.GetNullableString(reader, "AnnouncementDescription");

            if (reader.IsColumnExists("CreatedBy"))
                announcementModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                announcementModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                announcementModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                announcementModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Status"))
                announcementModel.Status = SqlHelper.GetNullableInt32(reader, "Status");

            return announcementModel;
        }

        public static List<AnnouncementGetModel> TranslateAsAnnouncementList(this SqlDataReader reader)
        {
            var announcementList = new List<AnnouncementGetModel>();
            while (reader.Read())
            {
                announcementList.Add(TranslateAsAnnouncement(reader, true));
            }

            return announcementList;
        }

        public static AnnouncementPutModel TranslateAsPutAnnouncement(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var announcementModel = new AnnouncementPutModel();

            if (reader.IsColumnExists("AnnouncementID"))
                announcementModel.AnnouncementID = SqlHelper.GetNullableInt32(reader, "AnnouncementID");

            if (reader.IsColumnExists("SourceOU"))
                announcementModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                announcementModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("AnnouncementType"))
                announcementModel.AnnouncementType = SqlHelper.GetNullableString(reader, "AnnouncementType");

            if (reader.IsColumnExists("AnnouncementDescription"))
                announcementModel.AnnouncementDescription = SqlHelper.GetNullableString(reader, "AnnouncementDescription");

            if (reader.IsColumnExists("UpdatedBy"))
                announcementModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                announcementModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Action"))
                announcementModel.Action = SqlHelper.GetNullableString(reader, "Action");

            return announcementModel;
        }

        public static List<AnnouncementPutModel> TranslateAsPutAnnouncementList(this SqlDataReader reader)
        {
            var announcementList = new List<AnnouncementPutModel>();
            while (reader.Read())
            {
                announcementList.Add(TranslateAsPutAnnouncement(reader, true));
            }

            return announcementList;
        }
    }
}
