using RulersCourt.Models.CitizenAffair;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.CAComplaintSuggestions
{
    public static class CAComplaintSuggestionsTranslator
    {
        public static object CAComplaintSuggestionsModel { get; private set; }

        public static CAComplaintSuggestionsGetModel TranslateAsCAComplaintSuggestionsGetbyID(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var cAComplaintSuggestionsModel = new CAComplaintSuggestionsGetModel();

            if (reader.IsColumnExists("CAComplaintSuggestionsID"))
                cAComplaintSuggestionsModel.CAComplaintSuggestionsID = SqlHelper.GetNullableInt32(reader, "CAComplaintSuggestionsID");

            if (reader.IsColumnExists("ReferenceNumber"))
                cAComplaintSuggestionsModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Type"))
                cAComplaintSuggestionsModel.Type = SqlHelper.GetNullableString(reader, "Type");

            if (reader.IsColumnExists("Source"))
                cAComplaintSuggestionsModel.Source = SqlHelper.GetNullableString(reader, "Source");

            if (reader.IsColumnExists("Subject"))
                cAComplaintSuggestionsModel.Subject = SqlHelper.GetNullableString(reader, "Subject");

            if (reader.IsColumnExists("Details"))
                cAComplaintSuggestionsModel.Details = SqlHelper.GetNullableString(reader, "Details");

            if (reader.IsColumnExists("RequestCreatedBy"))
                cAComplaintSuggestionsModel.RequestCreatedBy = SqlHelper.GetNullableString(reader, "RequestCreatedBy");

            if (reader.IsColumnExists("MailId"))
                cAComplaintSuggestionsModel.MailID = SqlHelper.GetNullableString(reader, "MailId");

            if (reader.IsColumnExists("PhoneNumber"))
                cAComplaintSuggestionsModel.PhoneNumber = SqlHelper.GetNullableString(reader, "PhoneNumber");

            if (reader.IsColumnExists("ActionTaken"))
                cAComplaintSuggestionsModel.ActionTaken = SqlHelper.GetNullableString(reader, "ActionTaken");

            if (reader.IsColumnExists("CreatedBy"))
                cAComplaintSuggestionsModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                cAComplaintSuggestionsModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("Status"))
                cAComplaintSuggestionsModel.Status = SqlHelper.GetNullableInt32(reader, "Status");

            if (reader.IsColumnExists("CreatedDateTime"))
                cAComplaintSuggestionsModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                cAComplaintSuggestionsModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            return cAComplaintSuggestionsModel;
        }

        public static List<CAComplaintSuggestionsGetModel> TranslateAsCAComplaintSuggestionsList(this SqlDataReader reader)
        {
            var cAComplaintSuggestionsList = new List<CAComplaintSuggestionsGetModel>();
            while (reader.Read())
            {
                cAComplaintSuggestionsList.Add(TranslateAsCAComplaintSuggestionsGetbyID(reader, true));
            }

            return cAComplaintSuggestionsList;
        }

        public static CAComplaintSuggestionsPutModel TranslateAsPutCAComplaintSuggestions(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var cAComplaintSuggestionsPutModel = new CAComplaintSuggestionsPutModel();

            if (reader.IsColumnExists("CAComplaintSuggestionsID"))
                cAComplaintSuggestionsPutModel.CAComplaintSuggestionsID = SqlHelper.GetNullableInt32(reader, "CAComplaintSuggestionsID");

            if (reader.IsColumnExists("Type"))
                cAComplaintSuggestionsPutModel.Type = SqlHelper.GetNullableString(reader, "Type");

            if (reader.IsColumnExists("Source"))
                cAComplaintSuggestionsPutModel.Source = SqlHelper.GetNullableString(reader, "Source");

            if (reader.IsColumnExists("Subject"))
                cAComplaintSuggestionsPutModel.Subject = SqlHelper.GetNullableString(reader, "Subject");

            if (reader.IsColumnExists("Details"))
                cAComplaintSuggestionsPutModel.Details = SqlHelper.GetNullableString(reader, "Details");

            if (reader.IsColumnExists("RequestCreatedBy"))
                cAComplaintSuggestionsPutModel.RequestCreatedBy = SqlHelper.GetNullableString(reader, "RequestCreatedBy");

            if (reader.IsColumnExists("ActionTaken"))
                cAComplaintSuggestionsPutModel.ActionTaken = SqlHelper.GetNullableString(reader, "ActionTaken");

            if (reader.IsColumnExists("MailId"))
                cAComplaintSuggestionsPutModel.MailId = SqlHelper.GetNullableString(reader, "MailId");

            if (reader.IsColumnExists("PhoneNumber"))
                cAComplaintSuggestionsPutModel.PhoneNumber = SqlHelper.GetNullableString(reader, "PhoneNumber");

            if (reader.IsColumnExists("UpdatedBy"))
                cAComplaintSuggestionsPutModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                cAComplaintSuggestionsPutModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Action"))
                cAComplaintSuggestionsPutModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                cAComplaintSuggestionsPutModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            return cAComplaintSuggestionsPutModel;
        }

        public static List<CAComplaintSuggestionsPutModel> TranslateAsPutCAComplaintSuggestionsList(this SqlDataReader reader)
        {
            var cAComplaintSuggestionsPutList = new List<CAComplaintSuggestionsPutModel>();
            while (reader.Read())
            {
                cAComplaintSuggestionsPutList.Add(TranslateAsPutCAComplaintSuggestions(reader, true));
            }

            return cAComplaintSuggestionsPutList;
        }
    }
}
