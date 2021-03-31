using RulersCourt.Models;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class M_TitleClient
    {
        public List<M_TitleModel> GetTitle(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_TitleModel>>(connString, "Get_M_Title", r => r.TranslateAsTitle(), parama);
        }
    }
}
