using RulersCourt.Models.LeaveRequest;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class LeaveTranslator
    {
        public static LeaveGetModel TranslateAsLeave(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var leaveModel = new LeaveGetModel();

            if (reader.IsColumnExists("LeaveID"))
                leaveModel.LeaveID = SqlHelper.GetNullableInt32(reader, "LeaveID");

            if (reader.IsColumnExists("ReferenceNumber"))
                leaveModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("SourceOU"))
                leaveModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                leaveModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("LeaveType"))
                leaveModel.LeaveType = SqlHelper.GetNullableString(reader, "LeaveType");

            if (reader.IsColumnExists("LeaveTypeOther"))
                leaveModel.LeaveTypeOther = SqlHelper.GetNullableInt32(reader, "LeaveTypeOther");

            if (reader.IsColumnExists("Reason"))
                leaveModel.Reason = SqlHelper.GetNullableString(reader, "Reason");

            if (reader.IsColumnExists("DOANameID"))
                leaveModel.DOANameID = SqlHelper.GetNullableInt32(reader, "DOANameID");

            if (reader.IsColumnExists("DOADepartmentID"))
                leaveModel.DOADepartmentID = SqlHelper.GetNullableInt32(reader, "DOADepartmentID");

            if (reader.IsColumnExists("AssigneeID"))
                leaveModel.AssigneeID = SqlHelper.GetNullableInt32(reader, "AssigneeID");

            if (reader.IsColumnExists("StartDate"))
                leaveModel.StartDate = SqlHelper.GetDateTime(reader, "StartDate");

            if (reader.IsColumnExists("EndDate"))
                leaveModel.EndDate = SqlHelper.GetDateTime(reader, "EndDate");

            if (reader.IsColumnExists("BalanceLeave"))
                leaveModel.BalanceLeave = SqlHelper.GetNullableString(reader, "BalanceLeave");

            if (reader.IsColumnExists("CreatedBy"))
                leaveModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                leaveModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("Status"))
                leaveModel.Status = SqlHelper.GetNullableInt32(reader, "Status");

            if (reader.IsColumnExists("CreatedDateTime"))
                leaveModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                leaveModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("ApproverDepartmentID"))
                leaveModel.ApproverDepartmentID = SqlHelper.GetNullableInt32(reader, "ApproverDepartmentID");

            if (reader.IsColumnExists("ApproverNameID"))
                leaveModel.ApproverID = SqlHelper.GetNullableInt32(reader, "ApproverNameID");

            if (reader.IsColumnExists("HRManagerUserID"))
                leaveModel.HRManagerUserID = SqlHelper.GetNullableInt32(reader, "HRManagerUserID");

            return leaveModel;
        }

        public static List<LeaveGetModel> TranslateAsLeaveList(this SqlDataReader reader)
        {
            var leaveList = new List<LeaveGetModel>();
            while (reader.Read())
            {
                leaveList.Add(TranslateAsLeave(reader, true));
            }

            return leaveList;
        }

        public static LeavePutModel TranslateAsPutLeave(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var leaveModel = new LeavePutModel();

            if (reader.IsColumnExists("LeaveID"))
                leaveModel.LeaveID = SqlHelper.GetNullableInt32(reader, "LeaveID");

            if (reader.IsColumnExists("SourceOU"))
                leaveModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                leaveModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("LeaveType"))
                leaveModel.LeaveType = SqlHelper.GetNullableString(reader, "LeaveType");

            if (reader.IsColumnExists("LeaveTypeOther"))
                leaveModel.LeaveTypeOther = SqlHelper.GetNullableInt32(reader, "LeaveTypeOther");

            if (reader.IsColumnExists("Reason"))
                leaveModel.Reason = SqlHelper.GetNullableString(reader, "Reason");

            if (reader.IsColumnExists("DOANameID"))
                leaveModel.DOANameID = SqlHelper.GetNullableInt32(reader, "DOANameID");

            if (reader.IsColumnExists("DOADepartmentID"))
                leaveModel.DOADepartmentID = SqlHelper.GetNullableInt32(reader, "DOADepartmentID");

            if (reader.IsColumnExists("StartDate"))
                leaveModel.StartDate = SqlHelper.GetDateTime(reader, "StartDate");

            if (reader.IsColumnExists("EndDate"))
                leaveModel.EndDate = SqlHelper.GetDateTime(reader, "EndDate");

            if (reader.IsColumnExists("BalanceLeave"))
                leaveModel.BalanceLeave = SqlHelper.GetNullableString(reader, "BalanceLeave");

            if (reader.IsColumnExists("UpdatedBy"))
                leaveModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                leaveModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Action"))
                leaveModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("ApproverID"))
                leaveModel.ApproverID = SqlHelper.GetNullableInt32(reader, "ApproverID");

            if (reader.IsColumnExists("ApproverDepartmentID"))
                leaveModel.ApproverDepartmentID = SqlHelper.GetNullableInt32(reader, "ApproverDepartmentID");

            if (reader.IsColumnExists("HRManagerUserID"))
                leaveModel.HRManagerUserID = SqlHelper.GetNullableInt32(reader, "HRManagerUserID");

            return leaveModel;
        }

        public static List<LeavePutModel> TranslateAsPutLeaveList(this SqlDataReader reader)
        {
            var leaveList = new List<LeavePutModel>();
            while (reader.Read())
            {
                leaveList.Add(TranslateAsPutLeave(reader, true));
            }

            return leaveList;
        }
    }
}
