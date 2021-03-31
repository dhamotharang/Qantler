using RulersCourt.Models;
using RulersCourt.Models.Design;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.Media.Design
{
    public static class DesignHistoryLog
    {
        public static DesignCommunicationHistory TranslateAsDesignHistoryLog(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var designCommunicationHistory = new DesignCommunicationHistory();

            string photoGuid = string.Empty, photoName = string.Empty;

            if (reader.IsColumnExists("PhotoGuid"))
                photoGuid = SqlHelper.GetNullableString(reader, "PhotoGuid");

            if (reader.IsColumnExists("PhotoName"))
                photoName = SqlHelper.GetNullableString(reader, "PhotoName");

            if (!string.IsNullOrEmpty(photoGuid) && !string.IsNullOrEmpty(photoName))
            {
                designCommunicationHistory.Photo = photoGuid + "|" + photoName;
            }

            if (reader.IsColumnExists("CommunicationID"))
                designCommunicationHistory.CommunicationID = SqlHelper.GetNullableInt32(reader, "CommunicationID");

            if (reader.IsColumnExists("DesignID"))
                designCommunicationHistory.DesignID = SqlHelper.GetNullableInt32(reader, "DesignID");

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

        public static List<DesignCommunicationHistory> TranslateAsDesignHistoryLogList(this SqlDataReader reader)
        {
            var designCommunicationHistoryList = new List<DesignCommunicationHistory>();
            while (reader.Read())
            {
                designCommunicationHistoryList.Add(TranslateAsDesignHistoryLog(reader, true));
            }

            return designCommunicationHistoryList;
        }
    }
}
