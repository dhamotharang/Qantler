using RulersCourt.Models.Meeting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Translators.Meeting
{
    public static class MOMGetTranslator
    {
        public static MOMGetModel TranslateAsLetter(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var meeting = new MOMGetModel();

            if (reader.IsColumnExists("MOMID"))
            {
                meeting.MOMID = SqlHelper.GetNullableInt32(reader, "MOMID");
            }

            if (reader.IsColumnExists("MeetingID"))
            {
                meeting.MeetingID = SqlHelper.GetNullableInt32(reader, "MeetingID");
            }

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                meeting.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
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

            if (reader.IsColumnExists("Attendees"))
            {
                meeting.Attendees = SqlHelper.GetNullableString(reader, "Attendees");
            }

            if (reader.IsColumnExists("StartDateTime"))
            {
                meeting.StartDateTime = SqlHelper.GetDateTime(reader, "StartDateTime");
            }

            if (reader.IsColumnExists("EndDateTime"))
            {
                meeting.EndDateTime = SqlHelper.GetDateTime(reader, "EndDateTime");
            }

            if (reader.IsColumnExists("PointsDiscussed"))
            {
                meeting.PointsDiscussed = SqlHelper.GetNullableString(reader, "PointsDiscussed");
            }

            if (reader.IsColumnExists("PendingPoints"))
            {
                meeting.PendingPoints = SqlHelper.GetNullableString(reader, "PendingPoints");
            }

            if (reader.IsColumnExists("Suggestion"))
            {
                meeting.Suggestion = SqlHelper.GetNullableString(reader, "Suggestion");
            }

            if (reader.IsColumnExists("CreatedBy"))
            {
                meeting.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");
            }

            if (reader.IsColumnExists("CreatedDateTime"))
            {
                meeting.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");
            }

            return meeting;
        }

        public static List<MOMGetModel> TranslateAsMOMList(this SqlDataReader reader)
        {
            var letterList = new List<MOMGetModel>();
            while (reader.Read())
            {
                letterList.Add(TranslateAsLetter(reader, true));
            }

            return letterList;
        }
    }
}