using RulersCourt.Models.Letter.LetterInbound;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Letter.LetterInbound
{
    public static class LetterInboundRelatedOutgoingTranslator
    {
        public static LetterInboundRelatedOutgoingModel TranslateAsGetLetterInboundRefNO(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var outgoingRefNo = new LetterInboundRelatedOutgoingModel();

            if (reader.IsColumnExists("OutgoingLetterReferenceNo"))
                outgoingRefNo.OutgoingLetterReferenceNo = SqlHelper.GetNullableString(reader, "OutgoingLetterReferenceNo");

            if (reader.IsColumnExists("LetterID"))
                outgoingRefNo.LetterID = SqlHelper.GetNullableString(reader, "LetterID");

            return outgoingRefNo;
        }

        public static List<LetterInboundRelatedOutgoingModel> TranslateAsLetterInboundRefNoList(this SqlDataReader reader)
        {
            var outgoingReferenceNumbers = new List<LetterInboundRelatedOutgoingModel>();
            while (reader.Read())
            {
                outgoingReferenceNumbers.Add(TranslateAsGetLetterInboundRefNO(reader, true));
            }

            return outgoingReferenceNumbers;
        }
    }
}
