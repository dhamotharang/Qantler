using RulersCourt.Models.AdminManagement;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Gift
{
    public class M_AdminManagementClient
    {
        public string AdminManagementPost(string connString, M_AdminManagementPostModel data)
        {
            SqlParameter[] paramPost = { new SqlParameter("@P_Category", data.Category),
                                    new SqlParameter("@P_DisplayName", data.DisplayName),
                                    new SqlParameter("@P_DisplayOrder", data.DisplayOrder),
                                    new SqlParameter("@P_CreatedBy", data.CreatedBy),
                                    new SqlParameter("@P_CreatedDateTime", data.CreatedDateTime)
            };
            var result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_MasterAdminManagement", paramPost);
            return result;
        }

        public string AdminManagementPut(string connString, M_AdminManagementPutModel data)
        {
            SqlParameter[] paramPut = { new SqlParameter("@P_LookupsID", data.LookupsID),
                                    new SqlParameter("@P_Category", data.Category),
                                    new SqlParameter("@P_DisplayName", data.DisplayName),
                                    new SqlParameter("@P_DisplayOrder", data.DisplayOrder),
                                    new SqlParameter("@P_UpdatedBy", data.UpdatedBy),
                                    new SqlParameter("@P_UpdatedDateTime", data.UpdatedDateTime),
                                    new SqlParameter("@P_DeleteFlag", data.DeleteFlag)
            };
            var result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_MasterAdminManagement", paramPut);
            return result;
        }
    }
}