using RulersCourt.Models.Protocol.Media;
using RulersCourt.Translators.Protocol.Media;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Protocol.Media
{
    public class MediaExportClient
    {
        public List<MediaReport> GetMediaReportExportList(string connString, MediaExportModel report, string lang)
        {
            List<MediaReport> list = new List<MediaReport>();
            if (!string.IsNullOrEmpty(report.SourceOU) & !string.IsNullOrEmpty(report.SourceOU))
            {
                report.SourceOU = report.SourceOU.Replace("amp;", "&");
            }

            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", 0),
            new SqlParameter("@P_PageSize", 0),
            new SqlParameter("@P_Method", 0),
            new SqlParameter("@P_RequestType", report.RequestType),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_RequestDateFrom", report.ReqDateFrom),
            new SqlParameter("@P_RequestDateTo", report.ReqDateTo),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_SourceOU", report.SourceOU),
            new SqlParameter("@P_SourceName", report.SourceName),
            new SqlParameter("@P_SmartSearch", report.SmartSearch),
            new SqlParameter("@P_Language", lang)
            };

            list = SqlHelper.ExecuteProcedureReturnData<List<MediaReport>>(connString, "MediaReport", r => r.TranslateAsMediaReportList(), param);

            return list;
        }
    }
}
