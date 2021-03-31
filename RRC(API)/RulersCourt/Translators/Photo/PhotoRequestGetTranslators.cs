using RulersCourt.Models.Photo;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.Media.PhotoRequestTranslators
{
    public static class PhotoRequestGetTranslators
    {
        public static PhotoGetModel TranslatePhotoGetModel(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var photoGetDetail = new PhotoGetModel();

            if (reader.IsColumnExists("PhotoID"))
                photoGetDetail.PhotoID = SqlHelper.GetNullableInt32(reader, "PhotoID");

            if (reader.IsColumnExists("ReferenceNumber"))
                photoGetDetail.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Date"))                photoGetDetail.Date = SqlHelper.GetDateTime(reader, "Date");

            if (reader.IsColumnExists("SourceOU"))
                photoGetDetail.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                photoGetDetail.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("EventDate"))                photoGetDetail.EventDate = SqlHelper.GetDateTime(reader, "EventDate");

            if (reader.IsColumnExists("Status"))
                photoGetDetail.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("Location"))
                photoGetDetail.Location = SqlHelper.GetNullableString(reader, "Location");

            if (reader.IsColumnExists("EventName"))
                photoGetDetail.EventName = SqlHelper.GetNullableString(reader, "EventName");

            if (reader.IsColumnExists("PhotoDescription"))
                photoGetDetail.PhotoDescription = SqlHelper.GetNullableString(reader, "PhotoDescription");

            if (reader.IsColumnExists("UpdatedBy"))
                photoGetDetail.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("CreatedBy"))
                photoGetDetail.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))                photoGetDetail.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))                photoGetDetail.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("ApproverID"))
                photoGetDetail.ApproverID = SqlHelper.GetNullableInt32(reader, "ApproverID");

            if (reader.IsColumnExists("ApproverDepartmentID"))
                photoGetDetail.ApproverDepartmentID = SqlHelper.GetNullableInt32(reader, "ApproverDepartmentID");

            return photoGetDetail;
        }

        public static List<PhotoGetModel> TranslatePhotoGetModelList(this SqlDataReader reader)
        {
            var photoList = new List<PhotoGetModel>();
            while (reader.Read())
            {
                photoList.Add(TranslatePhotoGetModel(reader, true));
            }

            return photoList;
        }

        public static PhotoPutModel TranslateAsPutPhotoRequest(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var photoPutModels = new PhotoPutModel();

            if (reader.IsColumnExists("PhotoID"))
                photoPutModels.PhotoID = SqlHelper.GetNullableInt32(reader, "PhotoID");

            if (reader.IsColumnExists("SourceOU"))
                photoPutModels.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                photoPutModels.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("UpdatedBy"))
                photoPutModels.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))                photoPutModels.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Date"))                photoPutModels.Date = SqlHelper.GetDateTime(reader, "Date");

            if (reader.IsColumnExists("EventDate"))                photoPutModels.EventDate = SqlHelper.GetDateTime(reader, "EventDate");

            if (reader.IsColumnExists("Location"))
                photoPutModels.Location = SqlHelper.GetNullableString(reader, "Location");

            if (reader.IsColumnExists("EventName"))
                photoPutModels.EventName = SqlHelper.GetNullableString(reader, "EventName");

            if (reader.IsColumnExists("PhotoDescription"))
                photoPutModels.PhotoDescription = SqlHelper.GetNullableString(reader, "PhotoDescription");

            if (reader.IsColumnExists("Status"))
                photoPutModels.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("Comments"))
                photoPutModels.Comments = SqlHelper.GetNullableString(reader, "Comments");

            return photoPutModels;
        }

        public static List<PhotoPutModel> TranslateAsPutPhotoRequestList(this SqlDataReader reader)
        {
            var photoRequestList = new List<PhotoPutModel>();
            while (reader.Read())
            {
                photoRequestList.Add(TranslateAsPutPhotoRequest(reader, true));
            }

            return photoRequestList;
        }
    }
}
