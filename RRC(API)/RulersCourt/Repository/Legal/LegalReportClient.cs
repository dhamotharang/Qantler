using RulersCourt.Models.Legal;
using RulersCourt.Translators.Legal;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Legal
{
    public class LegalReportClient
    {
        public LegalReportListModel GetLegalReportList(string connString, int pageNumber, int pageSize, LegalReportRequestModel report)
        {
            LegalReportListModel list = new LegalReportListModel();

            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", pageNumber),
            new SqlParameter("@P_PageSize", pageSize),
            new SqlParameter("@P_Method", 0),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_SourceOU", report.SourceOU),
            new SqlParameter("@P_Subject", report.Subject),
            new SqlParameter("@P_RequestDateFrom", report.RequestDateFrom),
            new SqlParameter("@P_RequestDateTo", report.RequestDateTo),
            new SqlParameter("@P_Label", report.Label),
            new SqlParameter("@P_AttendedBy", report.AttendedBy),
            new SqlParameter("@P_SmartSearch", report.SmartSearch)
            };

            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<LegalReport>>(connString, "LegalReport", r => r.TranslateAsLegalReportList(), param);

            SqlParameter[] parama = {
            new SqlParameter("@P_PageNumber", pageNumber),
            new SqlParameter("@P_PageSize", pageSize),
            new SqlParameter("@P_Type", "Search"),
            new SqlParameter("@P_Method", 1),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_SourceOU", report.SourceOU),
            new SqlParameter("@P_Subject", report.Subject),
            new SqlParameter("@P_RequestDateFrom", report.RequestDateFrom),
            new SqlParameter("@P_RequestDateTo", report.RequestDateTo),
            new SqlParameter("@P_Label", report.Label),
            new SqlParameter("@P_AttendedBy", report.AttendedBy),
            new SqlParameter("@P_SmartSearch", report.SmartSearch)
            };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "LegalReport", parama);
            return list;
        }

        public List<LegalReport> GetReportExporttList(string connString, LegalReportRequestModel report, string lang)
        {
            List<LegalReport> list = new List<LegalReport>();

            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", 0),
            new SqlParameter("@P_PageSize", 0),
            new SqlParameter("@P_Method", 2),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_SourceOU", report.SourceOU),
            new SqlParameter("@P_Subject", report.Subject),
            new SqlParameter("@P_RequestDateFrom", report.RequestDateFrom),
            new SqlParameter("@P_RequestDateTo", report.RequestDateTo),
            new SqlParameter("@P_Label", report.Label),
            new SqlParameter("@P_AttendedBy", report.AttendedBy),
            new SqlParameter("@P_CreatedBy", report.CreatedBy),
            new SqlParameter("@P_Language", lang),
            new SqlParameter("@P_SmartSearch", report.SmartSearch),
            new SqlParameter("@P_ApprovedBy", report.ApprovedBy)
            };

            list = SqlHelper.ExecuteProcedureReturnData<List<LegalReport>>(connString, "LegalReport", r => r.TranslateAsLegalReportList(), param);

            return list;
        }
    }
}