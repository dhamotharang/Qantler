using RulersCourt.Models;
using RulersCourt.Models.Photographer;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.Media.Photographer
{
    public static class PhotographerCommunicationHistoryTranslator
    {
        public static PhotographerCommunicationHistory TranslateAsPhotographerCommunicationHistory(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var photographerHistory = new PhotographerCommunicationHistory();

            string photoGuid = string.Empty, photoName = string.Empty;

            if (reader.IsColumnExists("PhotoGuid"))
                photoGuid = SqlHelper.GetNullableString(reader, "PhotoGuid");

            if (reader.IsColumnExists("PhotoName"))
                photoName = SqlHelper.GetNullableString(reader, "PhotoName");

            if (!string.IsNullOrEmpty(photoGuid) && !string.IsNullOrEmpty(photoName))
            {
                photographerHistory.Photo = photoGuid + "|" + photoName;
            }

            if (reader.IsColumnExists("CommunicationID"))
                photographerHistory.CommunicationID = SqlHelper.GetNullableInt32(reader, "CommunicationID");

            if (reader.IsColumnExists("PhotographerID"))
                photographerHistory.PhotographerID = SqlHelper.GetNullableInt32(reader, "PhotographerID");

            if (reader.IsColumnExists("Message"))
                photographerHistory.Message = SqlHelper.GetNullableString(reader, "Message");

            if (reader.IsColumnExists("ParentCommunicationID"))
                photographerHistory.ParentCommunicationID = SqlHelper.GetNullableInt32(reader, "ParentCommunicationID");

            if (reader.IsColumnExists("Action"))
                photographerHistory.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("CreatedBy"))
                photographerHistory.CreatedBy = SqlHelper.GetNullableString(reader, "CreatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                photographerHistory.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime").GetValueOrDefault();
            return photographerHistory;
        }

        public static List<PhotographerCommunicationHistory> TranslateAsPhotographerCommunicationHistoryList(this SqlDataReader reader)
        {
            var communicationHistory = new List<PhotographerCommunicationHistory>();
            while (reader.Read())
            {
                communicationHistory.Add(TranslateAsPhotographerCommunicationHistory(reader, true));
            }

            return communicationHistory;
        }
    }
}
