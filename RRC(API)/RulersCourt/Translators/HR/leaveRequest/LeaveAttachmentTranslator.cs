using RulersCourt.Models.LeaveRequest;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.LeaveRequest
{
    public static class LeaveAttachmentTranslator
    {
        public static LeaveAttachmentGetModel TranslateAsGetLeaveAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new LeaveAttachmentGetModel();

            if (reader.IsColumnExists("ServiceID"))
                attachment.LeaveID = SqlHelper.GetNullableInt32(reader, "ServiceID");

            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            return attachment;
        }

        public static List<LeaveAttachmentGetModel> TranslateAsLeaveAttachmentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new List<LeaveAttachmentGetModel>();
            while (reader.Read())
            {
                mailAttachmentList.Add(TranslateAsGetLeaveAttachment(reader, true));
            }

            return mailAttachmentList;
        }

        public static LeaveAttachmentPutModel TranslateAsPutLeaveAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new LeaveAttachmentPutModel();

            if (reader.IsColumnExists("LeaveAttachmentID"))
                attachment.LeaveAttachmentID = SqlHelper.GetNullableInt32(reader, "LeaveAttachmentID");

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

        public static LeaveAttachmentPutModel TranslateAsPutLeaveAttachmentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new LeaveAttachmentPutModel();
            while (reader.Read())
            {
                mailAttachmentList = TranslateAsPutLeaveAttachment(reader, true);
            }

            return mailAttachmentList;
        }
    }
}
