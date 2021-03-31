using RulersCourt.Models.OfficialTaskCompensation.OfficialTask;
using RulersCourt.Translators.OfficialTaskCompensation.OfficialTask;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.OfficialTaskCompensation.OfficialTask
{
    public class OfficialTaskReportClient
    {
        public OfficialTaskReportListModel GetOfficialTaskReportList(string connString, int pageNumber, int pageSize, OfficialTaskReportRequestModel report)
        {
            OfficialTaskReportListModel list = new OfficialTaskReportListModel();

            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", pageNumber),
            new SqlParameter("@P_PageSize", pageSize),
            new SqlParameter("@P_Method", 0),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_SourceOU", report.SourceOU),
            new SqlParameter("@P_SubjectName", report.SubjectName),
            new SqlParameter("@P_OfficialTaskType", report.OfficialTaskType),
            new SqlParameter("@P_AttendedBy", report.AttendedBy),
            new SqlParameter("@P_SmartSearch", report.SmartSearch)
            };

            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<OfficialTaskReportModel>>(connString, "OfficialTaskReport", r => r.TranslateAsOfficialTaskReportList(), param);

            SqlParameter[] parama = {
            new SqlParameter("@P_PageNumber", pageNumber),
            new SqlParameter("@P_PageSize", pageSize),
            new SqlParameter("@P_Type", "Search"),
            new SqlParameter("@P_Method", 1),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_SourceOU", report.SourceOU),
            new SqlParameter("@P_SubjectName", report.SubjectName),
            new SqlParameter("@P_OfficialTaskType", report.OfficialTaskType),
            new SqlParameter("@P_AttendedBy", report.AttendedBy),
            new SqlParameter("@P_SmartSearch", report.SmartSearch)
            };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "OfficialTaskReport", parama);

            return list;
        }

        public List<OfficialTaskReportModel> GetReportExportList(string connString, OfficialTaskReportRequestModel report, string lang)
        {
            List<OfficialTaskReportModel> list = new List<OfficialTaskReportModel>();

            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", 0),
            new SqlParameter("@P_PageSize", 0),
            new SqlParameter("@P_Method", 2),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_SourceOU", report.SourceOU),
            new SqlParameter("@P_SubjectName", report.SubjectName),
            new SqlParameter("@P_OfficialTaskType", report.OfficialTaskType),
            new SqlParameter("@P_Language", lang),
            new SqlParameter("@P_AttendedBy", report.AttendedBy),
            };

            list = SqlHelper.ExecuteProcedureReturnData<List<OfficialTaskReportModel>>(connString, "OfficialTaskReport", r => r.TranslateAsOfficialTaskReportList(), param);

            return list;
        }
    }
}