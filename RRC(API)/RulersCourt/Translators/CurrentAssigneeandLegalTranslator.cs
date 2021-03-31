using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class CurrentAssigneeandLegalTranslator
    {
        public static CurrentLegalAssigneeModel TranslateAsCurrentLegalAssignee(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var currentLegalAssignee = new CurrentLegalAssigneeModel();
            if (reader.IsColumnExists("LegalUsedIds"))
                currentLegalAssignee.AssigneeId = SqlHelper.GetNullableInt32(reader, "LegalUsedIds");
            return currentLegalAssignee;
        }

        public static List<CurrentLegalAssigneeModel> TranslateAsCurrentLegalAssigneeList(this SqlDataReader reader)
        {
            var currentLegalAssigneeList = new List<CurrentLegalAssigneeModel>();
            while (reader.Read())
            {
                currentLegalAssigneeList.Add(TranslateAsCurrentLegalAssignee(reader, true));
            }

            return currentLegalAssigneeList;
        }

        public static CurrentLegalHeadModel TranslateAsCurrentLegalHead(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var currentAssignee = new CurrentLegalHeadModel();
            if (reader.IsColumnExists("LegalUsedIds"))
                currentAssignee.LegalHeadUsedId = SqlHelper.GetNullableInt32(reader, "LegalUsedIds");
            return currentAssignee;
        }

        public static List<CurrentLegalHeadModel> TranslateAsCurrentLegalHeadList(this SqlDataReader reader)
        {
            var currentLegalHeadList = new List<CurrentLegalHeadModel>();
            while (reader.Read())
            {
                currentLegalHeadList.Add(TranslateAsCurrentLegalHead(reader, true));
            }

            return currentLegalHeadList;
        }
    }
}