using RulersCourt.Models.Master.M_Photos;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Master
{
    public static class BannerGetTranslator
    {
        public static M_BannerGetModel TranslateAsBannerGet(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var banner = new M_BannerGetModel();

            if (reader.IsColumnExists("BannerID"))
                banner.BannerID = SqlHelper.GetNullableInt32(reader, "BannerID");

            if (reader.IsColumnExists("AttachmentName"))
                banner.AttachmentName = SqlHelper.GetNullableString(reader, "AttachmentName");

            if (reader.IsColumnExists("AttachmentGuid"))
                banner.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("CreatedBy"))
                banner.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                banner.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            return banner;
        }

        public static M_BannerGetModel TranslateAsBannerList(this SqlDataReader reader)
        {
            var banner = new M_BannerGetModel();
            while (reader.Read())
            {
                banner = TranslateAsBannerGet(reader, true);
            }

            return banner;
        }
    }
}