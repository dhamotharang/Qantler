using RulersCourt.Models.Contact;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Contact
{
    public static class ContactHistoryLogTranslator
    {
        public static ContactHistoryLogModel TranslateAsContactHistoryLog(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var contactHistoryLogModel = new ContactHistoryLogModel();

            if (reader.IsColumnExists("HistoryID"))
                contactHistoryLogModel.HistoryID = SqlHelper.GetNullableInt32(reader, "HistoryID");

            if (reader.IsColumnExists("ContactID"))
                contactHistoryLogModel.ContactID = SqlHelper.GetNullableInt32(reader, "ContactID");

            if (reader.IsColumnExists("Action"))
                contactHistoryLogModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                contactHistoryLogModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("ActionBy"))
                contactHistoryLogModel.ActionBy = SqlHelper.GetNullableString(reader, "ActionBy");

            if (reader.IsColumnExists("ActionDateTime"))
                contactHistoryLogModel.ActionDateTime = SqlHelper.GetDateTime(reader, "ActionDateTime");

            return contactHistoryLogModel;
        }

        public static List<ContactHistoryLogModel> TranslateAsContactHistoryLogList(this SqlDataReader reader)
        {
            var contactHistoryLogList = new List<ContactHistoryLogModel>();
            while (reader.Read())
            {
                contactHistoryLogList.Add(TranslateAsContactHistoryLog(reader, true));
            }

            return contactHistoryLogList;
        }
    }
}