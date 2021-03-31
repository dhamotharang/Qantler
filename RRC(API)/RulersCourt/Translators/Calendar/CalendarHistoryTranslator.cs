using RulersCourt.Models.Calendar;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Calendar
{
    public static class CalendarHistoryTranslator
    {
        public static CalendarHistoryModel TranslateAsDesignHistoryLog(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var designCommunicationHistory = new CalendarHistoryModel();

            if (reader.IsColumnExists("CommunicationID"))
                designCommunicationHistory.CommunicationID = SqlHelper.GetNullableInt32(reader, "CommunicationID");

            if (reader.IsColumnExists("CalendarID"))
                designCommunicationHistory.CalendarID = SqlHelper.GetNullableInt32(reader, "CalendarID");

            if (reader.IsColumnExists("ParentCommunicationID"))
                designCommunicationHistory.ParentCommunicationID = SqlHelper.GetNullableInt32(reader, "ParentCommunicationID");

            if (reader.IsColumnExists("Action"))
                designCommunicationHistory.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comment"))
                designCommunicationHistory.Comment = SqlHelper.GetNullableString(reader, "Comment");

            if (reader.IsColumnExists("CreatedBy"))
                designCommunicationHistory.CreatedBy = SqlHelper.GetNullableString(reader, "CreatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                designCommunicationHistory.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            return designCommunicationHistory;
        }

        public static List<CalendarHistoryModel> TranslateAsCalendarHistoryLogList(this SqlDataReader reader)
        {
            var designCommunicationHistoryList = new List<CalendarHistoryModel>();
            while (reader.Read())
            {
                designCommunicationHistoryList.Add(TranslateAsDesignHistoryLog(reader, true));
            }

            return designCommunicationHistoryList;
        }
    }
}