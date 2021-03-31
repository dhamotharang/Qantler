using RulersCourt.Models.Vehicle.TripCoPassengers;
using RulersCourt.Translators.Vehicle;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Vehicle
{
    public class TripCoPassengersClient
    {
        public void SaveCoPassengers(string connString, List<TripCoPassengersModel> coPassengersId, int? vehicleReqID)
        {
            SqlParameter[] getPassengersparam = {
                    new SqlParameter("@P_VehicleReqID", vehicleReqID),
                    new SqlParameter("@P_Type", 1) };
            var oldCoPassengers = SqlHelper.ExecuteProcedureReturnData<List<TripCoPassengersModel>>(connString, "Get_TripCoPassengers", r => r.TranslateAsCoPassengersUserList(), getPassengersparam);

            foreach (var item in coPassengersId)
            {
                try
                {
                    if (oldCoPassengers.Count > 0)
                    {
                        var temp = false;
                        try
                        {
                            temp = Convert.ToBoolean(oldCoPassengers.Find(a => a.CoPassengerID.Equals(item.CoPassengerID)).CoPassengerID);
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
                            new SqlParameter("@P_CoPassengerID", item.CoPassengerID),
                            new SqlParameter("@P_Type", 1),
                            new SqlParameter("@P_OthersCoPassengerName", item.OtherCoPassengerName)
                            };
                            SqlHelper.ExecuteProcedureReturnString(connString, "Save_TripCoPassengers", coPassengersparam);
                        }
                    }
                    else
                    {
                        SqlParameter[] coPassengersparam = {
                            new SqlParameter("@P_VehicleReqID", vehicleReqID),
                            new SqlParameter("@P_CoPassengerID", item.CoPassengerID),
                            new SqlParameter("@P_Type", 1),
                            new SqlParameter("@P_OthersCoPassengerName", item.OtherCoPassengerName)
                        };

                        SqlHelper.ExecuteProcedureReturnString(connString, "Save_TripCoPassengers", coPassengersparam);
                    }
                }
                catch (Exception)
                {
                }
            }

            foreach (var item in oldCoPassengers)
            {
                if (coPassengersId.Find(a => a.CoPassengerID.Equals(item.CoPassengerID)) is null)
                {
                    SqlParameter[] coPassengersparam = {
                    new SqlParameter("@P_VehicleReqID", vehicleReqID),
                    new SqlParameter("@P_CoPassengerID", item.CoPassengerID),
                    new SqlParameter("@P_Type", 2),
                    new SqlParameter("@P_OthersCoPassengerName", item.OtherCoPassengerName)
                    };
                    SqlHelper.ExecuteProcedureReturnString(connString, "Save_CoPassengers", coPassengersparam);
                }
            }
        }
    }
}
