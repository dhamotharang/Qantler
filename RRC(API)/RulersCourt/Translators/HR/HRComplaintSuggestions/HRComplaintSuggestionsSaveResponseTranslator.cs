using RulersCourt.Models.HRComplaintSuggestions;
using System.Data.SqlClient;

namespace RulersCourt.Translators.HRComplaintSuggestions
{
    public static class HRComplaintSuggestionsSaveResponseTranslator
    {
        public static HRComplaintSuggestionsWorkflowModel TranslateAsHRComplaintSuggestionsSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var hRComplaintSuggestionsSave = new HRComplaintSuggestionsWorkflowModel();

            if (reader.IsColumnExists("HRComplaintSuggestionsID"))
                hRComplaintSuggestionsSave.HRComplaintSuggestionsID = SqlHelper.GetNullableInt32(reader, "HRComplaintSuggestionsID");

            if (reader.IsColumnExists("ReferenceNumber"))
                hRComplaintSuggestionsSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                hRComplaintSuggestionsSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                hRComplaintSuggestionsSave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return hRComplaintSuggestionsSave;
        }

        public static HRComplaintSuggestionsWorkflowModel TranslateAsHRComplaintSuggestionsSaveResponseList(this SqlDataReader reader)
        {
            var hRComplaintSuggestionsSaveResponse = new HRComplaintSuggestionsWorkflowModel();
            while (reader.Read())
            {
                hRComplaintSuggestionsSaveResponse = TranslateAsHRComplaintSuggestionsSaveResponse(reader, true);
            }

            return hRComplaintSuggestionsSaveResponse;
        }
    }
}