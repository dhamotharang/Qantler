using RulersCourt.Models.Announcement;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Announcement
{
    public static class AnnouncementTypeAndDescriptionTranslator
    {
        public static AnnouncementTypeAndDescriptionModel TranslateAsGetAnnouncementTypeAndDescription(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var announcementTypeAndDescription = new AnnouncementTypeAndDescriptionModel();

            if (reader.IsColumnExists("AnnouncementTypeID"))
                announcementTypeAndDescription.AnnouncementTypeID = SqlHelper.GetNullableInt32(reader, "AnnouncementTypeID");

            if (reader.IsColumnExists("AnnouncementTypeName"))
                announcementTypeAndDescription.AnnouncementTypeName = SqlHelper.GetNullableString(reader, "AnnouncementTypeName");

            if (reader.IsColumnExists("AnnouncementTypeNameAr"))
                announcementTypeAndDescription.AnnouncementTypeNameAr = SqlHelper.GetNullableString(reader, "AnnouncementTypeNameAr");

            if (reader.IsColumnExists("Description"))
                announcementTypeAndDescription.Description = SqlHelper.GetNullableString(reader, "Description");

            if (reader.IsColumnExists("DescriptionAr"))
                announcementTypeAndDescription.DescriptionAr = SqlHelper.GetNullableString(reader, "DescriptionAr");

            return announcementTypeAndDescription;
        }

        public static List<AnnouncementTypeAndDescriptionModel> TranslateAsAnnouncementTypeAndDescriptionList(this SqlDataReader reader)
        {
            var announcementTypeAndDescriptionList = new List<AnnouncementTypeAndDescriptionModel>();
            while (reader.Read())
            {
                announcementTypeAndDescriptionList.Add(TranslateAsGetAnnouncementTypeAndDescription(reader, true));
            }

            return announcementTypeAndDescriptionList;
        }
    }
}
