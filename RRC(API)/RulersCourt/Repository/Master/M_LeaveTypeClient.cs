using RulersCourt.Models;
using RulersCourt.Models.Master;
using RulersCourt.Translators.Master;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Master
{
    public class M_LeaveTypeClient
    {
        public List<M_LeaveTypeModel> GetLeaveType(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LeaveTypeModel>>(connString, "Get_M_LeaveType", r => r.TranslateAsLeaveType(), parama);
        }

        public string PostLeaveType(string connString, int userID, M_MasterLookupsPostModel leaveType, string language)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", leaveType.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", leaveType.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", leaveType.DisplayOrder),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_Language", language) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_LeaveType", parama);
        }

        public string PutLeaveType(string connString, int userID, M_MasterLookupsPutModel leaveType, string language)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", leaveType.LookupsID),
                                    new SqlParameter("@P_ARDisplayName", leaveType.ArDisplayName),
                                    new SqlParameter("@P_DisplayName", leaveType.DisplayName),
                                    new SqlParameter("@P_DisplayOrder", leaveType.DisplayOrder),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_Language", language) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_LeaveType", parama);
        }

        public string DeleteLeaveType(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_LeaveType", parama);
        }
    }
}
