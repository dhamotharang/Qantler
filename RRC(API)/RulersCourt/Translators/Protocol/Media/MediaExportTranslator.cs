using RulersCourt.Models.Protocol.Media;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.Media
{
    public static class MediaExportTranslator
    {
        public static MediaReport TranslateAsCircularReport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var report = new MediaReport();

            if (reader.IsColumnExists("ReferenceNumber"))
                report.RefID = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("SourceOU"))
                report.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                report.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("Status"))
                report.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("RequestType"))
                report.RequestType = SqlHelper.GetNullableString(reader, "RequestType");

            if (reader.IsColumnExists("RequestDate"))
            {
                DateTime? date = SqlHelper.GetDateTime(reader, "RequestDate");
                report.RequestDate = date == null ? string.Empty : date?.ToString("dd-MM-yyyy").Replace("-", "/");
            }

            return report;
        }

        public static List<MediaReport> TranslateAsMediaReportList(this SqlDataReader reader)
        {
            var list = new List<MediaReport>();
            while (reader.Read())
            {
                list.Add(TranslateAsCircularReport(reader, true));
            }

            return list;
        }
    }
}
