using RulersCourt.Models.Master.M_Photos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Translators.Master
{
    public static class PhotoGetTranslator
    {
        public static M_PhotoGetModel TranslateAsDesignGetbyID(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var photoModel = new M_PhotoGetModel();

            if (reader.IsColumnExists("PhotoID"))
                photoModel.PhotoID = SqlHelper.GetNullableInt32(reader, "PhotoID");

            if (reader.IsColumnExists("AttachmentName"))
                photoModel.AttachmentName = SqlHelper.GetNullableString(reader, "AttachmentName");

            if (reader.IsColumnExists("AttachmentGuid"))
                photoModel.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("ExpiryDate"))
                photoModel.ExpiryDate = SqlHelper.GetDateTime(reader, "ExpiryDate");

            if (reader.IsColumnExists("CreatedBy"))
                photoModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                photoModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                photoModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                photoModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            return photoModel;
        }

        public static List<M_PhotoGetModel> TranslateAsDesignList(this SqlDataReader reader)
        {
            var babyAdditionList = new List<M_PhotoGetModel>();
            while (reader.Read())
            {
                babyAdditionList.Add(TranslateAsDesignGetbyID(reader, true));
            }

            return babyAdditionList;
        }

        public static M_PhotoModel TranslateAsPhotoList(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var photoModel = new M_PhotoModel();

            if (reader.IsColumnExists("PhotoID"))
                photoModel.PhotoID = SqlHelper.GetNullableInt32(reader, "PhotoID");

            if (reader.IsColumnExists("AttachmentName"))
                photoModel.AttachmentName = SqlHelper.GetNullableString(reader, "AttachmentName");

            if (reader.IsColumnExists("AttachmentGuid"))
                photoModel.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("ExpiryDate"))
                photoModel.ExpiryDate = SqlHelper.GetDateTime(reader, "ExpiryDate");

            if (reader.IsColumnExists("CreatedBy"))
                photoModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                photoModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                photoModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                photoModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            return photoModel;
        }

        public static List<M_PhotoModel> TranslateAsPhotoAllList(this SqlDataReader reader)
        {
            var babyAdditionList = new List<M_PhotoModel>();
            while (reader.Read())
            {
                babyAdditionList.Add(TranslateAsPhotoList(reader, true));
            }

            return babyAdditionList;
        }
    }
}