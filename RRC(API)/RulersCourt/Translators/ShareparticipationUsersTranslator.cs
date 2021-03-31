using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class ShareparticipationUsersTranslator
    {
        public static ShareparticipationUsersModel TranslateAsGetShareUser(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var shareUser = new ShareparticipationUsersModel();

            if (reader.IsColumnExists("UserID"))
                shareUser.UserID = SqlHelper.GetNullableInt32(reader, "UserID");

            return shareUser;
        }

        public static List<ShareparticipationUsersModel> TranslateAsGetShareUserList(this SqlDataReader reader)
        {
            var shareUserList = new List<ShareparticipationUsersModel>();
            while (reader.Read())
            {
                shareUserList.Add(TranslateAsGetShareUser(reader, true));
            }

            return shareUserList;
        }
    }
}
