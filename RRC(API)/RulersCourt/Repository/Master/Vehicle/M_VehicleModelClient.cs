using RulersCourt.Models;
using RulersCourt.Models.Master.M_Vehicle;
using RulersCourt.Translators.Master.Vehicle;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Master.Vehicle
{
    public class M_VehicleModelClient
    {
        public List<M_VehicleModel> GetVehicleModel(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_VehicleModel>>(connString, "Get_M_VehicleModel", r => r.TranslateAsVehicleModel(), parama);
        }

        public string PostVehicleModel(string connString, int userID, M_MasterLookupsPostModel vehicleModel, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", vehicleModel.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", vehicleModel.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", vehicleModel.DisplayOrder),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_VehicleModel", parama);
        }

        public string PutVehicleModel(string connString, int userID, M_MasterLookupsPutModel tripDestination, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", tripDestination.LookupsID),
                                    new SqlParameter("@P_DisplayName", tripDestination.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", tripDestination.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", tripDestination.DisplayOrder),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_VehicleModel", parama);
        }

        public string DeleteVehicleModel(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_VehicleModel", parama);
        }
    }
}
