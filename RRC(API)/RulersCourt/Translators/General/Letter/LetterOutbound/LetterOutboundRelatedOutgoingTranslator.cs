using RulersCourt.Models.Letter;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Letter
{
    public static class LetterOutboundRelatedOutgoingTranslator
    {
        public static LetterOutboundRelatedOutgoingModel TranslateAsGetLetterOutgoingRefNO(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var outgoingRefNo = new LetterOutboundRelatedOutgoingModel();

            if (reader.IsColumnExists("OutgoingLetterReferenceNo"))
                outgoingRefNo.OutgoingLetterReferenceNo = SqlHelper.GetNullableString(reader, "OutgoingLetterReferenceNo");

            if (reader.IsColumnExists("LetterID"))
                outgoingRefNo.LetterID = SqlHelper.GetNullableString(reader, "LetterID");

            return outgoingRefNo;
        }

        public static List<LetterOutboundRelatedOutgoingModel> TranslateAsLetterOutgoingRefNoList(this SqlDataReader reader)
        {
            var outgoingReferenceNumbers = new List<LetterOutboundRelatedOutgoingModel>();
            while (reader.Read())
            {
                outgoingReferenceNumbers.Add(TranslateAsGetLetterOutgoingRefNO(reader, true));
            }

            return outgoingReferenceNumbers;
        }
    }
}
