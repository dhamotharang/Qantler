using RulersCourt.Models;
using RulersCourt.Models.OfficalTask;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.OfficialTaskCompensation.OfficalTask
{
    public static class OfficialTaskCommunicationHistoryTranslator
    {
        public static OfficialTaskCommunicationHistoryModel TranslateAsOfficialTaskCommunicationHistory(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var officalTaskCommunicationHistory = new OfficialTaskCommunicationHistoryModel();
            if (reader.IsColumnExists("CommunicationID"))
                officalTaskCommunicationHistory.CommunicationID = SqlHelper.GetNullableInt32(reader, "CommunicationID");

            if (reader.IsColumnExists("OfficialTaskID"))
                officalTaskCommunicationHistory.OfficialTaskID = SqlHelper.GetNullableInt32(reader, "OfficialTaskID");

            if (reader.IsColumnExists("Message"))
                officalTaskCommunicationHistory.Message = SqlHelper.GetNullableString(reader, "Message");

            if (reader.IsColumnExists("ParentCommunicationID"))
                officalTaskCommunicationHistory.ParentCommunicationID = SqlHelper.GetNullableInt32(reader, "ParentCommunicationID");

            if (reader.IsColumnExists("Action"))
                officalTaskCommunicationHistory.Action = SqlHelper.GetNullableString(reader, "Action");
            string photoGuid = string.Empty, photoName = string.Empty;

            if (reader.IsColumnExists("PhotoGuid"))
                photoGuid = SqlHelper.GetNullableString(reader, "PhotoGuid");

            if (reader.IsColumnExists("PhotoName"))
                photoName = SqlHelper.GetNullableString(reader, "PhotoName");

            if (!string.IsNullOrEmpty(photoGuid) && !string.IsNullOrEmpty(photoName))
            {
                officalTaskCommunicationHistory.Photo = photoGuid + "|" + photoName;
            }

            if (reader.IsColumnExists("CreatedBy"))
                officalTaskCommunicationHistory.CreatedBy = SqlHelper.GetNullableString(reader, "CreatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                officalTaskCommunicationHistory.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime").GetValueOrDefault();
            return officalTaskCommunicationHistory;
        }

        public static List<OfficialTaskCommunicationHistoryModel> TranslateAsOfficialTaskCommunicationHistoryList(this SqlDataReader reader)
        {
            var communicationHistory = new List<OfficialTaskCommunicationHistoryModel>();
            while (reader.Read())
            {
                communicationHistory.Add(TranslateAsOfficialTaskCommunicationHistory(reader, true));
            }

            return communicationHistory;
        }
    }
}