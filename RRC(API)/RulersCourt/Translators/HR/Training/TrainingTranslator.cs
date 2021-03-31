using RulersCourt.Models.Training;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Training
{
    public static class TrainingTranslator
    {
        public static TrainingGetModel TranslateAsTraining(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var trainingModel = new TrainingGetModel();

            if (reader.IsColumnExists("TrainingID"))
            {
                trainingModel.TrainingID = SqlHelper.GetNullableInt32(reader, "TrainingID");
            }

            if (reader.IsColumnExists("TrainingReferenceID"))
            {
                trainingModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "TrainingReferenceID");
            }

            if (reader.IsColumnExists("TraineeName"))
            {
                trainingModel.TraineeName = SqlHelper.GetNullableString(reader, "TraineeName");
            }

            if (reader.IsColumnExists("TraineeDepartmentID"))
            {
                trainingModel.TraineeDepartmentID = SqlHelper.GetNullableInt32(reader, "TraineeDepartmentID");
            }

            if (reader.IsColumnExists("TrainingName"))
            {
                trainingModel.TrainingName = SqlHelper.GetNullableString(reader, "TrainingName");
            }

            if (reader.IsColumnExists("TrainingFor"))
            {
                trainingModel.TrainingFor = SqlHelper.GetBoolean(reader, "TrainingFor");
            }

            if (reader.IsColumnExists("SourceOU"))
            {
                trainingModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");
            }

            if (reader.IsColumnExists("SourceName"))
            {
                trainingModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");
            }

            if (reader.IsColumnExists("ApproverID"))
            {
                trainingModel.ApproverID = SqlHelper.GetNullableInt32(reader, "ApproverID");
            }

            if (reader.IsColumnExists("ApproverDepartmentID"))
            {
                trainingModel.ApproverDepartmentID = SqlHelper.GetNullableInt32(reader, "ApproverDepartmentID");
            }

            if (reader.IsColumnExists("Status"))
            {
                trainingModel.Status = SqlHelper.GetNullableInt32(reader, "Status");
            }

            if (reader.IsColumnExists("StartDate"))
            {
                trainingModel.StartDate = SqlHelper.GetDateTime(reader, "StartDate");
            }

            if (reader.IsColumnExists("EndDate"))
            {
                trainingModel.EndDate = SqlHelper.GetDateTime(reader, "EndDate");
            }

            if (reader.IsColumnExists("CreatedBy"))
            {
                trainingModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");
            }

            if (reader.IsColumnExists("UpdatedBy"))
            {
                trainingModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");
            }

            if (reader.IsColumnExists("CreatedDateTime"))
            {
                trainingModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");
            }

            if (reader.IsColumnExists("UpdatedDateTime"))
            {
                trainingModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");
            }

            if (reader.IsColumnExists("HRManagerUserID"))
            {
                trainingModel.HRManagerUserID = SqlHelper.GetNullableInt32(reader, "HRManagerUserID");
            }

            if (reader.IsColumnExists("IsNotificationReceived"))
            {
                trainingModel.IsNotificationReceived = SqlHelper.GetBoolean(reader, "IsNotificationReceived");
            }

            return trainingModel;
        }

        public static List<TrainingGetModel> TranslateAsTrainingList(this SqlDataReader reader)
        {
            var trainingList = new List<TrainingGetModel>();
            while (reader.Read())
            {
                trainingList.Add(TranslateAsTraining(reader, true));
            }

            return trainingList;
        }

        public static TrainingWorkflowModel TranslateAsTrainingSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var trainingSave = new TrainingWorkflowModel();

            if (reader.IsColumnExists("TrainingID"))
            {
                trainingSave.TrainingID = SqlHelper.GetNullableInt32(reader, "TrainingID");
            }

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                trainingSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("CreatorID"))
            {
                trainingSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();
            }

            if (reader.IsColumnExists("FromID"))
            {
                trainingSave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();
            }

            if (reader.IsColumnExists("CurrentStatus"))
            {
                trainingSave.CurrentStatus = SqlHelper.GetNullableInt32(reader, "CurrentStatus").GetValueOrDefault();
            }

            if (reader.IsColumnExists("TrainingID"))
            {
                trainingSave.TrainingID = SqlHelper.GetNullableInt32(reader, "TrainingID").GetValueOrDefault();
            }

            return trainingSave;
        }

        public static TrainingWorkflowModel TranslateAsTrainingSaveResponseList(this SqlDataReader reader)
        {
            var trainingSaveResponse = new TrainingWorkflowModel();
            while (reader.Read())
            {
                trainingSaveResponse = TranslateAsTrainingSaveResponse(reader, true);
            }

            return trainingSaveResponse;
        }

        public static TrainingPutModel TranslateAsPutTraining(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var trainingModel = new TrainingPutModel();

            if (reader.IsColumnExists("TrainingID"))
            {
                trainingModel.TrainingID = SqlHelper.GetNullableInt32(reader, "TrainingID");
            }

            if (reader.IsColumnExists("TrainingFor"))
            {
                trainingModel.TrainingFor = SqlHelper.GetBoolean(reader, "TrainingFor");
            }

            if (reader.IsColumnExists("TraineeName"))
            {
                trainingModel.TraineeName = SqlHelper.GetNullableString(reader, "TraineeName");
            }

            if (reader.IsColumnExists("TrainingName"))
            {
                trainingModel.TrainingName = SqlHelper.GetNullableString(reader, "TrainingName");
            }

            if (reader.IsColumnExists("SourceOU"))
            {
                trainingModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");
            }

            if (reader.IsColumnExists("SourceName"))
            {
                trainingModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");
            }

            if (reader.IsColumnExists("AssigneeID"))
            {
                trainingModel.AssigneeID = SqlHelper.GetNullableInt32(reader, "AssigneeID");
            }

            if (reader.IsColumnExists("ApproverID"))
            {
                trainingModel.ApproverID = SqlHelper.GetNullableInt32(reader, "ApproverID");
            }

            if (reader.IsColumnExists("ApproverDepartmentID"))
            {
                trainingModel.ApproverDepartmentID = SqlHelper.GetNullableInt32(reader, "ApproverDepartmentID");
            }

            if (reader.IsColumnExists("StartDate"))
            {
                trainingModel.StartDate = SqlHelper.GetDateTime(reader, "StartDate");
            }

            if (reader.IsColumnExists("EndDate"))
            {
                trainingModel.EndDate = SqlHelper.GetDateTime(reader, "EndDate");
            }

            if (reader.IsColumnExists("UpdatedBy"))
            {
                trainingModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");
            }

            if (reader.IsColumnExists("UpdatedDateTime"))
            {
                trainingModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");
            }

            if (reader.IsColumnExists("Action"))
            {
                trainingModel.Action = SqlHelper.GetNullableString(reader, "Action");
            }

            if (reader.IsColumnExists("HRManagerUserID"))
            {
                trainingModel.HRManagerUserID = SqlHelper.GetNullableInt32(reader, "HRManagerUserID");
            }

            return trainingModel;
        }

        public static List<TrainingPutModel> TranslateAsPutTrainingList(this SqlDataReader reader)
        {
            var trainingList = new List<TrainingPutModel>();
            while (reader.Read())
            {
                trainingList.Add(TranslateAsPutTraining(reader, true));
            }

            return trainingList;
        }
    }
}
