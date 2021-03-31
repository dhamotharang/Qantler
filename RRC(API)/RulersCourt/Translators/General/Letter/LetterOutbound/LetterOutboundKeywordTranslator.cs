using RulersCourt.Models.Letter;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Letter
{
    public static class LetterOutboundKeywordTranslator
    {
        public static LetterOutboundKeywordsModel TranslateAsGetLetterKeyword(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var keyword = new LetterOutboundKeywordsModel();

            if (reader.IsColumnExists("Keywords"))
                keyword.Keywords = SqlHelper.GetNullableString(reader, "Keywords");

            return keyword;
        }

        public static List<LetterOutboundKeywordsModel> TranslateAsLetterKeywordsList(this SqlDataReader reader)
        {
            var keywords = new List<LetterOutboundKeywordsModel>();
            while (reader.Read())
            {
                keywords.Add(TranslateAsGetLetterKeyword(reader, true));
            }

            return keywords;
        }
    }
}
