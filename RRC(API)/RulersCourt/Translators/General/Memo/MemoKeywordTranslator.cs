using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class MemoKeywordTranslator
    {
        public static MemoKeywordsModel TranslateAsGetKeyword(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var keyword = new MemoKeywordsModel();

            if (reader.IsColumnExists("Keywords"))
                keyword.Keywords = SqlHelper.GetNullableString(reader, "Keywords");

            if (reader.IsColumnExists("UserID"))
                keyword.UserID = SqlHelper.GetNullableInt32(reader, "UserID");

            return keyword;
        }

        public static List<MemoKeywordsModel> TranslateAsKeywordsList(this SqlDataReader reader)
        {
            var keywords = new List<MemoKeywordsModel>();
            while (reader.Read())
            {
                keywords.Add(TranslateAsGetKeyword(reader, true));
            }

            return keywords;
        }
    }
}
