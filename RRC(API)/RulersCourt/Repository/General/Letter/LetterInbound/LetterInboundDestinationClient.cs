using RulersCourt.Models.Letter.LetterInbound;
using RulersCourt.Translators.Letter.LetterInbound;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Letter.LetterInbound
{
    public class LetterInboundDestinationClient
    {
        public void SaveUser(string connString, List<LetterInboundDestinationUsersModel> destinationUserId, int? letterId, string lang)
        {
            SqlParameter[] getDestinationparam = {
                    new SqlParameter("@P_LetterID", letterId),
                    new SqlParameter("@P_Language", lang),
            };
            var oldDestinationuser = SqlHelper.ExecuteProcedureReturnData<List<LetterInboundDestinationUsersModel>>(connString, "Get_LetterInboundDestinationUsers", r => r.TranslateAsLetterInboundDestinationUserList(), getDestinationparam);

            foreach (var item in destinationUserId)
            {
                try
                {
                    if (oldDestinationuser.Count > 0)
                    {
                        var temp = false;
                        try
                        {
                            temp = Convert.ToBoolean(oldDestinationuser.Find(a => a.LetterDestinationUsersID.Equals(item.LetterDestinationUsersID)).LetterDestinationUsersID);
                            temp = false;
                        }
                        catch (NullReferenceException)
                        {
                            temp = true;
                        }

                        if (temp)
                        {
                            SqlParameter[] destinationparam = {
                            new SqlParameter("@P_LetterID", letterId),
                            new SqlParameter("@P_UserID", item.LetterDestinationUsersID),
                            new SqlParameter("@P_Type", 1) };
                            SqlHelper.ExecuteProcedureReturnString(connString, "Save_LetterInboundDestinationUser", destinationparam);
                        }
                    }
                    else
                    {
                        SqlParameter[] destinationparam = {
                            new SqlParameter("@P_LetterID", letterId),
                            new SqlParameter("@P_UserID", item.LetterDestinationUsersID),
                            new SqlParameter("@P_Type", 1) };
                        SqlHelper.ExecuteProcedureReturnString(connString, "Save_LetterInboundDestinationUser", destinationparam);
                    }
                }
                catch (Exception)
                {
                }
            }

            foreach (var item in oldDestinationuser)
            {
                if (destinationUserId.Find(a => a.LetterDestinationUsersID.Equals(item.LetterDestinationUsersID)) is null)
                {
                    SqlParameter[] destinationparam = {
                    new SqlParameter("@P_LetterID", letterId),
                    new SqlParameter("@P_UserID", item.LetterDestinationUsersID),
                    new SqlParameter("@P_Type", 2)
                    };
                    SqlHelper.ExecuteProcedureReturnString(connString, "Save_LetterInboundDestinationUser", destinationparam);
                }
            }
        }

        public void SaveDepartment(string connString, List<LetterInboundDestinationDepartmentModel> destinationDepartmentId, int? letterId)
        {
            SqlParameter[] getDestinationparam = {
                    new SqlParameter("@P_LetterID", letterId) };
            var oldDestinationDepartment = SqlHelper.ExecuteProcedureReturnData<List<LetterInboundDestinationDepartmentModel>>(connString, "Get_LetterInboundDestinationDepartment", r => r.TranslateAsLetterInboundDestinationDepartmentList(), getDestinationparam);

            foreach (var item in destinationDepartmentId)
            {
                try
                {
                    if (oldDestinationDepartment.Count > 0)
                    {
                        var temp = false;
                        try
                        {
                            temp = Convert.ToBoolean(oldDestinationDepartment.Find(a => a.LetterDestinationDepartmentID.Equals(item.LetterDestinationDepartmentID)).LetterDestinationDepartmentID);
                            temp = false;
                        }
                        catch (NullReferenceException)
                        {
                            temp = true;
                        }

                        if (temp)
                        {
                            SqlParameter[] destinationparam = {
                            new SqlParameter("@P_LetterID", letterId),
                            new SqlParameter("@P_DepartmentID", item.LetterDestinationDepartmentID),
                            new SqlParameter("@P_Type", 1) };
                            SqlHelper.ExecuteProcedureReturnString(connString, "Save_LetterInboundDestinationDepartment", destinationparam);
                        }
                    }
                    else
                    {
                        SqlParameter[] destinationparam = {
                            new SqlParameter("@P_LetterID", letterId),
                            new SqlParameter("@P_DepartmentID", item.LetterDestinationDepartmentID),
                            new SqlParameter("@P_Type", 1) };
                        SqlHelper.ExecuteProcedureReturnString(connString, "Save_LetterInboundDestinationDepartment", destinationparam);
                    }
                }
                catch (Exception)
                {
                }
            }

            foreach (var item in oldDestinationDepartment)
            {
                if (destinationDepartmentId.Find(a => a.LetterDestinationDepartmentID.Equals(item.LetterDestinationDepartmentID)) is null)
                {
                    SqlParameter[] destinationparam = {
                    new SqlParameter("@P_LetterID", letterId),
                    new SqlParameter("@P_DepartmentID", item.LetterDestinationDepartmentID),
                    new SqlParameter("@P_Type", 2) };
                    SqlHelper.ExecuteProcedureReturnString(connString, "Save_LetterInboundDestinationDepartment", destinationparam);
                }
            }
        }
    }
}
