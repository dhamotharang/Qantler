using RulersCourt.Models;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class M_LocationClient
    {
        public List<M_LocationModel> GetLocation(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LocationModel>>(connString, "Get_M_Location", r => r.TranslateAsLocation(), parama);
        }

        public string PostLocation(string connString, int userID, M_MasterLookupsPostModel location, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", location.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", location.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", location.DisplayOrder),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Location", parama);
        }

        public string PutLocation(string connString, int userID, M_MasterLookupsPutModel location, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", location.LookupsID),
                                    new SqlParameter("@P_DisplayName", location.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", location.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", location.DisplayOrder),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Location", parama);
        }

        public string DeleteLocation(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Location", parama);
        }
    }
}
