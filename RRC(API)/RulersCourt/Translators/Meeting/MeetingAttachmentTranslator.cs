using RulersCourt.Models.Meeting;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Meeting
{
    public static class MeetingAttachmentTranslator
    {
        public static MeetingAttachmentGetModel TranslateAsDesignGetAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new MeetingAttachmentGetModel();

            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            if (reader.IsColumnExists("ServiceID"))
                attachment.MeetingID = SqlHelper.GetNullableInt32(reader, "ServiceID");

            return attachment;
        }

        public static List<MeetingAttachmentGetModel> TranslateAsMeetingAttachmentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new List<MeetingAttachmentGetModel>();
            while (reader.Read())
            {
                mailAttachmentList.Add(TranslateAsDesignGetAttachment(reader, true));
            }

            return mailAttachmentList;
        }
    }
}