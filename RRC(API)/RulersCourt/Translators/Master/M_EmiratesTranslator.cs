using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_EmiratesTranslator
    {
        public static M_EmiratesModel TranslateAsGetEmirates(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var emirates = new M_EmiratesModel();

            if (reader.IsColumnExists("EmiratesID"))
                emirates.EmiratesID = SqlHelper.GetNullableInt32(reader, "EmiratesID");

            if (reader.IsColumnExists("EmiratesName"))
                emirates.EmiratesName = SqlHelper.GetNullableString(reader, "EmiratesName");

            if (reader.IsColumnExists("DisplayOrder"))
                emirates.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return emirates;
        }

        public static List<M_EmiratesModel> TranslateAsEmirates(this SqlDataReader reader)
        {
            var emirates = new List<M_EmiratesModel>();
            while (reader.Read())
            {
                emirates.Add(TranslateAsGetEmirates(reader, true));
            }

            return emirates;
        }
    }
}
