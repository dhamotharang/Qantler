using RulersCourt.Models;
using RulersCourt.Models.HR.Training;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.HR.Training
{
    public static class TrainingCommunicationHistoryTranslator
    {
        public static TrainingCommunicationHistory TranslateAsTrainingCommunicationHistory(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var trainingHistory = new TrainingCommunicationHistory();

            if (reader.IsColumnExists("CommunicationID"))
                trainingHistory.CommunicationID = SqlHelper.GetNullableInt32(reader, "CommunicationID");

            if (reader.IsColumnExists("TrainingID"))
                trainingHistory.TrainingID = SqlHelper.GetNullableInt32(reader, "TrainingID");

            if (reader.IsColumnExists("Message"))
                trainingHistory.Message = SqlHelper.GetNullableString(reader, "Message");

            if (reader.IsColumnExists("ParentCommunicationID"))
                trainingHistory.ParentCommunicationID = SqlHelper.GetNullableInt32(reader, "ParentCommunicationID");

            if (reader.IsColumnExists("Action"))
                trainingHistory.Action = SqlHelper.GetNullableString(reader, "Action");
            string photoGuid = string.Empty, photoName = string.Empty;

            if (reader.IsColumnExists("PhotoGuid"))
                photoGuid = SqlHelper.GetNullableString(reader, "PhotoGuid");

            if (reader.IsColumnExists("PhotoName"))
                photoName = SqlHelper.GetNullableString(reader, "PhotoName");

            if (!string.IsNullOrEmpty(photoGuid) && !string.IsNullOrEmpty(photoName))
            {
                trainingHistory.Photo = photoGuid + "|" + photoName;
            }

            if (reader.IsColumnExists("CreatedBy"))
                trainingHistory.CreatedBy = SqlHelper.GetNullableString(reader, "CreatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                trainingHistory.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            return trainingHistory;
        }

        public static List<TrainingCommunicationHistory> TranslateAsTrainingCommunicationHistoryList(this SqlDataReader reader)
        {
            var communicationHistory = new List<TrainingCommunicationHistory>();
            while (reader.Read())
            {
                communicationHistory.Add(TranslateAsTrainingCommunicationHistory(reader, true));
            }

            return communicationHistory;
        }
    }
}
