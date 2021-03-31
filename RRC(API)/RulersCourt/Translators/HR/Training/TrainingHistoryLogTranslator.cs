using RulersCourt.Models.Training;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Training
{
    public static class TrainingHistoryLogTranslator
    {
        public static TrainingHistoryLogModel TranslateAsTrainingHistoryLog(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var trainingHistoryLogModel = new TrainingHistoryLogModel();

            if (reader.IsColumnExists("HistoryID"))
                trainingHistoryLogModel.HistoryID = SqlHelper.GetNullableInt32(reader, "HistoryID");

            if (reader.IsColumnExists("Action"))
                trainingHistoryLogModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                trainingHistoryLogModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("ActionBy"))
                trainingHistoryLogModel.ActionBy = SqlHelper.GetNullableString(reader, "ActionBy");

            if (reader.IsColumnExists("ActionDateTime"))
                trainingHistoryLogModel.ActionDateTime = SqlHelper.GetDateTime(reader, "ActionDateTime");

            return trainingHistoryLogModel;
        }

        public static List<TrainingHistoryLogModel> TranslateAsTrainingHistoryLogList(this SqlDataReader reader)
        {
            var trainingHistoryLogList = new List<TrainingHistoryLogModel>();
            while (reader.Read())
            {
                trainingHistoryLogList.Add(TranslateAsTrainingHistoryLog(reader, true));
            }

            return trainingHistoryLogList;
        }
    }
}
