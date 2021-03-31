using RulersCourt.Models.BabyAddition;
using RulersCourt.Translators.BabyAddition;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.BabyAddition
{
    public class BabyAdditionHistoryLogClient
    {
        public List<BabyAdditionHistoryLogModel> BabyAdditionHistoryLogByBabyAdditionID(string connString, int babyAdditionID, string lang)
        {
            SqlParameter[] contractIDParam = {
                new SqlParameter("@P_BabyAdditionID", babyAdditionID),
                new SqlParameter("@P_Language", lang)
            };

            List<BabyAdditionHistoryLogModel> babyAdditionDetails = new List<BabyAdditionHistoryLogModel>();

            babyAdditionDetails = SqlHelper.ExecuteProcedureReturnData<List<BabyAdditionHistoryLogModel>>(connString, "Get_BabyAdditionHistoryByID", r => r.TranslateAsBabyAdditionHistoryLogList(), contractIDParam);

            return babyAdditionDetails;
        }
    }
}
