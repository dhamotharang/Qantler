using RulersCourt.Models;
using RulersCourt.Translators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class GetApproverConfiguration
    {
        public List<ApproverDeparmentModel> GetM_ApproverDeparment(string connString, string lang)
        {
            SqlParameter[] orgParam = {
                new SqlParameter("@P_Language", lang)
            };
            var e = SqlHelper.ExecuteProcedureReturnData<List<ApproverDeparmentModel>>(connString, "Get_M_ApproverDepartments", r => r.TranslateAsApproverDepartmentList(), orgParam);
            return e;
        }
    }
}
