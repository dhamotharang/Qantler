using RulersCourt.Models.HRComplaintSuggestions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Translators.HRComplaintSuggestions
{
    public static class HRComplaintSuggestionsTranslator
    {
        public static object HRComplaintSuggestionsModel { get; private set; }

        public static HRComplaintSuggestionsGetModel TranslateAsHRComplaintSuggestionsGetbyID(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var hRComplaintSuggestionsModel = new HRComplaintSuggestionsGetModel();

            if (reader.IsColumnExists("HRComplaintSuggestionsId"))
                hRComplaintSuggestionsModel.HRComplaintSuggestionsID = SqlHelper.GetNullableInt32(reader, "HRComplaintSuggestionsId");

            if (reader.IsColumnExists("ReferenceNumber"))
                hRComplaintSuggestionsModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Type"))
                hRComplaintSuggestionsModel.Type = SqlHelper.GetNullableString(reader, "Type");

            if (reader.IsColumnExists("Subject"))
                hRComplaintSuggestionsModel.Subject = SqlHelper.GetNullableString(reader, "Subject");

            if (reader.IsColumnExists("Source"))
                hRComplaintSuggestionsModel.Source = SqlHelper.GetNullableString(reader, "Source");

            if (reader.IsColumnExists("Details"))
                hRComplaintSuggestionsModel.Details = SqlHelper.GetNullableString(reader, "Details");

            if (reader.IsColumnExists("RequestCreatedBy"))
                hRComplaintSuggestionsModel.RequestCreatedBy = SqlHelper.GetNullableString(reader, "RequestCreatedBy");

            if (reader.IsColumnExists("MailId"))
                hRComplaintSuggestionsModel.MailID = SqlHelper.GetNullableString(reader, "MailId");

            if (reader.IsColumnExists("PhoneNumber"))
                hRComplaintSuggestionsModel.PhoneNumber = SqlHelper.GetNullableString(reader, "PhoneNumber");

            if (reader.IsColumnExists("ActionTaken"))
                hRComplaintSuggestionsModel.ActionTaken = SqlHelper.GetNullableString(reader, "ActionTaken");

            if (reader.IsColumnExists("CreatedBy"))
                hRComplaintSuggestionsModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                hRComplaintSuggestionsModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("Status"))
                hRComplaintSuggestionsModel.Status = SqlHelper.GetNullableInt32(reader, "Status");

            if (reader.IsColumnExists("CreatedDateTime"))
                hRComplaintSuggestionsModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                hRComplaintSuggestionsModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            return hRComplaintSuggestionsModel;
        }

        public static List<HRComplaintSuggestionsGetModel> TranslateAsHRComplaintSuggestionsList(this SqlDataReader reader)
        {
            var hRComplaintSuggestionsList = new List<HRComplaintSuggestionsGetModel>();
            while (reader.Read())
            {
                hRComplaintSuggestionsList.Add(TranslateAsHRComplaintSuggestionsGetbyID(reader, true));
            }

            return hRComplaintSuggestionsList;
        }

        public static HRComplaintSuggestionsPutModel TranslateAsPutHRComplaintSuggestions(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var hRComplaintSuggestionsPutModel = new HRComplaintSuggestionsPutModel();

            if (reader.IsColumnExists("HRComplaintSuggestionsId"))
                hRComplaintSuggestionsPutModel.HRComplaintSuggestionsID = SqlHelper.GetNullableInt32(reader, "HRComplaintSuggestionsId");

            if (reader.IsColumnExists("Type"))
                hRComplaintSuggestionsPutModel.Type = SqlHelper.GetNullableString(reader, "Type");

            if (reader.IsColumnExists("Subject"))
                hRComplaintSuggestionsPutModel.Subject = SqlHelper.GetNullableString(reader, "Subject");

            if (reader.IsColumnExists("Source"))
                hRComplaintSuggestionsPutModel.Source = SqlHelper.GetNullableString(reader, "Source");

            if (reader.IsColumnExists("Details"))
                hRComplaintSuggestionsPutModel.Details = SqlHelper.GetNullableString(reader, "Details");

            if (reader.IsColumnExists("RequestCreatedBy"))
                hRComplaintSuggestionsPutModel.RequestCreatedBy = SqlHelper.GetNullableString(reader, "RequestCreatedBy");

            if (reader.IsColumnExists("MailId"))
                hRComplaintSuggestionsPutModel.MailID = SqlHelper.GetNullableString(reader, "MailId");

            if (reader.IsColumnExists("PhoneNumber"))
                hRComplaintSuggestionsPutModel.PhoneNumber = SqlHelper.GetNullableString(reader, "PhoneNumber");

            if (reader.IsColumnExists("ActionTaken"))
                hRComplaintSuggestionsPutModel.ActionTaken = SqlHelper.GetNullableString(reader, "ActionTaken");

            if (reader.IsColumnExists("UpdatedBy"))
                hRComplaintSuggestionsPutModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                hRComplaintSuggestionsPutModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Action"))
                hRComplaintSuggestionsPutModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                hRComplaintSuggestionsPutModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            return hRComplaintSuggestionsPutModel;
        }

        public static List<HRComplaintSuggestionsPutModel> TranslateAsPutHRComplaintSuggestionsList(this SqlDataReader reader)
        {
            var hRComplaintSuggestionsPutList = new List<HRComplaintSuggestionsPutModel>();
            while (reader.Read())
            {
                hRComplaintSuggestionsPutList.Add(TranslateAsPutHRComplaintSuggestions(reader, true));
            }

            return hRComplaintSuggestionsPutList;
        }
    }
}