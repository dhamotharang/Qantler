using RulersCourt.Models.DutyTask;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.DutyTask
{
    public static class DutyTaskReportListTranslator
    {
        public static DutyTaskReportListModel TranslateAsReportDutyTask(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var dutyTaskModel = new DutyTaskReportListModel();

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

            if (reader.IsColumnExists("ParticipantUsers"))
                dutyTaskModel.Participations = SqlHelper.GetNullableString(reader, "ParticipantUsers");

            return dutyTaskModel;
        }

        public static List<DutyTaskReportListModel> TranslateAsDutyTaskReportList(this SqlDataReader reader)
        {
            var list = new List<DutyTaskReportListModel>();
            while (reader.Read())
            {
                list.Add(TranslateAsReportDutyTask(reader, true));
            }

            return list;
        }
    }
}
