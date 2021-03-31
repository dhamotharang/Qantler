using RulersCourt.Models;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class M_EmployeeStatusClient
    {
        public List<M_EmployeeStatusModel> GetEmployeeStatus(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_EmployeeStatusModel>>(connString, "Get_M_EmployeeStatus", r => r.TranslateAsEmployeeStatus(), parama);
        }

        public string PostEmployeeStatus(string connString, int userID, M_MasterLookupsPostModel employeeStatus, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", employeeStatus.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", employeeStatus.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", employeeStatus.DisplayOrder),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_EmployeeStatus", parama);
        }

        public string PutEmployeeStatus(string connString, int userID, M_MasterLookupsPutModel employeeStatus, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", employeeStatus.LookupsID),
                                    new SqlParameter("@P_DisplayName", employeeStatus.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", employeeStatus.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", employeeStatus.DisplayOrder),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_EmployeeStatus", parama);
        }

        public string DeleteEmployeeStatus(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_EmployeeStatus", parama);
        }
    }
}