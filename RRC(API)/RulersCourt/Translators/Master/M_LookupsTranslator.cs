using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_LookupsTranslator
    {
        public static M_LookupsModel TranslateAsM_Lookups(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var m_LookupsModel = new M_LookupsModel();

            if (reader.IsColumnExists("LookupsID"))
                m_LookupsModel.LookupsID = SqlHelper.GetNullableInt32(reader, "LookupsID");

            if (reader.IsColumnExists("DisplayName"))
                m_LookupsModel.DisplayName = SqlHelper.GetNullableString(reader, "DisplayName");

            if (reader.IsColumnExists("DisplayOrder"))
                m_LookupsModel.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return m_LookupsModel;
        }

        public static List<M_LookupsModel> TranslateAsM_LookupsList(this SqlDataReader reader)
        {
            var m_LookupsList = new List<M_LookupsModel>();
            while (reader.Read())
            {
                m_LookupsList.Add(TranslateAsM_Lookups(reader, true));
            }

            return m_LookupsList;
        }
    }
}
