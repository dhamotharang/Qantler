using RulersCourt.Models.Certificate;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Certificate
{
    public static class CertificateTranslator
    {
        public static CertificateGetModel TranslateAsCertificate(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var certificateModel = new CertificateGetModel();

            if (reader.IsColumnExists("CertificateID"))
                certificateModel.CertificateID = SqlHelper.GetNullableInt32(reader, "CertificateID");

            if (reader.IsColumnExists("ReferenceNumber"))
                certificateModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("SourceOU"))
                certificateModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                certificateModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("Attention"))
                certificateModel.Attention = SqlHelper.GetNullableString(reader, "Attention");

            if (reader.IsColumnExists("To"))
                certificateModel.To = SqlHelper.GetNullableString(reader, "To");

            if (reader.IsColumnExists("SalaryCertificateClassification"))
                certificateModel.SalaryCertificateClassification = SqlHelper.GetNullableString(reader, "SalaryCertificateClassification");

            if (reader.IsColumnExists("Reason"))
                certificateModel.Reason = SqlHelper.GetNullableString(reader, "Reason");

            if (reader.IsColumnExists("CreatedBy"))
                certificateModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                certificateModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                certificateModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                certificateModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Status"))
                certificateModel.Status = SqlHelper.GetNullableInt32(reader, "Status");

            if (reader.IsColumnExists("CertificateType"))
                certificateModel.CertificateType = SqlHelper.GetNullableString(reader, "CertificateType");

            return certificateModel;
        }

        public static List<CertificateGetModel> TranslateAsCertificateList(this SqlDataReader reader)
        {
            var certificateList = new List<CertificateGetModel>();
            while (reader.Read())
            {
                certificateList.Add(TranslateAsCertificate(reader, true));
            }

            return certificateList;
        }

        public static CertificatePutModel TranslateAsPutCertificate(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var certificateModel = new CertificatePutModel();

            if (reader.IsColumnExists("CertificateID"))
                certificateModel.CertificateID = SqlHelper.GetNullableInt32(reader, "CertificateID");

            if (reader.IsColumnExists("SourceOU"))
                certificateModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                certificateModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("Attention"))
                certificateModel.Attention = SqlHelper.GetNullableString(reader, "Attention");

            if (reader.IsColumnExists("To"))
                certificateModel.To = SqlHelper.GetNullableString(reader, "To");

            if (reader.IsColumnExists("SalaryCertificateClassification"))
                certificateModel.SalaryCertificateClassification = SqlHelper.GetNullableString(reader, "SalaryCertificateClassification");

            if (reader.IsColumnExists("Reason"))
                certificateModel.Reason = SqlHelper.GetNullableString(reader, "Reason");

            if (reader.IsColumnExists("UpdatedBy"))
                certificateModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                certificateModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Action"))
                certificateModel.Action = SqlHelper.GetNullableString(reader, "Action");

            return certificateModel;
        }

        public static List<CertificatePutModel> TranslateAsPutCertificateList(this SqlDataReader reader)
        {
            var certificateList = new List<CertificatePutModel>();
            while (reader.Read())
            {
                certificateList.Add(TranslateAsPutCertificate(reader, true));
            }

            return certificateList;
        }
    }
}
