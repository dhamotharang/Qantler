using RulersCourt.Models.CitizenAffair;
using RulersCourt.Translators.CitizenAffair;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.CitizenAffair
{
    public class GiftHistoryLogClient
    {
        public List<GiftHistoryLogModel> GiftHistory(string connString, int? giftID, string lang)
        {
            SqlParameter[] giftParam = {
                new SqlParameter("@P_GiftID", giftID),
                new SqlParameter("@P_Language", lang)
                };

            List<GiftHistoryLogModel> citizenAffairDetails = new List<GiftHistoryLogModel>();

            citizenAffairDetails = SqlHelper.ExecuteProcedureReturnData<List<GiftHistoryLogModel>>(connString, "Get_GiftHistoryByID", r => r.TranslateAsGiftHistoryLogList(), giftParam);

            return citizenAffairDetails;
        }
    }
}
