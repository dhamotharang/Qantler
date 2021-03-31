using RulersCourt.Models.Master.M_Vehicle;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Master.Vehicle
{
    public static class M_TripReleaseLocationTranslator
    {
        public static M_TripReleaseLocationModel TranslateAsGetTripReleaseLocation(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var tripReleaseLocation = new M_TripReleaseLocationModel();

            if (reader.IsColumnExists("TripReleaseLocationID"))
                tripReleaseLocation.TripReleaseLocationID = SqlHelper.GetNullableInt32(reader, "TripReleaseLocationID");

            if (reader.IsColumnExists("TripReleaseLocationName"))
                tripReleaseLocation.TripReleaseLocationName = SqlHelper.GetNullableString(reader, "TripReleaseLocationName");

            if (reader.IsColumnExists("DisplayOrder"))
                tripReleaseLocation.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return tripReleaseLocation;
        }

        public static List<M_TripReleaseLocationModel> TranslateAsTripReleaseLocation(this SqlDataReader reader)
        {
            var tripReleaseLocation = new List<M_TripReleaseLocationModel>();
            while (reader.Read())
            {
                tripReleaseLocation.Add(TranslateAsGetTripReleaseLocation(reader, true));
            }

            return tripReleaseLocation;
        }
    }
}
