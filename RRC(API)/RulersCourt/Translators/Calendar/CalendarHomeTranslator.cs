using RulersCourt.Models.Calendar;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Calendar
{
    public static class CalendarHomeTranslator
    {
        public static CalendarListViewModel TranslateAsLetterDashboard(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var calendar = new CalendarListViewModel();

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

            if (reader.IsColumnExists("isbulkevent"))
            {
                calendar.Isbulkevent = SqlHelper.GetNullableInt32(reader, "isbulkevent");
            }

            if (reader.IsColumnExists("DateFrom"))
            {
                calendar.DateFrom = SqlHelper.GetDateTime(reader, "DateFrom");
            }

            if (reader.IsColumnExists("DateTo"))
            {
                calendar.DateTo = SqlHelper.GetDateTime(reader, "DateTo");
            }

            if (reader.IsColumnExists("CreatedBy"))
            {
                calendar.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");
            }

            if (reader.IsColumnExists("Status"))
            {
                calendar.Status = SqlHelper.GetNullableString(reader, "Status");
            }

            return calendar;
        }

        public static List<CalendarListViewModel> TranslateAsCalendarDashboardList(this SqlDataReader reader)
        {
            var letterDashboardList = new List<CalendarListViewModel>();
            while (reader.Read())
            {
                letterDashboardList.Add(TranslateAsLetterDashboard(reader, true));
            }

            return letterDashboardList;
        }

        public static CalendarViewListModel TranslateAsCalendarDashboard(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var calendar = new CalendarViewListModel();

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

            if (reader.IsColumnExists("DateFrom"))
            {
                calendar.DateFrom = SqlHelper.GetDateTime(reader, "DateFrom");
            }

            if (reader.IsColumnExists("DateTo"))
            {
                calendar.DateTo = SqlHelper.GetDateTime(reader, "DateTo");
            }

            if (reader.IsColumnExists("CreatedBy"))
            {
                calendar.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");
            }

            if (reader.IsColumnExists("Holiday"))
            {
                calendar.Holiday = SqlHelper.GetDateTime(reader, "Holiday");
            }

            if (reader.IsColumnExists("HolidayMessage"))
            {
                calendar.HolidayMessage = SqlHelper.GetNullableString(reader, "HolidayMessage");
            }

            if (reader.IsColumnExists("Status"))
            {
                calendar.Status = SqlHelper.GetNullableInt32(reader, "Status");
            }

            if (reader.IsColumnExists("AllDayEvents"))
            {
                calendar.AllDayEvents = SqlHelper.GetBoolean(reader, "AllDayEvents");
            }

            return calendar;
        }

        public static List<CalendarViewListModel> TranslateAsCalenderViewDashboardList(this SqlDataReader reader)
        {
            var letterDashboardList = new List<CalendarViewListModel>();
            while (reader.Read())
            {
                letterDashboardList.Add(TranslateAsCalendarDashboard(reader, true));
            }

            return letterDashboardList;
        }

        public static CalendarcountModel TranslateAsCalendaraDashboardCount(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var mediahomemodel = new CalendarcountModel();

            if (reader.IsColumnExists("AllEvents"))
            {
                mediahomemodel.AllEvents = SqlHelper.GetNullableInt32(reader, "AllEvents");
            }

            if (reader.IsColumnExists("MyEvents"))
            {
                mediahomemodel.MyEvents = SqlHelper.GetNullableInt32(reader, "MyEvents");
            }

            if (reader.IsColumnExists("MyPendingRequest"))
            {
                mediahomemodel.MyPendingRequest = SqlHelper.GetNullableInt32(reader, "MyPendingRequest");
            }

            if (reader.IsColumnExists("Approved"))
            {
                mediahomemodel.Approved = SqlHelper.GetNullableInt32(reader, "Approved");
            }

            return mediahomemodel;
        }

        public static CalendarcountModel TranslateasCalendarDashboardcount(this SqlDataReader reader)
        {
            var mediahomemodel = new CalendarcountModel();
            while (reader.Read())
            {
                mediahomemodel = TranslateAsCalendaraDashboardCount(reader, true);
            }

            return mediahomemodel;
        }
    }
}
