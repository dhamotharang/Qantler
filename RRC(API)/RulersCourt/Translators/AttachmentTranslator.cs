using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class AttachmentTranslator
    {
        public static AttachmentGetModel TranslateAsGetAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new AttachmentGetModel();

            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            return attachment;
        }

        public static List<AttachmentGetModel> TranslateAsAttachmentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new List<AttachmentGetModel>();
            while (reader.Read())
            {
                mailAttachmentList.Add(TranslateAsGetAttachment(reader, true));
            }

            return mailAttachmentList;
        }

        public static AttachmentPutModel TranslateAsPutAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new AttachmentPutModel();

            if (reader.IsColumnExists("MemoAttachmentID"))
                attachment.MemoAttachmentID = SqlHelper.GetNullableInt32(reader, "MemoAttachmentID");

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

        public static AttachmentPutModel TranslateAsPutAttachmentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new AttachmentPutModel();
            while (reader.Read())
            {
                mailAttachmentList = TranslateAsPutAttachment(reader, true);
            }

            return mailAttachmentList;
        }
    }
}
