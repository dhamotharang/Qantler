using RulersCourt.Models.BabyAddition;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.BabyAddition
{
    public static class BabyAdditionAttachmentTranslator
    {
        public static BabyAdditionAttachmentGetModel TranslateAsGetBabyAdditionAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new BabyAdditionAttachmentGetModel();

            if (reader.IsColumnExists("ServiceID"))
                attachment.BabyAdditionID = SqlHelper.GetNullableInt32(reader, "ServiceID");

            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            return attachment;
        }

        public static List<BabyAdditionAttachmentGetModel> TranslateAsBabyAdditionAttachmentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new List<BabyAdditionAttachmentGetModel>();
            while (reader.Read())
            {
                mailAttachmentList.Add(TranslateAsGetBabyAdditionAttachment(reader, true));
            }

            return mailAttachmentList;
        }

        public static BabyAdditionAttachmentPutModel TranslateAsPutBabyAdditionAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new BabyAdditionAttachmentPutModel();

            if (reader.IsColumnExists("BabyAdditionAttachmentID"))
                attachment.BabyAdditionAttachmentID = SqlHelper.GetNullableInt32(reader, "BabyAdditionAttachmentID");

            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            if (reader.IsColumnExists("UpdatedBy"))
                attachment.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                attachment.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            return attachment;
        }

        public static BabyAdditionAttachmentPutModel TranslateAsPutBabyAttachmentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new BabyAdditionAttachmentPutModel();
            while (reader.Read())
            {
                mailAttachmentList = TranslateAsPutBabyAdditionAttachment(reader, true);
            }

            return mailAttachmentList;
        }
    }
}