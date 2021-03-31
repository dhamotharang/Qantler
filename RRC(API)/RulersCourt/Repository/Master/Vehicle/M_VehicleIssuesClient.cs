using RulersCourt.Models;
using RulersCourt.Models.Master.M_Vehicle;
using RulersCourt.Translators.Master.Vehicle;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Master.Vehicle
{
    public class M_VehicleIssuesClient
    {
        public List<M_VehicleIssuesModel> GetVehicleIssues(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_VehicleIssuesModel>>(connString, "Get_VehicleIssues", r => r.TranslateAsVehicleIssues(), parama);
        }

        public string PostVehicleIssues(string connString, int userID, M_MasterLookupsPostModel vehicleIssues, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", vehicleIssues.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", vehicleIssues.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", vehicleIssues.DisplayOrder),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_VehicleIssues", parama);
        }

        public string PutVehicleIssues(string connString, int userID, M_MasterLookupsPutModel vehicleIssues, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", vehicleIssues.LookupsID),
                                    new SqlParameter("@P_DisplayName", vehicleIssues.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", vehicleIssues.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", vehicleIssues.DisplayOrder),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_VehicleIssues", parama);
        }

        public string DeleteVehicleIssues(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_VehicleIssues", parama);
        }
    }
}
