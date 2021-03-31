using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class CurrentApproverTranslator
    {
        public static CurrentApproverModel TranslateAsCurrentApprover(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var currentApprover = new CurrentApproverModel();

            if (reader.IsColumnExists("ApproverId"))
                currentApprover.ApproverId = SqlHelper.GetNullableInt32(reader, "ApproverId");

            return currentApprover;
        }

        public static List<CurrentApproverModel> TranslateAsCurrentApproverList(this SqlDataReader reader)
        {
            var currentApproverList = new List<CurrentApproverModel>();
            while (reader.Read())
            {
                currentApproverList.Add(TranslateAsCurrentApprover(reader, true));
            }

            return currentApproverList;
        }
    }
}
