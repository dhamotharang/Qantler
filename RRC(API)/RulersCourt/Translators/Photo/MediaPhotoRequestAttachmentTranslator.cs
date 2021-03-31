using RulersCourt.Models.Photo;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.Media.PhotoRequestTranslators
{
    public static class MediaPhotoRequestAttachmentTranslator
    {
        public static PhotoAttachmentGetModel TranslateAsPhotoGetAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new PhotoAttachmentGetModel();

            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            if (reader.IsColumnExists("ServiceID"))
                attachment.PhotoID = SqlHelper.GetNullableInt32(reader, "ServiceID");

            return attachment;
        }

        public static List<PhotoAttachmentGetModel> TranslateAsPhotoAttachmentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new List<PhotoAttachmentGetModel>();
            while (reader.Read())
            {
                mailAttachmentList.Add(TranslateAsPhotoGetAttachment(reader, true));
            }

            return mailAttachmentList;
        }
    }
}
