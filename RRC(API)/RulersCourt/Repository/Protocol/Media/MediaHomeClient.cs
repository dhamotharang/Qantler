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
    public class MediaHomeClient
    {
        public MediaHomeModel GetAllModulesPendingTasksCount(string connString, int userID)
        {
            MediaHomeModel list = new MediaHomeModel();
            SqlParameter[] myRequestparam = {
                       new SqlParameter("@P_UserID", userID),
                        };
            list = SqlHelper.ExecuteProcedureReturnData<MediaHomeModel>(connString, "MediaDashboardCount", r => r.TranslateasMediaDashboardcount(), myRequestparam);
            return list;
        }

        public MediaHomeListModel GetAllMediaModules(string connString, int pageNumber, int pageSize, string requestType, string userID, string status, DateTime? dateFrom, DateTime? dateTo, string paramSmartSearch, string paramSource, string type, string lang)
        {
            MediaHomeListModel list = new MediaHomeListModel();

            SqlParameter[] param = {
                   new SqlParameter("@P_PageNumber", pageNumber),
                   new SqlParameter("@P_PageSize", pageSize),
                   new SqlParameter("@P_RequestType", requestType),
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_Method", 0),
                   new SqlParameter("@P_Status", status),
                   new SqlParameter("@P_RequestDateFrom", dateFrom),
                   new SqlParameter("@P_RequestDateTo", dateTo),
                   new SqlParameter("@P_SmartSearch", paramSmartSearch),
                   new SqlParameter("@P_SourceOU", paramSource),
                   new SqlParameter("@P_Type", type),
                   new SqlParameter("@P_Language", lang) };
            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<MediaHomeDashboardListModel>>(connString, "MediaDashboardList", r => r.TranslateAsMediaDashboardList(), param);

            SqlParameter[] countparam = {
                   new SqlParameter("@P_PageNumber", pageNumber),
                   new SqlParameter("@P_PageSize", pageSize),
                   new SqlParameter("@P_RequestType", requestType),
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_Method", 1),
                   new SqlParameter("@P_Status", status),
                   new SqlParameter("@P_RequestDateFrom", dateFrom),
                   new SqlParameter("@P_RequestDateTo", dateTo),
                   new SqlParameter("@P_SmartSearch", paramSmartSearch),
                   new SqlParameter("@P_SourceOU", paramSource),
                   new SqlParameter("@P_Type", type),
                   new SqlParameter("@P_Language", lang)
                    };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "MediaDashboardList", countparam);

            Parallel.Invoke(
                     () => list.LookupsList = GetM_Lookups(connString, requestType, lang),
                     () => list.M_OrganizationList = GetM_Organisation(connString, lang),
                     () => list.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));

            return list;
        }

        public List<OrganizationModel> GetM_Organisation(string connString, string lang)
        {
            var temp = new OrganizationModel();
            SqlParameter[] param = { new SqlParameter("@P_Language", lang) };
            if (lang == "EN")
            {
                temp.OrganizationUnits = "All";
            }
            else
            {
                temp.OrganizationUnits = "الكل";
            }

            var e = SqlHelper.ExecuteProcedureReturnData<List<OrganizationModel>>(connString, "Get_Organization", r => r.TranslateAsOrganizationList(), param);
            var organisationList = new List<OrganizationModel>();
            organisationList.Add(temp);
            foreach (OrganizationModel o in e)
            {
                organisationList.Add(o);
            }

            return organisationList;
        }

        public List<M_LookupsModel> GetM_Lookups(string connString, string type, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Type", "MediaDesignRequest"), new SqlParameter("@P_Language", lang) };
            var temp = SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
            List<M_LookupsModel> list = new List<M_LookupsModel>();
            var all = new M_LookupsModel();
            if (lang == "EN")
            {
                all.DisplayName = "All";
            }
            else
            {
                all.DisplayName = "الكل";
            }

            list.Add(all);
            foreach (M_LookupsModel model in temp)
            {
                list.Add(model);
            }

            return list;
        }
    }
}
