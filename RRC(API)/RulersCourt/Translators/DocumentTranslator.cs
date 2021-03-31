using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class DocumentTranslator
    {
        public static DocumentGetModel TranslateAsGetDocument(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new DocumentGetModel();

            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            if (reader.IsColumnExists("AttachmentID"))
                attachment.AttachmentID = SqlHelper.GetNullableInt32(reader, "AttachmentID");

            if (reader.IsColumnExists("CreatedBy"))
                attachment.CreatedBy = SqlHelper.GetNullableString(reader, "CreatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                attachment.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedBy"))
                attachment.UpdatedBy = SqlHelper.GetNullableString(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                attachment.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            return attachment;
        }

        public static List<DocumentGetModel> TranslateAsCocumentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new List<DocumentGetModel>();
            while (reader.Read())
            {
                mailAttachmentList.Add(TranslateAsGetDocument(reader, true));
            }

            return mailAttachmentList;
        }
    }
}
