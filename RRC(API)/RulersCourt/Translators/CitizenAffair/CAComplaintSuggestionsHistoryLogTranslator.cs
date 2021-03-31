using RulersCourt.Models.CitizenAffair;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.CAComplaintSuggestions
{
    public static class CAComplaintSuggestionsHistoryLogTranslator
    {
        public static CAComplaintSuggestionsHistoryLogModel TranslateAsCAComplaintSuggestionsHistoryLog(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var cAComplaintSuggestionsHistoryLogModel = new CAComplaintSuggestionsHistoryLogModel();

            if (reader.IsColumnExists("HistoryID"))
                cAComplaintSuggestionsHistoryLogModel.HistoryID = SqlHelper.GetNullableInt32(reader, "HistoryID");

            if (reader.IsColumnExists("CAComplaintSuggestionsId"))
                cAComplaintSuggestionsHistoryLogModel.CAComplaintSuggestionsID = SqlHelper.GetNullableInt32(reader, "CAComplaintSuggestionsId");

            if (reader.IsColumnExists("Action"))
                cAComplaintSuggestionsHistoryLogModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                cAComplaintSuggestionsHistoryLogModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("ActionBy"))
                cAComplaintSuggestionsHistoryLogModel.ActionBy = SqlHelper.GetNullableString(reader, "ActionBy");

            if (reader.IsColumnExists("ActionDateTime"))
                cAComplaintSuggestionsHistoryLogModel.ActionDateTime = SqlHelper.GetDateTime(reader, "ActionDateTime");

            return cAComplaintSuggestionsHistoryLogModel;
        }

        public static List<CAComplaintSuggestionsHistoryLogModel> TranslateAsCAComplaintSuggestionsHistoryLogList(this SqlDataReader reader)
        {
            var cAComplaintSuggestionsHistoryLogList = new List<CAComplaintSuggestionsHistoryLogModel>();
            while (reader.Read())
            {
                cAComplaintSuggestionsHistoryLogList.Add(TranslateAsCAComplaintSuggestionsHistoryLog(reader, true));
            }

            return cAComplaintSuggestionsHistoryLogList;
        }
    }
}
