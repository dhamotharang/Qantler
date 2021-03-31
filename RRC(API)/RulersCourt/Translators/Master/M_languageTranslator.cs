using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_LanguageTranslator
    {
        public static M_LanguageModel TranslateAsGetLanguage(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var language = new M_LanguageModel();

            if (reader.IsColumnExists("LanguageID"))
                language.LanguageID = SqlHelper.GetNullableInt32(reader, "LanguageID");

            if (reader.IsColumnExists("LanguageName"))
                language.LanguageName = SqlHelper.GetNullableString(reader, "LanguageName");

            if (reader.IsColumnExists("DisplayOrder"))
                language.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return language;
        }

        public static List<M_LanguageModel> TranslateAsLanguage(this SqlDataReader reader)
        {
            var language = new List<M_LanguageModel>();
            while (reader.Read())
            {
                language.Add(TranslateAsGetLanguage(reader, true));
            }

            return language;
        }
    }
}
