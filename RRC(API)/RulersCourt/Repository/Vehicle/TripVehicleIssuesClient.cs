using RulersCourt.Models.Vehicle.TripVehicleIssues;
using RulersCourt.Translators.Vehicle;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Vehicle
{
    public class TripVehicleIssuesClient
    {
        public void SaveVehicleIssues(string connString, List<TripVehicleIssuesPostModel> issues, int? vehicleReqID)
        {
            SqlParameter[] getPassengersparam = {
                    new SqlParameter("@P_VehicleReqID", vehicleReqID),
                    new SqlParameter("@P_Type", 1) };
            var oldCoPassengers = SqlHelper.ExecuteProcedureReturnData<List<TripVehicleIssuesPostModel>>(connString, "Get_TripVehicleIssues", r => r.TranslateAsVehicleIssuesList(), getPassengersparam);

            foreach (var item in issues)
            {
                try
                {
                    if (oldCoPassengers.Count > 0)
                    {
                        var temp = false;
                        try
                        {
                            temp = Convert.ToBoolean(oldCoPassengers.Find(a => a.IssueID.Equals(item.IssueID)).IssueID);
                            temp = false;
                        }
                        catch (NullReferenceException)
                        {
                            temp = true;
                        }

                        if (temp)
                        {
                            SqlParameter[] coPassengersparam = {
                            new SqlParameter("@P_VehicleReqID", vehicleReqID),
                            new SqlParameter("@P_IssueID", item.IssueID),
                            new SqlParameter("@P_Type", 1)
                            };
                            SqlHelper.ExecuteProcedureReturnString(connString, "Save_TripVehicleIssues", coPassengersparam);
                        }
                    }
                    else
                    {
                        SqlParameter[] coPassengersparam = {
                            new SqlParameter("@P_VehicleReqID", vehicleReqID),
                            new SqlParameter("@P_IssueID", item.IssueID),
                            new SqlParameter("@P_Type", 1)
                        };
                        SqlHelper.ExecuteProcedureReturnString(connString, "Save_TripVehicleIssues", coPassengersparam);
                    }
                }
                catch (Exception)
                {
                }
            }

            foreach (var item in oldCoPassengers)
            {
                if (issues.Find(a => a.IssueID.Equals(item.IssueID)) is null)
                {
                    SqlParameter[] coPassengersparam = {
                    new SqlParameter("@P_VehicleReqID", vehicleReqID),
                    new SqlParameter("@P_IssueID", item.IssueID),
                    new SqlParameter("@P_Type", 2)
                    };
                    SqlHelper.ExecuteProcedureReturnString(connString, "Save_TripVehicleIssues", coPassengersparam);
                }
            }
        }
    }
}
