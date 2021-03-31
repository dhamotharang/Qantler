using RulersCourt.Models;
using RulersCourt.Models.Protocol.Media;
using RulersCourt.Translators;
using RulersCourt.Translators.Protocol.Media;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Protocol.Media
{
    public class CitizenAffairHomeClient
    {
        public MediaHomeModel GetAllModulesPendingTasksCount(string connString, int userID)
        {
            MediaHomeModel list = new MediaHomeModel();
            SqlParameter[] myRequestparam = {
                       new SqlParameter("@P_UserID", userID),
                        };
            list = SqlHelper.ExecuteProcedureReturnData<MediaHomeModel>(connString, "HRDashboardList", r => r.TranslateasMediaDashboardcount(), myRequestparam);
            return list;
        }

        public MediaHomeListModel GetAllMediaModules(string connString, int pageNumber, int pageSize, string type, string userID, string status, DateTime? dateFrom, DateTime? dateTo)
        {
            MediaHomeListModel list = new MediaHomeListModel();
            SqlParameter[] param = {
                   new SqlParameter("@P_PageNumber", pageNumber),
                   new SqlParameter("@P_PageSize", pageSize),
                   new SqlParameter("@P_RequestType", type),
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_Method", 0),
                   new SqlParameter("@P_Status", status),
                   new SqlParameter("@P_RequestDateFrom", dateFrom),
                   new SqlParameter("@P_RequestDateTo", dateTo)
                   };
            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<MediaHomeDashboardListModel>>(connString, "HRDashboardList", r => r.TranslateAsMediaDashboardList(), param);

            SqlParameter[] countparam = {
                   new SqlParameter("@P_PageNumber", 1),
                   new SqlParameter("@P_PageSize", 10),
                   new SqlParameter("@P_RequestType", type),
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_Method", 1),
                   new SqlParameter("@P_Status", status),
                   new SqlParameter("@P_RequestDateFrom", dateFrom),
                   new SqlParameter("@P_RequestDateTo", dateTo)
                    };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "HRDashboardList", countparam);

            Parallel.Invoke(
                   () => list.LookupsList = GetM_Lookups(connString, type));

            return list;
        }

        public List<M_LookupsModel> GetM_Lookups(string connString, string type)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Type", 0)
            };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }
    }
}
