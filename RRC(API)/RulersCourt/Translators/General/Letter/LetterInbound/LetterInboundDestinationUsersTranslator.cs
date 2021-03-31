using RulersCourt.Models.Letter.LetterInbound;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Letter.LetterInbound
{
    public static class LetterInboundDestinationUsersTranslator
    {
        public static LetterInboundDestinationUsersModel TranslateAsGetDestinationUser(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var destinationUser = new LetterInboundDestinationUsersModel();

            if (reader.IsColumnExists("LetterDestinationUsersID"))
                destinationUser.LetterDestinationUsersID = SqlHelper.GetNullableInt32(reader, "LetterDestinationUsersID");

            if (reader.IsColumnExists("LetterDestinationUsersName"))
                destinationUser.LetterDestinationUsersName = SqlHelper.GetNullableString(reader, "LetterDestinationUsersName");

            return destinationUser;
        }

        public static List<LetterInboundDestinationUsersModel> TranslateAsLetterInboundDestinationUserList(this SqlDataReader reader)
        {
            var destinationUserList = new List<LetterInboundDestinationUsersModel>();
            while (reader.Read())
            {
                destinationUserList.Add(TranslateAsGetDestinationUser(reader, true));
            }

            return destinationUserList;
        }

        public static LetterInboundDestinationDepartmentModel TranslateAsGetDestinationDepartment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var destinationDepartment = new LetterInboundDestinationDepartmentModel();

            if (reader.IsColumnExists("LetterDestinationDepartmentID"))
                destinationDepartment.LetterDestinationDepartmentID = SqlHelper.GetNullableInt32(reader, "LetterDestinationDepartmentID");

            if (reader.IsColumnExists("LetterDestinationDepartmentName"))
                destinationDepartment.LetterDestinationDepartmentName = SqlHelper.GetNullableString(reader, "LetterDestinationDepartmentName");

            return destinationDepartment;
        }

        public static List<LetterInboundDestinationDepartmentModel> TranslateAsLetterInboundDestinationDepartmentList(this SqlDataReader reader)
        {
            var destinationDepartmentList = new List<LetterInboundDestinationDepartmentModel>();
            while (reader.Read())
            {
                destinationDepartmentList.Add(TranslateAsGetDestinationDepartment(reader, true));
            }

            return destinationDepartmentList;
        }
    }
}
