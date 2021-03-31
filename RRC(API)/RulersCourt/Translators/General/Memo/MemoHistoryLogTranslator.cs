using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class MemoHistoryLogTranslator
    {
        public static MemoHistoryLogModel TranslateAsMemoHistoryLog(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var memoHistoryLogModel = new MemoHistoryLogModel();

            if (reader.IsColumnExists("HistoryID"))
                memoHistoryLogModel.HistoryID = SqlHelper.GetNullableInt32(reader, "HistoryID");

            if (reader.IsColumnExists("MemoID"))
                memoHistoryLogModel.MemoID = SqlHelper.GetNullableInt32(reader, "MemoID");

            if (reader.IsColumnExists("Action"))
                memoHistoryLogModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                memoHistoryLogModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("ActionBy"))
                memoHistoryLogModel.ActionBy = SqlHelper.GetNullableString(reader, "ActionBy");

            if (reader.IsColumnExists("ActionDateTime"))
                memoHistoryLogModel.ActionDateTime = SqlHelper.GetDateTime(reader, "ActionDateTime");

            if (reader.IsColumnExists("Escalate_RedirectUser"))
                memoHistoryLogModel.Escalate_RedirectUser = SqlHelper.GetNullableString(reader, "Escalate_RedirectUser");

            return memoHistoryLogModel;
        }

        public static List<MemoHistoryLogModel> TranslateAsMemoHistoryLogList(this SqlDataReader reader)
        {
            var memoHistoryLogList = new List<MemoHistoryLogModel>();
            while (reader.Read())
            {
                memoHistoryLogList.Add(TranslateAsMemoHistoryLog(reader, true));
            }

            return memoHistoryLogList;
        }
    }
}
