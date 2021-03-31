using RulersCourt.Models;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class M_MediaChannelClient
    {
        public List<M_MediaChannelModel> GetMediaChannel(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_MediaChannelModel>>(connString, "Get_M_MediaChannel", r => r.TranslateAsMediaChannel(), parama);
        }

        public string PostMediaChannel(string connString, int userID, M_MasterLookupsPostModel mediaChannel, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", mediaChannel.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", mediaChannel.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", mediaChannel.DisplayOrder),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_MediaChannel", parama);
        }

        public string PutMediaChannel(string connString, int userID, M_MasterLookupsPutModel mediaChannel, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", mediaChannel.LookupsID),
                                    new SqlParameter("@P_DisplayName", mediaChannel.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", mediaChannel.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", mediaChannel.DisplayOrder),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_MediaChannel", parama);
        }

        public string DeleteMediaChannel(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_MediaChannel", parama);
        }
    }
}
