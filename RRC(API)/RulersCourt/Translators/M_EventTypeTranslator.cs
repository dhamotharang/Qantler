using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_EventTypeTranslator
    {
        public static M_EventTypeModel TranslateAsGetLocation(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var location = new M_EventTypeModel();

            if (reader.IsColumnExists("EventTypeID"))
                location.EventTypeID = SqlHelper.GetNullableInt32(reader, "EventTypeID");

            if (reader.IsColumnExists("EventTypeName"))
                location.EventTypeName = SqlHelper.GetNullableString(reader, "EventTypeName");

            if (reader.IsColumnExists("DisplayOrder"))
                location.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return location;
        }

        public static List<M_EventTypeModel> TranslateAsEventType(this SqlDataReader reader)
        {
            var location = new List<M_EventTypeModel>();
            while (reader.Read())
            {
                location.Add(TranslateAsGetLocation(reader, true));
            }

            return location;
        }
    }
}