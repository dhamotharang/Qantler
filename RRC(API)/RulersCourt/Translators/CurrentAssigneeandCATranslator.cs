using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class CurrentAssigneeandCATranslator
    {
        public static CurrentCitizenAffairAssigneeModel TranslateAsCurrentCitizenAffairAssignee(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var currentCitizenAffairAssignee = new CurrentCitizenAffairAssigneeModel();
            if (reader.IsColumnExists("CADeptUsedIds"))
                currentCitizenAffairAssignee.AssigneeId = SqlHelper.GetNullableInt32(reader, "CADeptUsedIds");
            return currentCitizenAffairAssignee;
        }

        public static List<CurrentCitizenAffairAssigneeModel> TranslateAsCurrentCitizenAffairAssigneeList(this SqlDataReader reader)
        {
            var currentCitizenAffairAssigneeList = new List<CurrentCitizenAffairAssigneeModel>();
            while (reader.Read())
            {
                currentCitizenAffairAssigneeList.Add(TranslateAsCurrentCitizenAffairAssignee(reader, true));
            }

            return currentCitizenAffairAssigneeList;
        }

        public static CurrentCitizenAffairHeadModel TranslateAsCurrentCitizenAffairHead(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var currentAssignee = new CurrentCitizenAffairHeadModel();
            if (reader.IsColumnExists("CADeptUsedIds"))
                currentAssignee.CAHeadUsedId = SqlHelper.GetNullableInt32(reader, "CADeptUsedIds");
            return currentAssignee;
        }

        public static List<CurrentCitizenAffairHeadModel> TranslateAsCurrentCitizenAffairHeadList(this SqlDataReader reader)
        {
            var currentCitizenAffairHeadList = new List<CurrentCitizenAffairHeadModel>();
            while (reader.Read())
            {
                currentCitizenAffairHeadList.Add(TranslateAsCurrentCitizenAffairHead(reader, true));
            }

            return currentCitizenAffairHeadList;
        }
    }
}
