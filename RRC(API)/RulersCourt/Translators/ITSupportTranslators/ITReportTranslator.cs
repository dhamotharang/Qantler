using RulersCourt.Models.ITSupport;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.ITSupportTranslators
{
    public static class ITReportTranslator
    {
        public static ITReport TranslateAsITReport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var iTReport = new ITReport();

            if (reader.IsColumnExists("ReferenceNumber"))
                iTReport.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Creator"))
                iTReport.Creator = SqlHelper.GetNullableString(reader, "Creator");

            if (reader.IsColumnExists("RequestType"))
                iTReport.RequestType = SqlHelper.GetNullableString(reader, "RequestType");

            if (reader.IsColumnExists("Status"))
                iTReport.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("RequestDate"))
            {
                string date = SqlHelper.GetNullableString(reader, "RequestDate");
                iTReport.RequestDate = string.IsNullOrEmpty(date) ? string.Empty : DateTime.Parse(date).ToString("dd-MM-yyyy").Replace("-", "/");
            }

            return iTReport;
        }

        public static List<ITReport> TranslateAsITReportList(this SqlDataReader reader)
        {
            var hRList = new List<ITReport>();
            while (reader.Read())
            {
                hRList.Add(TranslateAsITReport(reader, true));
            }

            return hRList;
        }
    }
}