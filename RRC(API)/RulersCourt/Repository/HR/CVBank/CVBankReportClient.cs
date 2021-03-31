using RulersCourt.Models.HR.CVBank;
using RulersCourt.Translators.Contact;
using RulersCourt.Translators.HR.CVBank;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.HR.CVBank
{
    public class CVBankReportClient
    {
        public CVBankReportListModel GetCVBankReportList(string connString, int pageNumber, int pageSize, CVBankReportRequestModel report)
        {
            CVBankReportListModel list = new CVBankReportListModel();

            return list;
        }

        public List<CVBankReport> GetReportExportList(string connString, CVBankReportRequestModel report, string lang)
        {
            List<CVBankReport> list = new List<CVBankReport>();

            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", 1),
            new SqlParameter("@P_PageSize", 10),
            new SqlParameter("@P_Method", 2),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_CandidateName", report.CandidateName),
            new SqlParameter("@P_CountryofResidence", report.CountryofResidence),
            new SqlParameter("@P_DateFrom", report.DateFrom),
            new SqlParameter("@P_DateTo", report.DateTo),
            new SqlParameter("@P_EducationalQualification", report.EducationalQualification),
            new SqlParameter("@P_Specializations", report.Specializations),
            new SqlParameter("@P_YearsofExperience", report.YearsofExperience),
            new SqlParameter("@P_Language", lang),
            new SqlParameter("@P_SmartSearch", report.SmartSearch)
            };

            list = SqlHelper.ExecuteProcedureReturnData<List<CVBankReport>>(connString, "CVBankReport", r => r.TranslateAsCVBankReportList(), param);
            SqlParameter[] parama = {
            new SqlParameter("@P_PageNumber", 1),
            new SqlParameter("@P_PageSize", 10),
            new SqlParameter("@P_Method", 2),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_CandidateName", report.CandidateName),
            new SqlParameter("@P_CountryofResidence", report.CountryofResidence),
            new SqlParameter("@P_DateFrom", report.DateFrom),
            new SqlParameter("@P_DateTo", report.DateTo),
            new SqlParameter("@P_Language", lang),
            new SqlParameter("@P_EducationalQualification", report.EducationalQualification),
            new SqlParameter("@P_Specializations", report.Specializations),
            new SqlParameter("@P_YearsofExperience", report.YearsofExperience),
            new SqlParameter("@P_SmartSearch", report.SmartSearch)
            };
            return list;
        }
    }
}
