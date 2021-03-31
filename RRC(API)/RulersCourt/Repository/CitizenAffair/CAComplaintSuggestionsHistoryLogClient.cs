using RulersCourt.Models.CitizenAffair;
using RulersCourt.Translators.CAComplaintSuggestions;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.CAComplaintSuggestions
{
    public class CAComplaintSuggestionsHistoryLogClient
    {
        public List<CAComplaintSuggestionsHistoryLogModel> CAComplaintSuggestionsHistoryLogByCAComplaintSuggestionsID(string connString, int caComplaintSuggestionsID, string lang)
        {
            SqlParameter[] contractIDParam = {
                    new SqlParameter("@P_CAComplaintSuggestionsID", caComplaintSuggestionsID),
                    new SqlParameter("@P_Language", lang)
            };

            List<CAComplaintSuggestionsHistoryLogModel> caComplaintSuggestionsDetails = new List<CAComplaintSuggestionsHistoryLogModel>();

            caComplaintSuggestionsDetails = SqlHelper.ExecuteProcedureReturnData<List<CAComplaintSuggestionsHistoryLogModel>>(connString, "Get_CAComplaintSuggestionsHistoryByID", r => r.TranslateAsCAComplaintSuggestionsHistoryLogList(), contractIDParam);
            return caComplaintSuggestionsDetails;
        }
    }
}
