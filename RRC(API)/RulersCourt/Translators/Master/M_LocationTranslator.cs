using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_LocationTranslator
    {
        public static M_LocationModel TranslateAsGetLocation(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var location = new M_LocationModel();

            if (reader.IsColumnExists("LocationID"))
                location.LocationID = SqlHelper.GetNullableInt32(reader, "LocationID");

            if (reader.IsColumnExists("LocationName"))
                location.LocationName = SqlHelper.GetNullableString(reader, "LocationName");

            if (reader.IsColumnExists("DisplayOrder"))
                location.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return location;
        }

        public static List<M_LocationModel> TranslateAsLocation(this SqlDataReader reader)
        {
            var location = new List<M_LocationModel>();
            while (reader.Read())
            {
                location.Add(TranslateAsGetLocation(reader, true));
            }

            return location;
        }
    }
}
