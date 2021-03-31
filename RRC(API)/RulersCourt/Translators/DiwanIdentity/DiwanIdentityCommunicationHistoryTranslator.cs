using RulersCourt.Models;
using RulersCourt.Models.DiwanIdentity;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.Media.DiwanIdentity
{
    public static class DiwanIdentityCommunicationHistoryTranslator
    {
        public static DiwanIdentityCommunicationHistory TranslateAsDiwanIdentityCommunicationHistory(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var diwanIdentityHistory = new DiwanIdentityCommunicationHistory();

            string photoGuid = string.Empty, photoName = string.Empty;

            if (reader.IsColumnExists("PhotoGuid"))
                photoGuid = SqlHelper.GetNullableString(reader, "PhotoGuid");

            if (reader.IsColumnExists("PhotoName"))
                photoName = SqlHelper.GetNullableString(reader, "PhotoName");

            if (!string.IsNullOrEmpty(photoGuid) && !string.IsNullOrEmpty(photoName))
            {
                diwanIdentityHistory.Photo = photoGuid + "|" + photoName;
            }

            if (reader.IsColumnExists("CommunicationID"))
                diwanIdentityHistory.CommunicationID = SqlHelper.GetNullableInt32(reader, "CommunicationID");

            if (reader.IsColumnExists("DiwanIdentityID"))
                diwanIdentityHistory.DiwanIdentityID = SqlHelper.GetNullableInt32(reader, "DiwanIdentityID");

            if (reader.IsColumnExists("Message"))
                diwanIdentityHistory.Message = SqlHelper.GetNullableString(reader, "Message");

            if (reader.IsColumnExists("ParentCommunicationID"))
                diwanIdentityHistory.ParentCommunicationID = SqlHelper.GetNullableInt32(reader, "ParentCommunicationID");

            if (reader.IsColumnExists("Action"))
                diwanIdentityHistory.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("CreatedBy"))
                diwanIdentityHistory.CreatedBy = SqlHelper.GetNullableString(reader, "CreatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                diwanIdentityHistory.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime").GetValueOrDefault();
            return diwanIdentityHistory;
        }

        public static List<DiwanIdentityCommunicationHistory> TranslateAsDiwanIdentityCommunicationHistoryList(this SqlDataReader reader)
        {
            var communicationHistory = new List<DiwanIdentityCommunicationHistory>();
            while (reader.Read())
            {
                communicationHistory.Add(TranslateAsDiwanIdentityCommunicationHistory(reader, true));
            }

            return communicationHistory;
        }
    }
}
