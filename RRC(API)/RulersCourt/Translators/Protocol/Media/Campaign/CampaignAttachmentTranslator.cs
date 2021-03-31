using RulersCourt.Models.Campaign;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.Media.Campaign
{
    public static class CampaignAttachmentTranslator
    {
        public static CampaignAttachmentGetModel TranslateAsDesignGetAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new CampaignAttachmentGetModel();

            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            if (reader.IsColumnExists("ServiceID"))
                attachment.CampaignID = SqlHelper.GetNullableInt32(reader, "ServiceID");

            return attachment;
        }

        public static List<CampaignAttachmentGetModel> TranslateAsCampaignAttachmentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new List<CampaignAttachmentGetModel>();
            while (reader.Read())
            {
                mailAttachmentList.Add(TranslateAsDesignGetAttachment(reader, true));
            }

            return mailAttachmentList;
        }
    }
}
