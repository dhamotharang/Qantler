using RulersCourt.Models;
using RulersCourt.Models.Master.M_Vehicle;
using RulersCourt.Translators.Master.Vehicle;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Master.Vehicle
{
    public class M_VehicleRequestStatusClient
    {
        public List<M_VehicleRequestStatusModel> GetVehicleRequestStatus(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_VehicleRequestStatusModel>>(connString, "Get_VehicleRequestStatus", r => r.TranslateAsVehicleRequestStatusModel(), parama);
        }

        public string PostVehicleRequestStatus(string connString, int userID, M_MasterLookupsPostModel vehicleRequestStatus, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", vehicleRequestStatus.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", vehicleRequestStatus.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", vehicleRequestStatus.DisplayOrder),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_VehicleRequestStatus", parama);
        }

        public string PutVehicleRequestStatus(string connString, int userID, M_MasterLookupsPutModel vehicleRequestStatus, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", vehicleRequestStatus.LookupsID),
                                    new SqlParameter("@P_DisplayName", vehicleRequestStatus.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", vehicleRequestStatus.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", vehicleRequestStatus.DisplayOrder),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_VehicleRequestStatus", parama);
        }

        public string DeleteVehicleRequestStatus(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_VehicleRequestStatus", parama);
        }
    }
}
