using RulersCourt.Models.DutyTasks;
using System.Data.SqlClient;

namespace RulersCourt.Translators.DutyTask
{
    public static class DutyTaskHomeTranslator
    {
        public static DutyTaskHomeModel TranslateAsHomeCounts(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var label = new DutyTaskHomeModel();

            if (reader.IsColumnExists("MyTask"))
                label.MyTask = SqlHelper.GetNullableInt32(reader, "MyTask");

            if (reader.IsColumnExists("AssignedTask"))
                label.AssignedTask = SqlHelper.GetNullableInt32(reader, "AssignedTask");

            if (reader.IsColumnExists("TaskParticipations"))
                label.TaskParticipations = SqlHelper.GetNullableInt32(reader, "TaskParticipations");

            if (reader.IsColumnExists("NoOfNewTask"))
                label.NoOfNewTask = SqlHelper.GetNullableInt32(reader, "NoOfNewTask");

            if (reader.IsColumnExists("TaskBTStartReminderDate"))
                label.TaskBTStartReminderDate = SqlHelper.GetNullableInt32(reader, "TaskBTStartReminderDate");

            if (reader.IsColumnExists("TaskBTReminderEndDate"))
                label.TaskBTReminderEndDate = SqlHelper.GetNullableInt32(reader, "TaskBTReminderEndDate");

            if (reader.IsColumnExists("TaskEndDateGtActualdate"))
                label.TaskEndDateGtActualdate = SqlHelper.GetNullableInt32(reader, "TaskEndDateGtActualdate");

            if (reader.IsColumnExists("TaskInprogress"))
                label.TaskInprogress = SqlHelper.GetNullableInt32(reader, "TaskInprogress");

            if (reader.IsColumnExists("TaskCompleted"))
                label.TaskCompleted = SqlHelper.GetNullableInt32(reader, "TaskCompleted");

            if (reader.IsColumnExists("TaskClosed"))
                label.TaskClosed = SqlHelper.GetNullableInt32(reader, "TaskClosed");

            return label;
        }

        public static DutyTaskHomeModel TranslateAsHomeCount(this SqlDataReader reader)
        {
            var homeModel = new DutyTaskHomeModel();
            while (reader.Read())
            {
                homeModel = TranslateAsHomeCounts(reader, true);
            }

            return homeModel;
        }
    }
}
