using RulersCourt.Models.Calendar;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Calendar
{
    public static class CalendarBulkApprovalTranslator
    {
        public static CalendarWorkFlowModel TranslateAsLetterBulkWorkflow(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var calendar = new CalendarWorkFlowModel();

            if (reader.IsColumnExists("CalendarID"))
            {
                calendar.CalendarID = SqlHelper.GetNullableInt32(reader, "CalendarID");
            }

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                calendar.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("status"))
            {
                calendar.Status = SqlHelper.GetNullableInt32(reader, "status").GetValueOrDefault();
            }

            if (reader.IsColumnExists("CreatedBy"))
            {
                calendar.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatedBy").GetValueOrDefault();
            }

            if (reader.IsColumnExists("CreatedBy"))
            {
                calendar.FromID = SqlHelper.GetNullableInt32(reader, "CreatedBy").GetValueOrDefault();
            }

            if (reader.IsColumnExists("ApproverNameID"))
            {
                calendar.ApproverID = SqlHelper.GetNullableInt32(reader, "ApproverNameID");
            }

            return calendar;
        }

        public static List<CalendarWorkFlowModel> TranslateAsLetterBulkWorkflowList(this SqlDataReader reader)
        {
            var letterBulkWorkflowList = new List<CalendarWorkFlowModel>();
            while (reader.Read())
            {
                letterBulkWorkflowList.Add(TranslateAsLetterBulkWorkflow(reader, true));
            }

            return letterBulkWorkflowList;
        }
    }
}
