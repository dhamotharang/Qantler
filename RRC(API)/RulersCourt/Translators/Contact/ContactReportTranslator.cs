using RulersCourt.Models.Contact;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Contact
{
    public static class ContactReportTranslator
    {
        public static InternalContactReport TranslateAsContactReport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var internalContactReport = new InternalContactReport();

            if (reader.IsColumnExists("ReferenceNumber"))
                internalContactReport.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Department"))
                internalContactReport.Department = SqlHelper.GetNullableString(reader, "Department");

            if (reader.IsColumnExists("UserName"))
                internalContactReport.UserName = SqlHelper.GetNullableString(reader, "UserName");

            if (reader.IsColumnExists("Designation"))
                internalContactReport.Designation = SqlHelper.GetNullableString(reader, "Designation");

            if (reader.IsColumnExists("EmailId"))
                internalContactReport.EmailId = SqlHelper.GetNullableString(reader, "EmailId");

            if (reader.IsColumnExists("PhoneNumber"))
                internalContactReport.PhoneNumber = SqlHelper.GetNullableString(reader, "PhoneNumber");

            return internalContactReport;
        }

        public static List<InternalContactReport> TranslateAsContactReportList(this SqlDataReader reader)
        {
            var contactList = new List<InternalContactReport>();
            while (reader.Read())
            {
                contactList.Add(TranslateAsContactReport(reader, true));
            }

            return contactList;
        }

        public static ExternalContactReport TranslateAsExternalContactReport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var externalContactReport = new ExternalContactReport();

            if (reader.IsColumnExists("ReferenceNumber"))
                externalContactReport.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("EntityName"))
                externalContactReport.EntityName = SqlHelper.GetNullableString(reader, "EntityName");

            if (reader.IsColumnExists("GoverenmentEntity"))
                externalContactReport.GoverenmentEntity = SqlHelper.GetNullableString(reader, "GoverenmentEntity");

            if (reader.IsColumnExists("ContactName"))
                externalContactReport.ContactName = SqlHelper.GetNullableString(reader, "ContactName");

            if (reader.IsColumnExists("EmailId"))
                externalContactReport.EmailId = SqlHelper.GetNullableString(reader, "EmailId");

            if (reader.IsColumnExists("PhoneNumber"))
                externalContactReport.PhoneNumber = SqlHelper.GetNullableString(reader, "PhoneNumber");

            return externalContactReport;
        }

        public static List<ExternalContactReport> TranslateAsExternalContactReportList(this SqlDataReader reader)
        {
            var contactList = new List<ExternalContactReport>();
            while (reader.Read())
            {
                contactList.Add(TranslateAsExternalContactReport(reader, true));
            }

            return contactList;
        }
    }
}