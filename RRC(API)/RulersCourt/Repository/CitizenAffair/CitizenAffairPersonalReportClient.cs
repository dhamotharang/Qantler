using RulersCourt.Models.CitizenAffair;
using RulersCourt.Translators.CitizenAffair;
using System.Data.SqlClient;

namespace RulersCourt.Repository.CitizenAffair
{
    public class CitizenAffairPersonalReportClient
    {
        public CitizenAffairPersonalReportModel CAGetPersonalReportByID(string connString, int citizenAffairID)
        {
            SqlParameter conParam = new SqlParameter("@P_CitizenAffairID", citizenAffairID);

            CitizenAffairPersonalReportModel citizenAffairPersonalDetails = new CitizenAffairPersonalReportModel();

            citizenAffairPersonalDetails = SqlHelper.ExecuteProcedureReturnData<CitizenAffairPersonalReportModel>(connString, "Get_CitizenAffairPersonalReportByID", r => r.TranslateAsPersonalReportList(), conParam);

            return citizenAffairPersonalDetails;
        }

        public CitizenAffairFieldVisitModel CAGetFieldVisitReportByID(string connString, int citizenAffairID)
        {
            SqlParameter conParam = new SqlParameter("@P_CitizenAffairID", citizenAffairID);

            CitizenAffairFieldVisitModel citizenAffairFieldVisitDetails = new CitizenAffairFieldVisitModel();

            citizenAffairFieldVisitDetails = SqlHelper.ExecuteProcedureReturnData<CitizenAffairFieldVisitModel>(connString, "Get_CitizenAffarFieldVisitByID", r => r.TranslateAsCitizenAffairFieldVisitList(), conParam);

            return citizenAffairFieldVisitDetails;
        }

        public CitizenAffairPersonalReportModel CAPatchPersonalReportByID(string connString, int citizenAffairID)
        {
            SqlParameter conParam = new SqlParameter("@P_CitizenAffairID", citizenAffairID);

            CitizenAffairPersonalReportModel citizenAffairPersonalDetails = new CitizenAffairPersonalReportModel();

            citizenAffairPersonalDetails = SqlHelper.ExecuteProcedureReturnData<CitizenAffairPersonalReportModel>(connString, "Get_CitizenAffairByID", r => r.TranslateAsPatchPersonalReportList(), conParam);

            return citizenAffairPersonalDetails;
        }

        public CitizenAffairFieldVisitModel CAPatchFieldVisitReportByID(string connString, int citizenAffairID)
        {
            SqlParameter conParam = new SqlParameter("@P_CitizenAffairID", citizenAffairID);

            CitizenAffairFieldVisitModel citizenAffairFieldVisitDetails = new CitizenAffairFieldVisitModel();

            citizenAffairFieldVisitDetails = SqlHelper.ExecuteProcedureReturnData<CitizenAffairFieldVisitModel>(connString, "Get_CitizenAffairByID", r => r.TranslatePatchCitizenAffairFieldVisitList(), conParam);

            return citizenAffairFieldVisitDetails;
        }
    }
}
