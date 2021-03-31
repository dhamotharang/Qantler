using RulersCourt.Models;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class M_CityClient
    {
        public List<M_CityModel> GetCity(string connString, int userID, string countryID, string lang, string emiratesID)
        {
            SqlParameter[] parama = {
                new SqlParameter("@P_CountryID", countryID),
                new SqlParameter("@P_EmiratesID", emiratesID),
                new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_CityModel>>(connString, "Get_M_City", r => r.TranslateAsCity(), parama);
        }

        public string PostCity(string connString, int userID, M_MasterLookupsPostModel city, int countryID, string lang, int emiratesID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", city.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", city.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", city.DisplayOrder),
                                    new SqlParameter("@P_CountryID", countryID),
                                    new SqlParameter("@P_EmiratesID", emiratesID),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_City", parama);
        }

        public string PutCity(string connString, int userID, M_MasterLookupsPutModel city, int countryID, string lang, int emirates)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", city.LookupsID),
                                    new SqlParameter("@P_DisplayName", city.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", city.DisplayName),
                                    new SqlParameter("@P_DisplayOrder", city.DisplayOrder),
                                    new SqlParameter("@P_CountryID", countryID),
                                    new SqlParameter("@P_EmiratesID", emirates),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_City", parama);
        }

        public string DeleteCity(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_City", parama);
        }
    }
}