using RulersCourt.Models;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class ReportClient
    {
        public ReportListModel GetReportList(string connString, int pageNumber, int pageSize, ReportRequestModel report)
        {
            ReportListModel list = new ReportListModel();

            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", pageNumber),
            new SqlParameter("@P_PageSize", pageSize),
            new SqlParameter("@P_Method", 0),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_SourceOU", report.SourceOU),
            new SqlParameter("@P_DestinationOU", report.DestinationOU),
            new SqlParameter("@P_DateRangeForm", report.DateRangeForm),
            new SqlParameter("@P_DateRangeTo", report.DateRangeTo),
            new SqlParameter("@P_Private", report.Private),
            new SqlParameter("@P_Priority", report.Priority),
            new SqlParameter("@P_SmartSearch", report.SmartSearch)
            };

            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<Report>>(connString, "MemoList_Search", r => r.TranslateAsReportList(), param);

            SqlParameter[] parama = {
            new SqlParameter("@P_PageNumber", pageNumber),
            new SqlParameter("@P_PageSize", pageSize),
            new SqlParameter("@P_Type", "Search"),
            new SqlParameter("@P_Method", 1),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_SourceOU", report.SourceOU),
            new SqlParameter("@P_DestinationOU", report.DestinationOU),
            new SqlParameter("@P_DateRangeForm", report.DateRangeForm),
            new SqlParameter("@P_DateRangeTo", report.DateRangeTo),
            new SqlParameter("@P_Private", report.Private),
            new SqlParameter("@P_Priority", report.Priority),
            new SqlParameter("@P_SmartSearch", report.SmartSearch)
            };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "MemoList_Search", parama);

            return list;
        }

        public List<Report> GetReporExporttList(string connString, ReportRequestModel report, string lang)
        {
            List<Report> list = new List<Report>();
            if (!string.IsNullOrEmpty(report.SourceOU) & !string.IsNullOrEmpty(report.SourceOU))
            {
                report.SourceOU = report.SourceOU.Replace("amp;", "&");
            }

            if (!string.IsNullOrEmpty(report.DestinationOU) & !string.IsNullOrEmpty(report.DestinationOU))
            {
                report.DestinationOU = report.DestinationOU.Replace("amp;", "&");
            }

            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", 0),
            new SqlParameter("@P_PageSize", 0),
            new SqlParameter("@P_Method", 2),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_SourceOU", report.SourceOU),
            new SqlParameter("@P_DestinationOU", report.DestinationOU),
            new SqlParameter("@P_DateFrom", report.DateRangeForm),
            new SqlParameter("@P_DateTo", report.DateRangeTo),
            new SqlParameter("@P_SmartSearch", report.SmartSearch),
            new SqlParameter("@P_Private", report.Private),
            new SqlParameter("@P_Priority", report.Priority),
            new SqlParameter("@P_Language", lang)
            };

            list = SqlHelper.ExecuteProcedureReturnData<List<Report>>(connString, "MemoReport", r => r.TranslateAsReportList(), param);

            return list;
        }
    }
}
