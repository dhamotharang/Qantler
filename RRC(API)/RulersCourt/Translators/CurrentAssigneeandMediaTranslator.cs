using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class CurrentAssigneeandMediaTranslator
    {
        public static CurrentMediaAssigneeModel TranslateAsCurrentMediaAssignee(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var currentMediaAssignee = new CurrentMediaAssigneeModel();
            if (reader.IsColumnExists("MediaDeptUsedIds"))
                currentMediaAssignee.AssigneeId = SqlHelper.GetNullableInt32(reader, "MediaDeptUsedIds");
            return currentMediaAssignee;
        }

        public static List<CurrentMediaAssigneeModel> TranslateAsCurrentMediaAssigneeList(this SqlDataReader reader)
        {
            var currentMediaAssigneeList = new List<CurrentMediaAssigneeModel>();
            while (reader.Read())
            {
                currentMediaAssigneeList.Add(TranslateAsCurrentMediaAssignee(reader, true));
            }

            return currentMediaAssigneeList;
        }

        public static CurrentMediaHeadModel TranslateAsCurrentMediaHead(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var currentAssignee = new CurrentMediaHeadModel();
            if (reader.IsColumnExists("MediaDeptUsedIds"))
                currentAssignee.MediaHeadUsedId = SqlHelper.GetNullableInt32(reader, "MediaDeptUsedIds");
            return currentAssignee;
        }

        public static List<CurrentMediaHeadModel> TranslateAsCurrentMediaHeadList(this SqlDataReader reader)
        {
            var currentMediaHeadList = new List<CurrentMediaHeadModel>();
            while (reader.Read())
            {
                currentMediaHeadList.Add(TranslateAsCurrentMediaHead(reader, true));
            }

            return currentMediaHeadList;
        }
    }
}
