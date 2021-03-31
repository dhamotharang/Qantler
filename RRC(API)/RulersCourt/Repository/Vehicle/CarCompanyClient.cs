using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.Vehicle.CarCompany;
using RulersCourt.Models.Vehicle.TripVehicleIssues;
using RulersCourt.Models.Vehicle.VehicleFine;
using RulersCourt.Translators;
using RulersCourt.Translators.Vehicle;
using RulersCourt.Translators.Vehicle.Vehicles;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Vehicle
{
    public class CarCompanyClient
    {
        public CarCompanyGetModel GetCarCompanyByID(string connString, int carCompanyID, int userID, string lang)
        {
            CarCompanyGetModel carCompanyDetails = new CarCompanyGetModel();

            SqlParameter[] getparam = {
                new SqlParameter("@P_CarCompanyID", carCompanyID) };
            if (carCompanyID != 0)
            {
                carCompanyDetails = SqlHelper.ExecuteProcedureReturnData<List<CarCompanyGetModel>>(connString, "Get_CarCompanyByID", r => r.TranslateAsCarCompanyList(), getparam).FirstOrDefault();
            }

            Parallel.Invoke(
            () => carCompanyDetails.M_OrganizationList = GetM_Organisation(connString, lang),
            () => carCompanyDetails.M_LookupsList = GetM_Lookups(connString, lang));

            return carCompanyDetails;
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

        public VehicleFineCarDashboardListModel GetVehicleList(string connString, string userID, string plateNumber, string vehicleID)
        {
            VehicleFineCarDashboardListModel res = new VehicleFineCarDashboardListModel();
            SqlParameter[] listParam = {
                new SqlParameter("@P_PlateNumber", plateNumber),
                new SqlParameter("@P_VehicleID", vehicleID),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Method", 0)
            };

            SqlParameter[] countParam = {
                new SqlParameter("@P_PlateNumber", plateNumber),
                new SqlParameter("@P_VehicleID", vehicleID),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Method", 1)
            };

            res.Collection = SqlHelper.ExecuteProcedureReturnData<List<VehicleFineCarListModel>>(connString, "Get_VehicelByPlateNumber", r => r.TranslateAsVehicleList(), listParam);
            int i = 0;
            foreach (VehicleFineCarListModel car in res.Collection)
            {
                res.Collection[i].VehicleIssues = GetVehicleIsues(car.VehicleID, connString);
                i = i + 1;
            }

            res.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_VehicelByPlateNumber", countParam);
            return res;
        }

        public List<CarCompanyGetModel> GetCarCompanyList(string connString, string userID, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_PageNumber", 0),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Language", lang) };

            return SqlHelper.ExecuteProcedureReturnData<List<CarCompanyGetModel>>(connString, "Get_CarCompanyList", r => r.TranslateAsCarCompanyList(), param);
        }

        public CarCompanyListModel GetCarCompany(string connString, int pageNumber, int pageSize, string userID, DateTime? createdDateFrom, DateTime? createdDateTo, string lang, string smartSearch)
        {
            CarCompanyListModel list = new CarCompanyListModel();

            SqlParameter[] param = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Method", 0),
                new SqlParameter("@P_CreatedDateFrom", createdDateFrom),
                new SqlParameter("@P_CreatedDateTo", createdDateTo),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_SmartSearch", smartSearch) };

            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<CarCompanyGetModel>>(connString, "Get_CarCompanyList", r => r.TranslateAsCarCompanyList(), param);

            SqlParameter[] parama = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Method", 1),
                new SqlParameter("@P_CreatedDateFrom", createdDateFrom),
                new SqlParameter("@P_CreatedDateTo", createdDateTo),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_CarCompanyList", parama);
            return list;
        }

        public CarCompanySaveResponseModel PostContact(string connString, CarCompanyPostModel carCompany)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_CompanyName", carCompany.CompanyName),
                new SqlParameter("@P_ContactName", carCompany.ContactName),
                new SqlParameter("@P_ContactNumber", carCompany.ContactNumber),
                new SqlParameter("@P_DeleteFlag", carCompany.DeleteFlag),
                new SqlParameter("@P_CreatedBy", carCompany.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", carCompany.CreatedDateTime)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<CarCompanySaveResponseModel>(connString, "Save_CarCompany", r => r.TranslateAsCarCompanySaveResponseList(), param);

            return result;
        }

        public CarCompanySaveResponseModel PutCarCompany(string connString, CarCompanyPutModel carCompany)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_CarCompanyID", carCompany.CarCompanyID),
                new SqlParameter("@P_CompanyName", carCompany.CompanyName),
                new SqlParameter("@P_ContactName", carCompany.ContactName),
                new SqlParameter("@P_ContactNumber", carCompany.ContactNumber),
                new SqlParameter("@P_DeleteFlag", carCompany.DeleteFlag),
                new SqlParameter("@P_UpdatedBy", carCompany.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", carCompany.UpdatedDateTime)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<CarCompanySaveResponseModel>(connString, "Save_CarCompany", r => r.TranslateAsCarCompanySaveResponseList(), param);
            return result;
        }

        public string DeleteCarCompany(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_CarCompanyID", id)
             };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_CarCompanyByID", param);
        }

        public CarCompanyPutModel GetPatchCarCompanyByID(string connString, int carCompanyID)
        {
            CarCompanyPutModel carCompanyDetails = new CarCompanyPutModel();

            SqlParameter[] getparam = {
                new SqlParameter("@P_CarCompanyID", carCompanyID) };
            if (carCompanyID != 0)
            {
                carCompanyDetails = SqlHelper.ExecuteProcedureReturnData<List<CarCompanyPutModel>>(connString, "Get_CarCompanyByID",  r => r.TranslateAsPutCarCompanyList(), getparam).FirstOrDefault();
            }

            return carCompanyDetails;
        }

        public List<CarCompanyReport> GetCarCompanyReportExportList(string connString, string lang, string parameCreatedDateTo, string parameCreatedDateFrom, string paramSmartSearch)
        {
            List<CarCompanyReport> list = new List<CarCompanyReport>();

            SqlParameter[] param = {
             new SqlParameter("@P_CreatedDateFrom", parameCreatedDateFrom),
             new SqlParameter("@P_CreatedDateTo", parameCreatedDateTo),
             new SqlParameter("@P_SmartSearch", paramSmartSearch),
             new SqlParameter("@P_Language", lang)
            };

            list = SqlHelper.ExecuteProcedureReturnData<List<CarCompanyReport>>(connString, "Get_CarCompanyReport",  r => r.TranslateAsCarReportList(), param);

            return list;
        }

        public List<TripVehicleIssuesPostModel> GetVehicleIsues(string vehicleID, string connString)
        {
            SqlParameter[] getIssuesparam = { new SqlParameter("@P_VehicleID", vehicleID) };
            return SqlHelper.ExecuteProcedureReturnData<List<TripVehicleIssuesPostModel>>(connString, "Get_TripVehicleIssues", r => r.TranslateAsVehicleIssuesList(), getIssuesparam);
        }

        internal CarCompanySaveResponseModel PatchCarCompany(string connString, int id, JsonPatchDocument<CarCompanyPutModel> value)
        {
            var result = GetPatchCarCompanyByID(connString, id);
            value.ApplyTo(result);
            var res = PutCarCompany(connString, result);
            return res;
        }
    }
}
