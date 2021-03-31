using RulersCourt.Models.Master.M_Vehicle;
using RulersCourt.Translators.Master.Vehicle;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Master.Vehicle
{
    public class M_VehicleissueClient
    {
        public List<M_VehicleIssueModel> GetVehicleIssues(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_VehicleIssueModel>>(connString, "Get_M_VehicleIssues", r => r.TranslateAsGetVehicleIssueLIst(), parama);
        }
    }
}
