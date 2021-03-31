using RulersCourt.Models.Certificate;
using RulersCourt.Translators.Certificate;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Certificate
{
    public class CertificateHistoryLogClient
    {
        public List<CertificateHistoryLogModel> CertificateHistoryLogByCertificateID(string connString, int certificateID, string lang)
        {
            SqlParameter[] contractIDParam = {
                new SqlParameter("@P_CertificateID", certificateID),
                new SqlParameter("@P_Language", lang),
            };

            List<CertificateHistoryLogModel> certificateDetails = new List<CertificateHistoryLogModel>();

            certificateDetails = SqlHelper.ExecuteProcedureReturnData<List<CertificateHistoryLogModel>>(connString, "Get_CertificateHistoryByID", r => r.TranslateAsCertificateHistoryLogList(), contractIDParam);

            return certificateDetails;
        }
    }
}
