using RulersCourt.Models.LeaveRequest;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.LeaveRequest
{
    public static class LeaveHistoryLogTranslator
    {
        public static LeaveHistoryLogModel TranslateAsLeaveHistoryLog(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var leaveHistoryLogModel = new LeaveHistoryLogModel();

            if (reader.IsColumnExists("HistoryID"))
                leaveHistoryLogModel.HistoryID = SqlHelper.GetNullableInt32(reader, "HistoryID");

            if (reader.IsColumnExists("LeaveID"))
                leaveHistoryLogModel.LeaveID = SqlHelper.GetNullableInt32(reader, "LeaveID");

            if (reader.IsColumnExists("Action"))
                leaveHistoryLogModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                leaveHistoryLogModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("ActionBy"))
                leaveHistoryLogModel.ActionBy = SqlHelper.GetNullableString(reader, "ActionBy");

            if (reader.IsColumnExists("ActionDateTime"))
                leaveHistoryLogModel.ActionDateTime = SqlHelper.GetDateTime(reader, "ActionDateTime");

            return leaveHistoryLogModel;
        }

        public static List<LeaveHistoryLogModel> TranslateAsLeaveHistoryLogList(this SqlDataReader reader)
        {
            var leaveHistoryLogList = new List<LeaveHistoryLogModel>();
            while (reader.Read())
            {
                leaveHistoryLogList.Add(TranslateAsLeaveHistoryLog(reader, true));
            }

            return leaveHistoryLogList;
        }
    }
}
