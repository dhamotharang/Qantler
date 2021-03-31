using RulersCourt.Models;
using RulersCourt.Services;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Workflow;

namespace RulersCourt.Translators
{
    public static class UserTranslator
    {
        public static UserModel TranslateAsGetUser(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var user = new UserModel();

            if (reader.IsColumnExists("UserID"))
                user.UserID = SqlHelper.GetNullableInt32(reader, "UserID");

            if (reader.IsColumnExists("EmployeeName"))
                user.EmployeeName = SqlHelper.GetNullableString(reader, "EmployeeName");

            if (reader.IsColumnExists("AREmployeeName"))
                user.AREmployeeName = SqlHelper.GetNullableString(reader, "AREmployeeName");

            if (reader.IsColumnExists("OrgUnitID"))
                user.OrgUnitID = SqlHelper.GetNullableInt32(reader, "OrgUnitID");

            if (reader.IsColumnExists("IsOrgHead"))
                user.IsOrgHead = SqlHelper.GetBoolean(reader, "IsOrgHead");

            if (reader.IsColumnExists("IsHOD"))
                user.IsHOD = SqlHelper.GetBoolean(reader, "IsHOD");

            if (reader.IsColumnExists("Gender"))
                user.Gender = SqlHelper.GetNullableString(reader, "Gender");

            if (reader.IsColumnExists("Age"))
                user.Age = SqlHelper.GetNullableString(reader, "Age");

            if (reader.IsColumnExists("Birthday"))
                user.Birthday = SqlHelper.GetDateTime(reader, "Birthday");

            if (reader.IsColumnExists("MobileNumber"))
                user.MobileNumber = SqlHelper.GetNullableString(reader, "MobileNumber");

            if (reader.IsColumnExists("Religion"))
                user.Religion = SqlHelper.GetNullableString(reader, "Religion");

            if (reader.IsColumnExists("OfficialEmailID"))
                user.OfficialEmailID = SqlHelper.GetNullableString(reader, "OfficialEmailID");

            if (reader.IsColumnExists("CanSendEmail"))
                user.CanSendEmail = SqlHelper.GetBoolean(reader, "CanSendEmail");

            if (reader.IsColumnExists("CanSendSMS"))
                user.CanSendSMS = SqlHelper.GetBoolean(reader, "CanSendSMS");

            if (reader.IsColumnExists("DepartmentID"))
                user.DepartmentID = SqlHelper.GetNullableInt32(reader, "DepartmentID");

            return user;
        }

        public static WorkflowUserModel TranslateAsWorkflowGetUser(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var user = new WorkflowUserModel();

            if (reader.IsColumnExists("UserID"))
                user.UserID = SqlHelper.GetNullableInt32(reader, "UserID");

            if (reader.IsColumnExists("EmployeeName"))
                user.EmployeeName = SqlHelper.GetNullableString(reader, "EmployeeName");

            if (reader.IsColumnExists("AREmployeeName"))
                user.AREmployeeName = SqlHelper.GetNullableString(reader, "AREmployeeName");

            if (reader.IsColumnExists("OrgUnitID"))
                user.OrgUnitID = SqlHelper.GetNullableInt32(reader, "OrgUnitID");

            if (reader.IsColumnExists("IsOrgHead"))
                user.IsOrgHead = SqlHelper.GetBoolean(reader, "IsOrgHead");

            if (reader.IsColumnExists("IsHOD"))
                user.IsHOD = SqlHelper.GetBoolean(reader, "IsHOD");

            if (reader.IsColumnExists("Gender"))
                user.Gender = SqlHelper.GetNullableString(reader, "Gender");

            if (reader.IsColumnExists("Age"))
                user.Age = SqlHelper.GetNullableString(reader, "Age");

            if (reader.IsColumnExists("Birthday"))
                user.Birthday = SqlHelper.GetDateTime(reader, "Birthday");

            if (reader.IsColumnExists("MobileNumber"))
                user.MobileNumber = SqlHelper.GetNullableString(reader, "MobileNumber");

            if (reader.IsColumnExists("Religion"))
                user.Religion = SqlHelper.GetNullableString(reader, "Religion");

            if (reader.IsColumnExists("OfficialEmailID"))
                user.OfficialEmailID = SqlHelper.GetNullableString(reader, "OfficialEmailID");

            if (reader.IsColumnExists("CanSendEmail"))
                user.CanSendEmail = SqlHelper.GetBoolean(reader, "CanSendEmail");

            if (reader.IsColumnExists("CanSendSMS"))
                user.CanSendSMS = SqlHelper.GetBoolean(reader, "CanSendSMS");

            if (reader.IsColumnExists("DepartmentID"))
                user.DepartmentID = SqlHelper.GetNullableInt32(reader, "DepartmentID");

            return user;
        }

