using RulersCourt.Models.Circular;
using RulersCourt.Translators.Circular;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Circular
{
    public class CircularHistoryLogClient
    {
        public List<CircularHistoryLogModel> CircularHistoryLogByCircularID(string connString, int circularID, string lang)
        {
            SqlParameter[] contractIDParam = {
                new SqlParameter("@P_CircularID", circularID),
                new SqlParameter("@P_Language", lang),
            };

            List<CircularHistoryLogModel> circularDetails = new List<CircularHistoryLogModel>();

            circularDetails = SqlHelper.ExecuteProcedureReturnData<List<CircularHistoryLogModel>>(connString, "Get_CircularHistoryByID", r => r.TranslateAsCircularHistoryLogList(), contractIDParam);

            return circularDetails;
        }
    }
}
