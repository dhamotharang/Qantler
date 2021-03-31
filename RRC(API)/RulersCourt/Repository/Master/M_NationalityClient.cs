using RulersCourt.Models;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class M_NationalityClient
    {
        public List<M_NationalityModel> GetNationality(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_NationalityModel>>(connString, "Get_M_Nationality", r => r.TranslateAsNationality(), parama);
        }

        public string PostNationality(string connString, int userID, M_MasterLookupsPostModel nationality, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", nationality.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", nationality.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", nationality.DisplayOrder),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Nationality", parama);
        }

        public string PutNationality(string connString, int userID, M_MasterLookupsPutModel nationality, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", nationality.LookupsID),
                                    new SqlParameter("@P_DisplayName", nationality.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", nationality.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", nationality.DisplayOrder),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Nationality", parama);
        }

        public string DeleteNationality(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Nationality", parama);
        }
    }
}
