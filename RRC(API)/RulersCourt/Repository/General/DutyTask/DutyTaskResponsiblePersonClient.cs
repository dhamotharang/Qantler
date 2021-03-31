using RulersCourt.Models.DutyTask;
using RulersCourt.Translators.DutyTask;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.DutyTask
{
    public class DutyTaskResponsiblePersonClient
    {
        public void SaveResponsiblePersonUserID(string connString, List<DutyTaskResponsibleUsersModel> responsibleUsers, int? taskID, int? user, DateTime? actionDateTime, string lang)
        {
            SqlParameter[] getDestinationparam = {
                    new SqlParameter("@P_DutyTaskID", taskID),
                    new SqlParameter("@P_Language", lang)
            };
            var oldResponsibleUsers = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskResponsibleUsersModel>>(connString, "Get_DutyResponsibleUsersID", r => r.TranslateAsResponsibleUserList(), getDestinationparam);

            foreach (var item in responsibleUsers)
            {
                if (oldResponsibleUsers.Count > 0)
                {
                    var temp = false;
                    try
                    {
                        temp = Convert.ToBoolean(oldResponsibleUsers.Find(a => a.TaskResponsibleUsersID.Equals(item.TaskResponsibleUsersID)).TaskResponsibleUsersID);
                        temp = false;
                    }
                    catch (NullReferenceException)
                    {
                        temp = true;
                    }

                    if (temp)
                    {
                        SqlParameter[] destinationparam = {
                            new SqlParameter("@P_DutyTaskID", taskID),
                            new SqlParameter("@P_ResponsibleUsersID", item.TaskResponsibleUsersID),
                            new SqlParameter("@P_Type", 1),
                            new SqlParameter("@P_UserID", user),
                            new SqlParameter("@P_ActionDateTime", actionDateTime)
                        };
                        SqlHelper.ExecuteProcedureReturnString(connString, "Save_DutyTaskResponsibleUsers", destinationparam);
                    }
                }
                else
                {
                    SqlParameter[] destinationparam = {
                            new SqlParameter("@P_DutyTaskID", taskID),
                            new SqlParameter("@P_ResponsibleUsersID", item.TaskResponsibleUsersID),
                            new SqlParameter("@P_Type", 1),
                            new SqlParameter("@P_UserID", user),
                            new SqlParameter("@P_ActionDateTime", actionDateTime)
                    };
                    SqlHelper.ExecuteProcedureReturnString(connString, "Save_DutyTaskResponsibleUsers", destinationparam);
                }
            }

            foreach (var item in oldResponsibleUsers)
            {
                if (responsibleUsers.Find(a => a.TaskResponsibleUsersID.Equals(item.TaskResponsibleUsersID)) is null)
                {
                    SqlParameter[] destinationparam = {
                    new SqlParameter("@P_DutyTaskID", taskID),
                    new SqlParameter("@P_ResponsibleUsersID", item.TaskResponsibleUsersID),
                    new SqlParameter("@P_Type", 2),
                    new SqlParameter("@P_UserID", user),
                    new SqlParameter("@P_ActionDateTime", actionDateTime)
                    };

                    SqlHelper.ExecuteProcedureReturnString(connString, "Save_DutyTaskResponsibleUsers", destinationparam);
                }
            }
        }

        public void SaveResponsibleDepartment(string connString, List<DutyTaskResponsibleDepartmentModel> responsibleDepartment, int? taskID, int? user, DateTime? actionDateTime)
        {
            SqlParameter[] getDestinationparam = {
                    new SqlParameter("@P_DutyTaskID", taskID)
            };
            var oldResponsibleDepartment = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskResponsibleDepartmentModel>>(connString, "Get_DutyResponsibleDepartmentID", r => r.TranslateAsResponsibleUserDepartmentList(), getDestinationparam);

            foreach (var item in responsibleDepartment)
            {
                if (oldResponsibleDepartment.Count > 0)
                {
                    var temp = false;
                    try
                    {
                        temp = Convert.ToBoolean(oldResponsibleDepartment.Find(a => a.TaskResponsibleDepartmentID.Equals(item.TaskResponsibleDepartmentID)).TaskResponsibleDepartmentID);
                        temp = false;
                    }
                    catch (NullReferenceException)
                    {
                        temp = true;
                    }

                    if (temp)
                    {
                        SqlParameter[] destinationparam = {
                            new SqlParameter("@P_DutyTaskID", taskID),
                            new SqlParameter("@P_ResponsibleDepartmentID", item.TaskResponsibleDepartmentID),
                            new SqlParameter("@P_Type", 1),
                            new SqlParameter("@P_UserID", user),
                            new SqlParameter("@P_ActionDateTime", actionDateTime)
                        };
                        SqlHelper.ExecuteProcedureReturnString(connString, "Save_DutyTaskResponsibleDepartment", destinationparam);
                    }
                }
                else
                {
                    SqlParameter[] destinationparam = {
                            new SqlParameter("@P_DutyTaskID", taskID),
                            new SqlParameter("@P_ResponsibleDepartmentID", item.TaskResponsibleDepartmentID),
                            new SqlParameter("@P_Type", 1),
                            new SqlParameter("@P_UserID", user),
                            new SqlParameter("@P_ActionDateTime", actionDateTime)
                    };
                    SqlHelper.ExecuteProcedureReturnString(connString, "Save_DutyTaskResponsibleDepartment", destinationparam);
                }
            }

            foreach (var item in oldResponsibleDepartment)
            {
                if (responsibleDepartment.Find(a => a.TaskResponsibleDepartmentID.Equals(item.TaskResponsibleDepartmentID)) is null)
                {
                    SqlParameter[] destinationparam = {
                    new SqlParameter("@P_DutyTaskID", taskID),
                    new SqlParameter("@P_ResponsibleDepartmentID", item.TaskResponsibleDepartmentID),
                    new SqlParameter("@P_Type", 2),
                    new SqlParameter("@P_UserID", user),
                    new SqlParameter("@P_ActionDateTime", actionDateTime)
                    };

                    SqlHelper.ExecuteProcedureReturnString(connString, "Save_DutyTaskResponsibleDepartment", destinationparam);
                }
            }
        }
    }
}