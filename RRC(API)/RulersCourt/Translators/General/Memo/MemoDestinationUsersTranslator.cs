using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class MemoDestinationUsersTranslator
    {
        public static MemoDestinationUsersGetModel TranslateAsGetDestinationUser(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var destinationUser = new MemoDestinationUsersGetModel();

            if (reader.IsColumnExists("MemoDestinationUsersID"))
                destinationUser.MemoDestinationUsersID = SqlHelper.GetNullableInt32(reader, "MemoDestinationUsersID");

            if (reader.IsColumnExists("MemoDestinationUsersName"))
                destinationUser.MemoDestinationUsersName = SqlHelper.GetNullableString(reader, "MemoDestinationUsersName");

            return destinationUser;
        }

        public static List<MemoDestinationUsersGetModel> TranslateAsDestinationUserList(this SqlDataReader reader)
        {
            var destinationUserList = new List<MemoDestinationUsersGetModel>();
            while (reader.Read())
            {
                destinationUserList.Add(TranslateAsGetDestinationUser(reader, true));
            }

            return destinationUserList;
        }

        public static MemoDestinationDepartmentGetModel TranslateAsGetDestinationDepartment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var destinationDepartment = new MemoDestinationDepartmentGetModel();

            if (reader.IsColumnExists("DepartmentID"))
                destinationDepartment.MemoDestinationDepartmentID = SqlHelper.GetNullableInt32(reader, "DepartmentID");

            if (reader.IsColumnExists("MemoDestinationDepartmentName"))
                destinationDepartment.MemoDestinationDepartmentName = SqlHelper.GetNullableString(reader, "MemoDestinationDepartmentName");

            return destinationDepartment;
        }

        public static List<MemoDestinationDepartmentGetModel> TranslateAsDestinationDepartmentList(this SqlDataReader reader)
        {
            var destinationDepartmentList = new List<MemoDestinationDepartmentGetModel>();
            while (reader.Read())
            {
                destinationDepartmentList.Add(TranslateAsGetDestinationDepartment(reader, true));
            }

            return destinationDepartmentList;
        }
    }
}
