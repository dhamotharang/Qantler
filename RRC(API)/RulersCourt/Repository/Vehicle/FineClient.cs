using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.Vehicle.VehicleFine;
using RulersCourt.Translators;
using RulersCourt.Translators.Vehicle;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Vehicle
{
    public class FineClient
    {
        public VehicleFineListModel GetFineCarList(string connString, int pageNumber, int pageSize, string userID,  string paramStatus, DateTime? paramFineDateFrom, DateTime? paramFineDateTo, string lang, string smartSearch, string paramPlateNumber, string paramIssuedAgainstDepartment, string paramIssuedAgainstName)
        {
            VehicleFineListModel list = new VehicleFineListModel();

            SqlParameter[] param = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Method", 0),
                new SqlParameter("@P_FineDateFrom", paramFineDateFrom),
                new SqlParameter("@P_FineDateTo", paramFineDateTo),
                new SqlParameter("@P_PlateNumber", paramPlateNumber),
                new SqlParameter("@P_IssuedAgainstDepartment", paramIssuedAgainstDepartment),
                new SqlParameter("@P_Status", paramStatus),
                new SqlParameter("@P_IssuedAgainstName", paramIssuedAgainstName),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<VehicleFineGetList>>(connString, "Get_VehicelFineList", r => r.TranslateAsVehicleGetFineModelList(), param);

            Parallel.Invoke(
              () => list.Organizations = GetM_Organisation(connString, lang));

            SqlParameter[] parama = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Method", 1),
                new SqlParameter("@P_FineDateFrom", paramFineDateFrom),
                new SqlParameter("@P_FineDateTo", paramFineDateTo),
                new SqlParameter("@P_PlateNumber", paramPlateNumber),
                new SqlParameter("@P_IssuedAgainstDepartment", paramIssuedAgainstDepartment),
                new SqlParameter("@P_Status", paramStatus),
                new SqlParameter("@P_IssuedAgainstName", paramIssuedAgainstName),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_VehicelFineList", parama);

            return list;
        }

        public VehicleFineGetModel GetFineCarByID(string connString, int vehicleFineID, int userID, string lang)
        {
            VehicleFineGetModel vehicleDetails = new VehicleFineGetModel();

            SqlParameter[] getparam = {
                new SqlParameter("@P_VehicleFineID", vehicleFineID) };
            if (vehicleFineID != 0)
            {
                vehicleDetails = SqlHelper.ExecuteProcedureReturnData<List<VehicleFineGetModel>>(connString, "Get_VehicleFineByID", r => r.TranslateAsGetFineCarListVehicle(), getparam).FirstOrDefault();
            }

            Parallel.Invoke(
              () => vehicleDetails.M_OrganizationList = GetM_Organisation(connString, lang),
              () => vehicleDetails.M_LookupsList = GetM_Lookups(connString, lang));
            return vehicleDetails;
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

        public VehicleFineGetModel PostFineCar(string connString, VehicleFinePostModel vehicle)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_VehicleModelID", vehicle.VehicleModelID),
                new SqlParameter("@P_FineNumber", vehicle.FineNumber),
                new SqlParameter("@P_FinedDate", vehicle.FinedDate),
                new SqlParameter("@P_Location", vehicle.Location),
                new SqlParameter("@P_BlackPoints", vehicle.BlackPoints),
                new SqlParameter("@P_Status", vehicle.Status),
                new SqlParameter("@P_Description", vehicle.Description),
                new SqlParameter("@P_EmailTo", vehicle.EmailTo),
                new SqlParameter("@P_VehicleID", vehicle.VehicleID),
                new SqlParameter("@P_EmailCCDepartmentID", vehicle.EmailCCDepartmentID),
                new SqlParameter("@P_EmailCCUserID", vehicle.EmailCCUserID),
                new SqlParameter("@P_DeleteFlag", vehicle.DeleteFlag),
                new SqlParameter("@P_CreatedBy", vehicle.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", vehicle.CreatedDateTime)
            };

            var result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_VehicleFine", param);
            SqlParameter[] getparam = {
                new SqlParameter("@P_VehicleFineID", result) };
            VehicleFineGetModel vehicleDetails = new VehicleFineGetModel();
            vehicleDetails = SqlHelper.ExecuteProcedureReturnData<List<VehicleFineGetModel>>(connString, "Get_VehicleFineByID",  r => r.TranslateAsGetFineCarListVehicle(), getparam).FirstOrDefault();
            return vehicleDetails;
        }

        public string PutFineCar(string connString, VehicleFinePutModel vehicle)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_VehicleFineID", vehicle.VehicleFineID),
                new SqlParameter("@P_VehicleModelID", vehicle.VehicleModelID),
                new SqlParameter("@P_FineNumber", vehicle.FineNumber),
                new SqlParameter("@P_FinedDate", vehicle.FinedDate),
                new SqlParameter("@P_Location", vehicle.Location),
                new SqlParameter("@P_BlackPoints", vehicle.BlackPoints),
                new SqlParameter("@P_Status", vehicle.Status),
                new SqlParameter("@P_Description", vehicle.Description),
                new SqlParameter("@P_EmailTo", vehicle.EmailTo),
                new SqlParameter("@P_EmailCCDepartmentID", vehicle.EmailCCDepartmentID),
                new SqlParameter("@P_EmailCCUserID", vehicle.EmailCCUserID),
                new SqlParameter("@P_DeleteFlag", vehicle.DeleteFlag),
                new SqlParameter("@P_UpdatedBy", vehicle.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", vehicle.UpdatedDateTime)
            };

            var result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_VehicleFine", param);

            return result;
        }

        public VehicleFinePutModel GetPatchFineCarByID(string connString, int vehicleFineID)
        {
            VehicleFinePutModel driverDetails = new VehicleFinePutModel();

            SqlParameter[] getparam = {
                new SqlParameter("@P_VehicleFineID", vehicleFineID)
            };
            if (vehicleFineID != 0)
            {
                driverDetails = SqlHelper.ExecuteProcedureReturnData<List<VehicleFinePutModel>>(connString, "Get_VehicleFineByID", r => r.TranslateAsPutFineCarList(), getparam).FirstOrDefault();
            }

            return driverDetails;
        }

        public string DeleteFineCar(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_VehicleFineID", id)
             };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_VehicleFineByID", param);
        }

        public List<VehicleFineGetReportList> GetVehicleFineReportExportList(string connString, string lang, string paramStatus, DateTime? paramFineDateFrom, DateTime? paramFineDateTo, string smartSearch, string paramPlateNumber, string paramIssuedAgainstDepartment, string paramIssuedAgainstName)
        {
            List<VehicleFineGetReportList> list = new List<VehicleFineGetReportList>();

            SqlParameter[] param = {
                new SqlParameter("@P_FineDateFrom", paramFineDateFrom),
                new SqlParameter("@P_FineDateTo", paramFineDateTo),
                new SqlParameter("@P_PlateNumber", paramPlateNumber),
                new SqlParameter("@P_IssuedAgainstDepartment", paramIssuedAgainstDepartment),
                new SqlParameter("@P_Status", paramStatus),
                new SqlParameter("@P_IssuedAgainstName", paramIssuedAgainstName),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list = SqlHelper.ExecuteProcedureReturnData<List<VehicleFineGetReportList>>(connString, "Get_VehicalFineReport", r => r.TranslateAsVehiclesFineReportList(), param);

            return list;
        }

        internal string PatchFineCar(string connString, int id, JsonPatchDocument<VehicleFinePutModel> value)
        {
            var result = GetPatchFineCarByID(connString, id);
            value.ApplyTo(result);
            var res = PutFineCar(connString, result);
            return res;
        }
    }
}