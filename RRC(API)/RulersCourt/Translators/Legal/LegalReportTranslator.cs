using RulersCourt.Models.Legal;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Legal
{
    public static class LegalReportTranslator
    {
        public static LegalReport TranslateAsLegalReport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var legalReport = new LegalReport();

            if (reader.IsColumnExists("ReferenceNumber"))
                legalReport.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Status"))
                legalReport.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("SourceOU"))
                legalReport.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("Subject"))
                legalReport.Subject = SqlHelper.GetNullableString(reader, "Subject");

            if (reader.IsColumnExists("RequestDate"))
            {
                string date = SqlHelper.GetNullableString(reader, "RequestDate");
                legalReport.RequestDate = string.IsNullOrEmpty(date) ? string.Empty : DateTime.Parse(date).ToString("dd-MM-yyyy").Replace("-", "/");
            }

            if (reader.IsColumnExists("AttendedBy"))
                legalReport.AttendedBy = SqlHelper.GetNullableString(reader, "AttendedBy");

            if (reader.IsColumnExists("CreatedBy"))
                legalReport.CreatedBy = SqlHelper.GetNullableString(reader, "CreatedBy");

            if (reader.IsColumnExists("ApprovedBy"))
                legalReport.ApprovedBy = SqlHelper.GetNullableString(reader, "ApprovedBy");

            return legalReport;
        }

        public static List<LegalReport> TranslateAsLegalReportList(this SqlDataReader reader)
        {
            var legalList = new List<LegalReport>();
            while (reader.Read())
            {
                legalList.Add(TranslateAsLegalReport(reader, true));
            }

            return legalList;
        }
    }
}