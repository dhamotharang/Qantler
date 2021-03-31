using RulersCourt.Models.Vehicle.CarCompany;
using RulersCourt.Models.Vehicle.VehicleFine;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Vehicle.Vehicles
{
    public static class CarCompanyTranslator
    {
        public static CarCompanyGetModel TranslateAsGetCarCompany(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var carCompanyGetModel = new CarCompanyGetModel();

            if (reader.IsColumnExists("CarCompanyID"))
                carCompanyGetModel.CarCompanyID = SqlHelper.GetNullableInt32(reader, "CarCompanyID");

            if (reader.IsColumnExists("CompanyName"))
                carCompanyGetModel.CompanyName = SqlHelper.GetNullableString(reader, "CompanyName");

            if (reader.IsColumnExists("ContactName"))
                carCompanyGetModel.ContactName = SqlHelper.GetNullableString(reader, "ContactName");

            if (reader.IsColumnExists("ContactNumber"))
                carCompanyGetModel.ContactNumber = SqlHelper.GetNullableString(reader, "ContactNumber");

            if (reader.IsColumnExists("CreatedBy"))
                carCompanyGetModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                carCompanyGetModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                carCompanyGetModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                carCompanyGetModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            return carCompanyGetModel;
        }

        public static List<CarCompanyGetModel> TranslateAsCarCompanyList(this SqlDataReader reader)
        {
            var carCompanyList = new List<CarCompanyGetModel>();
            while (reader.Read())
            {
                carCompanyList.Add(TranslateAsGetCarCompany(reader, true));
            }

            return carCompanyList;
        }

        public static CarCompanyPutModel TranslateAsPutCarCompany(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var carCompanyModel = new CarCompanyPutModel();

            if (reader.IsColumnExists("CarCompanyID"))
                carCompanyModel.CarCompanyID = SqlHelper.GetNullableInt32(reader, "CarCompanyID");

            if (reader.IsColumnExists("CompanyName"))
                carCompanyModel.CompanyName = SqlHelper.GetNullableString(reader, "CompanyName");

            if (reader.IsColumnExists("ContactName"))
                carCompanyModel.ContactName = SqlHelper.GetNullableString(reader, "ContactName");

            if (reader.IsColumnExists("ContactNumber"))
                carCompanyModel.ContactNumber = SqlHelper.GetNullableString(reader, "ContactNumber");

            if (reader.IsColumnExists("DeleteFlag"))
                carCompanyModel.DeleteFlag = SqlHelper.GetBoolean(reader, "DeleteFlag");

            if (reader.IsColumnExists("UpdatedDateTime"))
                carCompanyModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("UpdatedBy"))
                carCompanyModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                carCompanyModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            return carCompanyModel;
        }

        public static List<CarCompanyPutModel> TranslateAsPutCarCompanyList(this SqlDataReader reader)
        {
            var carCompanyList = new List<CarCompanyPutModel>();
            while (reader.Read())
            {
                carCompanyList.Add(TranslateAsPutCarCompany(reader, true));
            }

            return carCompanyList;
        }

        public static CarCompanySaveResponseModel TranslateAsCarCompanySaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var carCompanySave = new CarCompanySaveResponseModel();

            if (reader.IsColumnExists("CarCompanyID"))
                carCompanySave.CarCompanyID = SqlHelper.GetNullableInt32(reader, "CarCompanyID");

            return carCompanySave;
        }

        public static CarCompanySaveResponseModel TranslateAsCarCompanySaveResponseList(this SqlDataReader reader)
        {
            var carCompanySaveResponse = new CarCompanySaveResponseModel();
            while (reader.Read())
            {
                carCompanySaveResponse = TranslateAsCarCompanySaveResponse(reader, true);
            }

            return carCompanySaveResponse;
        }

        public static CarCompanyReport TranslateAsCarReport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var report = new CarCompanyReport();

            if (reader.IsColumnExists("CompanyName"))
                report.CompanyName = SqlHelper.GetNullableString(reader, "CompanyName");

            if (reader.IsColumnExists("ContactName"))
                report.ContactName = SqlHelper.GetNullableString(reader, "ContactName");

            if (reader.IsColumnExists("ContactNumber"))
                report.ContactNumber = SqlHelper.GetNullableString(reader, "ContactNumber");

            if (reader.IsColumnExists("CreatedDateTime"))
            {
                DateTime? date = SqlHelper.GetDateTime(reader, "CreatedDateTime");
                report.CreatedDateTime = date == null ? string.Empty : date?.ToString("dd-MM-yyyy").Replace("-", "/");
            }

            return report;
        }

        public static List<CarCompanyReport> TranslateAsCarReportList(this SqlDataReader reader)
        {
            var list = new List<CarCompanyReport>();
            while (reader.Read())
            {
                list.Add(TranslateAsCarReport(reader, true));
            }

            return list;
        }

        public static VehicleFineCarListModel TranslateAsGetVehicle(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var vehicle = new VehicleFineCarListModel();

            if (reader.IsColumnExists("PlateColour"))
                vehicle.PlateColour = SqlHelper.GetNullableString(reader, "PlateColour");

            if (reader.IsColumnExists("VehicleName"))
                vehicle.VehicleName = SqlHelper.GetNullableString(reader, "VehicleName");

            if (reader.IsColumnExists("VehicleMake"))
                vehicle.VehicleMake = SqlHelper.GetNullableString(reader, "VehicleMake");

            if (reader.IsColumnExists("VehicleID"))
                vehicle.VehicleID = SqlHelper.GetNullableString(reader, "VehicleID");

            if (reader.IsColumnExists("PlateNumber"))
                vehicle.PlateNumber = SqlHelper.GetNullableString(reader, "PlateNumber");

            if (reader.IsColumnExists("ModelName"))
                vehicle.ModelName = SqlHelper.GetNullableString(reader, "ModelName");

            if (reader.IsColumnExists("CurrentMileage"))
                vehicle.CurrentMileage = SqlHelper.GetNullableString(reader, "CurrentMileage");

            return vehicle;
        }

        public static List<VehicleFineCarListModel> TranslateAsVehicleList(this SqlDataReader reader)
        {
            var carCompanyList = new List<VehicleFineCarListModel>();
            while (reader.Read())
            {
                carCompanyList.Add(TranslateAsGetVehicle(reader, true));
            }

            return carCompanyList;
        }
    }
}
