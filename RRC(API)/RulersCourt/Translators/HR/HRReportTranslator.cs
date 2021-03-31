using RulersCourt.Models.HR;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.HR
{
    public static class HRReportTranslator
    {
        public static HRReport TranslateAsHRReport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var hRReport = new HRReport();

            if (reader.IsColumnExists("ReferenceNumber"))
                hRReport.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Creator"))
                hRReport.Creator = SqlHelper.GetNullableString(reader, "Creator");

            if (reader.IsColumnExists("RequestType"))
                hRReport.RequestType = SqlHelper.GetNullableString(reader, "RequestType");

            if (reader.IsColumnExists("Status"))
                hRReport.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("RequestDate"))
                hRReport.RequestDate = SqlHelper.GetNullableString(reader, "RequestDate");

            return hRReport;
        }

        public static List<HRReport> TranslateAsHRReportList(this SqlDataReader reader)
        {
            var hRList = new List<HRReport>();
            while (reader.Read())
            {
                hRList.Add(TranslateAsHRReport(reader, true));
            }

            return hRList;
        }
    }
}