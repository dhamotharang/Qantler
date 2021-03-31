using RulersCourt.Models.HRComplaintSuggestions;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.HRComplaintSuggestions
{
    public static class HRComplaintSuggestionsHistoryLogTranslator
    {
        public static HRComplaintSuggestionsHistoryLogModel TranslateAsHRComplaintSuggestionsHistoryLog(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var hRComplaintSuggestionsHistoryLogModel = new HRComplaintSuggestionsHistoryLogModel();

            if (reader.IsColumnExists("HistoryID"))
                hRComplaintSuggestionsHistoryLogModel.HistoryID = SqlHelper.GetNullableInt32(reader, "HistoryID");

            if (reader.IsColumnExists("HRComplaintSuggestionsID"))
                hRComplaintSuggestionsHistoryLogModel.HRComplaintSuggestionsID = SqlHelper.GetNullableInt32(reader, "HRComplaintSuggestionsID");

            if (reader.IsColumnExists("Action"))
                hRComplaintSuggestionsHistoryLogModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                hRComplaintSuggestionsHistoryLogModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("ActionBy"))
                hRComplaintSuggestionsHistoryLogModel.ActionBy = SqlHelper.GetNullableString(reader, "ActionBy");

            if (reader.IsColumnExists("ActionDateTime"))
                hRComplaintSuggestionsHistoryLogModel.ActionDateTime = SqlHelper.GetDateTime(reader, "ActionDateTime");

            return hRComplaintSuggestionsHistoryLogModel;
        }

        public static List<HRComplaintSuggestionsHistoryLogModel> TranslateAsHRComplaintSuggestionsHistoryLogList(this SqlDataReader reader)
        {
            var hRComplaintSuggestionsHistoryLogList = new List<HRComplaintSuggestionsHistoryLogModel>();
            while (reader.Read())
            {
                hRComplaintSuggestionsHistoryLogList.Add(TranslateAsHRComplaintSuggestionsHistoryLog(reader, true));
            }

            return hRComplaintSuggestionsHistoryLogList;
        }
    }
}
