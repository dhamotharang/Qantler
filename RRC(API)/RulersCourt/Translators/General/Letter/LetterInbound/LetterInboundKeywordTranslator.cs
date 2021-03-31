using RulersCourt.Models.Letter.LetterInbound;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Letter.LetterInbound
{
    public static class LetterInboundKeywordTranslator
    {
        public static LetterInboundKeywordsModel TranslateAsGetLetterInboundKeyword(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var keyword = new LetterInboundKeywordsModel();

            if (reader.IsColumnExists("Keywords"))
                keyword.Keywords = SqlHelper.GetNullableString(reader, "Keywords");

            return keyword;
        }

        public static List<LetterInboundKeywordsModel> TranslateAsLetterInboundKeywordsList(this SqlDataReader reader)
        {
            var keywords = new List<LetterInboundKeywordsModel>();
            while (reader.Read())
            {
                keywords.Add(TranslateAsGetLetterInboundKeyword(reader, true));
            }

            return keywords;
        }
    }
}
