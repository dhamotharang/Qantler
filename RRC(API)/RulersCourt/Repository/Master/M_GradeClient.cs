using RulersCourt.Models;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class M_GradeClient
    {
        public List<M_GradeModel> GetGrade(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_GradeModel>>(connString, "Get_M_Grade", r => r.TranslateAsGrade(), parama);
        }
    }
}
