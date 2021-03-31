using RulersCourt.Models.CitizenAffair;
using RulersCourt.Translators.CitizenAffair;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.CitizenAffair
{
    public class CitizenAffairHistoryLogClient
    {
        public List<CitizenAffairHistoryLogModel> CAHistoryLogByMemoID(string connString, int citizenAffairID, string lang)
        {
            SqlParameter[] contractIDParam = {
                new SqlParameter("@P_CitizenAffairID", citizenAffairID),
                new SqlParameter("@P_Language", lang),
            };

            List<CitizenAffairHistoryLogModel> citizenAffairDetails = new List<CitizenAffairHistoryLogModel>();

            citizenAffairDetails = SqlHelper.ExecuteProcedureReturnData<List<CitizenAffairHistoryLogModel>>(connString, "Get_CitizenAffarHistoryByID", r => r.TranslateAsCAHistoryLogList(), contractIDParam);

            return citizenAffairDetails;
        }
    }
}
