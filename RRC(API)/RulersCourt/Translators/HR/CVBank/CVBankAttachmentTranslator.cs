using RulersCourt.Models.CVBank;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.HR.CVBank
{
    public static class CVBankAttachmentTranslator
    {
        public static CVBankAttachmentGetModel TranslateAsCVBankGetAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new CVBankAttachmentGetModel();
            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            if (reader.IsColumnExists("ServiceID"))
                attachment.CVBankId = SqlHelper.GetNullableInt32(reader, "ServiceID");

            return attachment;
        }

        public static List<CVBankAttachmentGetModel> TranslateAsCVBankAttachmentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new List<CVBankAttachmentGetModel>();
            while (reader.Read())
            {
                mailAttachmentList.Add(TranslateAsCVBankGetAttachment(reader, true));
            }

            return mailAttachmentList;
        }
    }
}
