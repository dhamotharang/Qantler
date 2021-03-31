using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_MediaChannelTranslator
    {
        public static M_MediaChannelModel TranslateAsGetMediaChannel(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var mediaChannel = new M_MediaChannelModel();

            if (reader.IsColumnExists("MediaChannelID"))
                mediaChannel.MediaChannelID = SqlHelper.GetNullableInt32(reader, "MediaChannelID");

            if (reader.IsColumnExists("MediaChannelName"))
                mediaChannel.MediaChannelName = SqlHelper.GetNullableString(reader, "MediaChannelName");

            if (reader.IsColumnExists("DisplayOrder"))
                mediaChannel.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return mediaChannel;
        }

        public static List<M_MediaChannelModel> TranslateAsMediaChannel(this SqlDataReader reader)
        {
            var mediaChannel = new List<M_MediaChannelModel>();
            while (reader.Read())
            {
                mediaChannel.Add(TranslateAsGetMediaChannel(reader, true));
            }

            return mediaChannel;
        }
    }
}
