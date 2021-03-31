using RulersCourt.Models.Master.M_Photos;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Master
{
    public static class PhotosPostTranslator
    {
        public static PhotoResponseModel TranslateAsDesignSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var photoSave = new PhotoResponseModel();

            if (reader.IsColumnExists("PhotoID"))
                photoSave.PhotoID = SqlHelper.GetNullableInt32(reader, "PhotoID");

            return photoSave;
        }

        public static PhotoResponseModel TranslateAsPhotoSaveResponseList(this SqlDataReader reader)
        {
            var photoSaveResponse = new PhotoResponseModel();
            while (reader.Read())
            {
                photoSaveResponse = TranslateAsDesignSaveResponse(reader, true);
            }

            return photoSaveResponse;
        }
    }
}
