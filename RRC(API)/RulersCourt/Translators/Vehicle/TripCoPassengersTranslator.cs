using RulersCourt.Models.Vehicle.TripCoPassengers;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Vehicle
{
    public static class TripCoPassengersTranslator
    {
        public static TripCoPassengersModel TranslateAsGetCoPassengers(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var coPassengers = new TripCoPassengersModel();

            if (reader.IsColumnExists("CoPassengerID"))
                coPassengers.CoPassengerID = SqlHelper.GetNullableInt32(reader, "CoPassengerID");

            if (reader.IsColumnExists("CoPassengerName"))
                coPassengers.CoPassengerName = SqlHelper.GetNullableString(reader, "CoPassengerName");

            if (reader.IsColumnExists("CoPassengerDepartment"))
                coPassengers.CoPassengerDepartment = SqlHelper.GetNullableString(reader, "CoPassengerDepartment");

            if (reader.IsColumnExists("CoPassengerDepartmentID"))
                coPassengers.CoPassengerDepartmentID = SqlHelper.GetNullableInt32(reader, "CoPassengerDepartmentID");

            if (reader.IsColumnExists("OtherCoPassengerName"))
                coPassengers.OtherCoPassengerName = SqlHelper.GetNullableString(reader, "OtherCoPassengerName");

            return coPassengers;
        }

        public static List<TripCoPassengersModel> TranslateAsCoPassengersUserList(this SqlDataReader reader)
        {
            var coPassengersUserList = new List<TripCoPassengersModel>();
            while (reader.Read())
            {
                coPassengersUserList.Add(TranslateAsGetCoPassengers(reader, true));
            }

            return coPassengersUserList;
        }
    }
}
