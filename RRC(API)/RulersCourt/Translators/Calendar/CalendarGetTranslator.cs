using RulersCourt.Models.Calendar;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Calendar
{
    public static class CalendarGetTranslator
    {
        public static CalendarGetModel TranslateAsLetter(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var calendar = new CalendarGetModel();

            if (reader.IsColumnExists("CalendarID"))
            {
                calendar.CalendarID = SqlHelper.GetNullableInt32(reader, "CalendarID");
            }

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                calendar.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("ParentReferenceNumber"))
            {
                calendar.ParentReferenceNumber = SqlHelper.GetNullableString(reader, "ParentReferenceNumber");
            }

            if (reader.IsColumnExists("ParentID"))
            {
                calendar.ParentID = SqlHelper.GetNullableInt32(reader, "ParentID");
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

            if (reader.IsColumnExists("City"))
            {
                calendar.City = SqlHelper.GetNullableInt32(reader, "City");
            }

            if (reader.IsColumnExists("Location"))
            {
                calendar.Location = SqlHelper.GetNullableInt32(reader, "Location");
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

            if (reader.IsColumnExists("IsBulkEvent"))
            {
                calendar.IsBulkEvent = SqlHelper.GetBoolean(reader, "IsBulkEvent");
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

            if (reader.IsColumnExists("UpdatedBy"))
            {
                calendar.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");
            }

            if (reader.IsColumnExists("CreatedDateTime"))
            {
                calendar.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");
            }

            if (reader.IsColumnExists("UpdatedDateTime"))
            {
                calendar.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");
            }

            return calendar;
        }

        public static List<CalendarGetModel> TranslateAsCalendarList(this SqlDataReader reader)
        {
            var letterList = new List<CalendarGetModel>();
            while (reader.Read())
            {
                letterList.Add(TranslateAsLetter(reader, true));
            }

            return letterList;
        }
    }
}
