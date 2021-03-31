using RulersCourt.Models.DutyTask;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.DutyTask
{
    public static class AttachmentTranslator
    {
        public static DutyTaskAttachmentGetModel TranslateAsGetAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new DutyTaskAttachmentGetModel();

            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            if (reader.IsColumnExists("ServiceID"))
                attachment.TaskID = SqlHelper.GetNullableString(reader, "ServiceID");

            return attachment;
        }

        public static List<DutyTaskAttachmentGetModel> TranslateAsGetAttachmentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new List<DutyTaskAttachmentGetModel>();
            while (reader.Read())
            {
                mailAttachmentList.Add(TranslateAsGetAttachment(reader, true));
            }

            return mailAttachmentList;
        }

        public static TaskAttachmentPutModel TranslateAsPutAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new TaskAttachmentPutModel();

            if (reader.IsColumnExists("TaskAttachmentID"))
                attachment.TaskAttachmentID = SqlHelper.GetNullableInt32(reader, "TaskAttachmentID");

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

        public static TaskAttachmentPutModel TranslateAsPutAttachmentList(this SqlDataReader reader)
        {
            var attachmentList = new TaskAttachmentPutModel();
            while (reader.Read())
            {
                attachmentList = TranslateAsPutAttachment(reader, true);
            }

            return attachmentList;
        }
    }
}
