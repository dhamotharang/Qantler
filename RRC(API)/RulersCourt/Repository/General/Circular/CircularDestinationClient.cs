using RulersCourt.Models.Circular;
using RulersCourt.Translators.Circular;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Circular
{
    public class CircularDestinationClient
    {
        public void SaveDepartment(string connString, List<CircularDestinationDepartmentGetModel> destinationDepartmentId, int? circularId)
        {
            SqlParameter[] getDestinationparam = {
                    new SqlParameter("@P_CircularID", circularId)
            };
            var oldDestinationDepartment = SqlHelper.ExecuteProcedureReturnData<List<CircularDestinationDepartmentGetModel>>(connString, "Get_CircularDestinationDepartment", r => r.CircularTranslateAsDestinationDepartmentList(), getDestinationparam);

            foreach (var item in destinationDepartmentId)
            {
                try
                {
                    if (oldDestinationDepartment.Count > 0)
                    {
                        var temp = false;
                        try
                        {
                            temp = Convert.ToBoolean(oldDestinationDepartment.Find(a => a.CircularDestinationDepartmentID.Equals(item.CircularDestinationDepartmentID)).CircularDestinationDepartmentID);
                            temp = false;
                        }
                        catch (NullReferenceException)
                        {
                            temp = true;
                        }

                        if (temp)
                        {
                            SqlParameter[] destinationparam = {
                            new SqlParameter("@P_CircularID", circularId),
                            new SqlParameter("@P_DepartmentID", item.CircularDestinationDepartmentID),
                            new SqlParameter("@P_Type", 1)
                            };
                            SqlHelper.ExecuteProcedureReturnString(connString, "Save_CircularDestinationDepartment", destinationparam);
                        }
                    }
                    else
                    {
                        SqlParameter[] destinationparam = {
                            new SqlParameter("@P_CircularID", circularId),
                            new SqlParameter("@P_DepartmentID", item.CircularDestinationDepartmentID),
                            new SqlParameter("@P_Type", 1)
                        };
                        SqlHelper.ExecuteProcedureReturnString(connString, "Save_CircularDestinationDepartment", destinationparam);
                    }
                }
                catch (Exception)
                {
                }
            }

            foreach (var item in oldDestinationDepartment)
            {
                if (destinationDepartmentId.Find(a => a.CircularDestinationDepartmentID.Equals(item.CircularDestinationDepartmentID)) is null)
                {
                    SqlParameter[] destinationparam = {
                    new SqlParameter("@P_CircularID", circularId),
                    new SqlParameter("@P_DepartmentID", item.CircularDestinationDepartmentID),
                    new SqlParameter("@P_Type", 2)
                    };
                    SqlHelper.ExecuteProcedureReturnString(connString, "Save_CircularDestinationDepartment", destinationparam);
                }
            }
        }
    }
}
