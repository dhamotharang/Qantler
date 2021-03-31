using RulersCourt.Models.Circular;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Circular
{
    public static class CircularAttachmentTranslator
    {
        public static CircularAttachmentGetModel TranslateAsCircularGetAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new CircularAttachmentGetModel();

            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            if (reader.IsColumnExists("ServiceID"))
                attachment.CircularID = SqlHelper.GetNullableInt32(reader, "ServiceID");

            return attachment;
        }

        public static List<CircularAttachmentGetModel> TranslateAsCircularAttachmentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new List<CircularAttachmentGetModel>();
            while (reader.Read())
            {
                mailAttachmentList.Add(TranslateAsCircularGetAttachment(reader, true));
            }

            return mailAttachmentList;
        }
    }
}
