using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_CountryTranslator
    {
        public static M_CountryModel TranslateAsGetCountry(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var country = new M_CountryModel();

            if (reader.IsColumnExists("CountryID"))
                country.CountryID = SqlHelper.GetNullableInt32(reader, "CountryID");

            if (reader.IsColumnExists("CountryName"))
                country.CountryName = SqlHelper.GetNullableString(reader, "CountryName");

            if (reader.IsColumnExists("DisplayOrder"))
                country.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return country;
        }

        public static List<M_CountryModel> TranslateAsCountry(this SqlDataReader reader)
        {
            var countrys = new List<M_CountryModel>();
            while (reader.Read())
            {
                countrys.Add(TranslateAsGetCountry(reader, true));
            }

            return countrys;
        }
    }
}
