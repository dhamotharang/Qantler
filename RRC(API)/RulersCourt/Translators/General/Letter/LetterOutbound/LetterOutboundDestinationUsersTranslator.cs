using RulersCourt.Models.Letter;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Letter
{
    public static class LetterOutboundDestinationUsersTranslator
    {
        public static LetterOutboundDestinationUsersModel TranslateAsGetDestinationUser(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var destinationUser = new LetterOutboundDestinationUsersModel();

            if (reader.IsColumnExists("LetterDestinationUsersName"))
                destinationUser.LetterDestinationUsersName = SqlHelper.GetNullableString(reader, "LetterDestinationUsersName");

            return destinationUser;
        }

        public static List<LetterOutboundDestinationUsersModel> TranslateAsLetterDestinationUserList(this SqlDataReader reader)
        {
            var destinationUserList = new List<LetterOutboundDestinationUsersModel>();
            while (reader.Read())
            {
                destinationUserList.Add(TranslateAsGetDestinationUser(reader, true));
            }

            return destinationUserList;
        }

        public static LetterOutboundDestinationDepartmentModel TranslateAsGetDestinationDepartment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var destinationDepartment = new LetterOutboundDestinationDepartmentModel();

            if (reader.IsColumnExists("LetterDestinationEntityID"))
                destinationDepartment.LetterDestinationEntityID = SqlHelper.GetNullableInt32(reader, "LetterDestinationEntityID");

            if (reader.IsColumnExists("LetterDestinationEntityName"))
                destinationDepartment.LetterDestinationEntityName = SqlHelper.GetNullableString(reader, "LetterDestinationEntityName");

            if (reader.IsColumnExists("LetterDestinationID"))
                destinationDepartment.LetterDestinationID = SqlHelper.GetNullableInt32(reader, "LetterDestinationID");

            if (reader.IsColumnExists("LetterDestinationUserName"))
                destinationDepartment.LetterDestinationUserName = SqlHelper.GetNullableString(reader, "LetterDestinationUserName");

            if (reader.IsColumnExists("IsGovernmentEntity"))
                destinationDepartment.IsGovernmentEntity = SqlHelper.GetBoolean(reader, "IsGovernmentEntity");

            return destinationDepartment;
        }

        public static List<LetterOutboundDestinationDepartmentModel> TranslateAsLetterDestinationDepartmentList(this SqlDataReader reader)
        {
            var destinationDepartmentList = new List<LetterOutboundDestinationDepartmentModel>();
            while (reader.Read())
            {
                destinationDepartmentList.Add(TranslateAsGetDestinationDepartment(reader, true));
            }

            return destinationDepartmentList;
        }
    }
}
