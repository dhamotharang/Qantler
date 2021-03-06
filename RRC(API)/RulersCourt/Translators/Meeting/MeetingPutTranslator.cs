using RulersCourt.Models.Meeting;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Meeting
{
    public static class MeetingPutTranslator
    {
        public static MeetingPutModel TranslateAsMeeting(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var meeting = new MeetingPutModel();

            if (reader.IsColumnExists("MeetingID"))
            {
                meeting.MeetingID = SqlHelper.GetNullableInt32(reader, "MeetingID");
            }

            if (reader.IsColumnExists("Subject"))
            {
                meeting.Subject = SqlHelper.GetNullableString(reader, "Subject");
            }

            if (reader.IsColumnExists("OrganizerDepartmentID"))
            {
                meeting.OrganizerDepartmentID = SqlHelper.GetNullableInt32(reader, "OrganizerDepartmentID");
            }

            if (reader.IsColumnExists("OrganizerUserID"))
            {
                meeting.OrganizerUserID = SqlHelper.GetNullableInt32(reader, "OrganizerUserID");
            }

            if (reader.IsColumnExists("Location"))
            {
                meeting.Location = SqlHelper.GetNullableString(reader, "Location");
            }

            if (reader.IsColumnExists("StartDateTime"))
            {
                meeting.StartDateTime = SqlHelper.GetDateTime(reader, "StartDateTime");
            }

            if (reader.IsColumnExists("EndDateTime"))
            {
                meeting.EndDateTime = SqlHelper.GetDateTime(reader, "EndDateTime");
            }

            if (reader.IsColumnExists("MeetingType"))
            {
                meeting.MeetingType = SqlHelper.GetNullableInt32(reader, "MeetingType");
            }

            if (reader.IsColumnExists("IsExternalInvitees"))
            {
                meeting.IsExternalInvitees = SqlHelper.GetBoolean(reader, "IsExternalInvitees");
            }

            if (reader.IsColumnExists("IsInternalInvitees"))
            {
                meeting.IsInternalInvitees = SqlHelper.GetBoolean(reader, "IsInternalInvitees");
            }

            if (reader.IsColumnExists("UpdatedBy"))
            {
                meeting.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");
            }

            if (reader.IsColumnExists("UpdatedDateTime"))
            {
                meeting.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");
            }

            return meeting;
        }

        public static List<MeetingPutModel> TranslateAsPutMeetingList(this SqlDataReader reader)
        {
            var letterList = new List<MeetingPutModel>();
            while (reader.Read())
            {
                letterList.Add(TranslateAsMeeting(reader, true));
            }

            return letterList;
        }
    }
}
