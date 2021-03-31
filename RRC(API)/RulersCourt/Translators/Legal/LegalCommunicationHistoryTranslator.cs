using RulersCourt.Models;
using RulersCourt.Models.Legal;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Legal
{
    public static class LegalCommunicationHistoryTranslator
    {
        public static LegalCommunicationHistory TranslateAsLegalCommunicationHistory(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var legalHistory = new LegalCommunicationHistory();
            if (reader.IsColumnExists("CommunicationID"))
                legalHistory.CommunicationID = SqlHelper.GetNullableInt32(reader, "CommunicationID");

            if (reader.IsColumnExists("LegalID"))
                legalHistory.LegalID = SqlHelper.GetNullableInt32(reader, "LegalID");

            if (reader.IsColumnExists("Message"))
                legalHistory.Message = SqlHelper.GetNullableString(reader, "Message");

            if (reader.IsColumnExists("ParentCommunicationID"))
                legalHistory.ParentCommunicationID = SqlHelper.GetNullableInt32(reader, "ParentCommunicationID");

            if (reader.IsColumnExists("Action"))
                legalHistory.Action = SqlHelper.GetNullableString(reader, "Action");

            string photoGuid = string.Empty, photoName = string.Empty;

            if (reader.IsColumnExists("PhotoGuid"))
                photoGuid = SqlHelper.GetNullableString(reader, "PhotoGuid");

            if (reader.IsColumnExists("PhotoName"))
                photoName = SqlHelper.GetNullableString(reader, "PhotoName");

            if (!string.IsNullOrEmpty(photoGuid) && !string.IsNullOrEmpty(photoName))
            {
                legalHistory.Photo = photoGuid + "|" + photoName;
            }

            if (reader.IsColumnExists("CreatedBy"))
                legalHistory.CreatedBy = SqlHelper.GetNullableString(reader, "CreatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                legalHistory.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime").GetValueOrDefault();
            return legalHistory;
        }

        public static List<LegalCommunicationHistory> TranslateAsLegalCommunicationHistoryList(this SqlDataReader reader)
        {
            var communicationHistory = new List<LegalCommunicationHistory>();
            while (reader.Read())
            {
                communicationHistory.Add(TranslateAsLegalCommunicationHistory(reader, true));
            }

            return communicationHistory;
        }
    }
}