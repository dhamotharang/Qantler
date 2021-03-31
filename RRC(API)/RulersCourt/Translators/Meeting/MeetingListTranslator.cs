using RulersCourt.Models.Meeting;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Meeting
{
    public static class MeetingListTranslator
    {
        public static MeetingListViewModel TranslateAsLetterDashboard(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var meetingDashboardModel = new MeetingListViewModel();

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
                meetingDashboardModel.MeetingType = SqlHelper.GetNullableString(reader, "MeetingType");
            }

            if (reader.IsColumnExists("Invitees"))
            {
                meetingDashboardModel.Invitees = SqlHelper.GetNullableString(reader, "Invitees");
            }

            if (reader.IsColumnExists("CreatedBy"))
            {
                meetingDashboardModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");
            }

            return meetingDashboardModel;
        }

        public static List<MeetingListViewModel> TranslateAsMeetingDashboardList(this SqlDataReader reader)
        {
            var letterDashboardList = new List<MeetingListViewModel>();
            while (reader.Read())
            {
                letterDashboardList.Add(TranslateAsLetterDashboard(reader, true));
            }

            return letterDashboardList;
        }
    }
}
