using RulersCourt.Models.CitizenAffair;
using RulersCourt.Translators.CitizenAffair;
using System.Data.SqlClient;

namespace RulersCourt.Repository.CitizenAffair
{
    public class CitizenAffairPreviewClient
    {
        public CitizenAffairPersonalReportPreviewModel CAGetPersonalReportByID(string connString, int citizenAffairID)
        {
            SqlParameter conParam = new SqlParameter("@P_CitizenAffairID", citizenAffairID);

            CitizenAffairPersonalReportPreviewModel citizenAffairPersonalDetails = new CitizenAffairPersonalReportPreviewModel();

            citizenAffairPersonalDetails = SqlHelper.ExecuteProcedureReturnData<CitizenAffairPersonalReportPreviewModel>(connString, "Get_CitizenAffairPersonalReportPreviewByID", r => r.TranslateAsPersonalReportPreviewList(), conParam);

            return citizenAffairPersonalDetails;
        }

        public CitizenAffairFieldVisitPreviewModel CAGetFieldVisitReportByID(string connString, int citizenAffairID)
        {
            SqlParameter conParam = new SqlParameter("@P_CitizenAffairID", citizenAffairID);

            CitizenAffairFieldVisitPreviewModel citizenAffairFieldVisitDetails = new CitizenAffairFieldVisitPreviewModel();

            citizenAffairFieldVisitDetails = SqlHelper.ExecuteProcedureReturnData<CitizenAffairFieldVisitPreviewModel>(connString, "Get_CitizenAffarFieldVisitPreviewByID", r => r.TranslateAsCitizenAffairFieldVisitPreviewList(), conParam);

            return citizenAffairFieldVisitDetails;
        }

        public CitizenAffairPersonalReportPreviewModel CAPatchPersonalReportByID(string connString, int citizenAffairID, string lang)
        {
            SqlParameter[] conParam = {
                new SqlParameter("@P_CitizenAffairID", citizenAffairID),
                new SqlParameter("@P_Language", lang),
            };

            CitizenAffairPersonalReportPreviewModel citizenAffairPersonalDetails = new CitizenAffairPersonalReportPreviewModel();

            citizenAffairPersonalDetails = SqlHelper.ExecuteProcedureReturnData<CitizenAffairPersonalReportPreviewModel>(connString, "Get_CitizenAffairPreview", r => r.TranslateAsPatchPersonalReportPreviewList(), conParam);

            return citizenAffairPersonalDetails;
        }

        public CitizenAffairFieldVisitPreviewModel CAPatchFieldVisitReportByID(string connString, int citizenAffairID, string lang)
        {
            SqlParameter[] conParam = {
                new SqlParameter("@P_CitizenAffairID", citizenAffairID),
                new SqlParameter("@P_Language", lang),
            };

            CitizenAffairFieldVisitPreviewModel citizenAffairFieldVisitDetails = new CitizenAffairFieldVisitPreviewModel();

            citizenAffairFieldVisitDetails = SqlHelper.ExecuteProcedureReturnData<CitizenAffairFieldVisitPreviewModel>(connString, "Get_CitizenAffairPreview", r => r.TranslatePatchCitizenAffairFieldVisitPreviewList(), conParam);

            return citizenAffairFieldVisitDetails;
        }
    }
}