using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.Vehicle.Drivers;
using RulersCourt.Translators;
using RulersCourt.Translators.Vehicle.DriverVehicles;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Vehicle
{
    public class DriverClient
    {
        public DriverListModel GetDriver(string connString, int pageNumber, int pageSize, string userID, DateTime? paramDateRangeFrom, DateTime? paramDateRangeTo, string lang, string smartSearch, DateTime? calendarDate)
        {
            DriverListModel list = new DriverListModel();
            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", pageNumber),
            new SqlParameter("@P_PageSize", pageSize),
            new SqlParameter("@P_UserID", userID),
            new SqlParameter("@P_Method", 0),
            new SqlParameter("@P_DateRangeFrom", paramDateRangeFrom),
            new SqlParameter("@P_DateRangeTo", paramDateRangeTo),
            new SqlParameter("@P_CalendarDate", calendarDate),
            new SqlParameter("@P_Language", lang),
            new SqlParameter("@P_SmartSearch", smartSearch)
             };
            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<DriverGetModel>>(connString, "Get_DriverList", r => r.TranslateAsDriverList(), param);
            SqlParameter[] parama = {
             new SqlParameter("@P_PageNumber", pageNumber),
             new SqlParameter("@P_PageSize", pageSize),
             new SqlParameter("@P_UserID", userID),
             new SqlParameter("@P_Method", 1),
             new SqlParameter("@P_DateRangeFrom", paramDateRangeFrom),
             new SqlParameter("@P_DateRangeTo", paramDateRangeTo),
             new SqlParameter("@P_Language", lang),
             new SqlParameter("@P_CalendarDate", calendarDate),
             new SqlParameter("@P_SmartSearch", smartSearch)
             };
            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_DriverList", parama);
            Parallel.Invoke(
             () => list.M_OrganizationList = GetM_Organisation(connString, lang),
             () => list.M_LookupsList = GetM_Lookups(connString, lang));
            int i = 0;
            foreach (DriverGetModel temp in list.Collection)
            {
                list.Collection[i].DriverTrips = GetDriverTripDays(temp.UserProfileID, connString, lang);
                i = i + 1;
            }

            return list;
        }

        public List<DriverGetTripDaysModel> GetDriverTripDays(int? driverID, string connString, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_DriverID", driverID),
                new SqlParameter("@P_Language", lang)
            };
            var e = SqlHelper.ExecuteProcedureReturnData<List<DriverGetTripDaysModel>>(connString, "Get_DriverTripDays", r => r.TranslateAsGetDriverTripDays(), param);
            return e;
        }

        public DriverGetModel GetDriverByID(string connString, int driverID, int userID, DateTime? paramDateRangeFrom, DateTime? paramDateRangeTo, string lang)
        {
            DriverGetModel driverDetails = new DriverGetModel();
            SqlParameter[] getparam = {
                 new SqlParameter("@P_DriverID", driverID)
            };

            SqlParameter[] param = {
                 new SqlParameter("@P_UserID", driverID),
                 new SqlParameter("@P_Method", 0),
                 new SqlParameter("@P_DateRangeFrom", paramDateRangeFrom),
                 new SqlParameter("@P_DateRangeTo", paramDateRangeTo),
             };

            if (driverID != 0)
            {
                var temp = SqlHelper.ExecuteProcedureReturnData<List<DriverGetModel>>(connString, "Get_DriverByID", r => r.TranslateAsDriverList(), getparam).FirstOrDefault();

                if (temp != null)
                    driverDetails = temp;

                if (driverDetails != null)
                    driverDetails.CompensateExtra = SqlHelper.ExecuteProcedureReturnData<List<DriverReport>>(connString, "Get_DriverExtraCompensate", r => r.TranslateAsCarDriverReportList(), param);
            }

            Parallel.Invoke(
                  () => driverDetails.M_OrganizationList = GetM_Organisation(connString, lang),
                  () => driverDetails.M_LookupsList = GetM_Lookups(connString, lang));
            return driverDetails;
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

        public string PostDriver(string connString, DriverPostModel driver)
        {
            SqlParameter[] param = {
                 new SqlParameter("@P_UserProfileID", driver.UserProfileID),
                 new SqlParameter("@P_LogDate", driver.LogDate),
                 new SqlParameter("@P_ExtraHours", driver.ExtraHours),
                 new SqlParameter("@P_CompensateHours", driver.CompensateHours),
                 new SqlParameter("@P_DeleteFlag", driver.DeleteFlag),
                 new SqlParameter("@P_CreatedBy", driver.CreatedBy),
                 new SqlParameter("@P_CreatedDateTime", driver.CreatedDateTime)
             };

            var result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_Driver", param);
            return result;
        }

        public string PutDriver(string connString, DriverPutModel driver)
        {
            SqlParameter[] param = {
                 new SqlParameter("@P_DriverID", driver.DriverID),
                 new SqlParameter("@P_UserProfileID", driver.UserProfileID),
                 new SqlParameter("@P_LogDate", driver.LogDate),
                 new SqlParameter("@P_ExtraHours", driver.ExtraHours),
                 new SqlParameter("@P_CompensateHours", driver.CompensateHours),
                 new SqlParameter("@P_DeleteFlag", driver.DeleteFlag),
                 new SqlParameter("@P_UpdatedBy", driver.UpdatedBy),
                 new SqlParameter("@P_UpdatedDateTime", driver.UpdatedDateTime)
             };

            var result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_Driver", param);
            return result;
        }

        public string DeleteDriver(string connString, int id)
        {
            SqlParameter[] param = {
                 new SqlParameter("@P_DriverID", id)
              };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_DriverByID", param);
        }

        public DriverPutModel GetPatchDriverByID(string connString, int driverID)
        {
            DriverPutModel driverDetails = new DriverPutModel();

            SqlParameter[] getparam = {
                 new SqlParameter("@P_DriverID", driverID) };
            if (driverID != 0)
            {
                driverDetails = SqlHelper.ExecuteProcedureReturnData<List<DriverPutModel>>(connString, "Get_DriverByID", r => r.TranslateAsPutDriverList(), getparam).FirstOrDefault();
            }

            return driverDetails;
        }

        public List<DriverReport> GetDriverReportExportList(string connString, string lang, DateTime? paramDateRangeFrom, DateTime? paramDateRangeTo, int userID, int id)
        {
            List<DriverReport> list = new List<DriverReport>();

            SqlParameter[] param = {
                 new SqlParameter("@P_UserID", id),
                 new SqlParameter("@P_Method", 0),
                 new SqlParameter("@P_DateRangeFrom", paramDateRangeFrom),
                 new SqlParameter("@P_DateRangeTo", paramDateRangeTo),
             };

            list = SqlHelper.ExecuteProcedureReturnData<List<DriverReport>>(connString, "Get_DriverExtraCompensate", r => r.TranslateAsCarDriverReportList(), param);

            return list;
        }

        public bool SaveMasterDriver(string connString, int userID, DriverBlinding data)
        {
            string userId = string.Join(";", data.DriverID);
            SqlParameter[] parama = {
                 new SqlParameter("@P_UserID", userId)
             };
            var res = SqlHelper.ExecuteProcedureReturnString(connString, "Get_DriverSelect", parama);
            return Convert.ToBoolean(res);
        }

        public DriverListMaster GetMasterDriver(string connString, int userID, bool driver, string lang)
        {
            DriverListMaster list = new DriverListMaster();

            SqlParameter[] param = {
                 new SqlParameter("@P_Driver", driver),
                 new SqlParameter("@P_Method", 0),
                 new SqlParameter("@P_Language", lang)
             };

            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<DriverGetMaster>>(connString, "Get_M_DriverManagement", r => r.TranslateAsMasterDriverList(), param);

            SqlParameter[] parama = {
                new SqlParameter("@P_Driver", driver),
                new SqlParameter("@P_Method", 1),
                new SqlParameter("@P_Language", lang)
                   };
            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_M_DriverManagement", parama);

            return list;
        }

        public List<DriverTrips> GetDriverTripList(string connString, string lang, int driver, string vehicleID)
        {
            List<DriverTrips> driverTrips = new List<DriverTrips>();

            SqlParameter[] param = {
             new SqlParameter("@P_Driver", driver),
             new SqlParameter("@P_Language", lang),
             new SqlParameter("@P_VehicleID", vehicleID)
             };

            driverTrips = SqlHelper.ExecuteProcedureReturnData<List<DriverTrips>>(connString, "Get_VehicleDriverTrips", r => r.TranslateAsDriverTripList(), param);

            return driverTrips;
        }

        public List<DriverGetModel> GetDriverPreviewCalender(string connString, DateTime? calendarDate, string lang)
        {
            List<DriverGetModel> list = new List<DriverGetModel>();
            SqlParameter[] param = {
            new SqlParameter("@P_CalendarDate", calendarDate),
            new SqlParameter("@P_Language", lang)
             };
            list = SqlHelper.ExecuteProcedureReturnData<List<DriverGetModel>>(connString, "Get_DriverPreviewCalenderList", r => r.TranslateAsDriverList(), param);
            int i = 0;
            foreach (DriverGetModel temp in list)
            {
                list[i].DriverTrips = GetDriverTripDays(temp.UserProfileID, connString, lang);
                i = i + 1;
            }

            return list;
        }

        internal string PatchDriver(string connString, int id, JsonPatchDocument<DriverPutModel> value)
        {
            var result = GetPatchDriverByID(connString, id);
            value.ApplyTo(result);
            var res = PutDriver(connString, result);
            return res;
        }
    }
}
