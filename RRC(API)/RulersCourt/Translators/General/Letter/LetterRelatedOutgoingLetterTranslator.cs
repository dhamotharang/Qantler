using RulersCourt.Models.General.Letter;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.General.Letter
{
    public static class LetterRelatedOutgoingLetterTranslator
    {
        public static LetterRelatedOutgoingLetterModel TranslateAsGetRelatedLetter(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var releatedLetter = new LetterRelatedOutgoingLetterModel();

            if (reader.IsColumnExists("ReferenceNo"))
                releatedLetter.ReferenceNo = SqlHelper.GetNullableString(reader, "ReferenceNo");

            if (reader.IsColumnExists("LetterID"))
                releatedLetter.LetterID = SqlHelper.GetNullableString(reader, "LetterID");

            return releatedLetter;
        }

        public static List<LetterRelatedOutgoingLetterModel> TranslateAsRelatedLetterList(this SqlDataReader reader)
        {
            var relatedLetterList = new List<LetterRelatedOutgoingLetterModel>();
            while (reader.Read())
            {
                relatedLetterList.Add(TranslateAsGetRelatedLetter(reader, true));
            }

            return relatedLetterList;
        }
    }
}
