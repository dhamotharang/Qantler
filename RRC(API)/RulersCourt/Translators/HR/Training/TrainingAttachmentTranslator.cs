using RulersCourt.Models.HR.Training;
using RulersCourt.Models.Training;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.HR.Training
{
    public static class TrainingAttachmentTranslator
    {
        public static TrainingAttachmentModel TranslateAsTrainingAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new TrainingAttachmentModel();

            if (reader.IsColumnExists("TrainingID"))
                attachment.TrainingID = SqlHelper.GetNullableInt32(reader, "TrainingID");

            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            if (reader.IsColumnExists("CreatedBy"))
                attachment.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                attachment.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedBy"))
                attachment.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                attachment.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            return attachment;
        }

        public static List<TrainingAttachmentModel> TranslateAsTrainingAttachmentList(this SqlDataReader reader)
        {
            var attachmentList = new List<TrainingAttachmentModel>();
            while (reader.Read())
            {
                attachmentList.Add(TranslateAsTrainingAttachment(reader, true));
            }

            return attachmentList;
        }
    }
}