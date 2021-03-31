using RulersCourt.Models.Design;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.Media.Design
{
    public static class DesignAttachmentTranslator
    {
        public static DesignAttachmentGetModel TranslateAsDesignGetAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new DesignAttachmentGetModel();

            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            if (reader.IsColumnExists("ServiceID"))
                attachment.DesignID = SqlHelper.GetNullableInt32(reader, "ServiceID");

            return attachment;
        }

        public static List<DesignAttachmentGetModel> TranslateAsDesignAttachmentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new List<DesignAttachmentGetModel>();
            while (reader.Read())
            {
                mailAttachmentList.Add(TranslateAsDesignGetAttachment(reader, true));
            }

            return mailAttachmentList;
        }
    }
}
