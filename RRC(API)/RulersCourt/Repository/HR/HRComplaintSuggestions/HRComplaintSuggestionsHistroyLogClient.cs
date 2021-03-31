using RulersCourt.Models.HRComplaintSuggestions;
using RulersCourt.Translators.HRComplaintSuggestions;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.HRComplaintSuggestions
{
    public class HRComplaintSuggestionsHistroyLogClient
    {
        public List<HRComplaintSuggestionsHistoryLogModel> HRComplaintSuggestionsHistoryLogByHRComplaintSuggestionsID(string connString, int hRComplaintSuggestionsID, string lang)
        {
            SqlParameter[] contractIDParam = {
                new SqlParameter("@P_HRComplaintSuggestionsID", hRComplaintSuggestionsID),
                new SqlParameter("@P_Language", lang),
            };

            List<HRComplaintSuggestionsHistoryLogModel> hRComplaintSuggestionsDetails = new List<HRComplaintSuggestionsHistoryLogModel>();

            hRComplaintSuggestionsDetails = SqlHelper.ExecuteProcedureReturnData<List<HRComplaintSuggestionsHistoryLogModel>>(connString, "Get_HRComplaintSuggestionsHistoryByID", r => r.TranslateAsHRComplaintSuggestionsHistoryLogList(), contractIDParam);

            return hRComplaintSuggestionsDetails;
        }
    }
}
