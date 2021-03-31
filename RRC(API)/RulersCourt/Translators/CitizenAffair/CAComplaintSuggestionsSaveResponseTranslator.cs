using RulersCourt.Models.CitizenAffairComplaintSuggestions;
using System.Data.SqlClient;

namespace RulersCourt.Translators.CAComplaintSuggestions
{
    public static class CAComplaintSuggestionsSaveResponseTranslator
    {
        public static CAComplaintSuggestionsWorkflowModel TranslateAsCAComplaintSuggestionsSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var cAComplaintSuggestionsSave = new CAComplaintSuggestionsWorkflowModel();

            if (reader.IsColumnExists("CAComplaintSuggestionsID"))
                cAComplaintSuggestionsSave.CAComplaintSuggestionsID = SqlHelper.GetNullableInt32(reader, "CAComplaintSuggestionsID");

            if (reader.IsColumnExists("ReferenceNumber"))
                cAComplaintSuggestionsSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                cAComplaintSuggestionsSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                cAComplaintSuggestionsSave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return cAComplaintSuggestionsSave;
        }

        public static CAComplaintSuggestionsWorkflowModel TranslateAsCAComplaintSuggestionsSaveResponseList(this SqlDataReader reader)
        {
            var cAComplaintSuggestionsSaveResponse = new CAComplaintSuggestionsWorkflowModel();
            while (reader.Read())
            {
                cAComplaintSuggestionsSaveResponse = TranslateAsCAComplaintSuggestionsSaveResponse(reader, true);
            }

            return cAComplaintSuggestionsSaveResponse;
        }
    }
}