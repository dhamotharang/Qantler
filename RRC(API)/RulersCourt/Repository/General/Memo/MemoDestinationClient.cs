using RulersCourt.Models;
using RulersCourt.Translators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class MemoDestinationClient
    {
        public void SaveUser(string connString, List<MemoDestinationUsersGetModel> destinationUserId, int? memoId, string lang)
        {
            SqlParameter[] getDestinationparam = {
                    new SqlParameter("@P_MemoID", memoId), new SqlParameter("@P_Language", lang) };
            var oldDestinationuser = SqlHelper.ExecuteProcedureReturnData<List<MemoDestinationUsersGetModel>>(connString, "Get_MemoDestinationUsers", r => r.TranslateAsDestinationUserList(), getDestinationparam);

            foreach (var item in destinationUserId)
            {
                try
                {
                    if (oldDestinationuser.Count > 0)
                    {
                        var temp = false;
                        try
                        {
                            temp = Convert.ToBoolean(oldDestinationuser.Find(a => a.MemoDestinationUsersID.Equals(item.MemoDestinationUsersID)).MemoDestinationUsersID);
                            temp = false;
                        }
                        catch (NullReferenceException)
                        {
                            temp = true;
                        }

                        if (temp)
                        {
                            SqlParameter[] destinationparam = {
                            new SqlParameter("@P_MemoID", memoId),
                            new SqlParameter("@P_UserID", item.MemoDestinationUsersID),
                            new SqlParameter("@P_Type", 1) };
                            SqlHelper.ExecuteProcedureReturnString(connString, "Save_MemoDestinationUser", destinationparam);
                        }
                    }
                    else
                    {
                        SqlParameter[] destinationparam = {
                            new SqlParameter("@P_MemoID", memoId),
                            new SqlParameter("@P_UserID", item.MemoDestinationUsersID),
                            new SqlParameter("@P_Type", 1) };
                        SqlHelper.ExecuteProcedureReturnString(connString, "Save_MemoDestinationUser", destinationparam);
                    }
                }
                catch (Exception)
                {
                }
            }

            foreach (var item in oldDestinationuser)
            {
                if (destinationUserId.Find(a => a.MemoDestinationUsersID.Equals(item.MemoDestinationUsersID)) is null)
                {
                    SqlParameter[] destinationparam = {
                    new SqlParameter("@P_MemoID", memoId),
                    new SqlParameter("@P_UserID", item.MemoDestinationUsersID),
                    new SqlParameter("@P_Type", 2) };
                    SqlHelper.ExecuteProcedureReturnString(connString, "Save_MemoDestinationUser", destinationparam);
                }
            }
        }

        public void SaveDepartment(string connString, List<MemoDestinationDepartmentGetModel> destinationDepartmentId, int? memoId)
        {
            SqlParameter[] getDestinationparam = {
                    new SqlParameter("@P_MemoID", memoId) };
            var oldDestinationDepartment = SqlHelper.ExecuteProcedureReturnData<List<MemoDestinationDepartmentGetModel>>(connString, "Get_MemoDestinationDepartment", r => r.TranslateAsDestinationDepartmentList(), getDestinationparam);

            foreach (var item in destinationDepartmentId)
            {
                try
                {
                    if (oldDestinationDepartment.Count > 0)
                    {
                        var temp = false;
                        try
                        {
                            temp = Convert.ToBoolean(oldDestinationDepartment.Find(a => a.MemoDestinationDepartmentID.Equals(item.MemoDestinationDepartmentID)).MemoDestinationDepartmentID);
                            temp = false;
                        }
                        catch (NullReferenceException)
                        {
                            temp = true;
                        }

                        if (temp)
                        {
                            SqlParameter[] destinationparam = {
                            new SqlParameter("@P_MemoID", memoId),
                            new SqlParameter("@P_DepartmentID", item.MemoDestinationDepartmentID),
                            new SqlParameter("@P_Type", 1) };
                            SqlHelper.ExecuteProcedureReturnString(connString, "Save_MemoDestinationDepartment", destinationparam);
                        }
                    }
                    else
                    {
                        SqlParameter[] destinationparam = {
                            new SqlParameter("@P_MemoID", memoId),
                            new SqlParameter("@P_DepartmentID", item.MemoDestinationDepartmentID),
                            new SqlParameter("@P_Type", 1) };
                        SqlHelper.ExecuteProcedureReturnString(connString, "Save_MemoDestinationDepartment", destinationparam);
                    }
                }
                catch (Exception)
                {
                }
            }

            foreach (var item in oldDestinationDepartment)
            {
                if (destinationDepartmentId.Find(a => a.MemoDestinationDepartmentID.Equals(item.MemoDestinationDepartmentID)) is null)
                {
                    SqlParameter[] destinationparam = {
                    new SqlParameter("@P_MemoID", memoId),
                    new SqlParameter("@P_DepartmentID", item.MemoDestinationDepartmentID),
                    new SqlParameter("@P_Type", 2) };
                    SqlHelper.ExecuteProcedureReturnString(connString, "Save_MemoDestinationDepartment", destinationparam);
                }
            }
        }
    }
}
