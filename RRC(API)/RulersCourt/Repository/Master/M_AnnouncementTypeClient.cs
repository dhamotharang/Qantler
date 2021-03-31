using RulersCourt.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Master
{
    public class M_AnnouncementTypeClient
    {
        public string PostAnnouncementTypeModel(string connString, int userID, M_MasterLookupsPostModel vehicleModel, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", vehicleModel.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", vehicleModel.ArDisplayName),
                                    new SqlParameter("@P_CreatedBy", userID) };

            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_AnnouncementType", parama);
        }

        public string PutAnnouncementTypeModel(string connString, int userID, M_MasterLookupsPutModel tripDestination, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", tripDestination.LookupsID),
                                    new SqlParameter("@P_DisplayName", tripDestination.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", tripDestination.ArDisplayName),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_AnnouncementType", parama);
        }

        public string DeleteAnnouncementTypeModel(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_AnnouncementType", parama);
        }

        public string PutAnnouncementTypeDescModel(string connString, int userID, M_MasterLookupsPutModel tripDestination, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", tripDestination.LookupsID),
                                    new SqlParameter("@P_DescFlag", 1),
                                    new SqlParameter("@P_DisplayDesc", tripDestination.DisplayName),
                                    new SqlParameter("@P_ARDisplayDesc", tripDestination.ArDisplayName),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_AnnouncementType", parama);
        }

        public string DeleteAnnouncementTypeDescModel(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DescFlag", 1),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_DisplayDesc", null),
                                    new SqlParameter("@P_ARDisplayDesc", null)
            };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_AnnouncementType", parama);
        }
    }
}
