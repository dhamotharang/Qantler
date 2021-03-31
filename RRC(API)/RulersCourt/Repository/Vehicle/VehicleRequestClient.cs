using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.Vehicle.TripCoPassengers;
using RulersCourt.Models.Vehicle.TripVehicleIssues;
using RulersCourt.Models.Vehicle.VehicleRequest;
using RulersCourt.Models.Vehicle.Vehicles;
using RulersCourt.Translators;
using RulersCourt.Translators.Vehicle;
using RulersCourt.Translators.Vehicle.DriverVehicles;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Vehicle
{
    public class VehicleRequestClient
    {
        public VehicleRequestGetModel GetVehicleRequestByID(string connString, int vehicleReqID, int userID, string lang)
        {
            VehicleRequestGetModel vehicleRequest = new VehicleRequestGetModel();

            SqlParameter[] getparam = {
                new SqlParameter("@P_VehicleReqID", vehicleReqID)
            };
            if (vehicleReqID != 0)
            {
                vehicleRequest = SqlHelper.ExecuteProcedureReturnData<List<VehicleRequestGetModel>>(connString, "Get_VehicleRequestByID", r => r.TranslateAsVehicleRequestList(), getparam).FirstOrDefault();
            }

            Parallel.Invoke(
              () => vehicleRequest.M_OrganizationList = GetM_Organisation(connString, lang),
              () => vehicleRequest.M_LookupsList = GetM_Lookups(connString, lang),
              () => vehicleRequest.M_CoPassengarList = GetM_CoPassangerOrg(connString, lang));

            SqlParameter[] getIssuesparam = {
                    new SqlParameter("@P_VehicleReqID", vehicleReqID),
                    new SqlParameter("@P_Type", 1),
                    new SqlParameter("@P_VehicleID", vehicleRequest.VehicleID)
            };

            vehicleRequest.TripVehicleIssues = SqlHelper.ExecuteProcedureReturnData<List<TripVehicleIssuesPostModel>>(connString, "Get_TripVehicleIssues", r => r.TranslateAsVehicleIssuesList(), getIssuesparam);

            SqlParameter[] getPassengersparam = {
                    new SqlParameter("@P_VehicleReqID", vehicleReqID),
                    new SqlParameter("@P_Type", 1)
            };

            vehicleRequest.TripCoPassengers = SqlHelper.ExecuteProcedureReturnData<List<TripCoPassengersModel>>(connString, "Get_TripCoPassengers", r => r.TranslateAsCoPassengersUserList(), getPassengersparam);

            SqlParameter[] getTripsparam = {
                    new SqlParameter("@P_VehicleID", vehicleReqID),
                    new SqlParameter("@P_Language", lang)
            };

            vehicleRequest.M_TripOnSameDay = SqlHelper.ExecuteProcedureReturnData<List<TripSameDayPeriodModel>>(connString, "Get_VehicleTripOnSameDayPeriod", r => r.TranslateAsTripList(), getTripsparam);

            return vehicleRequest;
        }

        public List<OrganizationModel> GetM_Organisation(string connString, string lang)
        {
            SqlParameter[] orgParam = {
                new SqlParameter("@P_Language", lang)
            };
            var e = SqlHelper.ExecuteProcedureReturnData<List<OrganizationModel>>(connString, "Get_Organization", r => r.TranslateAsOrganizationList(), orgParam);
            return e;
        }

        public List<OrganizationModel> GetM_CoPassangerOrg(string connString, string lang)
        {
            List<OrganizationModel> res = new List<OrganizationModel>();
            OrganizationModel org = new OrganizationModel();
            if (lang == "EN")
            {
                org.OrganizationUnits = "Others";
            }
            else
            {
                org.OrganizationUnits = new ArabicConstantModel().GetOther;
            }

            org.OrganizationID = 0;
            res.Add(org);
            SqlParameter[] orgParam = {
                new SqlParameter("@P_Language", lang) };
            List<OrganizationModel> e = SqlHelper.ExecuteProcedureReturnData<List<OrganizationModel>>(connString, "Get_Organization", r => r.TranslateAsOrganizationList(), orgParam);
            foreach (OrganizationModel m in e)
            {
                res.Add(m);
            }

            return res;
        }

        public List<M_LookupsModel> GetM_Lookups(string connString, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Type", "Vehicle"),
                new SqlParameter("@P_Language", lang)
            };

            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public VehicleRequestModel PostVehicleRequest(string connString, VehicleRequestPostModel vehicleRequest)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_RequestType", vehicleRequest.RequestType),
                new SqlParameter("@P_Requestor", vehicleRequest.Requestor),
                new SqlParameter("@P_RequestDateTime", vehicleRequest.RequestDateTime),
                new SqlParameter("@P_DriverID", vehicleRequest.DriverID),
                new SqlParameter("@P_TobeDrivenbyDepartmentID", vehicleRequest.TobeDrivenbyDepartmentID),
                new SqlParameter("@P_TobeDrivenbyDriverID", vehicleRequest.TobeDrivenbyDriverID),
                new SqlParameter("@P_TripTypeID", vehicleRequest.TripTypeID),
                new SqlParameter("@P_TripTypeOthers", vehicleRequest.TripTypeOthers),
                new SqlParameter("@P_Emirates", vehicleRequest.Emirates),
                new SqlParameter("@P_City", vehicleRequest.City),
                new SqlParameter("@P_Destination", vehicleRequest.Destination),
                new SqlParameter("@P_DestinationOthers", vehicleRequest.DestinationOthers),
                new SqlParameter("@P_TripPeriodFrom", vehicleRequest.TripPeriodFrom),
                new SqlParameter("@P_TripPeriodTo", vehicleRequest.TripPeriodTo),
                new SqlParameter("@P_VehicleModelID", vehicleRequest.VehicleModelID),
                new SqlParameter("@P_ApproverDepartment", vehicleRequest.ApproverDepartment),
                new SqlParameter("@P_ApproverName", vehicleRequest.ApproverName),
                new SqlParameter("@P_ReleaseDateTime", vehicleRequest.ReleaseDateTime),
                new SqlParameter("@P_LastMileageReading", vehicleRequest.LastMileageReading),
                new SqlParameter("@P_ReleaseLocationID", vehicleRequest.ReleaseLocationID),
                new SqlParameter("@P_ReturnDateTime", vehicleRequest.ReturnDateTime),
                new SqlParameter("@P_CurrentMileageReading", vehicleRequest.CurrentMileageReading),
                new SqlParameter("@P_HavePersonalBelongings", vehicleRequest.HavePersonalBelongings),
                new SqlParameter("@P_PersonalBelongingsText", vehicleRequest.PersonalBelongingsText),
                new SqlParameter("@P_VehicleID", vehicleRequest.VehicleID),
                new SqlParameter("@P_Notes", vehicleRequest.Notes),
                new SqlParameter("@P_DeleteFlag", vehicleRequest.DeleteFlag),
                new SqlParameter("@P_CreatedBy", vehicleRequest.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", vehicleRequest.CreatedDateTime),
                new SqlParameter("@P_Action", vehicleRequest.Action)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<VehicleRequestModel>(connString, "Save_VehicleRequest", r => r.TranslateAsVehicleWorkflowSaveResponseList(), param);

            if (vehicleRequest.TripCoPassengers != null)
            {
                new TripCoPassengersClient().SaveCoPassengers(connString, vehicleRequest.TripCoPassengers, result.VehicleReqID);
            }

            if (vehicleRequest.TripVehicleIssues != null)
            {
                if (vehicleRequest.TripVehicleIssues.Count != 0)
                    new TripVehicleIssuesClient().SaveVehicleIssues(connString, vehicleRequest.TripVehicleIssues, result.VehicleReqID);
            }

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Vehicle"),
                new SqlParameter("@P_Action", vehicleRequest.Action)
           };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));
            result.FromID = vehicleRequest.CreatedBy ?? default(int);
            result.Action = vehicleRequest.Action;
            result.RequestorID = vehicleRequest.Requestor;
            result.ApproverID = vehicleRequest.ApproverName;
            result.RequestorType = vehicleRequest.RequestType;
            return result;
        }

        public VehicleRequestModel PutVehicleRequest(string connString, VehicleRequestPutModel vehicleRequest, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_VehicleReqID", vehicleRequest.VehicleReqID),
                new SqlParameter("@P_RequestType", vehicleRequest.RequestType),
                new SqlParameter("@P_Requestor", vehicleRequest.Requestor),
                new SqlParameter("@P_RequestDateTime", vehicleRequest.RequestDateTime),
                new SqlParameter("@P_DriverID", vehicleRequest.DriverID),
                new SqlParameter("@P_TobeDrivenbyDepartmentID", vehicleRequest.TobeDrivenbyDepartmentID),
                new SqlParameter("@P_TobeDrivenbyDriverID", vehicleRequest.TobeDrivenbyDriverID),
                new SqlParameter("@P_TripTypeID", vehicleRequest.TripTypeID),
                new SqlParameter("@P_TripTypeOthers", vehicleRequest.TripTypeOthers),
                new SqlParameter("@P_Emirates", vehicleRequest.Emirates),
                new SqlParameter("@P_City", vehicleRequest.City),
                new SqlParameter("@P_Destination", vehicleRequest.Destination),
                new SqlParameter("@P_DestinationOthers", vehicleRequest.DestinationOthers),
                new SqlParameter("@P_TripPeriodFrom", vehicleRequest.TripPeriodFrom),
                new SqlParameter("@P_TripPeriodTo", vehicleRequest.TripPeriodTo),
                new SqlParameter("@P_VehicleModelID", vehicleRequest.VehicleModelID),
                new SqlParameter("@P_ApproverDepartment", vehicleRequest.ApproverDepartment),
                new SqlParameter("@P_ApproverName", vehicleRequest.ApproverName),
                new SqlParameter("@P_ReleaseDateTime", vehicleRequest.ReleaseDateTime),
                new SqlParameter("@P_LastMileageReading", vehicleRequest.LastMileageReading),
                new SqlParameter("@P_ReleaseLocationID", vehicleRequest.ReleaseLocationID),
                new SqlParameter("@P_ReturnDateTime", vehicleRequest.ReturnDateTime),
                new SqlParameter("@P_CurrentMileageReading", vehicleRequest.CurrentMileageReading),
                new SqlParameter("@P_HavePersonalBelongings", vehicleRequest.HavePersonalBelongings),
                new SqlParameter("@P_PersonalBelongingsText", vehicleRequest.PersonalBelongingsText),
                new SqlParameter("@P_VehicleID", vehicleRequest.VehicleID),
                new SqlParameter("@P_Notes", vehicleRequest.Notes),
                new SqlParameter("@P_DeleteFlag", vehicleRequest.DeleteFlag),
                new SqlParameter("@P_UpdatedBy", vehicleRequest.UpdatedBy),
                new SqlParameter("@P_ReturnLocationID", vehicleRequest.ReturnLocationID),
                new SqlParameter("@P_UpdatedDateTime", vehicleRequest.UpdatedDateTime),
                new SqlParameter("@P_Action", vehicleRequest.Action),
                new SqlParameter("@P_Reason", vehicleRequest.Reason)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<VehicleRequestModel>(connString, "Save_VehicleRequest", r => r.TranslateAsVehicleWorkflowSaveResponseList(), param);

            if (vehicleRequest.TripCoPassengers != null)
            {
                new TripCoPassengersClient().SaveCoPassengers(connString, vehicleRequest.TripCoPassengers, result.VehicleReqID);
            }

            if (vehicleRequest.TripVehicleIssues != null)
            {
                new TripVehicleIssuesClient().SaveVehicleIssues(connString, vehicleRequest.TripVehicleIssues, result.VehicleReqID);
            }

            if (vehicleRequest.DriverID == null || vehicleRequest.DriverID == 0)
            {
                result.DriverID = vehicleRequest.TobeDrivenbyDriverID;
            }
            else
            {
                result.DriverID = vehicleRequest.DriverID;
            }

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Vehicle"),
                new SqlParameter("@P_Action", vehicleRequest.Action),
                new SqlParameter("@P_ServiceID", vehicleRequest.VehicleReqID)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));
            result.FromID = vehicleRequest.UpdatedBy ?? default(int);
            result.Action = vehicleRequest.Action;
            result.RequestorID = vehicleRequest.Requestor;
            result.RequestorType = vehicleRequest.RequestType;

            SqlParameter[] paramid = {
                new SqlParameter("@P_Department", 13),
                new SqlParameter("@P_Type", 2),
                new SqlParameter("@P_Language", lang),
            };
            result.VehicleTeamID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_User", r => r.TranslateAsUserList(), paramid);

            SqlParameter[] paramsid = {
                new SqlParameter("@P_Department", 13),
                new SqlParameter("@P_Type", 2),
                new SqlParameter("@P_GetHead", 1),
                new SqlParameter("@P_Language", lang),
            };
            result.VehicleTeamHeadID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_User", r => r.TranslateAsUserList(), paramsid);
            return result;
        }

        public string DeleteVehicleRequest(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_VehicleReqID", id)
             };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_VehicleRequestByID", param);
        }

        public VehicleRequestPutModel GetPatchVehicleRequestByID(string connString, int vehicleReqID)
        {
            VehicleRequestPutModel vehicleRequestDetails = new VehicleRequestPutModel();
            SqlParameter[] getparam = {
                new SqlParameter("@P_VehicleReqID", vehicleReqID)
            };
            if (vehicleReqID != 0)
            {
                vehicleRequestDetails = SqlHelper.ExecuteProcedureReturnData<List<VehicleRequestPutModel>>(connString, "Get_VehicleRequestByID", r => r.TranslateAsPutVehicleList(), getparam).FirstOrDefault();
            }

            return vehicleRequestDetails;
        }

        public void PostVehicleIssues(string connString, List<TripVehicleIssuesPostModel> vehicleIssues, int vehicleReqID)
        {
            new TripVehicleIssuesClient().SaveVehicleIssues(connString, vehicleIssues, vehicleReqID);
        }

        public void PostTripCoPassengers(string connString, List<TripCoPassengersModel> coPassengers, int vehicleReqID, int? userID)
        {
            new TripCoPassengersClient().SaveCoPassengers(connString, coPassengers, vehicleReqID);
        }

        public List<VehicleRequestReportListModel> GetReportExporttList(string connString, VehicleRequestReportModel report, string lang)
        {
            List<VehicleRequestReportListModel> list = new List<VehicleRequestReportListModel>();

            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", 1),
            new SqlParameter("@P_PageSize", 20),
            new SqlParameter("@P_Method", 0),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_RequestType", report.RequestType),
            new SqlParameter("@P_Requestor", report.Requestor),
            new SqlParameter("@P_RequestorOfficeDepartment", report.RequestorOfficeDepartment),
            new SqlParameter("@P_TripDateFrom", report.TripDateFrom),
            new SqlParameter("@P_TripDateTo", report.TripDateTo),
            new SqlParameter("@P_Destination", report.Destination),
            new SqlParameter("@P_Language", lang),
            new SqlParameter("@P_SmartSearch", report.SmartSearch)
            };

            list = SqlHelper.ExecuteProcedureReturnData<List<VehicleRequestReportListModel>>(connString, "VehicleReport", r => r.TranslateAsLegalReportList(), param);

            return list;
        }

        public VehicleRequestListModel GetVehicleList(string connString, int pageNumber, int pageSize, string type, string userID, string status, string requestType, string requestor, DateTime? tripDateFrom, DateTime? tripDateTo, string destination, string requestorOfficeDepartment, string smartSearch, string lang)
        {
            VehicleRequestListModel list = new VehicleRequestListModel();

            SqlParameter[] param = {
                   new SqlParameter("@P_PageNumber", pageNumber),
                   new SqlParameter("@P_PageSize", pageSize),
                   new SqlParameter("@P_Type", type),
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_Method", 0),
                   new SqlParameter("@P_Status", status),
                   new SqlParameter("@P_RequestType", requestType),
                   new SqlParameter("@P_Requestor", requestor),
                   new SqlParameter("@P_TripDateFrom", tripDateFrom),
                   new SqlParameter("@P_TripDateTo", tripDateTo),
                   new SqlParameter("@P_Destination", destination),
                   new SqlParameter("@P_RequestorOfficeDepartment", requestorOfficeDepartment),
                   new SqlParameter("@P_Language", lang),
                   new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<VehicleRequestDashboardListModel>>(connString, "Get_VehicleList",  r => r.TranslateAsDashboardVehicleList(), param);

            SqlParameter[] parama = {
                   new SqlParameter("@P_PageNumber", pageNumber),
                   new SqlParameter("@P_PageSize", pageSize),
                   new SqlParameter("@P_Type", type),
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_Method", 1),
                   new SqlParameter("@P_Status", status),
                   new SqlParameter("@P_RequestType", requestType),
                   new SqlParameter("@P_Requestor", requestor),
                   new SqlParameter("@P_TripDateFrom", tripDateFrom),
                   new SqlParameter("@P_TripDateTo", tripDateTo),
                   new SqlParameter("@P_Destination", destination),
                   new SqlParameter("@P_RequestorOfficeDepartment", requestorOfficeDepartment),
                   new SqlParameter("@P_Language", lang),
                   new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_VehicleList", parama);
            Parallel.Invoke(
              () => list.OrganizationList = GetM_Organisation(connString, lang),
              () => list.LookupsList = GetM_Lookups(connString, lang));

            return list;
        }

        public VehicleHomeDashboardModel GetAllModulesPendingTasksCount(string connString, int userID, string lang)
        {
            VehicleHomeDashboardModel list = new VehicleHomeDashboardModel();

            SqlParameter[] myRequestparam = {
                       new SqlParameter("@P_UserID", userID),
                       new SqlParameter("@P_Language", lang),
                        };
            list = SqlHelper.ExecuteProcedureReturnData<VehicleHomeDashboardModel>(connString, "Get_VehicleListCount",  r => r.TranslateasvehicleDashboardcount(), myRequestparam);
            return list;
        }

        public List<VehicleRequestModel> ConfirmVehicleRequest(string connString, int userID, string lang)
        {
            List<VehicleRequestModel> list = new List<VehicleRequestModel>();
            var vehicles = SqlHelper.ExecuteProcedureReturnData<List<VehicleRequestAutoConfirmModel>>(connString, "Get_VehicleAutoComfirm", r => r.Translateasvehicles());
            foreach (var vehicle in vehicles)
            {
                var result = GetPatchVehicleRequestByID(connString, vehicle.VehicleReqID ?? 0);
                result.Action = "ReturnConfirm";
                var res = PutVehicleRequest(connString, result, lang);
                SqlParameter[] getparam = {
                new SqlParameter("@P_VehicleReqID", vehicle.VehicleReqID ?? 0) };
                res.CurrentStatus = SqlHelper.ExecuteProcedureReturnData<List<VehicleRequestGetModel>>(connString, "Get_VehicleRequestByID", r => r.TranslateAsVehicleRequestList(), getparam).FirstOrDefault().Status ?? 0;
                list.Add(res);
            }

            return list;
        }

        public VehiclePreviewModel GetVehicleRequestPreviewByID(string connString, int vehicleReqID, int isReturnForm, string lang)
        {
            VehiclePreviewModel vehicle = new VehiclePreviewModel();

            SqlParameter[] getparam = {
                new SqlParameter("@P_VehicleRequestID", vehicleReqID),
                new SqlParameter("@P_Language", lang)
            };

            if (vehicleReqID != 0)
            {
                vehicle = SqlHelper.ExecuteProcedureReturnData<VehiclePreviewModel>(connString, "Get_VehiclePreview",  r => r.TranslateAsVehiclePreviewModel(), getparam);
            }

            SqlParameter[] getIssuesparam = {
                    new SqlParameter("@P_VehicleReqID", vehicleReqID),
                    new SqlParameter("@P_Type", 1),
                    new SqlParameter("@P_PageType", isReturnForm),
                    new SqlParameter("@P_VehicleID", vehicle.VehicleID)
            };
            vehicle.TripIssues = SqlHelper.ExecuteProcedureReturnData<List<TripVehicleIssuesPostModel>>(connString, "Get_TripVehicleIssuesPreview",  r => r.TranslateAsVehicleIssuesList(), getIssuesparam);

            return vehicle;
        }

        internal VehicleRequestModel PatchVehicleRequest(string connString, int id, JsonPatchDocument<VehicleRequestPutModel> value, string lang)
        {
            var result = GetPatchVehicleRequestByID(connString, id);
            value.ApplyTo(result);
            var res = PutVehicleRequest(connString, result, lang);

            SqlParameter[] getparam = {
                new SqlParameter("@P_VehicleReqID", id) };
            var vehicleDetails = SqlHelper.ExecuteProcedureReturnData<List<VehicleRequestGetModel>>(connString, "Get_VehicleRequestByID", r => r.TranslateAsVehicleRequestList(), getparam).FirstOrDefault();
            res.CurrentStatus = vehicleDetails.Status ?? 0;
            res.ApproverID = vehicleDetails.ApproverName ?? 0;
            return res;
        }
    }
}
