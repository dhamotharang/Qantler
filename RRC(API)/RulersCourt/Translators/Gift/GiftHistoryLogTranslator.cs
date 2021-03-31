using RulersCourt.Models.CitizenAffair;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.CitizenAffair
{
    public static class GiftHistoryLogTranslator
    {
        public static GiftHistoryLogModel TranslateAsGiftHistoryLog(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var gift = new GiftHistoryLogModel();

            if (reader.IsColumnExists("HistoryID"))
                gift.HistoryID = SqlHelper.GetNullableInt32(reader, "HistoryID");

            if (reader.IsColumnExists("GiftID"))
                gift.GiftID = SqlHelper.GetNullableInt32(reader, "GiftID");

            if (reader.IsColumnExists("Action"))
                gift.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                gift.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("ActionBy"))
                gift.CreatedBy = SqlHelper.GetNullableString(reader, "ActionBy");

            if (reader.IsColumnExists("ActionDateTime"))
                gift.CreatedDateTime = SqlHelper.GetDateTime(reader, "ActionDateTime");

            return gift;
        }

        public static List<GiftHistoryLogModel> TranslateAsGiftHistoryLogList(this SqlDataReader reader)
        {
            var giftHistoryLogList = new List<GiftHistoryLogModel>();
            while (reader.Read())
            {
                giftHistoryLogList.Add(TranslateAsGiftHistoryLog(reader, true));
            }

            return giftHistoryLogList;
        }
    }
}
