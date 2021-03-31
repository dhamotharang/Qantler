using RulersCourt.Models.DutyTask;
using System.Data.SqlClient;

namespace RulersCourt.Translators.DutyTask
{
    public static class DutyTaskSaveResponseTranslator
    {
        public static DutyTaskWorkflowModel TranslateAsDutyTaskSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var dutyTaskSave = new DutyTaskWorkflowModel();

            if (reader.IsColumnExists("TaskID"))
                dutyTaskSave.TaskID = SqlHelper.GetNullableInt32(reader, "TaskID");

            if (reader.IsColumnExists("ReferenceNumber"))
                dutyTaskSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                dutyTaskSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("AssigneeUserId"))
                dutyTaskSave.AssigneeUserId = SqlHelper.GetNullableInt32(reader, "AssigneeUserId").GetValueOrDefault();

            return dutyTaskSave;
        }

        public static DutyTaskWorkflowModel TranslateAsDutyTaskSaveResponseList(this SqlDataReader reader)
        {
            var dutyTaskSaveResponse = new DutyTaskWorkflowModel();
            while (reader.Read())
            {
                dutyTaskSaveResponse = TranslateAsDutyTaskSaveResponse(reader, true);
            }

            return dutyTaskSaveResponse;
        }
    }
}
