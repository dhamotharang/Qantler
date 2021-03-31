using RulersCourt.Models.DutyTask;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.DutyTask
{
    public static class DutyTaskTranslator
    {
        public static DutyTaskGetModel TranslateAsDutyTask(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var dutyTaskModel = new DutyTaskGetModel();

            if (reader.IsColumnExists("TaskID"))
                dutyTaskModel.TaskID = SqlHelper.GetNullableInt32(reader, "TaskID");

            if (reader.IsColumnExists("ReferenceNumber"))
                dutyTaskModel.TaskReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("SourceOU"))
                dutyTaskModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                dutyTaskModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("Title"))
                dutyTaskModel.Title = SqlHelper.GetNullableString(reader, "Title");

            if (reader.IsColumnExists("StartDate"))
                dutyTaskModel.StartDate = SqlHelper.GetDateTime(reader, "StartDate");

            if (reader.IsColumnExists("EndDate"))
                dutyTaskModel.EndDate = SqlHelper.GetDateTime(reader, "EndDate");

            if (reader.IsColumnExists("TaskDetails"))
                dutyTaskModel.TaskDetails = SqlHelper.GetNullableString(reader, "TaskDetails");

            if (reader.IsColumnExists("Priority"))
                dutyTaskModel.Priority = SqlHelper.GetNullableString(reader, "Priority");

            if (reader.IsColumnExists("RemindMeAt"))
                dutyTaskModel.RemindMeAt = SqlHelper.GetDateTime(reader, "RemindMeAt");

            if (reader.IsColumnExists("Status"))
                dutyTaskModel.Status = SqlHelper.GetNullableInt32(reader, "Status");

            if (reader.IsColumnExists("CreatedBy"))
                dutyTaskModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                dutyTaskModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedBy"))
                dutyTaskModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                dutyTaskModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("AssigneeUserId"))
                dutyTaskModel.AssigneeUserId = SqlHelper.GetNullableInt32(reader, "AssigneeUserId");

            if (reader.IsColumnExists("AssigneeDepartmentId"))
                dutyTaskModel.AssigneeDepartmentId = SqlHelper.GetNullableInt32(reader, "AssigneeDepartmentId");

            if (reader.IsColumnExists("DelegateAssignee"))
                dutyTaskModel.DelegateAssignee = SqlHelper.GetNullableInt32(reader, "DelegateAssignee");

            if (reader.IsColumnExists("DeleteFlag"))
                dutyTaskModel.DeleteFlag = SqlHelper.GetBoolean(reader, "DeleteFlag");

            if (reader.IsColumnExists("Country"))
                dutyTaskModel.Country = SqlHelper.GetNullableInt32(reader, "Country");

            if (reader.IsColumnExists("City"))
                dutyTaskModel.City = SqlHelper.GetNullableString(reader, "City");

            if (reader.IsColumnExists("Emirates"))
                dutyTaskModel.Emirates = SqlHelper.GetNullableInt32(reader, "Emirates");

            return dutyTaskModel;
        }

        public static List<DutyTaskGetModel> TranslateAsDutyTaskList(this SqlDataReader reader)
        {
            var dutyTaskList = new List<DutyTaskGetModel>();
            while (reader.Read())
            {
                dutyTaskList.Add(TranslateAsDutyTask(reader, true));
            }

            return dutyTaskList;
        }

        public static DutyTaskPutModel TranslateAsPutDutyTask(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var dutyTaskModel = new DutyTaskPutModel();

            if (reader.IsColumnExists("TaskID"))
                dutyTaskModel.TaskID = SqlHelper.GetNullableInt32(reader, "TaskID");

            if (reader.IsColumnExists("ReferenceNumber"))
                dutyTaskModel.TaskReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("SourceOU"))
                dutyTaskModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                dutyTaskModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("Title"))
                dutyTaskModel.Title = SqlHelper.GetNullableString(reader, "Title");

            if (reader.IsColumnExists("StartDate"))
                dutyTaskModel.StartDate = SqlHelper.GetDateTime(reader, "StartDate");

            if (reader.IsColumnExists("EndDate"))
                dutyTaskModel.EndDate = SqlHelper.GetDateTime(reader, "EndDate");

            if (reader.IsColumnExists("TaskDetails"))
                dutyTaskModel.TaskDetails = SqlHelper.GetNullableString(reader, "TaskDetails");

            if (reader.IsColumnExists("Priority"))
                dutyTaskModel.Priority = SqlHelper.GetNullableString(reader, "Priority");

            if (reader.IsColumnExists("RemindMeAt"))
                dutyTaskModel.RemindMeAt = SqlHelper.GetDateTime(reader, "RemindMeAt");

            if (reader.IsColumnExists("UpdatedBy"))
                dutyTaskModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                dutyTaskModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("AssigneeUserId"))
                dutyTaskModel.AssigneeUserId = SqlHelper.GetNullableInt32(reader, "AssigneeUserId");

            if (reader.IsColumnExists("AssigneeDepartmentId"))
                dutyTaskModel.AssigneeDepartmentId = SqlHelper.GetNullableInt32(reader, "AssigneeDepartmentId");

            if (reader.IsColumnExists("DelegateAssignee"))
                dutyTaskModel.DelegateAssignee = SqlHelper.GetNullableInt32(reader, "DelegateAssignee");

            if (reader.IsColumnExists("City"))
                dutyTaskModel.City = SqlHelper.GetNullableString(reader, "City");

            if (reader.IsColumnExists("Emirates"))
                dutyTaskModel.Emirates = SqlHelper.GetNullableInt32(reader, "Emirates");

            if (reader.IsColumnExists("Country"))
                dutyTaskModel.Country = SqlHelper.GetNullableInt32(reader, "Country");

            return dutyTaskModel;
        }

        public static List<DutyTaskPutModel> TranslateAsPutDutyTaskList(this SqlDataReader reader)
        {
            var dutyTaskList = new List<DutyTaskPutModel>();
            while (reader.Read())
            {
                dutyTaskList.Add(TranslateAsPutDutyTask(reader, true));
            }

            return dutyTaskList;
        }
    }
}
