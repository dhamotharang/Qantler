using RulersCourt.Models.Announcement;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Announcement
{
    public static class AnnouncementHistoryLogTranslator
    {
        public static AnnouncementHistoryLogModel TranslateAsAnnouncementHistoryLog(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var announcementHistoryLogModel = new AnnouncementHistoryLogModel();

            if (reader.IsColumnExists("HistoryID"))
                announcementHistoryLogModel.HistoryID = SqlHelper.GetNullableInt32(reader, "HistoryID");

            if (reader.IsColumnExists("AnnouncementID"))
                announcementHistoryLogModel.AnnouncementID = SqlHelper.GetNullableInt32(reader, "AnnouncementID");

            if (reader.IsColumnExists("Action"))
                announcementHistoryLogModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                announcementHistoryLogModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("ActionBy"))
                announcementHistoryLogModel.ActionBy = SqlHelper.GetNullableString(reader, "ActionBy");

            if (reader.IsColumnExists("ActionDateTime"))
                announcementHistoryLogModel.ActionDateTime = SqlHelper.GetDateTime(reader, "ActionDateTime");

            return announcementHistoryLogModel;
        }

        public static List<AnnouncementHistoryLogModel> TranslateAsAnnouncementHistoryLogList(this SqlDataReader reader)
        {
            var announcementHistoryLogList = new List<AnnouncementHistoryLogModel>();
            while (reader.Read())
            {
                announcementHistoryLogList.Add(TranslateAsAnnouncementHistoryLog(reader, true));
            }

            return announcementHistoryLogList;
        }
    }
}
