using RulersCourt.Models;
using RulersCourt.Models.Master.M_Vehicle;
using RulersCourt.Translators.Master.Vehicle;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Master.Vehicle
{
    public class M_TripDestinationClient
    {
        public List<M_TripDestinationModel> GetTripDestination(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_TripDestinationModel>>(connString, "Get_M_TripDestination", r => r.TranslateAsTripDestination(), parama);
        }

        public string PostLogType(string connString, int userID, M_MasterLookupsPostModel tripDestination, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", tripDestination.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", tripDestination.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", tripDestination.DisplayOrder),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_TripDestination", parama);
        }

        public string PutTripDestination(string connString, int userID, M_MasterLookupsPutModel tripDestination, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", tripDestination.LookupsID),
                                    new SqlParameter("@P_DisplayName", tripDestination.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", tripDestination.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", tripDestination.DisplayOrder),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_TripDestination", parama);
        }

        public string DeleteTripDestination(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_TripDestination", parama);
        }
    }
}
