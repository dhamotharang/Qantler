using RulersCourt.Models.Meeting;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Meeting
{
    public static class MeetingReportTranslator
    {
        public static MeetingReportModel TranslateAsLetterDashboard(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var meetingDashboardModel = new MeetingReportModel();

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

            if (reader.IsColumnExists("MeetingType"))
            {
                meetingDashboardModel.MeetingType = SqlHelper.GetNullableString(reader, "MeetingType");
            }

            if (reader.IsColumnExists("UserName"))
            {
                meetingDashboardModel.UserName = SqlHelper.GetNullableString(reader, "UserName");
            }

            if (reader.IsColumnExists("Invitees"))
            {
                meetingDashboardModel.Invitees = SqlHelper.GetNullableString(reader, "Invitees");
            }

            return meetingDashboardModel;
        }

        public static List<MeetingReportModel> TranslateAsMeetingReportList(this SqlDataReader reader)
        {
            var letterDashboardList = new List<MeetingReportModel>();
            while (reader.Read())
            {
                letterDashboardList.Add(TranslateAsLetterDashboard(reader, true));
            }

            return letterDashboardList;
        }
    }
}
