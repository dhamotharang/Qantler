using RulersCourt.Models;
using RulersCourt.Models.OfficalTaskCompensation.Compensation;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.OfficalTaskCompensation.Compensation
{
    public static class CompensationCommunicationHistoryTranslator
    {
        public static CompensationCommunicationHistoryModel TranslateAsCompensationCommunicationHistory(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var compensationCommunicationHistory = new CompensationCommunicationHistoryModel();

            if (reader.IsColumnExists("CommunicationID"))
                compensationCommunicationHistory.CommunicationID = SqlHelper.GetNullableInt32(reader, "CommunicationID");

            if (reader.IsColumnExists("CompensationID"))
                compensationCommunicationHistory.CompensationID = SqlHelper.GetNullableInt32(reader, "CompensationID");

            if (reader.IsColumnExists("Message"))
                compensationCommunicationHistory.Message = SqlHelper.GetNullableString(reader, "Message");

            if (reader.IsColumnExists("ParentCommunicationID"))
                compensationCommunicationHistory.ParentCommunicationID = SqlHelper.GetNullableInt32(reader, "ParentCommunicationID");

            if (reader.IsColumnExists("Action"))
                compensationCommunicationHistory.Action = SqlHelper.GetNullableString(reader, "Action");
            string photoGuid = string.Empty, photoName = string.Empty;

            if (reader.IsColumnExists("PhotoGuid"))
                photoGuid = SqlHelper.GetNullableString(reader, "PhotoGuid");

            if (reader.IsColumnExists("PhotoName"))
                photoName = SqlHelper.GetNullableString(reader, "PhotoName");

            if (!string.IsNullOrEmpty(photoGuid) && !string.IsNullOrEmpty(photoName))
            {
                compensationCommunicationHistory.Photo = photoGuid + "|" + photoName;
            }

            if (reader.IsColumnExists("CreatedBy"))
                compensationCommunicationHistory.CreatedBy = SqlHelper.GetNullableString(reader, "CreatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                compensationCommunicationHistory.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime").GetValueOrDefault();

            return compensationCommunicationHistory;
        }

        public static List<CompensationCommunicationHistoryModel> TranslateAsCompensationCommunicationHistoryList(this SqlDataReader reader)
        {
            var communicationHistory = new List<CompensationCommunicationHistoryModel>();
            while (reader.Read())
            {
                communicationHistory.Add(TranslateAsCompensationCommunicationHistory(reader, true));
            }

            return communicationHistory;
        }
    }
}