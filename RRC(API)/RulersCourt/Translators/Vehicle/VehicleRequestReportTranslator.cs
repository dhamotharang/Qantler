using RulersCourt.Models.Vehicle.VehicleRequest;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Vehicle
{
    public static class VehicleRequestReportTranslator
    {
        public static VehicleRequestReportListModel TranslateAsVehicleReport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var vehicleReport = new VehicleRequestReportListModel();

            if (reader.IsColumnExists("ReferenceNumber"))
                vehicleReport.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Status"))
                vehicleReport.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("RequestType"))
                vehicleReport.RequestType = SqlHelper.GetNullableString(reader, "RequestType");

            if (reader.IsColumnExists("RequestorName"))
                vehicleReport.RequestorName = SqlHelper.GetNullableString(reader, "RequestorName");

            if (reader.IsColumnExists("RequestorOfficeDepartment"))
                vehicleReport.RequestorOfficeDepartment = SqlHelper.GetNullableString(reader, "RequestorOfficeDepartment");

            return vehicleReport;
        }

        public static List<VehicleRequestReportListModel> TranslateAsLegalReportList(this SqlDataReader reader)
        {
            var vehicleRequestList = new List<VehicleRequestReportListModel>();
            while (reader.Read())
            {
                vehicleRequestList.Add(TranslateAsVehicleReport(reader, true));
            }

            return vehicleRequestList;
        }

        public static VehicleHomeDashboardModel TranslateAsVehicleDashboardCount(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var vehiclehomemodel = new VehicleHomeDashboardModel();

            if (reader.IsColumnExists("Vehicle"))
            {
                vehiclehomemodel.Vehicle = SqlHelper.GetNullableInt32(reader, "Vehicle");
            }

            if (reader.IsColumnExists("Fine"))
            {
                vehiclehomemodel.Fine = SqlHelper.GetNullableInt32(reader, "Fine");
            }

            if (reader.IsColumnExists("Driver"))
            {
                vehiclehomemodel.Driver = SqlHelper.GetNullableInt32(reader, "Driver");
            }

            if (reader.IsColumnExists("OwnRequest"))
            {
                vehiclehomemodel.OwnRequest = SqlHelper.GetNullableInt32(reader, "OwnRequest");
            }

            if (reader.IsColumnExists("RentedCar"))
            {
                vehiclehomemodel.RentedCar = SqlHelper.GetNullableInt32(reader, "RentedCar");
            }

            if (reader.IsColumnExists("MyProcessedRequest"))
            {
                vehiclehomemodel.MyProcessedRequest = SqlHelper.GetNullableInt32(reader, "MyProcessedRequest");
            }

            if (reader.IsColumnExists("DriversOnTrip"))
            {
                vehiclehomemodel.DriversOnTrip = SqlHelper.GetNullableInt32(reader, "DriversOnTrip");
            }

            if (reader.IsColumnExists("DriversOffTrip"))
            {
                vehiclehomemodel.DriversOffTrip = SqlHelper.GetNullableInt32(reader, "DriversOffTrip");
            }

            if (reader.IsColumnExists("VehicleOnTrip"))
            {
                vehiclehomemodel.VehicleOnTrip = SqlHelper.GetNullableInt32(reader, "VehicleOnTrip");
            }

            if (reader.IsColumnExists("VehicleOffTrip"))
            {
                vehiclehomemodel.VehicleOffTrip = SqlHelper.GetNullableInt32(reader, "VehicleOffTrip");
            }

            return vehiclehomemodel;
        }

        public static VehicleHomeDashboardModel TranslateasvehicleDashboardcount(this SqlDataReader reader)
        {
            var vehiclehomemodel = new VehicleHomeDashboardModel();
            while (reader.Read())
            {
                vehiclehomemodel = TranslateAsVehicleDashboardCount(reader, true);
            }

            return vehiclehomemodel;
        }

        public static VehicleRequestAutoConfirmModel TranslateAsVehicle(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var vehicle = new VehicleRequestAutoConfirmModel();

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                vehicle.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("VehicleReqID"))
            {
                vehicle.VehicleReqID = SqlHelper.GetNullableInt32(reader, "VehicleReqID");
            }

            return vehicle;
        }

        public static List<VehicleRequestAutoConfirmModel> Translateasvehicles(this SqlDataReader reader)
        {
            var vehicles = new List<VehicleRequestAutoConfirmModel>();
            while (reader.Read())
            {
                vehicles.Add(TranslateAsVehicle(reader, true));
            }

            return vehicles;
        }
    }
}
