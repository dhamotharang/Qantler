using RulersCourt.Models.LeaveRequest;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.LeaveRequest
{
    public static class LeaveReportTranslator
    {
        public static LeaveReport TranslateAsLeaveReport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var report = new LeaveReport();

            if (reader.IsColumnExists("LeaveID"))
                report.LeaveID = SqlHelper.GetNullableInt32(reader, "LeaveID");

            if (reader.IsColumnExists("ReferenceNumber"))
                report.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Creator"))
                report.Creator = SqlHelper.GetNullableString(reader, "Creator");

            if (reader.IsColumnExists("RequestType"))
                report.RequestType = SqlHelper.GetNullableString(reader, "RequestType");

            if (reader.IsColumnExists("Status"))
                report.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("RequestDate"))
                report.RequestDate = SqlHelper.GetDateTime(reader, "RequestDate");

            return report;
        }

        public static List<LeaveReport> TranslateAsLeaveReportList(this SqlDataReader reader)
        {
            var list = new List<LeaveReport>();
            while (reader.Read())
            {
                list.Add(TranslateAsLeaveReport(reader, true));
            }

            return list;
        }
    }
}
