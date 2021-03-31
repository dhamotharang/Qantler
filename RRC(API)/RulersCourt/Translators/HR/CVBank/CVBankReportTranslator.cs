using RulersCourt.Models.HR.CVBank;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.HR.CVBank
{
    public static class CVBankReportTranslator
    {
        public static CVBankReport TranslateAsCVBankReport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var cVBankReport = new CVBankReport();

            if (reader.IsColumnExists("ReferenceNumber"))
                cVBankReport.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("UserName"))
                cVBankReport.UserName = SqlHelper.GetNullableString(reader, "UserName");

            if (reader.IsColumnExists("CandidateName"))
                cVBankReport.CandidateName = SqlHelper.GetNullableString(reader, "CandidateName");

            if (reader.IsColumnExists("EmailId"))
                cVBankReport.EmailId = SqlHelper.GetNullableString(reader, "EmailId");

            if (reader.IsColumnExists("JobTitle"))
                cVBankReport.JobTitle = SqlHelper.GetNullableString(reader, "JobTitle");

            if (reader.IsColumnExists("YearsofExperience"))
                cVBankReport.YearsofExperience = SqlHelper.GetNullableString(reader, "YearsofExperience");

            if (reader.IsColumnExists("Specializations"))
                cVBankReport.Specializations = SqlHelper.GetNullableString(reader, "Specializations");

            if (reader.IsColumnExists("EducationalQualification"))
                cVBankReport.EducationalQualification = SqlHelper.GetNullableString(reader, "EducationalQualification");

            if (reader.IsColumnExists("Country"))
                cVBankReport.Country = SqlHelper.GetNullableString(reader, "Country");

            return cVBankReport;
        }

        public static List<CVBankReport> TranslateAsCVBankReportList(this SqlDataReader reader)
        {
            var contactList = new List<CVBankReport>();
            while (reader.Read())
            {
                contactList.Add(TranslateAsCVBankReport(reader, true));
            }

            return contactList;
        }
    }
}