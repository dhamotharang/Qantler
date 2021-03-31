using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_TitleTranslator
    {
        public static M_TitleModel TranslateAsGetTitle(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var title = new M_TitleModel();

            if (reader.IsColumnExists("TitleID"))
                title.TitleID = SqlHelper.GetNullableInt32(reader, "TitleID");

            if (reader.IsColumnExists("TitleName"))
                title.TitleName = SqlHelper.GetNullableString(reader, "TitleName");

            if (reader.IsColumnExists("DisplayOrder"))
                title.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return title;
        }

        public static List<M_TitleModel> TranslateAsTitle(this SqlDataReader reader)
        {
            var titleList = new List<M_TitleModel>();
            while (reader.Read())
            {
                titleList.Add(TranslateAsGetTitle(reader, true));
            }

            return titleList;
        }
    }
}
