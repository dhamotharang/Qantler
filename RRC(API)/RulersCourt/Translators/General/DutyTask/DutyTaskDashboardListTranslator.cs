using RulersCourt.Models.DutyTask;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.DutyTask
{
    public static class DutyTaskDashboardListTranslator
    {
        public static DutyTaskDashboardListModel TranslateAsDutyTask(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var dutyTaskModel = new DutyTaskDashboardListModel();

            if (reader.IsColumnExists("TaskID"))
                dutyTaskModel.TaskID = SqlHelper.GetNullableInt32(reader, "TaskID");

            if (reader.IsColumnExists("TaskReferenceNumber"))
                dutyTaskModel.TaskReferenceNumber = SqlHelper.GetNullableString(reader, "TaskReferenceNumber");

            if (reader.IsColumnExists("Title"))
                dutyTaskModel.Title = SqlHelper.GetNullableString(reader, "Title");

            if (reader.IsColumnExists("Creator"))
                dutyTaskModel.Creator = SqlHelper.GetNullableString(reader, "Creator");

            if (reader.IsColumnExists("Assignee"))
                dutyTaskModel.Assignee = SqlHelper.GetNullableString(reader, "Assignee");

            if (reader.IsColumnExists("Status"))
                dutyTaskModel.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("Priority"))
                dutyTaskModel.Priority = SqlHelper.GetNullableString(reader, "Priority");

            if (reader.IsColumnExists("CreationDate"))
                dutyTaskModel.CreationDate = SqlHelper.GetDateTime(reader, "CreationDate");

            if (reader.IsColumnExists("DueDate"))
                dutyTaskModel.DueDate = SqlHelper.GetDateTime(reader, "DueDate");

            if (reader.IsColumnExists("LastUpdate"))
                dutyTaskModel.LastUpdate = SqlHelper.GetDateTime(reader, "LastUpdate");

            if (reader.IsColumnExists("DeleteFlag"))
                dutyTaskModel.DeleteFlag = SqlHelper.GetBoolean(reader, "DeleteFlag");

            return dutyTaskModel;
        }

        public static List<DutyTaskDashboardListModel> TranslateAsList(this SqlDataReader reader)
        {
            var list = new List<DutyTaskDashboardListModel>();
            while (reader.Read())
            {
                list.Add(TranslateAsDutyTask(reader, true));
            }

            return list;
        }
    }
}
