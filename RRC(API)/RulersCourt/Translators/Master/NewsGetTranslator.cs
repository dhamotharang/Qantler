using RulersCourt.Models.Master.M_News;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Master
{
    public static class NewsGetTranslator
    {
        public static M_NewsGetModel TranslateAsDesignGetbyID(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var newsModel = new M_NewsGetModel();
            if (reader.IsColumnExists("NewsID"))
                newsModel.NewsID = SqlHelper.GetNullableInt32(reader, "NewsID");

            if (reader.IsColumnExists("News"))
                newsModel.News = SqlHelper.GetNullableString(reader, "News");

            if (reader.IsColumnExists("ExpiryDate"))
                newsModel.ExpiryDate = SqlHelper.GetDateTime(reader, "ExpiryDate");

            if (reader.IsColumnExists("CreatedBy"))
                newsModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                newsModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                newsModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                newsModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            return newsModel;
        }

        public static List<M_NewsGetModel> TranslateAsNewsList(this SqlDataReader reader)
        {
            var babyAdditionList = new List<M_NewsGetModel>();
            while (reader.Read())
            {
                babyAdditionList.Add(TranslateAsDesignGetbyID(reader, true));
            }

            return babyAdditionList;
        }

        public static M_NewsModel TranslateAsPhotoList(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var newsModel = new M_NewsModel();

            if (reader.IsColumnExists("NewsID"))
                newsModel.NewsID = SqlHelper.GetNullableInt32(reader, "NewsID");

            if (reader.IsColumnExists("News"))
                newsModel.News = SqlHelper.GetNullableString(reader, "News");

            if (reader.IsColumnExists("ExpiryDate"))
                newsModel.ExpiryDate = SqlHelper.GetDateTime(reader, "ExpiryDate");

            if (reader.IsColumnExists("CreatedBy"))
                newsModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                newsModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                newsModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                newsModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            return newsModel;
        }

        public static List<M_NewsModel> TranslateAsNewsAllList(this SqlDataReader reader)
        {
            var babyAdditionList = new List<M_NewsModel>();
            while (reader.Read())
            {
                babyAdditionList.Add(TranslateAsPhotoList(reader, true));
            }

            return babyAdditionList;
        }
    }
}