using RulersCourt.Models.Protocol;
using RulersCourt.Translators.Protocol;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Protocol
{
    public class ProtocolHomeClient
    {
        public ProtocolHomeModel GetAllModulesPendingTasksCount(string connString, int userID)
        {
            ProtocolHomeModel list = new ProtocolHomeModel();
            SqlParameter[] myRequestparam = {
                       new SqlParameter("@P_UserID", userID) };
            list = SqlHelper.ExecuteProcedureReturnData<ProtocolHomeModel>(connString, "Get_ProtocolDashboardCount", r => r.TranslateasProtocolDashboardcount(), myRequestparam);
            return list;
        }
    }
}
