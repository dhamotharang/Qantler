using RulersCourt.Models.HR;
using RulersCourt.Translators.HR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.HR
{
    public class HRReportClient
    {
        public HRReportListModel GetHRReportList(string connString, int pageNumber, int pageSize, HRReportRequestModel report, string lang)
        {
            HRReportListModel list = new HRReportListModel();

            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", pageNumber),
            new SqlParameter("@P_PageSize", pageSize),
            new SqlParameter("@P_Method", 0),
            new SqlParameter("@P_UserName", report.Username),
            new SqlParameter("@P_Creator", report.Creator),
            new SqlParameter("@P_Language", lang),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_RequestType", report.RequestType),
            new SqlParameter("@P_RequestDateFrom", report.ReqDateFrom),
            new SqlParameter("@P_RequestDateTo", report.ReqDateTo),
            new SqlParameter("@P_SmartSearch", report.SmartSearch)
            };

            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<HRReport>>(connString, "HRReport", r => r.TranslateAsHRReportList(), param);
            SqlParameter[] parama = {
            new SqlParameter("@P_PageNumber", pageNumber),
            new SqlParameter("@P_PageSize", pageSize),
            new SqlParameter("@P_Method", 2),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_UserName", report.Username),
            new SqlParameter("@P_Creator", report.Creator),
            new SqlParameter("@P_Language", lang),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_RequestType", report.RequestType),
            new SqlParameter("@P_RequestDateFrom", report.ReqDateFrom),
            new SqlParameter("@P_RequestDateTo", report.ReqDateTo),
            new SqlParameter("@P_SmartSearch", report.SmartSearch)
            };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "HRReport", parama);
            return list;
        }

        public List<HRReport> GetHRReportExportList(string connString, HRReportRequestModel report, string lang)
        {
            List<HRReport> list = new List<HRReport>();

            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", 1),
            new SqlParameter("@P_PageSize", 10),
            new SqlParameter("@P_Method", 0),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_UserName", report.Username),
            new SqlParameter("@P_Creator", report.Creator),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_RequestType", report.RequestType),
            new SqlParameter("@P_RequestDateFrom", report.ReqDateFrom),
            new SqlParameter("@P_RequestDateTo", report.ReqDateTo),
            new SqlParameter("@P_SmartSearch", report.SmartSearch),
            new SqlParameter("@P_Language", lang)
            };

            list = SqlHelper.ExecuteProcedureReturnData<List<HRReport>>(connString, "HRReport", r => r.TranslateAsHRReportList(), param);
            return list;
        }
    }
}