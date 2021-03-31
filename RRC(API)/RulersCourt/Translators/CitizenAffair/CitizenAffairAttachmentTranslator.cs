using RulersCourt.Models.CitizenAffair;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.CitizenAffair
{
    public static class CitizenAffairAttachmentTranslator
    {
        public static CitizenAffairAttachmentGetModel TranslateAsGetCitizenAffairAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new CitizenAffairAttachmentGetModel();

            if (reader.IsColumnExists("ServiceID"))
                attachment.CitizenAffairID = SqlHelper.GetNullableInt32(reader, "ServiceID");

            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            return attachment;
        }

        public static List<CitizenAffairAttachmentGetModel> TranslateAsGetCitizenAffairAttachmentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new List<CitizenAffairAttachmentGetModel>();
            while (reader.Read())
            {
                mailAttachmentList.Add(TranslateAsGetCitizenAffairAttachment(reader, true));
            }

            return mailAttachmentList;
        }
    }
}
