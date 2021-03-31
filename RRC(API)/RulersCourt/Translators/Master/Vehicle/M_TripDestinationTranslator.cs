using RulersCourt.Models.Master.M_Vehicle;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Master.Vehicle
{
    public static class M_TripDestinationTranslator
    {
        public static M_TripDestinationModel TranslateAsGetTripDestination(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var tripDestination = new M_TripDestinationModel();

            if (reader.IsColumnExists("TripDestinationID"))
                tripDestination.TripDestinationID = SqlHelper.GetNullableInt32(reader, "TripDestinationID");

            if (reader.IsColumnExists("TripDestinationName"))
                tripDestination.TripDestinationName = SqlHelper.GetNullableString(reader, "TripDestinationName");

            if (reader.IsColumnExists("DisplayOrder"))
                tripDestination.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return tripDestination;
        }

        public static List<M_TripDestinationModel> TranslateAsTripDestination(this SqlDataReader reader)
        {
            var tripDestination = new List<M_TripDestinationModel>();
            while (reader.Read())
            {
                tripDestination.Add(TranslateAsGetTripDestination(reader, true));
            }

            return tripDestination;
        }
    }
}
