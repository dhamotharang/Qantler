using RulersCourt.Models.DutyTasks;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.DutyTask
{
    public static class DutyTaskCommunicationHistoryTranslator
    {
        public static DutyTaskCommunicationHistoryGetModel TranslateAsDutyTaskCommunicationHistory(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var dutyTaskHistory = new DutyTaskCommunicationHistoryGetModel();
            if (reader.IsColumnExists("CommunicationID"))
                dutyTaskHistory.CommunicationID = SqlHelper.GetNullableInt32(reader, "CommunicationID");

            if (reader.IsColumnExists("TaskID"))
                dutyTaskHistory.TaskID = SqlHelper.GetNullableInt32(reader, "TaskID");

            if (reader.IsColumnExists("Message"))
                dutyTaskHistory.Message = SqlHelper.GetNullableString(reader, "Message");

            if (reader.IsColumnExists("AttachmentGuid"))
                dutyTaskHistory.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentName"))
                dutyTaskHistory.AttachmentName = SqlHelper.GetNullableString(reader, "AttachmentName");

            if (reader.IsColumnExists("Action"))
                dutyTaskHistory.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("CreatedBy"))
                dutyTaskHistory.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("Creator"))
                dutyTaskHistory.Creator = SqlHelper.GetNullableString(reader, "Creator");

            if (reader.IsColumnExists("CreatedDateTime"))
                dutyTaskHistory.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime").GetValueOrDefault();
            dutyTaskHistory.Photo = "https://picsum.photos/id/1074/200/200";
            return dutyTaskHistory;
        }

        public static List<DutyTaskCommunicationHistoryGetModel> TranslateAsDutyTaskCommunicationHistoryList(this SqlDataReader reader)
        {
            var communicationHistory = new List<DutyTaskCommunicationHistoryGetModel>();
            while (reader.Read())
            {
                communicationHistory.Add(TranslateAsDutyTaskCommunicationHistory(reader, true));
            }

            return communicationHistory;
        }
    }
}
