using RulersCourt.Models;
using RulersCourt.Models.Master.M_Vehicle;
using RulersCourt.Translators.Master.Vehicle;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Master.Vehicle
{
    public class M_TripReleaseLocationClient
    {
        public List<M_TripReleaseLocationModel> GetTripReleaseLocation(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_TripReleaseLocationModel>>(connString, "Get_M_TripReleaseLocation", r => r.TranslateAsTripReleaseLocation(), parama);
        }

        public string PostTripReleaseLocation(string connString, int userID, M_MasterLookupsPostModel tripReleaseLocation, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", tripReleaseLocation.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", tripReleaseLocation.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", tripReleaseLocation.DisplayOrder),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_TripReleaseLocation", parama);
        }

        public string PutTripDestination(string connString, int userID, M_MasterLookupsPutModel tripReleaseLocation, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", tripReleaseLocation.LookupsID),
                                    new SqlParameter("@P_DisplayName", tripReleaseLocation.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", tripReleaseLocation.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", tripReleaseLocation.DisplayOrder),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_TripReleaseLocation", parama);
        }

        public string DeleteTripDestination(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_TripReleaseLocation", parama);
        }
    }
}
