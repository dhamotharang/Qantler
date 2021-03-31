using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class GlobalSearchListTranslator
    {
        public static GlobalSearchModel TranslateAsGetList(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var globalSarch = new GlobalSearchModel();

            if (reader.IsColumnExists("ID"))
                globalSarch.ID = SqlHelper.GetNullableInt32(reader, "ID");

            if (reader.IsColumnExists("ReferenceNumber"))
                globalSarch.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Title"))
                globalSarch.Title = SqlHelper.GetNullableString(reader, "Title");

            if (reader.IsColumnExists("Type"))
                globalSarch.Type = SqlHelper.GetNullableInt32(reader, "Type");

            if (reader.IsColumnExists("CanEdit"))
                globalSarch.CanEdit = SqlHelper.GetBoolean(reader, "CanEdit");
            return globalSarch;
        }

        public static List<GlobalSearchModel> TranslateAsGlobalSearchList(this SqlDataReader reader)
        {
            var list = new List<GlobalSearchModel>();
            while (reader.Read())
            {
                list.Add(TranslateAsGetList(reader, true));
            }

            return list;
        }
    }
}
