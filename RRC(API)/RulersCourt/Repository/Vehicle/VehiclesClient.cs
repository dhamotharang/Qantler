using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.Vehicle;
using RulersCourt.Models.Vehicle.Vehicles;
using RulersCourt.Translators;
using RulersCourt.Translators.Vehicle.Vehicles;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Vehicle
{
    public class VehiclesClient
    {
        public VehicleGetModel GetVehicleByID(string connString, int vehicleID, int userID, string lang)
        {
            VehicleGetModel vehicleDetails = new VehicleGetModel();

            SqlParameter[] getparam = {
                new SqlParameter("@P_VehicleID", vehicleID) };
            if (vehicleID != 0)
            {
                vehicleDetails = SqlHelper.ExecuteProcedureReturnData<List<VehicleGetModel>>(connString, "Get_VechicalByID", r => r.TranslateAsGetListVehicle(), getparam).FirstOrDefault();
            }

            Parallel.Invoke(
              () => vehicleDetails.M_OrganizationList = GetM_Organisation(connString, lang),
              () => vehicleDetails.M_LookupsList = GetM_Lookups(connString, lang));

            return vehicleDetails;
        }

        public VehicleListModel GetVehicleManagement(string connString, int pageNumber, int pageSize, string userID, string plateNumber, string plateColor, string lang, string smartSearch, string departmentOffice, string isAlternativeVehicle)
        {
            VehicleListModel list = new VehicleListModel();

            SqlParameter[] param = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Method", 0),
                new SqlParameter("@P_PlateNumber", plateNumber),
                new SqlParameter("@P_PlateColor", plateColor),
                new SqlParameter("@P_DepartmentOffice", departmentOffice),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_IsAlternativeVehicle", isAlternativeVehicle),
                new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<VehicleGetListModel>>(connString, "Get_VehicelList", r => r.TranslateAsVehicleGetModelList(), param);

            SqlParameter[] parama = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Method", 1),
                new SqlParameter("@P_PlateNumber", plateNumber),
                new SqlParameter("@P_PlateColor", plateColor),
                new SqlParameter("@P_DepartmentOffice", departmentOffice),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_IsAlternativeVehicle", isAlternativeVehicle),
                new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_VehicelList", parama);

            return list;
        }

        public List<OrganizationModel> GetM_Organisation(string connString, string lang)
        {
            SqlParameter[] orgParam = {
                new SqlParameter("@P_Language", lang) };
            var e = SqlHelper.ExecuteProcedureReturnData<List<OrganizationModel>>(connString, "Get_Organization", r => r.TranslateAsOrganizationList(), orgParam);
            return e;
        }

        public List<M_LookupsModel> GetM_Lookups(string connString, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Type", "Vehicle"),
                new SqlParameter("@P_Language", lang)
            };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public VehicleSaveResponseModel PostVehicle(string connString, VehiclePostModel vehicle)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_PlateNumber", vehicle.PlateNumber),
                new SqlParameter("@P_PlateColor", vehicle.PlateColor),
                new SqlParameter("@P_VehicleMake", vehicle.VehicleMake),
                new SqlParameter("@P_VehicleModel", vehicle.VehicleModel),
                new SqlParameter("@P_YearofManufacture", vehicle.YearofManufacture),
                new SqlParameter("@P_CarCompanyID", vehicle.CarCompanyID),
                new SqlParameter("@P_ContractNumber", vehicle.ContractNumber),
                new SqlParameter("@P_ContractDuration", vehicle.ContractDuration),
                new SqlParameter("@P_ContractStartDate", vehicle.ContractStartDate),
                new SqlParameter("@P_ContractEndDate", vehicle.ContractEndDate),
                new SqlParameter("@P_VehicleName", vehicle.VehicleName),
                new SqlParameter("@P_VehicleRegistrationNumber", vehicle.VehicleRegistrationNumber),
                new SqlParameter("@P_VehicleRegistrationExpiry", vehicle.VehicleRegistrationExpiry),
                new SqlParameter("@P_NextService", vehicle.NextService),
                new SqlParameter("@P_TyreChange", vehicle.TyreChange),
                new SqlParameter("@P_Notes", vehicle.Notes),
                new SqlParameter("@P_IsAlternativeVehicle", vehicle.IsAlternativeVehicle),
                new SqlParameter("@P_CreatedBy", vehicle.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", vehicle.CreatedDateTime)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<VehicleSaveResponseModel>(connString, "Save_Vehicles",  r => r.TranslateAsVehicleSaveResponseList(), param);

            return result;
        }

        public VehicleSaveResponseModel PutVehicle(string connString, VehiclePutModel vehicle)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_VehicleID", vehicle.VehicleID),
                new SqlParameter("@P_PlateNumber", vehicle.PlateNumber),
                new SqlParameter("@P_PlateColor", vehicle.PlateColor),
                new SqlParameter("@P_VehicleMake", vehicle.VehicleMake),
                new SqlParameter("@P_VehicleModel", vehicle.VehicleModel),
                new SqlParameter("@P_YearofManufacture", vehicle.YearofManufacture),
                new SqlParameter("@P_CarCompanyID", vehicle.CarCompanyID),
                new SqlParameter("@P_ContractNumber", vehicle.ContractNumber),
                new SqlParameter("@P_VehicleName", vehicle.VehicleName),
                new SqlParameter("@P_ContractDuration", vehicle.ContractDuration),
                new SqlParameter("@P_ContractStartDate", vehicle.ContractStartDate),
                new SqlParameter("@P_ContractEndDate", vehicle.ContractEndDate),
                new SqlParameter("@P_VehicleRegistrationNumber", vehicle.VehicleRegistrationNumber),
                new SqlParameter("@P_VehicleRegistrationExpiry", vehicle.VehicleRegistrationExpiry),
                new SqlParameter("@P_NextService", vehicle.NextService),
                new SqlParameter("@P_TyreChange", vehicle.TyreChange),
                new SqlParameter("@P_Notes", vehicle.Notes),
                new SqlParameter("@P_UpdatedBy", vehicle.UpdatedBy),
                new SqlParameter("@P_IsAlternativeVehicle", vehicle.IsAlternativeVehicle),
                new SqlParameter("@P_UpdatedDateTime", vehicle.UpdatedDateTime)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<VehicleSaveResponseModel>(connString, "Save_Vehicles",  r => r.TranslateAsVehicleSaveResponseList(), param);

            return result;
        }

        public string DeleteVehicle(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_VehicleID", id)
             };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_VehicleByID", param);
        }

        public VehiclePutModel GetPatchVehicleByID(string connString, int vehicleID)
        {
            VehiclePutModel vehicleDetails = new VehiclePutModel();

            SqlParameter[] getparam = {
                new SqlParameter("@P_VehicleID", vehicleID) };
            if (vehicleID != 0)
            {
                vehicleDetails = SqlHelper.ExecuteProcedureReturnData<List<VehiclePutModel>>(connString, "Get_VehicleByID",  r => r.TranslateAsPutVehicleList(), getparam).FirstOrDefault();
            }

            return vehicleDetails;
        }

        public string PostLogaServiceVehicle(string connString, VehicleLogServicePost vehicle)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_CurrentMileage", vehicle.CurrentMileage),
                new SqlParameter("@P_NextMileage", vehicle.NextMileage),
                new SqlParameter("@P_VehicleID", vehicle.VehicleID),
                new SqlParameter("@P_LogType", vehicle.LogType),
                new SqlParameter("@P_CreatedBy", vehicle.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", vehicle.CreatedDateTime)
            };

            var result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_LogService", param);

            return result;
        }

        public List<VehicleLogServicePost> GetLogaServiceVehicle(string connString, int id, int type)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_VehicleID", id),
                new SqlParameter("@P_Type", type)
            };

            return SqlHelper.ExecuteProcedureReturnData<List<VehicleLogServicePost>>(connString, "Get_VehicleSericeLog", r => r.TranslateAsVehicleServiceLog(), param);
        }

        public List<VehicleLogServiceReportModel> GetLogaServiceVehicleReport(string connString, int id, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_VehicleID", id),
                new SqlParameter("@P_Language", lang)
            };

            return SqlHelper.ExecuteProcedureReturnData<List<VehicleLogServiceReportModel>>(connString, "Get_VehicleSericeLogReport",  r => r.TranslateAsVehicleServiceLogList(), param);
        }

        public List<VehiclesReport> GetVehicleReportExportList(string connString, string userID, string plateNumber, string plateColor, string lang, string smartSearch, string departmentOffice, string isAlternateVehicle)
        {
            List<VehiclesReport> list = new List<VehiclesReport>();

            SqlParameter[] param = {
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_PlateNumber", plateNumber),
                new SqlParameter("@P_PlateColor", plateColor),
                new SqlParameter("@P_DepartmentOffice", departmentOffice),
                new SqlParameter("@P_IsAlternativeVehicle", isAlternateVehicle),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list = SqlHelper.ExecuteProcedureReturnData<List<VehiclesReport>>(connString, "Get_VehicalReport", r => r.TranslateAsVehiclesReportList(), param);

            return list;
        }

        public VehiclePlateNumberColorModel GetVehicleManagementAllPlateNumber(string connString, string userID, string lang)
        {
            VehiclePlateNumberColorModel list = new VehiclePlateNumberColorModel();

            SqlParameter[] plateColorParam = {
            new SqlParameter("@P_Type", 2)
            };

            list.PlateColor = SqlHelper.ExecuteProcedureReturnData<List<string>>(connString, "Get_VechilePlateNumberColor", r => r.TranslateAsVehicleStringList(), plateColorParam);

            SqlParameter[] plateNumberParam = {
            new SqlParameter("@P_Type", 1) };

            list.PlateNumber = SqlHelper.ExecuteProcedureReturnData<List<string>>(connString, "Get_VechilePlateNumberColor",  r => r.TranslateAsVehicleStringList(), plateNumberParam);
            return list;
        }

        internal VehicleSaveResponseModel PatchVehicle(string connString, int id, JsonPatchDocument<VehiclePutModel> value)
        {
            var result = GetPatchVehicleByID(connString, id);
            value.ApplyTo(result);
            var res = PutVehicle(connString, result);
            return res;
        }
    }
}