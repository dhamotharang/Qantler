using RulersCourt.Models.Certificate;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Certificate
{
    public static class CertificateSaveResponseTranslator
    {
        public static CertificateWorkflowModel TranslateAsCertificateSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var certificateSave = new CertificateWorkflowModel();

            if (reader.IsColumnExists("CertificateId"))
                certificateSave.CertificateId = SqlHelper.GetNullableInt32(reader, "CertificateId");

            if (reader.IsColumnExists("ReferenceNumber"))
                certificateSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                certificateSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                certificateSave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return certificateSave;
        }

        public static CertificateWorkflowModel TranslateAsCertificateSaveResponseList(this SqlDataReader reader)
        {
            var certificateSaveResponse = new CertificateWorkflowModel();
            while (reader.Read())
            {
                certificateSaveResponse = TranslateAsCertificateSaveResponse(reader, true);
            }

            return certificateSaveResponse;
        }
    }
}
