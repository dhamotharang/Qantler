using RulersCourt.Models.Vehicle.VehicleFine;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Translators.Vehicle
{
    public static class VehicleFineTranslator
    {
        public static VehicleFineGetModel TranslateAsGetFineVehicle(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var vehicleGetModel = new VehicleFineGetModel();

            if (reader.IsColumnExists("VehicleFineID"))
                vehicleGetModel.VehicleFineID = SqlHelper.GetNullableInt32(reader, "VehicleFineID");

            if (reader.IsColumnExists("ReferenceNumber"))
                vehicleGetModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("VehicleID"))
                vehicleGetModel.VehicleID = SqlHelper.GetNullableInt32(reader, "VehicleID");

            if (reader.IsColumnExists("VehicleModelID"))
                vehicleGetModel.VehicleModelID = SqlHelper.GetNullableString(reader, "VehicleModelID");

            if (reader.IsColumnExists("FineNumber"))
                vehicleGetModel.FineNumber = SqlHelper.GetNullableString(reader, "FineNumber");

            if (reader.IsColumnExists("Location"))
                vehicleGetModel.Location = SqlHelper.GetNullableString(reader, "Location");

            if (reader.IsColumnExists("BlackPoints"))
                vehicleGetModel.BlackPoints = SqlHelper.GetNullableInt32(reader, "BlackPoints");

            if (reader.IsColumnExists("Status"))
                vehicleGetModel.Status = SqlHelper.GetNullableInt32(reader, "Status");

            if (reader.IsColumnExists("Description"))
                vehicleGetModel.Description = SqlHelper.GetNullableString(reader, "Description");

            if (reader.IsColumnExists("EmailTo"))
                vehicleGetModel.EmailTo = SqlHelper.GetNullableString(reader, "EmailTo");

            if (reader.IsColumnExists("FinedDate"))
                vehicleGetModel.FinedDate = SqlHelper.GetDateTime(reader, "FinedDate");

            if (reader.IsColumnExists("EmailCCDepartmentID"))
                vehicleGetModel.EmailCCDepartmentID = SqlHelper.GetNullableInt32(reader, "EmailCCDepartmentID");

            if (reader.IsColumnExists("EmailCCUserID"))
                vehicleGetModel.EmailCCUserID = SqlHelper.GetNullableInt32(reader, "EmailCCUserID");

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

            if (reader.IsColumnExists("DriverID"))
                vehicleGetModel.DriverID = SqlHelper.GetNullableInt32(reader, "DriverID");

            return vehicleGetModel;
        }

        public static List<VehicleFineGetModel> TranslateAsGetFineCarListVehicle(this SqlDataReader reader)
        {
            var vehicleList = new List<VehicleFineGetModel>();
            while (reader.Read())
            {
                vehicleList.Add(TranslateAsGetFineVehicle(reader, true));
            }

            return vehicleList;
        }

        public static VehicleFineGetList TranslateAsGetFineListModelVehicle(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var vehicleGetListModel = new VehicleFineGetList();

            if (reader.IsColumnExists("PlateNumber"))
                vehicleGetListModel.PlateNumber = SqlHelper.GetNullableString(reader, "PlateNumber");

            if (reader.IsColumnExists("IssuedAgainstDepartment"))
                vehicleGetListModel.IssuedAgainstDepartment = SqlHelper.GetNullableString(reader, "IssuedAgainstDepartment");

            if (reader.IsColumnExists("IssuedAgainstName"))
                vehicleGetListModel.IssuedAgainstName = SqlHelper.GetNullableString(reader, "IssuedAgainstName");

            if (reader.IsColumnExists("Status"))
                vehicleGetListModel.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("Time"))
                vehicleGetListModel.Time = SqlHelper.GetDateTime(reader, "Time");

            if (reader.IsColumnExists("VehicleFineID"))
                vehicleGetListModel.VehicleFineID = SqlHelper.GetNullableInt32(reader, "VehicleFineID");

            if (reader.IsColumnExists("DriverID"))
                vehicleGetListModel.DriverID = SqlHelper.GetNullableInt32(reader, "DriverID");

            if (reader.IsColumnExists("DriverMailID"))
                vehicleGetListModel.DriverMailID = SqlHelper.GetNullableString(reader, "DriverMailID");

            return vehicleGetListModel;
        }

        public static List<VehicleFineGetList> TranslateAsVehicleGetFineModelList(this SqlDataReader reader)
        {
            var vehicleGetList = new List<VehicleFineGetList>();
            while (reader.Read())
            {
                vehicleGetList.Add(TranslateAsGetFineListModelVehicle(reader, true));
            }

            return vehicleGetList;
        }

        public static VehicleFineGetReportList TranslateAsVehiclesFineReport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var report = new VehicleFineGetReportList();

            if (reader.IsColumnExists("PlateNumber"))
                report.PlateNumber = SqlHelper.GetNullableString(reader, "PlateNumber");

            if (reader.IsColumnExists("IssuedAgainstDepartment"))
                report.IssuedAgainstDepartment = SqlHelper.GetNullableString(reader, "IssuedAgainstDepartment");

            if (reader.IsColumnExists("IssuedAgainstName"))
                report.IssuedAgainstName = SqlHelper.GetNullableString(reader, "IssuedAgainstName");

            if (reader.IsColumnExists("Status"))
                report.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("Time"))
            {
                DateTime? date = SqlHelper.GetDateTime(reader, "Time");
                report.Time = date == null ? string.Empty : date?.ToString("dd-MM-yyyy").Replace("-", "/");
            }

            return report;
        }

        public static List<VehicleFineGetReportList> TranslateAsVehiclesFineReportList(this SqlDataReader reader)
        {
            var list = new List<VehicleFineGetReportList>();
            while (reader.Read())
            {
                list.Add(TranslateAsVehiclesFineReport(reader, true));
            }

            return list;
        }

        public static VehicleFinePutModel TranslateAsPutFineCar(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var vehicleFinePutModel = new VehicleFinePutModel();

            if (reader.IsColumnExists("VehicleFineID"))
                vehicleFinePutModel.VehicleFineID = SqlHelper.GetNullableInt32(reader, "VehicleFineID");

            if (reader.IsColumnExists("VehicleModelID"))
                vehicleFinePutModel.VehicleModelID = SqlHelper.GetNullableString(reader, "VehicleModelID");

            if (reader.IsColumnExists("BlackPoints"))
                vehicleFinePutModel.BlackPoints = SqlHelper.GetNullableInt32(reader, "BlackPoints");

            if (reader.IsColumnExists("Status"))
                vehicleFinePutModel.Status = SqlHelper.GetNullableInt32(reader, "Status");

            if (reader.IsColumnExists("EmailCCDepartmentID"))
                vehicleFinePutModel.EmailCCDepartmentID = SqlHelper.GetNullableInt32(reader, "EmailCCDepartmentID");

            if (reader.IsColumnExists("EmailCCUserID"))
                vehicleFinePutModel.EmailCCUserID = SqlHelper.GetNullableInt32(reader, "EmailCCUserID");

            if (reader.IsColumnExists("Description"))
                vehicleFinePutModel.Description = SqlHelper.GetNullableString(reader, "Description");

            if (reader.IsColumnExists("EmailTo"))
                vehicleFinePutModel.EmailTo = SqlHelper.GetNullableString(reader, "EmailTo");

            if (reader.IsColumnExists("FineNumber"))
                vehicleFinePutModel.FineNumber = SqlHelper.GetNullableString(reader, "FineNumber");

            if (reader.IsColumnExists("FinedDate"))
                vehicleFinePutModel.FinedDate = SqlHelper.GetDateTime(reader, "FinedDate");

            if (reader.IsColumnExists("DeleteFlag"))
                vehicleFinePutModel.DeleteFlag = SqlHelper.GetBoolean(reader, "DeleteFlag");

            if (reader.IsColumnExists("UpdatedDateTime"))
                vehicleFinePutModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("UpdatedBy"))
                vehicleFinePutModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("Location"))
                vehicleFinePutModel.Location = SqlHelper.GetNullableString(reader, "Location");

            return vehicleFinePutModel;
        }

        public static List<VehicleFinePutModel> TranslateAsPutFineCarList(this SqlDataReader reader)
        {
            var carCompanyList = new List<VehicleFinePutModel>();
            while (reader.Read())
            {
                carCompanyList.Add(TranslateAsPutFineCar(reader, true));
            }

            return carCompanyList;
        }
    }
}