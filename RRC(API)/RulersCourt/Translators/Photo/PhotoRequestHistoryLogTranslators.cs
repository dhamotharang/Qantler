using RulersCourt.Models;
using RulersCourt.Models.Photo;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.Media.PhotoRequestTranslators
{
    public static class PhotoRequestHistoryLogTranslators
    {
        public static PhotoCommunicationHistory TranslateAsPhotoHistoryLog(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var photoHistoryLogModel = new PhotoCommunicationHistory();
            string photoGuid = string.Empty, photoName = string.Empty;

            if (reader.IsColumnExists("PhotoGuid"))
                photoGuid = SqlHelper.GetNullableString(reader, "PhotoGuid");

            if (reader.IsColumnExists("PhotoName"))
                photoName = SqlHelper.GetNullableString(reader, "PhotoName");

            if (!string.IsNullOrEmpty(photoGuid) && !string.IsNullOrEmpty(photoName))
            {
                photoHistoryLogModel.Photo = photoGuid + "|" + photoName;
            }

            if (reader.IsColumnExists("CommunicationID"))
                photoHistoryLogModel.CommunicationID = SqlHelper.GetNullableInt32(reader, "CommunicationID");

            if (reader.IsColumnExists("ParentCommunicationID"))
                photoHistoryLogModel.ParentCommunicationID = SqlHelper.GetNullableInt32(reader, "ParentCommunicationID");

            if (reader.IsColumnExists("PhotoID"))
                photoHistoryLogModel.PhotoID = SqlHelper.GetNullableInt32(reader, "PhotoID");

            if (reader.IsColumnExists("Action"))
                photoHistoryLogModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Message"))
                photoHistoryLogModel.Message = SqlHelper.GetNullableString(reader, "Message");

            if (reader.IsColumnExists("CreatedBy"))
                photoHistoryLogModel.CreatedBy = SqlHelper.GetNullableString(reader, "CreatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                photoHistoryLogModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            return photoHistoryLogModel;
        }

        public static List<PhotoCommunicationHistory> TranslateAsPhotoHistoryLogList(this SqlDataReader reader)
        {
            var photoHistoryLogList = new List<PhotoCommunicationHistory>();
            while (reader.Read())
            {
                photoHistoryLogList.Add(TranslateAsPhotoHistoryLog(reader, true));
            }

            return photoHistoryLogList;
        }
    }
}
