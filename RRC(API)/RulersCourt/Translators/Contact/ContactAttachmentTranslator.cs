using RulersCourt.Models.Contact;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Contact
{
    public static class ContactAttachmentTranslator
    {
        public static ContactAttachmentGetModel TranslateAsGetContactAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new ContactAttachmentGetModel();

            if (reader.IsColumnExists("ServiceID"))
                attachment.ContactID = SqlHelper.GetNullableInt32(reader, "ServiceID");

            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            return attachment;
        }

        public static List<ContactAttachmentGetModel> TranslateAsContactAttachmentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new List<ContactAttachmentGetModel>();
            while (reader.Read())
            {
                mailAttachmentList.Add(TranslateAsGetContactAttachment(reader, true));
            }

            return mailAttachmentList;
        }

        public static ContactAttachmentPutModel TranslateAsPutContactAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new ContactAttachmentPutModel();

            if (reader.IsColumnExists("ContactAttachmentID"))
                attachment.ContactAttachmentID = SqlHelper.GetNullableInt32(reader, "ContactAttachmentID");

            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            if (reader.IsColumnExists("UpdatedBy"))
                attachment.UpdatedBy = SqlHelper.GetNullableString(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                attachment.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            return attachment;
        }

        public static ContactAttachmentPutModel TranslateAsPutContactAttachmentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new ContactAttachmentPutModel();
            while (reader.Read())
            {
                mailAttachmentList = TranslateAsPutContactAttachment(reader, true);
            }

            return mailAttachmentList;
        }
    }
}