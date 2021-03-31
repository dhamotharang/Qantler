using RulersCourt.Models.DutyTasks;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.DutyTask
{
    public static class DutyTaskAssigneeAndCreatorTranslator
    {
        public static DutyTaskAssigneeAndCreatorModel TranslateAsGetUser(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var user = new DutyTaskAssigneeAndCreatorModel();

            if (reader.IsColumnExists("UserID"))
                user.UserID = SqlHelper.GetNullableInt32(reader, "UserID");

            if (reader.IsColumnExists("UserName"))
                user.UserName = SqlHelper.GetNullableString(reader, "UserName");
            return user;
        }

        public static List<DutyTaskAssigneeAndCreatorModel> TranslateAsGetUserList(this SqlDataReader reader)
        {
            var userList = new List<DutyTaskAssigneeAndCreatorModel>();
            while (reader.Read())
            {
                userList.Add(TranslateAsGetUser(reader, true));
            }

            return userList;
        }
    }
}
