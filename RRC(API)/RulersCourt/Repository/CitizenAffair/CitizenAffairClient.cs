using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.CitizenAffair;
using RulersCourt.Translators;
using RulersCourt.Translators.CitizenAffair;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.CitizenAffair
{
    public class CitizenAffairClient
    {
        public CitizenAffairListModel GetCitizenAffair(string connString, int pageNumber, int pageSize, string status, string requestType, string userID, string refNo, string personalLocationName, string phoneNo, DateTime? dateFrom, DateTime? dateTo, string smartSearch, string type, string lang, string sourceName)
        {
            CitizenAffairListModel list = new CitizenAffairListModel();
            SqlParameter[] param = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Type", type),
                new SqlParameter("@P_RequestType", requestType),
                new SqlParameter("@P_Method", 0),
                new SqlParameter("@P_RefNo", refNo),
                new SqlParameter("@P_Status", status),
                new SqlParameter("@P_PersonalLocationName", personalLocationName),
                new SqlParameter("@P_PhoneNo", phoneNo),
                new SqlParameter("@P_RequestDateFrom", dateFrom),
                new SqlParameter("@P_RequestDateTo", dateTo),
                new SqlParameter("@P_SourceName", sourceName),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_SmartSearch", smartSearch)
            };
            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<CitizenAffairDashboardListModel>>(connString, "Get_CitizenAffairList", r => r.TranslateAsCitizenAffairDashboardList(), param);
            SqlParameter[] parama = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Type", type),
                new SqlParameter("@P_RequestType", requestType),
                new SqlParameter("@P_Method", 1),
                new SqlParameter("@P_RefNo", refNo),
                new SqlParameter("@P_Status", status),
                new SqlParameter("@P_PersonalLocationName", personalLocationName),
                new SqlParameter("@P_PhoneNo", phoneNo),
                new SqlParameter("@P_RequestDateFrom", dateFrom),
                new SqlParameter("@P_SourceName", sourceName),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_RequestDateTo", dateTo),
                new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Count = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_CitizenAffairList", parama));
            Parallel.Invoke(
                () => list.OrganizationList = GetL_Organisation(connString, lang),
                () => list.LookupsList = GetM_Lookups(connString, lang),
                () => list.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));
            return list;
        }

        public CitizenAffairGetModel GetCitizenAffairByID(string connString, int citizenAffairID, int userID, string lang)
        {
            CitizenAffairGetModel citizenAffairDetails = new CitizenAffairGetModel();
            SqlParameter[] param = {
                new SqlParameter("@P_CitizenAffairID", citizenAffairID)
            };

            if (citizenAffairID != 0)
            {
                citizenAffairDetails = SqlHelper.ExecuteProcedureReturnData<List<CitizenAffairGetModel>>(connString, "Get_CitizenAffairByID", r => r.TranslateAsCitizenAffairList(), param).FirstOrDefault();

                citizenAffairDetails.PersonalReport = new CitizenAffairPersonalReportClient().CAGetPersonalReportByID(connString, citizenAffairID);

                citizenAffairDetails.FieldVisit = new CitizenAffairPersonalReportClient().CAGetFieldVisitReportByID(connString, citizenAffairID);

                citizenAffairDetails.Documents = new CitizenAffairAttachmentClient().GetAttachmentById(connString, citizenAffairDetails.CitizenAffairID, "Document");

                citizenAffairDetails.Photos = new CitizenAffairAttachmentClient().GetAttachmentById(connString, citizenAffairDetails.CitizenAffairID, "Photo");

                citizenAffairDetails.Comments = string.Empty;

                SqlParameter[] getApproverparam = {
                    new SqlParameter("@P_ReferenceNumber", citizenAffairDetails.ReferenceNumber),
                    new SqlParameter("@P_UserID", userID)
                };

                citizenAffairDetails.CurrentApproverID = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_CitizenAffairByApproverId", getApproverparam));

                citizenAffairDetails.HistoryLog = new CitizenAffairHistoryLogClient().CAHistoryLogByMemoID(connString, citizenAffairID, lang);

                userID = citizenAffairDetails.CreatedBy.GetValueOrDefault();
            }

            Parallel.Invoke(
              () => citizenAffairDetails.M_OrganizationList = GetL_Organisation(connString, lang),
              () => citizenAffairDetails.M_LookupsList = GetCitizenM_Lookups(connString, lang),
              () => citizenAffairDetails.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));

            return citizenAffairDetails;
        }

        public List<OrganizationModel> GetL_Organisation(string connString, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Language", lang)
            };
            var e = SqlHelper.ExecuteProcedureReturnData<List<OrganizationModel>>(connString, "Get_Organization", r => r.TranslateAsOrganizationList(), param);
            return e;
        }

        public List<M_LookupsModel> GetM_Lookups(string connString, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Type", "CA"),
                new SqlParameter("@P_Language", lang)
            };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public List<M_LookupsModel> GetCitizenM_Lookups(string connString, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Type", "CitizenAffair"),
                new SqlParameter("@P_Language", lang)
            };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public CitizenAffairWorkflowModel PostCitizenAffair(string connString, CitizenAffairPostModel citizenAffair)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_SourceOU", citizenAffair.SourceOU),
                new SqlParameter("@P_SourceName", citizenAffair.SourceName),
                new SqlParameter("@P_RequestType", citizenAffair.RequestType),
                new SqlParameter("@P_ExternalRequestEmailID", citizenAffair.ExternalRequestEmailID),
                new SqlParameter("@P_NotifyUpon", citizenAffair.NotifyUpon),
                new SqlParameter("@P_ApproverDepartmentID", citizenAffair.ApproverDepartmentId),
                new SqlParameter("@P_InternalRequestorID", citizenAffair.InternalRequestorID),
                new SqlParameter("@P_InternalRequestorDepartmentID", citizenAffair.InternalRequestorDepartmentID),
                new SqlParameter("@P_PersonalReportAge", citizenAffair.PersonalReport.Age),
                new SqlParameter("@P_PersonalReportCity", citizenAffair.PersonalReport.City),
                new SqlParameter("@P_PersonalReportDestination", citizenAffair.PersonalReport.Destination),
                new SqlParameter("@P_PersonalReportEmirates", citizenAffair.PersonalReport.Emirates),
                new SqlParameter("@P_PersonalReportEmiratesID", citizenAffair.PersonalReport.EmiratesID),
                new SqlParameter("@P_PersonalReportEmployer", citizenAffair.PersonalReport.Employer),
                new SqlParameter("@P_PersonalReportFindingNotes", citizenAffair.PersonalReport.FindingNotes),
                new SqlParameter("@P_PersonalReportMaritalStatus", citizenAffair.PersonalReport.MaritalStatus),
                new SqlParameter("@P_PersonalReportMonthlySalary", citizenAffair.PersonalReport.MonthlySalary),
                new SqlParameter("@P_PersonalReportName", citizenAffair.PersonalReport.Name),
                new SqlParameter("@P_PersonalReportNoOfChildrens", citizenAffair.PersonalReport.NoOfChildrens),
                new SqlParameter("@P_PersonalReportPhoneNumber", citizenAffair.PersonalReport.PhoneNumber),
                new SqlParameter("@P_PersonalReportRecommendation", citizenAffair.PersonalReport.Recommendation),
                new SqlParameter("@P_PersonalReportReportObjectives", citizenAffair.PersonalReport.ReportObjectives),
                new SqlParameter("@P_PersonalProfilePhotoID", citizenAffair.PersonalReport.ProfilePhotoID),
                new SqlParameter("@P_PersonalProfilePhotoName", citizenAffair.PersonalReport.ProfilePhotoName),
                new SqlParameter("@P_FieldVisitCity", citizenAffair.FieldVisit.City),
                new SqlParameter("@P_FieldVisitEmiratesID", citizenAffair.FieldVisit.EmiratesID),
                new SqlParameter("@P_FieldVisitDate", citizenAffair.FieldVisit.Date),
                new SqlParameter("@P_FieldVisitFindingNotes", citizenAffair.FieldVisit.FindingNotes),
                new SqlParameter("@P_FieldVisitForWhom", citizenAffair.FieldVisit.ForWhom),
                new SqlParameter("@P_FieldVisitLocation", citizenAffair.FieldVisit.Location),
                new SqlParameter("@P_FieldVisitEmirates", citizenAffair.FieldVisit.Emirates),
                new SqlParameter("@P_FieldVisitCityID", citizenAffair.FieldVisit.CityID),
                new SqlParameter("@P_FieldVisitLocationName", citizenAffair.FieldVisit.LocationName),
                new SqlParameter("@P_FieldVisitName", citizenAffair.FieldVisit.Name),
                new SqlParameter("@P_FieldVisitPhoneNumber", citizenAffair.FieldVisit.PhoneNumber),
                new SqlParameter("@P_FieldVisitRequetsedBy", citizenAffair.FieldVisit.RequetsedBy),
                new SqlParameter("@P_FieldVisitVisitObjective", citizenAffair.FieldVisit.VisitObjective),
                new SqlParameter("@P_Action", citizenAffair.Action),
                new SqlParameter("@P_Comment", citizenAffair.Comments),
                new SqlParameter("@P_CreatedBy", citizenAffair.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", citizenAffair.CreatedDateTime)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<CitizenAffairWorkflowModel>(connString, "Save_CitizenAffair", r => r.TranslateAsCitizenAffairSaveResponseList(), param);

            if (citizenAffair.Documents != null)
            {
                new CitizenAffairAttachmentClient().CitizenAffairPostAttachments(connString, "Document", citizenAffair.Documents, result.CitizenAffairID);
            }

            if (citizenAffair.Photos != null)
            {
                new CitizenAffairAttachmentClient().CitizenAffairPostAttachments(connString, "Photo", citizenAffair.Photos, result.CitizenAffairID);
            }

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "CitizenAffair"),
                new SqlParameter("@P_Action", citizenAffair.Action)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            SqlParameter[] parama = { new SqlParameter("@P_Department", 15), new SqlParameter("@P_GetHead", 1) };
            result.CitizenAffairHeadUserID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), parama);

            result.ApproverID = citizenAffair.ApproverId;
            result.Action = citizenAffair.Action;

            return result;
        }

        public CitizenAffairWorkflowModel PutCitizenAffair(string connString, CitizenAffairPutModel citizenAffair)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_CitizenAffairID", citizenAffair.CitizenAffairID),
                new SqlParameter("@P_SourceOU", citizenAffair.SourceOU),
                new SqlParameter("@P_SourceName", citizenAffair.SourceName),
                new SqlParameter("@P_RequestType", citizenAffair.RequestType),
                new SqlParameter("@P_ExternalRequestEmailID", citizenAffair.ExternalRequestEmailID),
                new SqlParameter("@P_NotifyUpon", citizenAffair.NotifyUpon),
                new SqlParameter("@P_ApproverDepartmentID", citizenAffair.ApproverDepartmentId),
                new SqlParameter("@P_InternalRequestorID", citizenAffair.InternalRequestorID),
                new SqlParameter("@P_InternalRequestorDepartmentID", citizenAffair.InternalRequestorDepartmentID),
                new SqlParameter("@P_PersonalReportAge", citizenAffair.PersonalReport.Age),
                new SqlParameter("@P_PersonalReportCity", citizenAffair.PersonalReport.City),
                new SqlParameter("@P_PersonalReportDestination", citizenAffair.PersonalReport.Destination),
                new SqlParameter("@P_PersonalReportEmirates", citizenAffair.PersonalReport.Emirates),
                new SqlParameter("@P_PersonalReportEmiratesID", citizenAffair.PersonalReport.EmiratesID),
                new SqlParameter("@P_PersonalReportEmployer", citizenAffair.PersonalReport.Employer),
                new SqlParameter("@P_PersonalReportFindingNotes", citizenAffair.PersonalReport.FindingNotes),
                new SqlParameter("@P_PersonalReportMaritalStatus", citizenAffair.PersonalReport.MaritalStatus),
                new SqlParameter("@P_PersonalReportMonthlySalary", citizenAffair.PersonalReport.MonthlySalary),
                new SqlParameter("@P_PersonalReportName", citizenAffair.PersonalReport.Name),
                new SqlParameter("@P_PersonalReportNoOfChildrens", citizenAffair.PersonalReport.NoOfChildrens),
                new SqlParameter("@P_PersonalReportPhoneNumber", citizenAffair.PersonalReport.PhoneNumber),
                new SqlParameter("@P_PersonalReportRecommendation", citizenAffair.PersonalReport.Recommendation),
                new SqlParameter("@P_PersonalReportReportObjectives", citizenAffair.PersonalReport.ReportObjectives),
                new SqlParameter("@P_PersonalProfilePhotoID", citizenAffair.PersonalReport.ProfilePhotoID),
                new SqlParameter("@P_PersonalProfilePhotoName", citizenAffair.PersonalReport.ProfilePhotoName),
                new SqlParameter("@P_FieldVisitCity", citizenAffair.FieldVisit.City),
                new SqlParameter("@P_FieldVisitEmiratesID", citizenAffair.FieldVisit.EmiratesID),
                new SqlParameter("@P_FieldVisitDate", citizenAffair.FieldVisit.Date),
                new SqlParameter("@P_FieldVisitFindingNotes", citizenAffair.FieldVisit.FindingNotes),
                new SqlParameter("@P_FieldVisitForWhom", citizenAffair.FieldVisit.ForWhom),
                new SqlParameter("@P_FieldVisitLocation", citizenAffair.FieldVisit.Location),
                new SqlParameter("@P_FieldVisitEmirates", citizenAffair.FieldVisit.Emirates),
                new SqlParameter("@P_FieldVisitCityID", citizenAffair.FieldVisit.CityID),
                new SqlParameter("@P_FieldVisitLocationName", citizenAffair.FieldVisit.LocationName),
                new SqlParameter("@P_FieldVisitName", citizenAffair.FieldVisit.Name),
                new SqlParameter("@P_FieldVisitPhoneNumber", citizenAffair.FieldVisit.PhoneNumber),
                new SqlParameter("@P_FieldVisitRequetsedBy", citizenAffair.FieldVisit.RequetsedBy),
                new SqlParameter("@P_FieldVisitVisitObjective", citizenAffair.FieldVisit.VisitObjective),
                new SqlParameter("@P_Action", citizenAffair.Action),
                new SqlParameter("@P_Comment", citizenAffair.Comments),
                new SqlParameter("@P_DeleteFlag", citizenAffair.DeleteFlag),
                new SqlParameter("@P_UpdatedBy", citizenAffair.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", citizenAffair.UpdatedDateTime)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<CitizenAffairWorkflowModel>(connString, "Save_CitizenAffair", r => r.TranslateAsCitizenAffairSaveResponseList(), param);

            if (citizenAffair.Documents != null)
            {
                new CitizenAffairAttachmentClient().CitizenAffairPostAttachments(connString, "Document", citizenAffair.Documents, result.CitizenAffairID);
            }

            if (citizenAffair.Photos != null)
            {
                new CitizenAffairAttachmentClient().CitizenAffairPostAttachments(connString, "Photo", citizenAffair.Photos, result.CitizenAffairID);
            }

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "CitizenAffair"),
                new SqlParameter("@P_Action", citizenAffair.Action)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            SqlParameter[] parama = { new SqlParameter("@P_Department", 15), new SqlParameter("@P_GetHead", 1) };
            result.CitizenAffairHeadUserID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), parama);

            result.ApproverID = citizenAffair.ApproverId;
            result.Action = citizenAffair.Action;

            SqlParameter[] param1 = {
                new SqlParameter("@P_CitizenAffairID", result.CitizenAffairID)
            };

            result.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<CitizenAffairGetModel>>(connString, "Get_CitizenAffairByID", r => r.TranslateAsCitizenAffairList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber)
            };

            result.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_CitizenAffairByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return result;
        }

        public string DeleteCitizenAffair(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_CitizenAffairID", id)
             };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_CitizenAffairByID", param);
        }

        public CitizenAffairPutModel GetPatchCitizenAffairByID(string connString, int citizenAffairID)
        {
            CitizenAffairPutModel citizenAffairDetails = new CitizenAffairPutModel();

            SqlParameter[] param = {
                new SqlParameter("@P_CitizenAffairID", citizenAffairID)
            };

            if (citizenAffairID != 0)
            {
                citizenAffairDetails = SqlHelper.ExecuteProcedureReturnData<CitizenAffairPutModel>(connString, "Get_CitizenAffairByID", r => r.TranslateAsCitizenAffairPutList(), param);

                citizenAffairDetails.PersonalReport = new CitizenAffairPersonalReportClient().CAPatchPersonalReportByID(connString, citizenAffairID);

                citizenAffairDetails.FieldVisit = new CitizenAffairPersonalReportClient().CAPatchFieldVisitReportByID(connString, citizenAffairID);
            }

            return citizenAffairDetails;
        }

        public List<CitizenAffairReportModel> GetReportExportList(string connString, CitizenAffairReportRequestModel report, string lang)
        {
            List<CitizenAffairReportModel> list = new List<CitizenAffairReportModel>();

            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", 0),
            new SqlParameter("@P_PageSize", 0),
            new SqlParameter("@P_Method", 2),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_RequestType", report.RequestType),
            new SqlParameter("@P_PersonalLocationName", report.PersonalLocationName),
            new SqlParameter("@P_RequestDateFrom", report.RequestDateRangeFrom),
            new SqlParameter("@P_RequestDateTo",  report.RequestDateRangeTo),
            new SqlParameter("@P_PhoneNumber", report.PhoneNumber),
            new SqlParameter("@P_SourceName", report.Sourcename),
            new SqlParameter("@P_ReferenceNumber", report.ReferenceNumber),
            new SqlParameter("@P_Language", lang),
            new SqlParameter("@P_SmartSearch", report.SmartSearch)
            };

            list = SqlHelper.ExecuteProcedureReturnData<List<CitizenAffairReportModel>>(connString, "CitizenAffairReport", r => r.TranslateAsCAReportList(), param);

            return list;
        }

        public CitizenHomeModel GetAllModulesPendingTasksCount(string connString, int userID, string lang)
        {
            CitizenHomeModel list = new CitizenHomeModel();
            SqlParameter[] myRequestparam = {
                       new SqlParameter("@P_UserID", userID),
                       new SqlParameter("@P_Language", lang),
                        };
            list = SqlHelper.ExecuteProcedureReturnData<CitizenHomeModel>(connString, "CitizenaffairDashboardCount", r => r.TranslateasCitizenDashboardcount(), myRequestparam);
            return list;
        }

        public CitizenAffairPreview_model GetCitizenAffairPreview(string connString, int citizenAffairID, int userID, string lang)
        {
            CitizenAffairPreview_model citizenAffairDetails = new CitizenAffairPreview_model();
            SqlParameter[] param = {
                new SqlParameter("@P_CitizenAffairID", citizenAffairID)
            };

            if (citizenAffairID != 0)
            {
                citizenAffairDetails = SqlHelper.ExecuteProcedureReturnData<List<CitizenAffairPreview_model>>(connString, "Get_CitizenAffairPreview", r => r.TranslateAsCitizenAffairPreviewList(), param).FirstOrDefault();

                citizenAffairDetails.PersonalReport = new CitizenAffairPreviewClient().CAGetPersonalReportByID(connString, citizenAffairID);

                citizenAffairDetails.FieldVisit = new CitizenAffairPreviewClient().CAGetFieldVisitReportByID(connString, citizenAffairID);

                citizenAffairDetails.Documents = new CitizenAffairAttachmentClient().GetAttachmentById(connString, citizenAffairDetails.CitizenAffairID, "Document");

                citizenAffairDetails.Photos = new CitizenAffairAttachmentClient().GetAttachmentById(connString, citizenAffairDetails.CitizenAffairID, "Photo");

                SqlParameter[] getApproverparam = {
                    new SqlParameter("@P_ReferenceNumber", citizenAffairDetails.ReferenceNumber)
                };

                citizenAffairDetails.CurrentApproverID = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_CitizenAffairByApproverId", getApproverparam));

                citizenAffairDetails.HistoryLog = new CitizenAffairHistoryLogClient().CAHistoryLogByMemoID(connString, citizenAffairID, lang);

                userID = citizenAffairDetails.CreatedBy.GetValueOrDefault();
            }

            Parallel.Invoke(
              () => citizenAffairDetails.M_OrganizationList = GetL_Organisation(connString, lang),
              () => citizenAffairDetails.M_LookupsList = GetCitizenM_Lookups(connString, lang));

            return citizenAffairDetails;
        }

        internal CitizenAffairWorkflowModel PatchCitizenAffair(string connString, int id, JsonPatchDocument<CitizenAffairPutModel> value)
        {
            var result = GetPatchCitizenAffairByID(connString, id);

            value.ApplyTo(result);
            var res = PutCitizenAffair(connString, result);
            if (result.Action == "Escalate")
            {
                res.ApproverID = result.ApproverId;
            }

            res.Action = result.Action;
            res.NotifyUpon = result.NotifyUpon;
            SqlParameter[] parama = { new SqlParameter("@P_Department", 15), new SqlParameter("@P_GetHead", 1) };
            res.CitizenAffairHeadUserID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), parama);
            SqlParameter[] param1 = {
                new SqlParameter("@P_CitizenAffairID", result.CitizenAffairID)
            };

            res.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<CitizenAffairGetModel>>(connString, "Get_CitizenAffairByID", r => r.TranslateAsCitizenAffairList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", res.ReferenceNumber)
            };

            res.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_CitizenAffairByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return res;
        }
    }
}
