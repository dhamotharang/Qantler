using RulersCourt.Models.Gift;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Legal
{
    public static class GiftSaveResponseTranslator
    {
        public static GiftSaveResponseModel TranslateAsGiftSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var giftSave = new GiftSaveResponseModel();

            if (reader.IsColumnExists("GiftID"))
                giftSave.GiftID = SqlHelper.GetNullableInt32(reader, "GiftID");

            if (reader.IsColumnExists("ReferenceNumber"))
                giftSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Status"))
                giftSave.Status = SqlHelper.GetNullableInt32(reader, "Status");

            return giftSave;
        }

        public static GiftSaveResponseModel TranslateAsGiftSaveResponseList(this SqlDataReader reader)
        {
            var giftSaveResponse = new GiftSaveResponseModel();
            while (reader.Read())
            {
                giftSaveResponse = TranslateAsGiftSaveResponse(reader, true);
            }

            return giftSaveResponse;
        }
    }
}