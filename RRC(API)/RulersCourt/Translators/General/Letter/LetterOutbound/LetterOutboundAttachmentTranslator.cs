using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Letter
{
    public static class LetterOutboundAttachmentTranslator
    {
        public static LetterAttachmentGetModel TranslateAsGetLetterAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new LetterAttachmentGetModel();

            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            return attachment;
        }

        public static List<LetterAttachmentGetModel> TranslateAsLetterAttachmentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new List<LetterAttachmentGetModel>();
            while (reader.Read())
            {
                mailAttachmentList.Add(TranslateAsGetLetterAttachment(reader, true));
            }

            return mailAttachmentList;
        }
    }
}
