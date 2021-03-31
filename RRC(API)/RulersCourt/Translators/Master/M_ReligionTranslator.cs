using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_ReligionTranslator
    {
        public static M_ReligionModel TranslateAsGetReligion(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var religion = new M_ReligionModel();

            if (reader.IsColumnExists("ReligionID"))
                religion.ReligionID = SqlHelper.GetNullableInt32(reader, "ReligionID");

            if (reader.IsColumnExists("ReligionName"))
                religion.ReligionName = SqlHelper.GetNullableString(reader, "ReligionName");

            if (reader.IsColumnExists("DisplayOrder"))
                religion.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return religion;
        }

        public static List<M_ReligionModel> TranslateAsReligion(this SqlDataReader reader)
        {
            var religions = new List<M_ReligionModel>();
            while (reader.Read())
            {
                religions.Add(TranslateAsGetReligion(reader, true));
            }

            return religions;
        }
    }
}
