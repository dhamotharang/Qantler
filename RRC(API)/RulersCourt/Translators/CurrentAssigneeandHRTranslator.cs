using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class CurrentAssigneeandHRTranslator
    {
        public static CurrentAssigneeModel TranslateAsCurrentAssignee(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var currentAssignee = new CurrentAssigneeModel();
            if (reader.IsColumnExists("HRDeptUsedIds"))
                currentAssignee.AssigneeId = SqlHelper.GetNullableInt32(reader, "HRDeptUsedIds");
            return currentAssignee;
        }

        public static List<CurrentAssigneeModel> TranslateAsCurrentAssigneeList(this SqlDataReader reader)
        {
            var currentAssigneeList = new List<CurrentAssigneeModel>();
            while (reader.Read())
            {
                currentAssigneeList.Add(TranslateAsCurrentAssignee(reader, true));
            }

            return currentAssigneeList;
        }

        public static CurrentHRHeadModel TranslateAsCurrentHRHead(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var currentAssignee = new CurrentHRHeadModel();
            if (reader.IsColumnExists("HRDeptUsedIds"))
                currentAssignee.HRHeadUsedId = SqlHelper.GetNullableInt32(reader, "HRDeptUsedIds");
            return currentAssignee;
        }

        public static List<CurrentHRHeadModel> TranslateAsCurrentHRHeadList(this SqlDataReader reader)
        {
            var currentHRHeadList = new List<CurrentHRHeadModel>();
            while (reader.Read())
            {
                currentHRHeadList.Add(TranslateAsCurrentHRHead(reader, true));
            }

            return currentHRHeadList;
        }
    }
}
