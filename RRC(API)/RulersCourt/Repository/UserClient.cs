using RulersCourt.Models;
using RulersCourt.Services;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class UserClient
    {
        public List<UserModel> GetUsers(string connString, string department, string userID, int type, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Department", department),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Type", type),
                new SqlParameter("@P_Language", lang),
            };

            return SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_User", r => r.TranslateAsUserList(), param);
        }

        public User GetLoginUsers(string connString, User user, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_UserName", user.ADUserName),
                new SqlParameter("@P_Language", lang),
            };
            User result = new User();
            result = SqlHelper.ExecuteProcedureReturnData<User>(connString, "Get_UserByADUserName", r => r.TranslateAsLoginUserList(), param);
            user.UserID = result.UserID;
            user.UnitID = result.UnitID;
            user.UnitName = result.UnitName;
            user.DepartmentID = result.DepartmentID;
            user.DepartmentName = result.DepartmentName;
            user.SectionID = result.SectionID;
            user.SectionName = result.SectionName;
            user.DisplayName = result.DisplayName;
            user.HOD = result.HOD;
            user.HOS = result.HOS;
            user.HOU = result.HOU;
            user.CanEditContact = result.CanEditContact;
            user.CanManageNews = result.CanManageNews;
            user.CanRaiseOfficalRequest = result.CanRaiseOfficalRequest;
            user.AttachmentGuid = result.AttachmentGuid;
            user.AttachmentName = result.AttachmentName;
            user.InTime = result.InTime;
            user.OutTime = result.OutTime;

            return user;
        }
    }
}
