using RulersCourt.Models.BabyAddition;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.BabyAddition
{
    public static class BabyAdditionHistroyLogTranslator
    {
        public static BabyAdditionHistoryLogModel TranslateAsBabyAdditionHistoryLog(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var babyAdditionHistoryLogModel = new BabyAdditionHistoryLogModel();

            if (reader.IsColumnExists("HistoryID"))
                babyAdditionHistoryLogModel.HistoryID = SqlHelper.GetNullableInt32(reader, "HistoryID");

            if (reader.IsColumnExists("BabyAdditionID"))
                babyAdditionHistoryLogModel.BabyAdditionID = SqlHelper.GetNullableInt32(reader, "BabyAdditionID");

            if (reader.IsColumnExists("Action"))
                babyAdditionHistoryLogModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                babyAdditionHistoryLogModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("ActionBy"))
                babyAdditionHistoryLogModel.ActionBy = SqlHelper.GetNullableString(reader, "ActionBy");

            if (reader.IsColumnExists("ActionDateTime"))
                babyAdditionHistoryLogModel.ActionDateTime = SqlHelper.GetDateTime(reader, "ActionDateTime");

            return babyAdditionHistoryLogModel;
        }

        public static List<BabyAdditionHistoryLogModel> TranslateAsBabyAdditionHistoryLogList(this SqlDataReader reader)
        {
            var babyAdditionHistoryLogList = new List<BabyAdditionHistoryLogModel>();
            while (reader.Read())
            {
                babyAdditionHistoryLogList.Add(TranslateAsBabyAdditionHistoryLog(reader, true));
            }

            return babyAdditionHistoryLogList;
        }
    }
}