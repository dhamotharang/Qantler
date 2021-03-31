using RulersCourt.Models.LeaveRequest;
using RulersCourt.Translators.LeaveRequest;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Leave
{
    public class LeaveReportClient
    {
        public LeaveReportListModel GetLeaveReportList(string connString, int pageNumber, int pageSize, LeaveReportRequestModel report)
        {
            LeaveReportListModel list = new LeaveReportListModel();

            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", pageNumber),
            new SqlParameter("@P_PageSize", pageSize),
            new SqlParameter("@P_Method", 0),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_RequestType", report.RequestType),
            new SqlParameter("@P_Creator", report.Creator),
            new SqlParameter("@P_RequestDateForm", report.RequestDateForm),
            new SqlParameter("@P_RequestDateTo", report.RequestDateTo),
            new SqlParameter("@P_SmartSearch", report.SmartSearch)
            };

            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<LeaveReport>>(connString, "LeaveList_Search", r => r.TranslateAsLeaveReportList(), param);

            SqlParameter[] parama = {
            new SqlParameter("@P_PageNumber", pageNumber),
            new SqlParameter("@P_PageSize", pageSize),
            new SqlParameter("@P_Type", "Search"),
            new SqlParameter("@P_Method", 1),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_RequestType", report.RequestType),
            new SqlParameter("@P_Creator", report.Creator),
            new SqlParameter("@P_RequestDateForm", report.RequestDateForm),
            new SqlParameter("@P_RequestDateTo", report.RequestDateTo),
            new SqlParameter("@P_SmartSearch", report.SmartSearch)
            };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "LeaveList", parama);
            return list;
        }

        public List<LeaveReport> GetLeaveReporExporttList(string connString, LeaveReportRequestModel report)
        {
            List<LeaveReport> list = new List<LeaveReport>();

            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", 0),
            new SqlParameter("@P_PageSize", 0),
            new SqlParameter("@P_Method", 2),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_RequestType", report.RequestType),
            new SqlParameter("@P_Creator", report.Creator),
            new SqlParameter("@P_RequestDateForm", report.RequestDateForm),
            new SqlParameter("@P_RequestDateTo", report.RequestDateTo)
            };

            list = SqlHelper.ExecuteProcedureReturnData<List<LeaveReport>>(connString, "LeaveReport", r => r.TranslateAsLeaveReportList(), param);
            return list;
        }
    }
}
