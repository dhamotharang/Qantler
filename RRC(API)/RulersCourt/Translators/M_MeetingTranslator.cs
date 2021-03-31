using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_MeetingTranslator
    {
        public static M_MeetingModel TranslateAsGetLocation(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var location = new M_MeetingModel();

            if (reader.IsColumnExists("MeetingTypeID"))
                location.MeetingTypeID = SqlHelper.GetNullableInt32(reader, "MeetingTypeID");

            if (reader.IsColumnExists("MeetingTypeName"))
                location.MeetingTypeName = SqlHelper.GetNullableString(reader, "MeetingTypeName");

            if (reader.IsColumnExists("DisplayOrder"))
                location.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return location;
        }

        public static List<M_MeetingModel> TranslateAsMeetingType(this SqlDataReader reader)
        {
            var location = new List<M_MeetingModel>();
            while (reader.Read())
            {
                location.Add(TranslateAsGetLocation(reader, true));
            }

            return location;
        }
    }
}
