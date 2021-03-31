using RulersCourt.Models.Meeting;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Meeting
{
    public static class MeetingPreviewTranslator
    {
        public static MeetingPreviewModel TranslateAsLetterDashboard(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var meetingDashboardModel = new MeetingPreviewModel();

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                meetingDashboardModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("OrganizerDepartmentID"))
            {
                meetingDashboardModel.OrganizerDepartmentID = SqlHelper.GetNullableString(reader, "OrganizerDepartmentID");
            }

            if (reader.IsColumnExists("OrganizerUserID"))
            {
                meetingDashboardModel.OrganizerUserID = SqlHelper.GetNullableString(reader, "OrganizerUserID");
            }

            if (reader.IsColumnExists("Subject"))
            {
                meetingDashboardModel.Subject = SqlHelper.GetNullableString(reader, "Subject");
            }

            if (reader.IsColumnExists("Location"))
            {
                meetingDashboardModel.Location = SqlHelper.GetNullableString(reader, "Location");
            }

            if (reader.IsColumnExists("Attendees"))
            {
                meetingDashboardModel.Attendees = SqlHelper.GetNullableString(reader, "Attendees");
            }

            if (reader.IsColumnExists("StartDateTime"))
            {
                meetingDashboardModel.StartDateTime = SqlHelper.GetDateTime(reader, "StartDateTime");
            }

            if (reader.IsColumnExists("EndDateTime"))
            {
                meetingDashboardModel.EndDateTime = SqlHelper.GetDateTime(reader, "EndDateTime");
            }

            if (reader.IsColumnExists("PointsDiscussed"))
            {
                meetingDashboardModel.PointsDiscussed = SqlHelper.GetNullableString(reader, "PointsDiscussed");
            }

            if (reader.IsColumnExists("PendingPoints"))
            {
                meetingDashboardModel.PendingPoints = SqlHelper.GetNullableString(reader, "PendingPoints");
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

        public static List<MeetingPreviewModel> TranslateAsMeetingPreviewList(this SqlDataReader reader)
        {
            var letterDashboardList = new List<MeetingPreviewModel>();
            while (reader.Read())
            {
                letterDashboardList.Add(TranslateAsLetterDashboard(reader, true));
            }

            return letterDashboardList;
        }
    }
}
