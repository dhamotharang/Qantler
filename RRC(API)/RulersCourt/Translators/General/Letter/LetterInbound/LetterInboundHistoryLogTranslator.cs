using RulersCourt.Models.Letter.LetterInbound;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Letter.LetterInbound
{
    public static class LetterInboundHistoryLogTranslator
    {
        public static LetterInboundHistoryLogModel TranslateAsLetterInboundHistoryLog(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var letterHistoryLogModel = new LetterInboundHistoryLogModel();

            if (reader.IsColumnExists("HistoryID"))
                letterHistoryLogModel.HistoryID = SqlHelper.GetNullableInt32(reader, "HistoryID");

            if (reader.IsColumnExists("LetterID"))
                letterHistoryLogModel.LetterID = SqlHelper.GetNullableInt32(reader, "LetterID");

            if (reader.IsColumnExists("Action"))
                letterHistoryLogModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                letterHistoryLogModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("ActionBy"))
                letterHistoryLogModel.ActionBy = SqlHelper.GetNullableString(reader, "ActionBy");

            if (reader.IsColumnExists("ActionDateTime"))
                letterHistoryLogModel.ActionDateTime = SqlHelper.GetDateTime(reader, "ActionDateTime");

            if (reader.IsColumnExists("Escalate_RedirectUser"))
                letterHistoryLogModel.Escalate_RedirectUser = SqlHelper.GetNullableString(reader, "Escalate_RedirectUser");

            return letterHistoryLogModel;
        }

        public static List<LetterInboundHistoryLogModel> TranslateAsLetterInboundHistoryLogList(this SqlDataReader reader)
        {
            var letterHistoryLogList = new List<LetterInboundHistoryLogModel>();
            while (reader.Read())
            {
                letterHistoryLogList.Add(TranslateAsLetterInboundHistoryLog(reader, true));
            }

            return letterHistoryLogList;
        }
    }
}
