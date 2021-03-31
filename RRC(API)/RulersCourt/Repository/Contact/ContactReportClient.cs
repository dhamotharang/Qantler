using RulersCourt.Models.Contact;
using RulersCourt.Translators.Contact;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Contact
{
    public class ContactReportClient
    {
        public List<InternalContactReport> GetReportExportList(string connString, InternalContactReportRequestModel report, string lang)
        {
            List<InternalContactReport> list = new List<InternalContactReport>();

            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", 1),
            new SqlParameter("@P_PageSize", 10),
            new SqlParameter("@P_Method", 2),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_Department", report.Department),
            new SqlParameter("@P_UserName", report.UserName),
            new SqlParameter("@P_Designation", report.Designation),
            new SqlParameter("@P_EmailId", report.EmailId),
            new SqlParameter("@P_Language", lang),
            new SqlParameter("@P_PhoneNumber", report.PhoneNumber),
            new SqlParameter("@P_SmartSearch", report.SmartSearch)
            };

            list = SqlHelper.ExecuteProcedureReturnData<List<InternalContactReport>>(connString, "InternalContactReport", r => r.TranslateAsContactReportList(), param);
            SqlParameter[] parama = {
            new SqlParameter("@P_PageNumber", 1),
            new SqlParameter("@P_PageSize", 10),
            new SqlParameter("@P_Method", 2),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_Department", report.Department),
            new SqlParameter("@P_UserName", report.UserName),
            new SqlParameter("@P_Designation", report.Designation),
            new SqlParameter("@P_EmailId", report.EmailId),
            new SqlParameter("@P_PhoneNumber", report.PhoneNumber),
            new SqlParameter("@P_Language", lang),
            new SqlParameter("@P_SmartSearch", report.SmartSearch)
            };
            return list;
        }

        public List<ExternalContactReport> GetExternalReportExportList(string connString, ExternalContactReportRequestModel report, string lang)
        {
            List<ExternalContactReport> list = new List<ExternalContactReport>();

            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", 1),
            new SqlParameter("@P_PageSize", 10),
            new SqlParameter("@P_Method", 2),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_EntityName", report.EntityName),
            new SqlParameter("@P_ContactName", report.ContactName),
            new SqlParameter("@P_GoverenmentEntity", report.IsGovernmentEntity),
            new SqlParameter("@P_EmailId", report.EmailId),
            new SqlParameter("@P_Language", lang),
            new SqlParameter("@P_PhoneNumber", report.PhoneNumber),
            new SqlParameter("@P_SmartSearch", report.SmartSearch)
            };

            list = SqlHelper.ExecuteProcedureReturnData<List<ExternalContactReport>>(connString, "ExternalContactReport", r => r.TranslateAsExternalContactReportList(), param);

            SqlParameter[] parama = {
            new SqlParameter("@P_PageNumber", 1),
            new SqlParameter("@P_PageSize", 10),
            new SqlParameter("@P_Method", 2),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_EntityName", report.EntityName),
            new SqlParameter("@P_ContactName", report.ContactName),
            new SqlParameter("@P_GoverenmentEntity", report.IsGovernmentEntity),
            new SqlParameter("@P_EmailId", report.EmailId),
            new SqlParameter("@P_Language", lang),
            new SqlParameter("@P_PhoneNumber", report.PhoneNumber),
            new SqlParameter("@P_SmartSearch", report.SmartSearch)
            };

            return list;
        }
    }
}