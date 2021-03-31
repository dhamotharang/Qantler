using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_CityTranslator
    {
        public static M_CityModel TranslateAsGetCity(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var city = new M_CityModel();

            if (reader.IsColumnExists("CityID"))
                city.CityID = SqlHelper.GetNullableInt32(reader, "CityID");

            if (reader.IsColumnExists("CityName"))
                city.CityName = SqlHelper.GetNullableString(reader, "CityName");

            if (reader.IsColumnExists("DisplayOrder"))
                city.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return city;
        }

        public static List<M_CityModel> TranslateAsCity(this SqlDataReader reader)
        {
            var citys = new List<M_CityModel>();
            while (reader.Read())
            {
                citys.Add(TranslateAsGetCity(reader, true));
            }

            return citys;
        }
    }
}
