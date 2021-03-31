using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Legal
{
    public static class LegalKeywordsTranslator
    {
        public static LegalKeywordsModel TranslateAsGetLegalkeywords(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var getKeyword = new LegalKeywordsModel();

            if (reader.IsColumnExists("Keywords"))
                getKeyword.Keywords = SqlHelper.GetNullableString(reader, "keywords");

            return getKeyword;
        }

        public static List<LegalKeywordsModel> TranslateAsGetLegalkeywordsList(this SqlDataReader reader)
        {
            var keywordsList = new List<LegalKeywordsModel>();
            while (reader.Read())
            {
                keywordsList.Add(TranslateAsGetLegalkeywords(reader, true));
            }

            return keywordsList;
        }
    }
}