using RulersCourt.Models.UserProfile;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.UserProfile
{
    public static class UserProfileAttachmentTranslator
    {
        public static UserProfileAttachmentGetModel TranslateAsUserProfileGetAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new UserProfileAttachmentGetModel();
            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentType"))
                attachment.AttachmentType = SqlHelper.GetNullableString(reader, "AttachmentType");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            if (reader.IsColumnExists("ServiceID"))
                attachment.UserProfileId = SqlHelper.GetNullableInt32(reader, "ServiceID");

            return attachment;
        }

        public static List<UserProfileAttachmentGetModel> TranslateAsUserProfileAttachmentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new List<UserProfileAttachmentGetModel>();
            while (reader.Read())
            {
                mailAttachmentList.Add(TranslateAsUserProfileGetAttachment(reader, true));
            }

            return mailAttachmentList;
        }
    }
}
