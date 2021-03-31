using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class LetterUserNameAndCreatorTranslator
    {
        public static LetterUserNameAndCreatorModel TranslateAsGetUser(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var user = new LetterUserNameAndCreatorModel();

            if (reader.IsColumnExists("UserID"))
                user.UserID = SqlHelper.GetNullableInt32(reader, "UserID");

            if (reader.IsColumnExists("UserName"))
                user.UserName = SqlHelper.GetNullableString(reader, "UserName");

            return user;
        }

        public static List<LetterUserNameAndCreatorModel> TranslateAsLetterGetUserList(this SqlDataReader reader)
        {
            var userList = new List<LetterUserNameAndCreatorModel>();
            while (reader.Read())
            {
                userList.Add(TranslateAsGetUser(reader, true));
            }

            return userList;
        }
    }
}
