using RulersCourt.Models.Master.M_Vehicle;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Master.Vehicle
{
    public static class M_LogTypeTranslator
    {
        public static M_LogTypeModel TranslateAsGetLogType(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var logType = new M_LogTypeModel();

            if (reader.IsColumnExists("LogTypeID"))
                logType.LogTypeID = SqlHelper.GetNullableInt32(reader, "LogTypeID");

            if (reader.IsColumnExists("LogTypeName"))
                logType.LogTypeName = SqlHelper.GetNullableString(reader, "LogTypeName");

            if (reader.IsColumnExists("DisplayOrder"))
                logType.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return logType;
        }

        public static List<M_LogTypeModel> TranslateAsLogType(this SqlDataReader reader)
        {
            var logType = new List<M_LogTypeModel>();
            while (reader.Read())
            {
                logType.Add(TranslateAsGetLogType(reader, true));
            }

            return logType;
        }
    }
}
