using RulersCourt.Models.Vehicle;
using RulersCourt.Models.Vehicle.Vehicles;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Vehicle.Vehicles
{
    public static class VehiclesTranslator
    {
        public static VehicleGetModel TranslateAsGetVehicle(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var vehicleGetModel = new VehicleGetModel();

            if (reader.IsColumnExists("VehicleID"))
                vehicleGetModel.VehicleID = SqlHelper.GetNullableInt32(reader, "vehicleID");

            if (reader.IsColumnExists("ReferenceNumber"))
                vehicleGetModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("PlateNumber"))
                vehicleGetModel.PlateNumber = SqlHelper.GetNullableString(reader, "PlateNumber");

            if (reader.IsColumnExists("PlateColor"))
                vehicleGetModel.PlateColor = SqlHelper.GetNullableString(reader, "PlateColor");

            if (reader.IsColumnExists("VehicleMake"))
                vehicleGetModel.VehicleMake = SqlHelper.GetNullableString(reader, "VehicleMake");

            if (reader.IsColumnExists("VehicleModel"))
                vehicleGetModel.VehicleModel = SqlHelper.GetNullableString(reader, "VehicleModel");

            if (reader.IsColumnExists("VehicleName"))
                vehicleGetModel.VehicleName = SqlHelper.GetNullableString(reader, "VehicleName");

            if (reader.IsColumnExists("YearofManufacture"))
                vehicleGetModel.YearofManufacture = SqlHelper.GetNullableInt32(reader, "YearofManufacture");

            if (reader.IsColumnExists("CarCompanyID"))
                vehicleGetModel.CarCompanyID = SqlHelper.GetNullableInt32(reader, "CarCompanyID");

            if (reader.IsColumnExists("ContractNumber"))
                vehicleGetModel.ContractNumber = SqlHelper.GetNullableString(reader, "ContractNumber");

            if (reader.IsColumnExists("ContractDuration"))
                vehicleGetModel.ContractDuration = SqlHelper.GetNullableInt32(reader, "ContractDuration");

            if (reader.IsColumnExists("ContractStartDate"))
                vehicleGetModel.ContractStartDate = SqlHelper.GetDateTime(reader, "ContractStartDate");

            if (reader.IsColumnExists("ContractEndDate"))
                vehicleGetModel.ContractEndDate = SqlHelper.GetDateTime(reader, "ContractEndDate");

            if (reader.IsColumnExists("VehicleRegistrationNumber"))
                vehicleGetModel.VehicleRegistrationNumber = SqlHelper.GetNullableString(reader, "VehicleRegistrationNumber");

            if (reader.IsColumnExists("VehicleRegistrationExpiry"))
                vehicleGetModel.VehicleRegistrationExpiry = SqlHelper.GetDateTime(reader, "VehicleRegistrationExpiry");

            if (reader.IsColumnExists("NextService"))
                vehicleGetModel.NextService = SqlHelper.GetNullableInt32(reader, "NextService");

            if (reader.IsColumnExists("TyreChange"))
                vehicleGetModel.TyreChange = SqlHelper.GetNullableInt32(reader, "TyreChange");

            if (reader.IsColumnExists("Notes"))
                vehicleGetModel.Notes = SqlHelper.GetNullableString(reader, "Notes");

            if (reader.IsColumnExists("CreatedDepartment"))
                vehicleGetModel.CreatedDepartment = SqlHelper.GetNullableInt32(reader, "CreatedDepartment");

            if (reader.IsColumnExists("CreatedBy"))
                vehicleGetModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                vehicleGetModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                vehicleGetModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                vehicleGetModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("DeleteFlag"))
                vehicleGetModel.DeleteFlag = SqlHelper.GetBoolean(reader, "DeleteFlag");

            if (reader.IsColumnExists("CurrentMileage"))
                vehicleGetModel.CurrentMileage = SqlHelper.GetNullableString(reader, "CurrentMileage");

            if (reader.IsColumnExists("IsAlternativeVehicle"))
                vehicleGetModel.IsAlternativeVehicle = SqlHelper.GetBoolean(reader, "IsAlternativeVehicle");

            return vehicleGetModel;
        }

        public static List<VehicleGetModel> TranslateAsGetListVehicle(this SqlDataReader reader)
        {
            var vehicleList = new List<VehicleGetModel>();
            while (reader.Read())
            {
                vehicleList.Add(TranslateAsGetVehicle(reader, true));
            }

            return vehicleList;
        }

        public static VehicleGetListModel TranslateAsGetListModelVehicle(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var vehicleGetListModel = new VehicleGetListModel();

            if (reader.IsColumnExists("VehicleID"))
                vehicleGetListModel.VehicleID = SqlHelper.GetNullableInt32(reader, "VehicleID");

            if (reader.IsColumnExists("PlateNumber"))
                vehicleGetListModel.PlateNumber = SqlHelper.GetNullableString(reader, "PlateNumber");

            if (reader.IsColumnExists("PlateColor"))
                vehicleGetListModel.PlateColor = SqlHelper.GetNullableString(reader, "PlateColor");

            if (reader.IsColumnExists("NextService"))
                vehicleGetListModel.NextService = SqlHelper.GetNullableInt32(reader, "NextService");

            if (reader.IsColumnExists("VehicleModel"))
                vehicleGetListModel.VehicleModel = SqlHelper.GetNullableString(reader, "VehicleModel");

            if (reader.IsColumnExists("Nameofuser"))
                vehicleGetListModel.Nameofuser = SqlHelper.GetNullableString(reader, "Nameofuser");

            if (reader.IsColumnExists("CreatedDepartment"))
                vehicleGetListModel.CreatedDepartment = SqlHelper.GetNullableString(reader, "CreatedDepartment");

            if (reader.IsColumnExists("TyreChange"))
                vehicleGetListModel.TyreChange = SqlHelper.GetNullableInt32(reader, "TyreChange");

            if (reader.IsColumnExists("VehicleRegistrationExpiry"))
                vehicleGetListModel.VehicleRegistrationExpiry = SqlHelper.GetDateTime(reader, "VehicleRegistrationExpiry");

            if (reader.IsColumnExists("ContractEndDate"))
                vehicleGetListModel.ContractEndDate = SqlHelper.GetDateTime(reader, "ContractEndDate");

            if (reader.IsColumnExists("CurrentMileage"))
                vehicleGetListModel.CurrentMileage = SqlHelper.GetNullableString(reader, "CurrentMileage");

            if (reader.IsColumnExists("NameofDepartment"))
                vehicleGetListModel.NameofDepartment = SqlHelper.GetNullableString(reader, "NameofDepartment");

            return vehicleGetListModel;
        }

        public static List<VehicleGetListModel> TranslateAsVehicleGetModelList(this SqlDataReader reader)
        {
            var vehicleGetList = new List<VehicleGetListModel>();
            while (reader.Read())
            {
                vehicleGetList.Add(TranslateAsGetListModelVehicle(reader, true));
            }

            return vehicleGetList;
        }

        public static VehiclePutModel TranslateAsPutVehicle(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var vehicleModel = new VehiclePutModel();

            if (reader.IsColumnExists("VehicleID"))
                vehicleModel.VehicleID = SqlHelper.GetNullableInt32(reader, "VehicleID");

            if (reader.IsColumnExists("PlateNumber"))
                vehicleModel.PlateNumber = SqlHelper.GetNullableString(reader, "PlateNumber");

            if (reader.IsColumnExists("PlateColor"))
                vehicleModel.PlateColor = SqlHelper.GetNullableString(reader, "PlateColor");

            if (reader.IsColumnExists("VehicleMake"))
                vehicleModel.VehicleMake = SqlHelper.GetNullableString(reader, "VehicleMake");

            if (reader.IsColumnExists("VehicleModel"))
                vehicleModel.VehicleModel = SqlHelper.GetNullableString(reader, "VehicleModel");

            if (reader.IsColumnExists("YearofManufacture"))
                vehicleModel.YearofManufacture = SqlHelper.GetNullableInt32(reader, "YearofManufacture");

            if (reader.IsColumnExists("CarCompanyID"))
                vehicleModel.CarCompanyID = SqlHelper.GetNullableInt32(reader, "CarCompanyID");

            if (reader.IsColumnExists("ContractNumber"))
                vehicleModel.ContractNumber = SqlHelper.GetNullableString(reader, "ContractNumber");

            if (reader.IsColumnExists("ContractDuration"))
                vehicleModel.ContractDuration = SqlHelper.GetNullableInt32(reader, "ContractDuration");

            if (reader.IsColumnExists("ContractStartDate"))
                vehicleModel.ContractStartDate = SqlHelper.GetDateTime(reader, "ContractStartDate");

            if (reader.IsColumnExists("ContractEndDate"))
                vehicleModel.ContractEndDate = SqlHelper.GetDateTime(reader, "ContractEndDate");

            if (reader.IsColumnExists("VehicleRegistrationNumber"))
                vehicleModel.VehicleRegistrationNumber = SqlHelper.GetNullableString(reader, "VehicleRegistrationNumber");

            if (reader.IsColumnExists("VehicleRegistrationExpiry"))
                vehicleModel.VehicleRegistrationExpiry = SqlHelper.GetDateTime(reader, "VehicleRegistrationExpiry");

            if (reader.IsColumnExists("NextService"))
                vehicleModel.NextService = SqlHelper.GetNullableInt32(reader, "NextService");

            if (reader.IsColumnExists("TyreChange"))
                vehicleModel.TyreChange = SqlHelper.GetNullableInt32(reader, "TyreChange");

            if (reader.IsColumnExists("UpdatedBy"))
                vehicleModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                vehicleModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Notes"))
                vehicleModel.Notes = SqlHelper.GetNullableString(reader, "Notes");

            if (reader.IsColumnExists("IsAlternativeVehicle"))
                vehicleModel.IsAlternativeVehicle = SqlHelper.GetBoolean(reader, "IsAlternativeVehicle");

            return vehicleModel;
        }

        public static List<VehiclePutModel> TranslateAsPutVehicleList(this SqlDataReader reader)
        {
            var vehicleList = new List<VehiclePutModel>();
            while (reader.Read())
            {
                vehicleList.Add(TranslateAsPutVehicle(reader, true));
            }

            return vehicleList;
        }

        public static VehicleSaveResponseModel TranslateAsVehicleSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var vehicleSave = new VehicleSaveResponseModel();

            if (reader.IsColumnExists("VehicleID"))
                vehicleSave.VehicleID = SqlHelper.GetNullableInt32(reader, "VehicleID");

            if (reader.IsColumnExists("ReferenceNumber"))
                vehicleSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            return vehicleSave;
        }

        public static VehicleSaveResponseModel TranslateAsVehicleSaveResponseList(this SqlDataReader reader)
        {
            var vehicleSaveResponse = new VehicleSaveResponseModel();
            while (reader.Read())
            {
                vehicleSaveResponse = TranslateAsVehicleSaveResponse(reader, true);
            }

            return vehicleSaveResponse;
        }

        public static VehiclesReport TranslateAsVehiclesReport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var report = new VehiclesReport();

            if (reader.IsColumnExists("PlateNumber"))
                report.PlateNumber = SqlHelper.GetNullableString(reader, "PlateNumber");

            if (reader.IsColumnExists("PlateColor"))
                report.PlateColor = SqlHelper.GetNullableString(reader, "PlateColor");

            if (reader.IsColumnExists("VehicleModel"))
                report.VehicleModel = SqlHelper.GetNullableString(reader, "VehicleModel");

            if (reader.IsColumnExists("NextService"))
                report.NextService = SqlHelper.GetNullableInt32(reader, "NextService");

            if (reader.IsColumnExists("TyreChange"))
                report.TyreChange = SqlHelper.GetNullableInt32(reader, "TyreChange");

            if (reader.IsColumnExists("ContractEndDate"))
            {
                DateTime? date = SqlHelper.GetDateTime(reader, "ContractEndDate");
                report.ContractEndDate = date == null ? string.Empty : date?.ToString("dd-MM-yyyy").Replace("-", "/");
            }

            if (reader.IsColumnExists("VehicleRegistrationExpiry"))
            {
                DateTime? date = SqlHelper.GetDateTime(reader, "VehicleRegistrationExpiry");
                report.VehicleRegistrationExpiry = date == null ? string.Empty : date?.ToString("dd-MM-yyyy").Replace("-", "/");
            }

            return report;
        }

        public static List<VehiclesReport> TranslateAsVehiclesReportList(this SqlDataReader reader)
        {
            var list = new List<VehiclesReport>();
            while (reader.Read())
            {
                list.Add(TranslateAsVehiclesReport(reader, true));
            }

            return list;
        }

        public static string TranslateAsGetString(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            string temp = string.Empty;

            if (reader.IsColumnExists("DisplayName"))
                temp = SqlHelper.GetNullableString(reader, "DisplayName");

            return temp;
        }

        public static List<string> TranslateAsVehicleStringList(this SqlDataReader reader)
        {
            var vehicleGetList = new List<string>();
            while (reader.Read())
            {
                vehicleGetList.Add(TranslateAsGetString(reader, true));
            }

            return vehicleGetList;
        }

        public static VehicleLogServicePost TranslateAsGetService(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            VehicleLogServicePost temp = new VehicleLogServicePost();

            if (reader.IsColumnExists("VehicleID"))
                temp.VehicleID = SqlHelper.GetNullableInt32(reader, "VehicleID");

            if (reader.IsColumnExists("CurrentMileage"))
                temp.CurrentMileage = SqlHelper.GetNullableInt32(reader, "CurrentMileage");

            if (reader.IsColumnExists("NextMileage"))
                temp.NextMileage = SqlHelper.GetNullableInt32(reader, "NextMileage");

            if (reader.IsColumnExists("LogType"))
                temp.LogType = SqlHelper.GetNullableInt32(reader, "LogType");

            if (reader.IsColumnExists("CreatedDateTime"))
                temp.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("CreatedBy"))
                temp.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("Date"))
                temp.Date = SqlHelper.GetNullableString(reader, "Date");

            return temp;
        }

        public static List<VehicleLogServicePost> TranslateAsVehicleServiceLog(this SqlDataReader reader)
        {
            var vehicleGetList = new List<VehicleLogServicePost>();
            while (reader.Read())
            {
                vehicleGetList.Add(TranslateAsGetService(reader, true));
            }

            return vehicleGetList;
        }

        public static VehicleLogServiceReportModel TranslateAsGetServiceLog(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            VehicleLogServiceReportModel temp = new VehicleLogServiceReportModel();

            if (reader.IsColumnExists("CurrentMileage"))
                temp.CurrentMileage = SqlHelper.GetNullableInt32(reader, "CurrentMileage");

            if (reader.IsColumnExists("NextMileage"))
                temp.NextMileage = SqlHelper.GetNullableInt32(reader, "NextMileage");

            if (reader.IsColumnExists("ServiceType"))
                temp.ServiceType = SqlHelper.GetNullableString(reader, "ServiceType");

            if (reader.IsColumnExists("Date"))
            {
               DateTime? date = SqlHelper.GetDateTime(reader, "Date");
               temp.Date = date == null ? string.Empty : date?.ToString("dd-MM-yyyy").Replace("-", "/");
            }

            return temp;
        }

        public static List<VehicleLogServiceReportModel> TranslateAsVehicleServiceLogList(this SqlDataReader reader)
        {
            var vehicleGetList = new List<VehicleLogServiceReportModel>();
            while (reader.Read())
            {
                vehicleGetList.Add(TranslateAsGetServiceLog(reader, true));
            }

            return vehicleGetList;
        }
    }
}