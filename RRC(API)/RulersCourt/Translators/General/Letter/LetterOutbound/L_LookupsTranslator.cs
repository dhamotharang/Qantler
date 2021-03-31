using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Letter
{
    public static class L_LookupsTranslator
    {
        public static M_LookupsModel TranslateAsL_Lookups(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var l_LookupsModel = new M_LookupsModel();

            if (reader.IsColumnExists("LookupsID"))
                l_LookupsModel.LookupsID = SqlHelper.GetNullableInt32(reader, "LookupsID");

            if (reader.IsColumnExists("DisplayName"))
                l_LookupsModel.DisplayName = SqlHelper.GetNullableString(reader, "DisplayName");

            if (reader.IsColumnExists("DisplayOrder"))
                l_LookupsModel.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return l_LookupsModel;
        }

        public static List<M_LookupsModel> TranslateAsL_LookupsList(this SqlDataReader reader)
        {
            var l_LookupsList = new List<M_LookupsModel>();
            while (reader.Read())
            {
                l_LookupsList.Add(TranslateAsL_Lookups(reader, true));
            }

            return l_LookupsList;
        }
    }
}
