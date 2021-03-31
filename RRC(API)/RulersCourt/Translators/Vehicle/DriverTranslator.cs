using RulersCourt.Models.Vehicle.Drivers;
using RulersCourt.Models.Vehicle.Vehicles;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Vehicle.DriverVehicles
{
    public static class DriverTranslator
    {
        public static DriverGetModel TranslateAsGetDriver(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var driverGetModel = new DriverGetModel();

            if (reader.IsColumnExists("DriverID"))
                driverGetModel.DriverID = SqlHelper.GetNullableInt32(reader, "DriverID");

            if (reader.IsColumnExists("UserProfileID"))
                driverGetModel.UserProfileID = SqlHelper.GetNullableInt32(reader, "UserProfileID");

            if (reader.IsColumnExists("DriverName"))
                driverGetModel.DriverName = SqlHelper.GetNullableString(reader, "DriverName");

            if (reader.IsColumnExists("TotalHour"))
                driverGetModel.TotalHour = SqlHelper.GetNullableInt32(reader, "TotalHour");

            if (reader.IsColumnExists("MobileNumber"))
                driverGetModel.MobileNumber = SqlHelper.GetNullableString(reader, "MobileNumber");

            return driverGetModel;
        }

        public static List<DriverGetModel> TranslateAsDriverList(this SqlDataReader reader)
        {
            var driverList = new List<DriverGetModel>();
            while (reader.Read())
            {
                driverList.Add(TranslateAsGetDriver(reader, true));
            }

            return driverList;
        }

        public static DriverPutModel TranslateAsPutDriver(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var driverModel = new DriverPutModel();

            if (reader.IsColumnExists("DriverID"))
                driverModel.DriverID = SqlHelper.GetNullableInt32(reader, "DriverID");

            if (reader.IsColumnExists("UserProfileID"))
                driverModel.UserProfileID = SqlHelper.GetNullableInt32(reader, "UserProfileID");

            if (reader.IsColumnExists("LogDate"))
                driverModel.LogDate = SqlHelper.GetDateTime(reader, "LogDate");

            if (reader.IsColumnExists("ExtraHours"))
                driverModel.ExtraHours = SqlHelper.GetLong(reader, "ExtraHours");

            if (reader.IsColumnExists("CompensateHours"))
                driverModel.CompensateHours = SqlHelper.GetLong(reader, "CompensateHours");

            if (reader.IsColumnExists("DeleteFlag"))
                driverModel.DeleteFlag = SqlHelper.GetBoolean(reader, "DeleteFlag");

            if (reader.IsColumnExists("UpdatedBy"))
                driverModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                driverModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            return driverModel;
        }

        public static List<DriverPutModel> TranslateAsPutDriverList(this SqlDataReader reader)
        {
            var driverList = new List<DriverPutModel>();
            while (reader.Read())
            {
                driverList.Add(TranslateAsPutDriver(reader, true));
            }

            return driverList;
        }

        public static DriverSaveResponseModel TranslateAsDriverSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var driverSave = new DriverSaveResponseModel();

            if (reader.IsColumnExists("DriverID"))
                driverSave.DriverID = SqlHelper.GetNullableInt32(reader, "DriverID");

            return driverSave;
        }

        public static DriverSaveResponseModel TranslateAsDriverSaveResponseList(this SqlDataReader reader)
        {
            var driverSaveResponse = new DriverSaveResponseModel();
            while (reader.Read())
            {
                driverSaveResponse = TranslateAsDriverSaveResponse(reader, true);
            }

            return driverSaveResponse;
        }

        public static DriverReport TranslateAsDriverReport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var report = new DriverReport();

            if (reader.IsColumnExists("LogDate"))
                report.LogDate = SqlHelper.GetDateTime(reader, "LogDate");

            if (reader.IsColumnExists("ExtraHours"))
                report.ExtraHours = SqlHelper.GetNullableInt32(reader, "ExtraHours");

            if (reader.IsColumnExists("CompensateHours"))
                report.CompensateHours = SqlHelper.GetNullableInt32(reader, "CompensateHours");

            if (reader.IsColumnExists("BalanceExtraHours"))
                report.BalanceExtraHours = SqlHelper.GetNullableInt32(reader, "BalanceExtraHours");

            return report;
        }

        public static List<DriverReport> TranslateAsCarDriverReportList(this SqlDataReader reader)
        {
            var list = new List<DriverReport>();
            while (reader.Read())
            {
                list.Add(TranslateAsDriverReport(reader, true));
            }

            return list;
        }

        public static DriverGetMaster TranslateAsGetMasterDriver(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var driverGetModel = new DriverGetMaster();

            if (reader.IsColumnExists("DriverID"))
                driverGetModel.DriverID = SqlHelper.GetNullableInt32(reader, "DriverID");

            if (reader.IsColumnExists("DriverName"))
                driverGetModel.DriverName = SqlHelper.GetNullableString(reader, "DriverName");

            return driverGetModel;
        }

        public static List<DriverGetMaster> TranslateAsMasterDriverList(this SqlDataReader reader)
        {
            var driverList = new List<DriverGetMaster>();
            while (reader.Read())
            {
                driverList.Add(TranslateAsGetMasterDriver(reader, true));
            }

            return driverList;
        }

        public static DriverTrips TranslateAsGetDriverTrip(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var driverTrip = new DriverTrips();

            if (reader.IsColumnExists("City"))
                driverTrip.City = SqlHelper.GetNullableString(reader, "City");

            if (reader.IsColumnExists("Destination"))
                driverTrip.Destination = SqlHelper.GetNullableString(reader, "Destination");

            if (reader.IsColumnExists("Requestor"))
                driverTrip.Requestor = SqlHelper.GetNullableString(reader, "Requestor");

            if (reader.IsColumnExists("TripTimeFrom"))
                driverTrip.TripTimeFrom = SqlHelper.GetDateTime(reader, "TripTimeFrom");

            if (reader.IsColumnExists("TripTimeTo"))
                driverTrip.TripTimeTo = SqlHelper.GetDateTime(reader, "TripTimeTo");

            return driverTrip;
        }

        public static List<DriverTrips> TranslateAsDriverTripList(this SqlDataReader reader)
        {
            var driverTripList = new List<DriverTrips>();
            while (reader.Read())
            {
                driverTripList.Add(TranslateAsGetDriverTrip(reader, true));
            }

            return driverTripList;
        }

        public static TripSameDayPeriodModel TranslateAsGetTrip(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var trip = new TripSameDayPeriodModel();

            if (reader.IsColumnExists("Requestor"))
                trip.Requestor = SqlHelper.GetNullableString(reader, "Requestor");

            if (reader.IsColumnExists("DriverName"))
                trip.DriverName = SqlHelper.GetNullableString(reader, "DriverName");

            if (reader.IsColumnExists("Destination"))
                trip.Destination = SqlHelper.GetNullableString(reader, "Destination");

            if (reader.IsColumnExists("CoPassenger"))
                trip.CoPassenger = SqlHelper.GetNullableString(reader, "CoPassenger");

            return trip;
        }

        public static List<TripSameDayPeriodModel> TranslateAsTripList(this SqlDataReader reader)
        {
            var driverTripList = new List<TripSameDayPeriodModel>();
            while (reader.Read())
            {
                driverTripList.Add(TranslateAsGetTrip(reader, true));
            }

            return driverTripList;
        }

        public static DriverGetTripDaysModel TranslateAsGetDriverTripDay(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var driverGetModel = new DriverGetTripDaysModel();

            if (reader.IsColumnExists("TripPeriodFrom"))
                driverGetModel.TripPeriodFrom = SqlHelper.GetDateTime(reader, "TripPeriodFrom");

            if (reader.IsColumnExists("TripPeriodTo"))
                driverGetModel.TripPeriodTo = SqlHelper.GetDateTime(reader, "TripPeriodTo");

            if (reader.IsColumnExists("TO"))
                driverGetModel.TO = SqlHelper.GetNullableString(reader, "TO");

            if (reader.IsColumnExists("With"))
                driverGetModel.With = SqlHelper.GetNullableString(reader, "With");

            return driverGetModel;
        }

        public static List<DriverGetTripDaysModel> TranslateAsGetDriverTripDays(this SqlDataReader reader)
        {
            var driverList = new List<DriverGetTripDaysModel>();
            while (reader.Read())
            {
                driverList.Add(TranslateAsGetDriverTripDay(reader, true));
            }

            return driverList;
        }
    }
}