        public static List<UserModel> TranslateAsUserList(this SqlDataReader reader)
        {
            var users = new List<UserModel>();
            while (reader.Read())
            {
                users.Add(TranslateAsGetUser(reader, true));
            }

            return users;
        }

        public static List<WorkflowUserModel> TranslateAsWorkflowUserList(this SqlDataReader reader)
        {
            var users = new List<WorkflowUserModel>();
            while (reader.Read())
            {
                users.Add(TranslateAsWorkflowGetUser(reader, true));
            }

            return users;
        }

        public static Actor TranslateAsGetActor(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var user = new Actor();

            if (reader.IsColumnExists("EmployeeName"))
                user.Name = SqlHelper.GetNullableString(reader, "EmployeeName");

            if (reader.IsColumnExists("AREmployeeName"))
                user.ARName = SqlHelper.GetNullableString(reader, "AREmployeeName");

            if (reader.IsColumnExists("MobileNumber"))
                user.PhoneNumber = SqlHelper.GetNullableString(reader, "MobileNumber");

            if (reader.IsColumnExists("OfficialEmailID"))
                user.Email = SqlHelper.GetNullableString(reader, "OfficialEmailID");

            if (reader.IsColumnExists("CanSendEmail"))
                user.CanSendEmail = SqlHelper.GetBoolean(reader, "CanSendEmail") ?? false;

            if (reader.IsColumnExists("CanSendSMS"))
                user.CanSendSMS = SqlHelper.GetBoolean(reader, "CanSendSMS") ?? false;

            return user;
        }

        public static Actor TranslateAsGetActors(this SqlDataReader reader)
        {
            var users = new List<Actor>();
            while (reader.Read())
            {
                users.Add(TranslateAsGetActor(reader, true));
            }

            return users.FirstOrDefault();
        }

        public static User TranslateAsGetLoginUser(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var user = new User();

            if (reader.IsColumnExists("UserID"))
                user.UserID = SqlHelper.GetNullableInt32(reader, "UserID");

            if (reader.IsColumnExists("UnitID"))
                user.UnitID = SqlHelper.GetNullableInt32(reader, "UnitID");

            if (reader.IsColumnExists("DepartmentID"))
                user.DepartmentID = SqlHelper.GetNullableInt32(reader, "DepartmentID");

            if (reader.IsColumnExists("SectionID"))
                user.SectionID = SqlHelper.GetNullableInt32(reader, "SectionID");

            if (reader.IsColumnExists("DisplayName"))
                user.DisplayName = SqlHelper.GetNullableString(reader, "DisplayName");

            if (reader.IsColumnExists("UnitName"))
                user.UnitName = SqlHelper.GetNullableString(reader, "UnitName");

            if (reader.IsColumnExists("SectionName"))
                user.SectionName = SqlHelper.GetNullableString(reader, "SectionName");

            if (reader.IsColumnExists("DepartmentName"))
                user.DepartmentName = SqlHelper.GetNullableString(reader, "DepartmentName");

            if (reader.IsColumnExists("HOD"))
                user.HOD = SqlHelper.GetBoolean(reader, "HOD");

            if (reader.IsColumnExists("HOS"))
                user.HOS = SqlHelper.GetBoolean(reader, "HOS");

            if (reader.IsColumnExists("HOU"))
                user.HOU = SqlHelper.GetBoolean(reader, "HOU");

            if (reader.IsColumnExists("CanEditContact"))
                user.CanEditContact = SqlHelper.GetBoolean(reader, "CanEditContact");

            if (reader.IsColumnExists("CanManageNews"))
                user.CanManageNews = SqlHelper.GetBoolean(reader, "CanManageNews");

            if (reader.IsColumnExists("CanRaiseOfficalRequest"))
                user.CanRaiseOfficalRequest = SqlHelper.GetBoolean(reader, "CanRaiseOfficalRequest");

            if (reader.IsColumnExists("AttachmentGuid"))
                user.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentName"))
                user.AttachmentName = SqlHelper.GetNullableString(reader, "AttachmentName");

            if (reader.IsColumnExists("InTime"))
                user.InTime = SqlHelper.GetDateTime(reader, "InTime");

            if (reader.IsColumnExists("OutTime"))
                user.OutTime = SqlHelper.GetDateTime(reader, "OutTime");

            return user;
        }

        public static User TranslateAsLoginUserList(this SqlDataReader reader)
        {
            var users = new User();
            while (reader.Read())
            {
                users = TranslateAsGetLoginUser(reader, true);
            }

            return users;
        }
    }
}
