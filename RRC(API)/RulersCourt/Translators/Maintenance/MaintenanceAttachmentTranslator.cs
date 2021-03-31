using RulersCourt.Models.Maintenance;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Maintenance
{
    public static class MaintenanceAttachmentTranslator
    {
        public static MaintenanceAttachmentGetModel TranslateAsGetMaintenanceAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new MaintenanceAttachmentGetModel();

            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            return attachment;
        }

        public static List<MaintenanceAttachmentGetModel> TranslateAsGetMaintenanceAttachmentList(this SqlDataReader reader)
        {
            var mailAttachmentList = new List<MaintenanceAttachmentGetModel>();
            while (reader.Read())
            {
                mailAttachmentList.Add(TranslateAsGetMaintenanceAttachment(reader, true));
            }

            return mailAttachmentList;
        }
    }
}
