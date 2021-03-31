using RulersCourt.Models;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class M_DesignTypeClient
    {
        public List<M_DesignTypeModel> GetDesignType(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_DesignTypeModel>>(connString, "Get_M_DesignType", r => r.TranslateAsDesignType(), parama);
        }

        public string PostDesignType(string connString, int userID, M_MasterLookupsPostModel designType, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", designType.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", designType.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", designType.DisplayOrder),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_DesignType", parama);
        }

        public string PutDesignType(string connString, int userID, M_MasterLookupsPutModel designType, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", designType.LookupsID),
                                    new SqlParameter("@P_DisplayName", designType.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", designType.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", designType.DisplayOrder),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_DesignType", parama);
        }

        public string DeleteDesignType(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_DesignType", parama);
        }
    }
}
