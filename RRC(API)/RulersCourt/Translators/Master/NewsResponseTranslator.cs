using RulersCourt.Models.Master.M_News;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Master
{
    public static class NewsResponseTranslator
    {
        public static NewsResponseModel TranslateAsDesignSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var newsSave = new NewsResponseModel();

            if (reader.IsColumnExists("NewsID"))
                newsSave.NewsID = SqlHelper.GetNullableInt32(reader, "NewsID");

            return newsSave;
        }

        public static NewsResponseModel TranslateAsNewsSaveResponseList(this SqlDataReader reader)
        {
            var newsSaveResponse = new NewsResponseModel();
            while (reader.Read())
            {
                newsSaveResponse = TranslateAsDesignSaveResponse(reader, true);
            }

            return newsSaveResponse;
        }
    }
}