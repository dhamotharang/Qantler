using RulersCourt.Models.Meeting;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Meeting
{
    public static class MeetingCalendarTranslator
    {
        public static MeetingCalendarViewModel TranslateAsLetterDashboard(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var meetingDashboardModel = new MeetingCalendarViewModel();

            if (reader.IsColumnExists("MeetingID"))
            {
                meetingDashboardModel.MeetingID = SqlHelper.GetNullableInt32(reader, "MeetingID");
            }

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                meetingDashboardModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("Subject"))
            {
                meetingDashboardModel.Subject = SqlHelper.GetNullableString(reader, "Subject");
            }

            if (reader.IsColumnExists("Location"))
            {
                meetingDashboardModel.Location = SqlHelper.GetNullableString(reader, "Location");
            }

            if (reader.IsColumnExists("StartDateTime"))
            {
                meetingDashboardModel.StartDateTime = SqlHelper.GetDateTime(reader, "StartDateTime");
            }

            if (reader.IsColumnExists("EndDateTime"))
            {
                meetingDashboardModel.EndDateTime = SqlHelper.GetDateTime(reader, "EndDateTime");
            }

            if (reader.IsColumnExists("MeetingType"))
            {
                meetingDashboardModel.MeetingType = SqlHelper.GetNullableInt32(reader, "MeetingType");
            }

            if (reader.IsColumnExists("PointsDiscussed"))
            {
                meetingDashboardModel.PointsDiscussed = SqlHelper.GetNullableString(reader, "PointsDiscussed");
            }

            if (reader.IsColumnExists("Suggestion"))
            {
                meetingDashboardModel.Suggestion = SqlHelper.GetNullableString(reader, "Suggestion");
            }

            if (reader.IsColumnExists("CreatedBy"))
            {
                meetingDashboardModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");
            }

            return meetingDashboardModel;
        }

        public static List<MeetingCalendarViewModel> TranslateAsMeetingCalenderDashboardList(this SqlDataReader reader)
        {
            var letterDashboardList = new List<MeetingCalendarViewModel>();
            while (reader.Read())
            {
                letterDashboardList.Add(TranslateAsLetterDashboard(reader, true));
            }

            return letterDashboardList;
        }
    }
}