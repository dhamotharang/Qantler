using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_UserManagementTranslator
    {
        public static M_UserManagementModel TranslateAsGetUser(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var user = new M_UserManagementModel();

            if (reader.IsColumnExists("UserProfileID"))
                user.UserProfileID = SqlHelper.GetNullableInt32(reader, "UserProfileID");

            if (reader.IsColumnExists("EmployeeName"))
                user.EmployeeName = SqlHelper.GetNullableString(reader, "EmployeeName");

            if (reader.IsColumnExists("DepartmentID"))
                user.DepartmentID = SqlHelper.GetNullableInt32(reader, "DepartmentID");

            if (reader.IsColumnExists("SectionID"))
                user.SectionID = SqlHelper.GetNullableInt32(reader, "SectionID");

            if (reader.IsColumnExists("UnitID"))
                user.UnitID = SqlHelper.GetNullableInt32(reader, "UnitID");

            if (reader.IsColumnExists("HOD"))
                user.HOD = SqlHelper.GetBoolean(reader, "HOD");

            if (reader.IsColumnExists("HOS"))
                user.HOS = SqlHelper.GetBoolean(reader, "HOS");

            if (reader.IsColumnExists("HOU"))
                user.HOU = SqlHelper.GetBoolean(reader, "HOU");

            if (reader.IsColumnExists("CanRaiseOfficalRequest"))
                user.CanRaiseOfficalRequest = SqlHelper.GetBoolean(reader, "CanRaiseOfficalRequest");

            if (reader.IsColumnExists("CanManageNews"))
                user.CanManageNews = SqlHelper.GetBoolean(reader, "CanManageNews");

            if (reader.IsColumnExists("CanEditContact"))
                user.CanEditContact = SqlHelper.GetBoolean(reader, "CanEditContact");

            if (reader.IsColumnExists("balanceLeave"))
                user.BalanceLeave = SqlHelper.GetNullableInt32(reader, "balanceLeave");

            return user;
        }

        public static List<M_UserManagementModel> TranslateAsUserManagement(this SqlDataReader reader)
        {
            var users = new List<M_UserManagementModel>();
            while (reader.Read())
            {
                users.Add(TranslateAsGetUser(reader, true));
            }

            return users;
        }
    }
}
