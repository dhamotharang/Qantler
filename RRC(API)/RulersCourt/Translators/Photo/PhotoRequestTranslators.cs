using RulersCourt.Models.Protocol.Media.Photo;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.Media.PhotoRequestTranslators
{
    public static class PhotoRequestTranslators
    {
        public static PhotoRequestWorkflowModel TranslateAsPhotoRequestWorkSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var photoRequestSave = new PhotoRequestWorkflowModel();

            if (reader.IsColumnExists("PhotoID"))
                photoRequestSave.PhotoID = SqlHelper.GetNullableInt32(reader, "PhotoID");

            if (reader.IsColumnExists("ReferenceNumber"))
                photoRequestSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                photoRequestSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                photoRequestSave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return photoRequestSave;
        }

        public static PhotoRequestWorkflowModel TranslateAsPhotoRequestWorkSaveResponseList(this SqlDataReader reader)
        {
            var photoRequestWorkSaveResponse = new PhotoRequestWorkflowModel();
            while (reader.Read())
            {
                photoRequestWorkSaveResponse = TranslateAsPhotoRequestWorkSaveResponse(reader, true);
            }

            return photoRequestWorkSaveResponse;
        }
    }
}