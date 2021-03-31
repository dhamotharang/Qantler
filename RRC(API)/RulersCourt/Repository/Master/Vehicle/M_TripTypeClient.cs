using RulersCourt.Models;
using RulersCourt.Models.Master.M_Vehicle;
using RulersCourt.Translators.Master.Vehicle;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Master.Vehicle
{
    public class M_TripTypeClient
    {
        public List<M_TripTypeModel> GetTripType(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_TripTypeModel>>(connString, "Get_TripType", r => r.TranslateAsTripType(), parama);
        }

        public string PostTripType(string connString, int userID, M_MasterLookupsPostModel tripType, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", tripType.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", tripType.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", tripType.DisplayOrder),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_TripType", parama);
        }

        public string PutTripType(string connString, int userID, M_MasterLookupsPutModel tripType, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", tripType.LookupsID),
                                    new SqlParameter("@P_DisplayName", tripType.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", tripType.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", tripType.DisplayOrder),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_TripType", parama);
        }

        public string DeleteTripType(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_TripType", parama);
        }
    }
}
