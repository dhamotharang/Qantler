using RulersCourt.Models;
using RulersCourt.Models.LeaveRequest;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.LeaveRequest
{
    public static class LeaveCommunicationHistoryTranslator
    {
        public static LeaveCommunicationHistory TranslateAsLeaveCommunicationHistory(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var leaveHistory = new LeaveCommunicationHistory();
            if (reader.IsColumnExists("CommunicationID"))
                leaveHistory.CommunicationID = SqlHelper.GetNullableInt32(reader, "CommunicationID");

            if (reader.IsColumnExists("LeaveID"))
                leaveHistory.LeaveID = SqlHelper.GetNullableInt32(reader, "LeaveID");

            if (reader.IsColumnExists("Message"))
                leaveHistory.Message = SqlHelper.GetNullableString(reader, "Message");

            if (reader.IsColumnExists("ParentCommunicationID"))
                leaveHistory.ParentCommunicationID = SqlHelper.GetNullableInt32(reader, "ParentCommunicationID");

            if (reader.IsColumnExists("Action"))
                leaveHistory.Action = SqlHelper.GetNullableString(reader, "Action");

            string photoGuid = string.Empty, photoName = string.Empty;

            if (reader.IsColumnExists("PhotoGuid"))
                photoGuid = SqlHelper.GetNullableString(reader, "PhotoGuid");

            if (reader.IsColumnExists("PhotoName"))
                photoName = SqlHelper.GetNullableString(reader, "PhotoName");

            if (!string.IsNullOrEmpty(photoGuid) && !string.IsNullOrEmpty(photoName))
            {
                leaveHistory.Photo = photoGuid + "|" + photoName;
            }

            if (reader.IsColumnExists("CreatedBy"))
                leaveHistory.CreatedBy = SqlHelper.GetNullableString(reader, "CreatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                leaveHistory.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime").GetValueOrDefault();
            return leaveHistory;
        }

        public static List<LeaveCommunicationHistory> TranslateAsLeaveCommunicationHistoryList(this SqlDataReader reader)
        {
            var communicationHistory = new List<LeaveCommunicationHistory>();
            while (reader.Read())
            {
                communicationHistory.Add(TranslateAsLeaveCommunicationHistory(reader, true));
            }

            return communicationHistory;
        }
    }
}
