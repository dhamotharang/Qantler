using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class ReportTranslator
    {
        public static Report TranslateAsReport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var report = new Report();

            if (reader.IsColumnExists("ReferenceNumber"))
                report.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Title"))
                report.Title = SqlHelper.GetNullableString(reader, "Title");

            if (reader.IsColumnExists("Source"))
                report.SourceOU = SqlHelper.GetNullableString(reader, "Source");

            if (reader.IsColumnExists("SourceName"))
                report.SourceOU = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("Destination"))
                report.DestinationOU = SqlHelper.GetNullableString(reader, "Destination");

            if (reader.IsColumnExists("Status"))
                report.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("Private"))
                report.Private = SqlHelper.GetNullableString(reader, "Private");

            if (reader.IsColumnExists("Priority"))
                report.Priority = SqlHelper.GetNullableString(reader, "Priority");

            if (reader.IsColumnExists("Creator"))
                report.Creator = SqlHelper.GetNullableString(reader, "Creator");

            if (reader.IsColumnExists("Destinator"))
                report.DestinationUserName = SqlHelper.GetNullableString(reader, "Destinator");

            if (reader.IsColumnExists("Receiver"))
                report.Approver = SqlHelper.GetNullableString(reader, "Receiver");

            return report;
        }

        public static List<Report> TranslateAsReportList(this SqlDataReader reader)
        {
            var list = new List<Report>();
            while (reader.Read())
            {
                list.Add(TranslateAsReport(reader, true));
            }

            return list;
        }
    }
}
