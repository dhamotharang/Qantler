using RulersCourt.Models.Meeting;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Meeting
{
    public static class MeetingGetInviteesTranslator
    {
        public static MeetingExternalInviteesModel TranslateAsGetMeetingExternalInvitees(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var meeting = new MeetingExternalInviteesModel();

            if (reader.IsColumnExists("MeetingExternalInviteesID"))
                meeting.MeetingExternalInviteesID = SqlHelper.GetNullableInt32(reader, "MeetingExternalInviteesID");

            if (reader.IsColumnExists("Organization"))
                meeting.Organization = SqlHelper.GetNullableString(reader, "Organization");

            if (reader.IsColumnExists("ContactPerson"))
                meeting.ContactPerson = SqlHelper.GetNullableString(reader, "ContactPerson");

            if (reader.IsColumnExists("EmailID"))
                meeting.EmailID = SqlHelper.GetNullableString(reader, "EmailID");

            return meeting;
        }

        public static List<MeetingExternalInviteesModel> TranslateAsMeetingExternalInviteesList(this SqlDataReader reader)
        {
            var destinationDepartmentList = new List<MeetingExternalInviteesModel>();
            while (reader.Read())
            {
                destinationDepartmentList.Add(TranslateAsGetMeetingExternalInvitees(reader, true));
            }

            return destinationDepartmentList;
        }

        public static MeetingInternalInviteesModel TranslateAsGetMeetingInternalInvitees(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var meeting = new MeetingInternalInviteesModel();

            if (reader.IsColumnExists("MeetingInternalInviteesID"))
                meeting.MeetingInternalInviteesID = SqlHelper.GetNullableInt32(reader, "MeetingInternalInviteesID");

            if (reader.IsColumnExists("DepartmentID"))
                meeting.DepartmentID = SqlHelper.GetNullableInt32(reader, "DepartmentID");

            if (reader.IsColumnExists("UserID"))
                meeting.UserID = SqlHelper.GetNullableInt32(reader, "UserID");

            return meeting;
        }

        public static List<MeetingInternalInviteesModel> TranslateAsMeetingInternalInviteesList(this SqlDataReader reader)
        {
            var destinationDepartmentList = new List<MeetingInternalInviteesModel>();
            while (reader.Read())
            {
                destinationDepartmentList.Add(TranslateAsGetMeetingInternalInvitees(reader, true));
            }

            return destinationDepartmentList;
        }
    }
}
