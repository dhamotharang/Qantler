using RulersCourt.Models.Certificate;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Certificate
{
    public static class CertificateHistoryLogTranslator
    {
        public static CertificateHistoryLogModel TranslateAsCertificateHistoryLog(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var certificateHistoryLogModel = new CertificateHistoryLogModel();

            if (reader.IsColumnExists("HistoryID"))
                certificateHistoryLogModel.HistoryID = SqlHelper.GetNullableInt32(reader, "HistoryID");

            if (reader.IsColumnExists("CertificateID"))
                certificateHistoryLogModel.CertificateID = SqlHelper.GetNullableInt32(reader, "CertificateID");

            if (reader.IsColumnExists("Action"))
                certificateHistoryLogModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                certificateHistoryLogModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("ActionBy"))
                certificateHistoryLogModel.ActionBy = SqlHelper.GetNullableString(reader, "ActionBy");

            if (reader.IsColumnExists("ActionDateTime"))
                certificateHistoryLogModel.ActionDateTime = SqlHelper.GetDateTime(reader, "ActionDateTime");

            return certificateHistoryLogModel;
        }

        public static List<CertificateHistoryLogModel> TranslateAsCertificateHistoryLogList(this SqlDataReader reader)
        {
            var certificateHistoryLogList = new List<CertificateHistoryLogModel>();
            while (reader.Read())
            {
                certificateHistoryLogList.Add(TranslateAsCertificateHistoryLog(reader, true));
            }

            return certificateHistoryLogList;
        }
    }
}
