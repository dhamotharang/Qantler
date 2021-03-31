using RulersCourt.Models.Meeting;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Meeting
{
    public static class MeetingCommunicationHistory
    {
        public static MeetingCommunicationHistoryModel TranslateAsDesignHistoryLog(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var designCommunicationHistory = new MeetingCommunicationHistoryModel();

            if (reader.IsColumnExists("CommunicationID"))
                designCommunicationHistory.CommunicationID = SqlHelper.GetNullableInt32(reader, "CommunicationID");

            if (reader.IsColumnExists("MeetingID"))
                designCommunicationHistory.MeetingID = SqlHelper.GetNullableInt32(reader, "MeetingID");

            if (reader.IsColumnExists("ParentCommunicationID"))
                designCommunicationHistory.ParentCommunicationID = SqlHelper.GetNullableInt32(reader, "ParentCommunicationID");

            if (reader.IsColumnExists("Action"))
                designCommunicationHistory.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Message"))
                designCommunicationHistory.Message = SqlHelper.GetNullableString(reader, "Message");

            if (reader.IsColumnExists("CreatedBy"))
                designCommunicationHistory.CreatedBy = SqlHelper.GetNullableString(reader, "CreatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                designCommunicationHistory.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            return designCommunicationHistory;
        }

        public static List<MeetingCommunicationHistoryModel> TranslateAsMeetingHistoryLogList(this SqlDataReader reader)
        {
            var designCommunicationHistoryList = new List<MeetingCommunicationHistoryModel>();
            while (reader.Read())
            {
                designCommunicationHistoryList.Add(TranslateAsDesignHistoryLog(reader, true));
            }

            return designCommunicationHistoryList;
        }
    }
}