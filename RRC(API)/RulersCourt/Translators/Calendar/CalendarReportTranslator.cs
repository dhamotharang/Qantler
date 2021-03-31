using RulersCourt.Models.Calendar;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Calendar
{
    public static class CalendarReportTranslator
    {
        public static CalendarReport TranslateAsLetterDashboard(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var calendar = new CalendarReport();

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                calendar.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("EventRequestor"))
            {
                calendar.EventRequestor = SqlHelper.GetNullableString(reader, "EventRequestor");
            }

            if (reader.IsColumnExists("Location"))
            {
                calendar.Location = SqlHelper.GetNullableString(reader, "Location");
            }

            if (reader.IsColumnExists("EventType"))
            {
                calendar.EventType = SqlHelper.GetNullableString(reader, "EventType");
            }

            if (reader.IsColumnExists("UserName"))
            {
                calendar.UserName = SqlHelper.GetNullableString(reader, "UserName");
            }

            if (reader.IsColumnExists("Status"))
            {
                calendar.Status = SqlHelper.GetNullableString(reader, "Status");
            }

            return calendar;
        }

        public static List<CalendarReport> TranslateAsCalendarReportList(this SqlDataReader reader)
        {
            var letterDashboardList = new List<CalendarReport>();
            while (reader.Read())
            {
                letterDashboardList.Add(TranslateAsLetterDashboard(reader, true));
            }

            return letterDashboardList;
        }
    }
}