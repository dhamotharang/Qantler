using RulersCourt.Models.Legal;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Legal
{
    public static class LegalAttachmentTranslator
    {
        public static LegalAttachmentGetModel TranslateAsGetLegalAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new LegalAttachmentGetModel();

            if (reader.IsColumnExists("ServiceID"))
                attachment.LegalID = SqlHelper.GetNullableInt32(reader, "ServiceID");

            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            return attachment;
        }

        public static List<LegalAttachmentGetModel> TranslateAsLegalAttachmentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new List<LegalAttachmentGetModel>();
            while (reader.Read())
            {
                mailAttachmentList.Add(TranslateAsGetLegalAttachment(reader, true));
            }

            return mailAttachmentList;
        }

        public static LegalAttachmentPutModel TranslateAsPutLegalAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new LegalAttachmentPutModel();

            if (reader.IsColumnExists("LegalAttachmentID"))
                attachment.LegalAttachmentID = SqlHelper.GetNullableInt32(reader, "LegalAttachmentID");

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

        public static LegalAttachmentPutModel TranslateAsPutLegalAttachmentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new LegalAttachmentPutModel();
            while (reader.Read())
            {
                mailAttachmentList = TranslateAsPutLegalAttachment(reader, true);
            }

            return mailAttachmentList;
        }
    }
}
