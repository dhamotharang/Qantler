using RulersCourt.Models.Gift;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Gift
{
    public static class GiftAttachmentTranslator
    {
        public static GiftAttachmentGetModel TranslateAsGetGiftAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new GiftAttachmentGetModel();

            if (reader.IsColumnExists("ServiceID"))
                attachment.GiftID = SqlHelper.GetNullableInt32(reader, "ServiceID");

            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            return attachment;
        }

        public static List<GiftAttachmentGetModel> TranslateAsGetGiftAttachmentList(this SqlDataReader reader)
        {
            var attachmentList = new List<GiftAttachmentGetModel>();
            while (reader.Read())
            {
                attachmentList.Add(TranslateAsGetGiftAttachment(reader, true));
            }

            return attachmentList;
        }
    }
}
