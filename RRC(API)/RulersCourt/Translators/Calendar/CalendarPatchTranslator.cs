using RulersCourt.Models.Calendar;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Calendar
{
    public static class CalendarPatchTranslator
    {
        public static CalendarPutModel TranslateAsDesignPatch(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
                reader.Read();
            }

            var calendar = new CalendarPutModel();

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
                calendar.EventType = SqlHelper.GetNullableInt32(reader, "EventType");
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

            if (reader.IsColumnExists("AllDayEvents"))
            {
                calendar.AllDayEvents = SqlHelper.GetBoolean(reader, "AllDayEvents");
            }

            if (reader.IsColumnExists("IsApologySent"))
            {
                calendar.IsApologySent = SqlHelper.GetBoolean(reader, "IsApologySent");
            }

            if (reader.IsColumnExists("Location"))
            {
                calendar.Location = SqlHelper.GetNullableInt32(reader, "Location");
            }

            if (reader.IsColumnExists("City"))
            {
                calendar.City = SqlHelper.GetNullableInt32(reader, "City");
            }

            if (reader.IsColumnExists("UpdatedBy"))
                calendar.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                calendar.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Action"))
                calendar.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                calendar.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("ParentReferenceNumber"))
            {
                calendar.ParentReferenceNumber = SqlHelper.GetNullableString(reader, "ParentReferenceNumber");
            }

            return calendar;
        }

        public static List<CalendarPutModel> TranslateAsCalendarPatchList(this SqlDataReader reader)
        {
            var babyAdditionList = new List<CalendarPutModel>();
            while (reader.Read())
            {
                babyAdditionList.Add(TranslateAsDesignPatch(reader, true));
            }

            return babyAdditionList;
        }
    }
}
