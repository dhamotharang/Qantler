using RulersCourt.Models.ITSupport;
using RulersCourt.Translators.ITSupportTranslators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.ITSupportClient
{
    public class ITSupportHistoryLogClientcs
    {
        public List<ITSupportHistoryLogModel> ITSupportHistoryLogModelByID(string connString, int iTSupportID, string lang)
        {
            SqlParameter[] contractIDParam = {
                new SqlParameter("@P_ITSupportID", iTSupportID),
                new SqlParameter("@P_Language", lang),
            };

            List<ITSupportHistoryLogModel> hRComplaintSuggestionsDetails = new List<ITSupportHistoryLogModel>();

            hRComplaintSuggestionsDetails = SqlHelper.ExecuteProcedureReturnData<List<ITSupportHistoryLogModel>>(connString, "Get_ITSupportHistoryByID", r => r.TranslateITSupportHistoryLogModelList(), contractIDParam);

            return hRComplaintSuggestionsDetails;
        }
    }
}
