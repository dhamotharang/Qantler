using RulersCourt.Models.Letter.LetterInbound;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Letter.LetterInbound
{
    public static class LetterInboundSaveResponseTranslator
    {
        public static LetterInboundWorkflowModel TranslateAsLetterInboundSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var letterSave = new LetterInboundWorkflowModel();

            if (reader.IsColumnExists("LetterID"))
                letterSave.LetterID = SqlHelper.GetNullableInt32(reader, "LetterID");

            if (reader.IsColumnExists("ReferenceNumber"))
                letterSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                letterSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                letterSave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            if (reader.IsColumnExists("status"))
                letterSave.Status = SqlHelper.GetNullableInt32(reader, "status").GetValueOrDefault();

            return letterSave;
        }

        public static LetterInboundWorkflowModel TranslateAsLetterInboundSaveResponseList(this SqlDataReader reader)
        {
            var letterSaveResponse = new LetterInboundWorkflowModel();
            while (reader.Read())
            {
                letterSaveResponse = TranslateAsLetterInboundSaveResponse(reader, true);
            }

            return letterSaveResponse;
        }
    }
}
