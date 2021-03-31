using RulersCourt.Models.Contact;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Contact
{
    public static class ContactSaveResponseTranslator
    {
        public static ContactSaveResponseModel TranslateAsContactSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var contactSave = new ContactSaveResponseModel();

            if (reader.IsColumnExists("ContactID"))
                contactSave.ContactID = SqlHelper.GetNullableInt32(reader, "ContactID");

            if (reader.IsColumnExists("ReferenceNumber"))
                contactSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            return contactSave;
        }

        public static ContactSaveResponseModel TranslateAsContactSaveResponseList(this SqlDataReader reader)
        {
            var contactSaveResponse = new ContactSaveResponseModel();
            while (reader.Read())
            {
                contactSaveResponse = TranslateAsContactSaveResponse(reader, true);
            }

            return contactSaveResponse;
        }
    }
}