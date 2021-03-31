using RulersCourt.Models;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class GlobalSearchCountTranslator
    {
        public static GlobalSearchCountModel TranslateAsGetCount(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var count = new GlobalSearchCountModel();

            if (reader.IsColumnExists("Memos"))
                count.Memos = SqlHelper.GetNullableInt32(reader, "Memos");

            if (reader.IsColumnExists("Letters"))
                count.Letters = SqlHelper.GetNullableInt32(reader, "Letters");

            if (reader.IsColumnExists("DutyTask"))
                count.DutyTask = SqlHelper.GetNullableInt32(reader, "DutyTask");

            if (reader.IsColumnExists("Meetings"))
                count.Meetings = SqlHelper.GetNullableInt32(reader, "Meetings");

            if (reader.IsColumnExists("Circulars"))
                count.Circulars = SqlHelper.GetNullableInt32(reader, "Circulars");

            if (reader.IsColumnExists("Legal"))
                count.Legal = SqlHelper.GetNullableInt32(reader, "Legal");

            if (reader.IsColumnExists("Protocol"))
                count.Protocol = SqlHelper.GetNullableInt32(reader, "Protocol");

            if (reader.IsColumnExists("HR"))
                count.HR = SqlHelper.GetNullableInt32(reader, "HR");

            if (reader.IsColumnExists("CitizenAffair"))
                count.CitizenAffair = SqlHelper.GetNullableInt32(reader, "CitizenAffair");

            if (reader.IsColumnExists("Maintenance"))
                count.Maintenance = SqlHelper.GetNullableInt32(reader, "Maintenance");

            if (reader.IsColumnExists("IT"))
                count.IT = SqlHelper.GetNullableInt32(reader, "IT");

            return count;
        }

        public static GlobalSearchCountModel TranslateAsGlobalSearchCount(this SqlDataReader reader)
        {
            var countAllModules = new GlobalSearchCountModel();
            while (reader.Read())
            {
                countAllModules = TranslateAsGetCount(reader, true);
            }

            return countAllModules;
        }
    }
}
