using RulersCourt.Models.Protocol;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol
{
    public static class ProtocolTranslator
    {
        public static ProtocolHomeModel TranslateAsProtocolDashboardCount(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var mediahomemodel = new ProtocolHomeModel();

            if (reader.IsColumnExists("Calendar"))
            {
                mediahomemodel.Calendar = SqlHelper.GetNullableInt32(reader, "Calendar");
            }

            if (reader.IsColumnExists("Gift"))
            {
                mediahomemodel.Gift = SqlHelper.GetNullableInt32(reader, "Gift");
            }

            if (reader.IsColumnExists("Media"))
            {
                mediahomemodel.Media = SqlHelper.GetNullableInt32(reader, "Media");
            }

            return mediahomemodel;
        }

        public static ProtocolHomeModel TranslateasProtocolDashboardcount(this SqlDataReader reader)
        {
            var mediahomemodel = new ProtocolHomeModel();
            while (reader.Read())
            {
                mediahomemodel = TranslateAsProtocolDashboardCount(reader, true);
            }

            return mediahomemodel;
        }
    }
}
