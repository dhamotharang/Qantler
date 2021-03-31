using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_ApproverConfigurationTranslator
    {
        public static int? TranslateAsGetApproverConfiguration(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            int? result;
            if (reader.IsColumnExists("ApproverID"))
                result = SqlHelper.GetNullableInt32(reader, "ApproverID");
            else result = null;
            return result;
        }

        public static List<int?> TranslateAsApproverConfiguration(this SqlDataReader reader)
        {
            var approvers = new List<int?>();
            while (reader.Read())
            {
                approvers.Add(TranslateAsGetApproverConfiguration(reader, true));
            }

            return approvers;
        }
    }
}
