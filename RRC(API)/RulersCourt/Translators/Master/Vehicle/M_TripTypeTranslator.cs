using RulersCourt.Models.Master.M_Vehicle;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Master.Vehicle
{
    public static class M_TripTypeTranslator
    {
        public static M_TripTypeModel TranslateAsGetTripType(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var tripType = new M_TripTypeModel();

            if (reader.IsColumnExists("TripTypeID"))
                tripType.TripTypeID = SqlHelper.GetNullableInt32(reader, "TripTypeID");

            if (reader.IsColumnExists("TripTypeName"))
                tripType.TripTypeName = SqlHelper.GetNullableString(reader, "TripTypeName");

            if (reader.IsColumnExists("DisplayOrder"))
                tripType.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return tripType;
        }

        public static List<M_TripTypeModel> TranslateAsTripType(this SqlDataReader reader)
        {
            var tripType = new List<M_TripTypeModel>();
            while (reader.Read())
            {
                tripType.Add(TranslateAsGetTripType(reader, true));
            }

            return tripType;
        }
    }
}
