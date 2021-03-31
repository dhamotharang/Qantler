using RulersCourt.Models.Announcement;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Announcement
{
    public static class AnnouncementSaveResponseTranslator
    {
        public static AnnouncementWorkflowModel TranslateAsAnnouncementSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var announcementSave = new AnnouncementWorkflowModel();

            if (reader.IsColumnExists("AnnouncementID"))
                announcementSave.AnnouncementID = SqlHelper.GetNullableInt32(reader, "AnnouncementID");

            if (reader.IsColumnExists("ReferenceNumber"))
                announcementSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                announcementSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                announcementSave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return announcementSave;
        }

        public static AnnouncementWorkflowModel TranslateAsAnnouncementSaveResponseList(this SqlDataReader reader)
        {
            var announcementSaveResponse = new AnnouncementWorkflowModel();
            while (reader.Read())
            {
                announcementSaveResponse = TranslateAsAnnouncementSaveResponse(reader, true);
            }

            return announcementSaveResponse;
        }
    }
}
