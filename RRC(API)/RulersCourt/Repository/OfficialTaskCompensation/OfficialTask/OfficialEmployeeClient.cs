using RulersCourt.Models.OfficialTaskCompensation.OfficialTask;
using RulersCourt.Translators.OfficialTaskCompensation.OfficialTask;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.OfficialTaskCompensation.OfficialTask
{
    public class OfficialEmployeeClient
    {
        public void SaveUser(string connString, List<OfficialTaskEmployeeNameModel> employeeUserId, int? officialTaskID)
        {
            SqlParameter[] getEmployeeparam = {
                    new SqlParameter("@P_OfficialTaskID", officialTaskID)
            };
            var oldEmployeeuser = SqlHelper.ExecuteProcedureReturnData<List<OfficialTaskEmployeeNameModel>>(connString, "Get_OfficialTaskEmployeeName", r => r.TranslateAsEmployeeList(), getEmployeeparam);

            foreach (var item in employeeUserId)
            {
                try
                {
                    if (oldEmployeeuser.Count > 0)
                    {
                        var temp = false;
                        try
                        {
                            temp = Convert.ToBoolean(oldEmployeeuser.Find(a => a.OfficialTaskEmployeeID.Equals(item.OfficialTaskEmployeeID)).OfficialTaskEmployeeID);
                            temp = false;
                        }
                        catch (NullReferenceException)
                        {
                            temp = true;
                        }

                        if (temp)
                        {
                            SqlParameter[] employeeparam = {
                            new SqlParameter("@P_OfficialTaskID", officialTaskID),
                            new SqlParameter("@P_UserID", item.OfficialTaskEmployeeID),
                            new SqlParameter("@P_Type", 1) };
                            SqlHelper.ExecuteProcedureReturnString(connString, "Save_OfficialTaskEmployeeName", employeeparam);
                        }
                    }
                    else
                    {
                        SqlParameter[] employeeparam = {
                            new SqlParameter("@P_OfficialTaskID", officialTaskID),
                            new SqlParameter("@P_UserID", item.OfficialTaskEmployeeID),
                            new SqlParameter("@P_Type", 1) };
                        SqlHelper.ExecuteProcedureReturnString(connString, "Save_OfficialTaskEmployeeName", employeeparam);
                    }
                }
                catch (Exception)
                {
                }
            }

            foreach (var item in oldEmployeeuser)
            {
                if (employeeUserId.Find(a => a.OfficialTaskEmployeeID.Equals(item.OfficialTaskEmployeeID)) is null)
                {
                    SqlParameter[] employeeparam = {
                    new SqlParameter("@P_OfficialTaskID", officialTaskID),
                    new SqlParameter("@P_UserID", item.OfficialTaskEmployeeID),
                    new SqlParameter("@P_Type", 2) };
                    SqlHelper.ExecuteProcedureReturnString(connString, "Save_OfficialTaskEmployeeName", employeeparam);
                }
            }
        }
    }
}