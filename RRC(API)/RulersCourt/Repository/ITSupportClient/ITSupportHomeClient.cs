using RulersCourt.Models;
using RulersCourt.Models.ITSupport;
using RulersCourt.Translators;
using RulersCourt.Translators.ITSupportTranslators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RulersCourt.Repository.ITSupportClient
{
    public class ITSupportHomeClient
    {
        public ITSupportHomeListModel GetITSupport(string connString, int pageNumber, int pageSize, string requestType, string userID, string username, string sourceOU, string subject, string status, string priority, DateTime? dateFrom, DateTime? dateTo, string smartSearch, string lang, string paramType)
        {
            ITSupportHomeListModel list = new ITSupportHomeListModel();

            SqlParameter[] param = {
                   new SqlParameter("@P_PageNumber", pageNumber),
                   new SqlParameter("@P_PageSize", pageSize),
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_RequestType", requestType),
                   new SqlParameter("@P_SourceOU", sourceOU),
                   new SqlParameter("@P_Subject", subject),
                   new SqlParameter("@P_Priority", priority),
                   new SqlParameter("@P_Method", 0),
                   new SqlParameter("@p_Type", paramType),
                   new SqlParameter("@P_Username", username),
                   new SqlParameter("@P_Language", lang),
                   new SqlParameter("@P_Status", status),
                   new SqlParameter("@P_RequestDateFrom", dateFrom),
                   new SqlParameter("@P_RequestDateTo", dateTo),
                   new SqlParameter("@P_SmartSearch", smartSearch)
            };
            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<ITSupportHomeDashboardListModel>>(connString, "ITDashboardList", r => r.TranslateAsITDashboardList(), param);

            SqlParameter[] countparam = {
                   new SqlParameter("@P_PageNumber", 1),
                   new SqlParameter("@P_PageSize", 10),
                   new SqlParameter("@P_RequestType", requestType),
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_Method", 1),
                   new SqlParameter("@P_Username", username),
                   new SqlParameter("@P_Subject", subject),
                   new SqlParameter("@P_SourceOU", sourceOU),
                   new SqlParameter("@P_Priority", priority),
                   new SqlParameter("@P_Status", status),
                   new SqlParameter("@P_Language", lang),
                   new SqlParameter("@p_Type", paramType),
                   new SqlParameter("@P_RequestDateFrom", dateFrom),
                   new SqlParameter("@P_RequestDateTo", dateTo),
                   new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "ITDashboardList", countparam);
            Parallel.Invoke(
                   () => list.LookupsList = GetM_Lookups(connString, requestType, lang),
                   () => list.M_OrganizationList = GetM_Organisation(connString, lang));

            return list;
        }

        public List<OrganizationModel> GetM_Organisation(string connString, string lang)
        {
            var temp = new OrganizationModel();
            SqlParameter[] param = {
                new SqlParameter("@P_Language", lang) };
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

        public List<M_LookupsModel> GetM_Lookups(string connString, string requestType, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Type", "ITSupport"),
                new SqlParameter("@P_Language", lang)
            };
            List<M_LookupsModel> temp = SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
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

        public List<ITReport> GetITSupportReportExportList(string connString, ITSupportReportModel report, string lang)
        {
            List<ITReport> list = new List<ITReport>();

            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", 0),
            new SqlParameter("@P_PageSize", 0),
            new SqlParameter("@P_Method", 2),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_Priority", report.Priority),
            new SqlParameter("@P_Subject", report.Subject),
            new SqlParameter("@P_SourceOU", report.SourceOU),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_RequestType", report.RequestType),
            new SqlParameter("@P_RequestDateFrom", report.RequestDateFrom),
            new SqlParameter("@P_RequestDateTo", report.RequestDateTo),
            new SqlParameter("@P_SmartSearch", report.SmartSearch),
            new SqlParameter("@P_Language", lang)
            };

            list = SqlHelper.ExecuteProcedureReturnData<List<ITReport>>(connString, "ITReport", r => r.TranslateAsITReportList(), param);

            return list;
        }

        public ITSupportHomeModel GetAllTasksCount(string connString, int userID)
        {
            {
                SqlParameter[] param = {
                new SqlParameter("@P_UserID", userID) };
                if (userID != 0)
                {
                    ITSupportHomeModel getITSupportUserIDDetails = new ITSupportHomeModel();
                    getITSupportUserIDDetails = SqlHelper.ExecuteProcedureReturnData<ITSupportHomeModel>(connString, "ITSupportDashboardListCount", r => r.TranslateITSupportHomeModel(), param);
                    return getITSupportUserIDDetails;
                }

                return null;
            }
        }
    }
}
