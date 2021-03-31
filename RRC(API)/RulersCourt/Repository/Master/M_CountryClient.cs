using RulersCourt.Models;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class M_CountryClient
    {
        public List<M_CountryModel> GetCountry(string connString, int userID, string lang, string module)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang),
             new SqlParameter("@P_Module", module) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_CountryModel>>(connString, "Get_M_Country", r => r.TranslateAsCountry(), parama);
        }

        public string PostCountry(string connString, int userID, M_MasterLookupsPostModel country, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", country.DisplayName),
                                    new SqlParameter("@P_DisplayOrder", country.DisplayOrder),
                                    new SqlParameter("@P_ARDisplayName", country.ArDisplayName),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Country", parama);
        }

        public string PutCountry(string connString, int userID, M_MasterLookupsPutModel country, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", country.LookupsID),
                                    new SqlParameter("@P_DisplayName", country.DisplayName),
                                    new SqlParameter("@P_DisplayOrder", country.DisplayOrder),
                                    new SqlParameter("@P_ARDisplayName", country.ArDisplayName),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Country", parama);
        }

        public string DeleteCountry(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Country", parama);
        }
    }
}