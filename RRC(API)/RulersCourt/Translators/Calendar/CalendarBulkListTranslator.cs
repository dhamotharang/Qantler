using RulersCourt.Models.Calendar;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Calendar
{
    public static class CalendarBulkListTranslator
    {
        public static CalendarBulkModel TranslateAsLetter(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var calendar = new CalendarBulkModel();

            if (reader.IsColumnExists("CalendarID"))
            {
                calendar.CalendarID = SqlHelper.GetNullableInt32(reader, "CalendarID");
            }

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                calendar.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("EventRequestor"))
            {
                calendar.EventRequestor = SqlHelper.GetNullableString(reader, "EventRequestor");
            }

            if (reader.IsColumnExists("EventType"))
            {
                calendar.EventType = SqlHelper.GetNullableString(reader, "EventType");
            }

            if (reader.IsColumnExists("EventDetails"))
            {
                calendar.EventDetails = SqlHelper.GetNullableString(reader, "EventDetails");
            }

            if (reader.IsColumnExists("DateFrom"))
            {
                calendar.DateFrom = SqlHelper.GetDateTime(reader, "DateFrom");
            }

            if (reader.IsColumnExists("DateTo"))
            {
                calendar.DateTo = SqlHelper.GetDateTime(reader, "DateTo");
            }

            if (reader.IsColumnExists("Location"))
            {
                calendar.Location = SqlHelper.GetNullableString(reader, "Location");
            }

            if (reader.IsColumnExists("Status"))
            {
                calendar.Status = SqlHelper.GetNullableInt32(reader, "Status");
            }

            if (reader.IsColumnExists("ApproverID"))
            {
                calendar.ApproverID = SqlHelper.GetNullableInt32(reader, "ApproverID");
            }

            if (reader.IsColumnExists("CreatedBy"))
            {
                calendar.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");
            }

            return calendar;
        }

        public static List<CalendarBulkModel> TranslateAsCalendarBulkList(this SqlDataReader reader)
        {
            var letterList = new List<CalendarBulkModel>();
            while (reader.Read())
            {
                letterList.Add(TranslateAsLetter(reader, true));
            }

            return letterList;
        }
    }
}