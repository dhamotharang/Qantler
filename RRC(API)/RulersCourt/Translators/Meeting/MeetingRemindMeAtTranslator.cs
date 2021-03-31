using RulersCourt.Models.Meeting;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Meeting
{
    public static class MeetingRemindMeAtTranslator
    {
        public static MeetingRemindMeAtModel TranslateAsGetMeetingExternalInvitees(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var meeting = new MeetingRemindMeAtModel();

            if (reader.IsColumnExists("MeetingRemindID"))
                meeting.MeetingRemindID = SqlHelper.GetNullableInt32(reader, "MeetingRemindID");

            if (reader.IsColumnExists("RemindMeDateTime"))
                meeting.RemindMeDateTime = SqlHelper.GetDateTime(reader, "RemindMeDateTime");

            return meeting;
        }

        public static List<MeetingRemindMeAtModel> TranslateAsMeetingRemindMeAtList(this SqlDataReader reader)
        {
            var destinationDepartmentList = new List<MeetingRemindMeAtModel>();
            while (reader.Read())
            {
                destinationDepartmentList.Add(TranslateAsGetMeetingExternalInvitees(reader, true));
            }

            return destinationDepartmentList;
        }
    }
}
