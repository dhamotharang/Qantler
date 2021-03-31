using RulersCourt.Models.Legal;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Legal
{
    public static class LegalHistoryLogTranslator
    {
        public static LegalHistoryLogModel TranslateAsLegalHistoryLog(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var legalHistoryLogModel = new LegalHistoryLogModel();

            if (reader.IsColumnExists("HistoryID"))
                legalHistoryLogModel.HistoryID = SqlHelper.GetNullableInt32(reader, "HistoryID");

            if (reader.IsColumnExists("LegalID"))
                legalHistoryLogModel.LegalID = SqlHelper.GetNullableInt32(reader, "LegalID");

            if (reader.IsColumnExists("Action"))
                legalHistoryLogModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                legalHistoryLogModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("ActionBy"))
                legalHistoryLogModel.ActionBy = SqlHelper.GetNullableString(reader, "ActionBy");

            if (reader.IsColumnExists("ActionDateTime"))
                legalHistoryLogModel.ActionDateTime = SqlHelper.GetDateTime(reader, "ActionDateTime");

            return legalHistoryLogModel;
        }

        public static List<LegalHistoryLogModel> TranslateAsLegalHistoryLogList(this SqlDataReader reader)
        {
            var legalHistoryLogList = new List<LegalHistoryLogModel>();
            while (reader.Read())
            {
                legalHistoryLogList.Add(TranslateAsLegalHistoryLog(reader, true));
            }

            return legalHistoryLogList;
        }
    }
}