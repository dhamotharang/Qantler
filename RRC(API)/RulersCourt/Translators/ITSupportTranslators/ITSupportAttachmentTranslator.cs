using RulersCourt.Models.ITSupport;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.ITSupportTranslators
{
    public static class ITSupportAttachmentTranslator
    {
        public static ITSupportAttachmentGetModel TranslateAsITSupportGetAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new ITSupportAttachmentGetModel();

            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            if (reader.IsColumnExists("ServiceID"))
                attachment.ITSupportID = SqlHelper.GetNullableInt32(reader, "ServiceID");

            return attachment;
        }

        public static List<ITSupportAttachmentGetModel> TranslateAsITSupportAttachmentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new List<ITSupportAttachmentGetModel>();
            while (reader.Read())
            {
                mailAttachmentList.Add(TranslateAsITSupportGetAttachment(reader, true));
            }

            return mailAttachmentList;
        }
    }
}
