using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_NationalityTranslator
    {
        public static M_NationalityModel TranslateAsGetNationality(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var nationality = new M_NationalityModel();

            if (reader.IsColumnExists("NationalityID"))
                nationality.NationalityID = SqlHelper.GetNullableInt32(reader, "NationalityID");

            if (reader.IsColumnExists("NationalityName"))
                nationality.NationalityName = SqlHelper.GetNullableString(reader, "NationalityName");

            if (reader.IsColumnExists("DisplayOrder"))
                nationality.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return nationality;
        }

        public static List<M_NationalityModel> TranslateAsNationality(this SqlDataReader reader)
        {
            var nationalitys = new List<M_NationalityModel>();
            while (reader.Read())
            {
                nationalitys.Add(TranslateAsGetNationality(reader, true));
            }

            return nationalitys;
        }
    }
}
