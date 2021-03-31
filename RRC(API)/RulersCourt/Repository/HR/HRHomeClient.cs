using RulersCourt.Models;
using RulersCourt.Models.HR;
using RulersCourt.Translators;
using RulersCourt.Translators.HR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RulersCourt.Repository.HR
{
    public class HRHomeClient
    {
        public HRHomeListModel GetAllHRModules(string connString, int pageNumber, int pageSize, string type, string username, string userID, string creator, string status, DateTime? dateFrom, DateTime? dateTo, string smartSearch, string lang)
        {
            HRHomeListModel list = new HRHomeListModel();

            SqlParameter[] param = {
                   new SqlParameter("@P_PageNumber", pageNumber),
                   new SqlParameter("@P_PageSize", pageSize),
                   new SqlParameter("@P_RequestType", type),
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_Method", 0),
                   new SqlParameter("@P_Username", username),
                   new SqlParameter("@P_Language", lang),
                   new SqlParameter("@P_Creator", creator),
                   new SqlParameter("@P_Status", status),
                   new SqlParameter("@P_RequestDateFrom", dateFrom),
                   new SqlParameter("@P_RequestDateTo", dateTo),
                   new SqlParameter("@P_SmartSearch", smartSearch)
            };
            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<HRHomeDashboardListModel>>(connString, "HRDashboardList", r => r.TranslateAsHRDashboardList(), param);

            SqlParameter[] countparam = {
                   new SqlParameter("@P_PageNumber", 1),
                   new SqlParameter("@P_PageSize", 10),
                   new SqlParameter("@P_RequestType", type),
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_Method", 1),
                   new SqlParameter("@P_Language", lang),
                   new SqlParameter("@P_Username", username),
                   new SqlParameter("@P_Creator", creator),
                   new SqlParameter("@P_Status", status),
                   new SqlParameter("@P_RequestDateFrom", dateFrom),
                   new SqlParameter("@P_RequestDateTo", dateTo),
                   new SqlParameter("@P_SmartSearch", smartSearch)
                    };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "HRDashboardList", countparam);

            Parallel.Invoke(
                   () => list.LookupsList = GetM_Lookups(connString, type, lang),
                   () => list.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));

            return list;
        }

        public HRHomeModel GetAllModulesPendingTasksCount(string connString, int userID)
        {
            HRHomeModel list = new HRHomeModel();
            SqlParameter[] announcementparam = {
                       new SqlParameter("@P_PageNumber", 1),
                       new SqlParameter("@P_PageSize", 10),
                       new SqlParameter("@P_RequestType", 1),
                       new SqlParameter("@P_UserID", userID),
                       new SqlParameter("@P_Method", 1)
                        };

            SqlParameter[] babyAdditionparam = {
                       new SqlParameter("@P_PageNumber", 1),
                       new SqlParameter("@P_PageSize", 10),
                       new SqlParameter("@P_RequestType", 2),
                       new SqlParameter("@P_UserID", userID),
                       new SqlParameter("@P_Method", 1)
                        };

            SqlParameter[] salaryCertificateparam = {
                       new SqlParameter("@P_PageNumber", 1),
                       new SqlParameter("@P_PageSize", 10),
                       new SqlParameter("@P_RequestType", 3),
                       new SqlParameter("@P_UserID", userID),
                       new SqlParameter("@P_Method", 1)
                        };

            SqlParameter[] experienceCertificateparam = {
                       new SqlParameter("@P_PageNumber", 1),
                       new SqlParameter("@P_PageSize", 10),
                       new SqlParameter("@P_RequestType", 4),
                       new SqlParameter("@P_UserID", userID),
                       new SqlParameter("@P_Method", 1)
                        };

            SqlParameter[] cVBankparam = {
                       new SqlParameter("@P_PageNumber", 1),
                       new SqlParameter("@P_PageSize", 10),
                       new SqlParameter("@P_RequestType", 5),
                       new SqlParameter("@P_UserID", userID),
                       new SqlParameter("@P_Method", 1)
                        };

            SqlParameter[] leaveparam = {
                       new SqlParameter("@P_PageNumber", 1),
                       new SqlParameter("@P_PageSize", 10),
                       new SqlParameter("@P_RequestType", 6),
                       new SqlParameter("@P_UserID", userID),
                       new SqlParameter("@P_Method", 1)
                        };

            SqlParameter[] officialTaskparam = {
                       new SqlParameter("@P_PageNumber", 1),
                       new SqlParameter("@P_PageSize", 10),
                       new SqlParameter("@P_RequestType", 7),
                       new SqlParameter("@P_UserID", userID),
                       new SqlParameter("@P_Method", 1)
                        };

            SqlParameter[] trainingparam = {
                       new SqlParameter("@P_PageNumber", 1),
                       new SqlParameter("@P_PageSize", 10),
                       new SqlParameter("@P_RequestType", 8),
                       new SqlParameter("@P_UserID", userID),
                       new SqlParameter("@P_Method", 1)
                        };

            SqlParameter[] userProfileparam = {
                       new SqlParameter("@P_PageNumber", 1),
                       new SqlParameter("@P_PageSize", 10),
                       new SqlParameter("@P_RequestType", 9),
                       new SqlParameter("@P_UserID", userID),
                       new SqlParameter("@P_Method", 1)
                        };

            SqlParameter[] compliantSuggestionsparam = {
                       new SqlParameter("@P_PageNumber", 1),
                       new SqlParameter("@P_PageSize", 10),
                       new SqlParameter("@P_RequestType", 10),
                       new SqlParameter("@P_UserID", userID),
                       new SqlParameter("@P_Method", 1)
                        };

            SqlParameter[] myRequestparam = {
                       new SqlParameter("@P_PageNumber", 1),
                       new SqlParameter("@P_PageSize", 10),
                       new SqlParameter("@P_UserID", userID),
                       new SqlParameter("@P_Method", 1)
                        };

            SqlParameter[] myPendingRequestparam = {
                       new SqlParameter("@P_PageNumber", 1),
                       new SqlParameter("@P_PageSize", 10),
                       new SqlParameter("@P_UserID", userID),
                       new SqlParameter("@P_Method", 1)
                        };

            SqlParameter[] myProcessedRequestparam = {
                       new SqlParameter("@P_PageNumber", 1),
                       new SqlParameter("@P_PageSize", 10),
                       new SqlParameter("@P_UserID", userID),
                       new SqlParameter("@P_Method", 1)
                        };
            Parallel.Invoke(
            () => list.AnnouncementRequests = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "HRDashboardListCount", announcementparam)),
            () => list.NewBabyAddition = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "HRDashboardListCount", babyAdditionparam)),
            () => list.SalaryCertificate = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "HRDashboardListCount", salaryCertificateparam)),
            () => list.ExperienceCertificate = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "HRDashboardListCount", experienceCertificateparam)),
            () => list.CVBank = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "HRDashboardListCount", cVBankparam)),
            () => list.LeaveRequests = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "HRDashboardListCount", leaveparam)),
            () => list.OfficialTaskRequests = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "HRDashboardListCount", officialTaskparam)),
            () => list.TrainingRequests = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "HRDashboardListCount", trainingparam)),
            () => list.EmployeesProfile = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "HRDashboardListCount", userProfileparam)),
            () => list.RaiseComplaintSuggestions = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "HRDashboardListCount", compliantSuggestionsparam)),
            () => list.MyPendingActions = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_MyPendingListInHRCount", myPendingRequestparam)),
            () => list.MyProcessedRequests = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_MyProcessedListInHR", myProcessedRequestparam)),
            () => list.MyRequests = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_MyOwnRequestsInHR", myRequestparam)));
            return list;
        }

        public HRHomeListModel GetMyPending(string connString, int pageNumber, int pageSize, string type, string username, string userID, string creator, string status, DateTime? dateFrom, DateTime? dateTo, string smartSearch, string lang)
        {
            HRHomeListModel list = new HRHomeListModel();

            SqlParameter[] param = {
                    new SqlParameter("@P_PageNumber", pageNumber),
                    new SqlParameter("@P_PageSize", pageSize),
                    new SqlParameter("@P_UserID", userID),
                    new SqlParameter("@P_Method", 0),
                    new SqlParameter("@P_Username", username),
                    new SqlParameter("@P_Creator", creator),
                    new SqlParameter("@P_Status", status),
                    new SqlParameter("@P_RequestType", type),
                    new SqlParameter("@P_Language", lang),
                    new SqlParameter("@P_RequestDateFrom", dateFrom),
                    new SqlParameter("@P_RequestDateTo", dateTo),
                    new SqlParameter("@P_SmartSearch", smartSearch)
            };
            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<HRHomeDashboardListModel>>(connString, "Get_MyPendingListInHR", r => r.TranslateAsHRDashboardList(), param);

            SqlParameter[] countparam = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Method", 1),
                new SqlParameter("@P_Username", username),
                new SqlParameter("@P_Creator", creator),
                new SqlParameter("@P_Status", status),
                new SqlParameter("@P_RequestType", type),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_RequestDateFrom", dateFrom),
                new SqlParameter("@P_RequestDateTo", dateTo),
                new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_MyPendingListInHRCount", countparam);

            Parallel.Invoke(
                   () => list.LookupsList = GetM_Lookups(connString, type, lang),
                   () => list.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));

            return list;
        }

        public HRHomeListModel GetMyProcessed(string connString, int pageNumber, int pageSize, string type, string username, string userID, string creator, string status, DateTime? dateFrom, DateTime? dateTo, string smartSearch, string lang)
        {
            HRHomeListModel list = new HRHomeListModel();
            SqlParameter[] param = {
                    new SqlParameter("@P_PageNumber", pageNumber),
                    new SqlParameter("@P_PageSize", pageSize),
                    new SqlParameter("@P_UserID", userID),
                    new SqlParameter("@P_Method", 0),
                    new SqlParameter("@P_Username", username),
                    new SqlParameter("@P_Creator", creator),
                    new SqlParameter("@P_Status", status),
                    new SqlParameter("@P_RequestType", type),
                    new SqlParameter("@P_Language", lang),
                    new SqlParameter("@P_RequestDateFrom", dateFrom),
                    new SqlParameter("@P_RequestDateTo", dateTo),
                    new SqlParameter("@P_SmartSearch", smartSearch)
            };
            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<HRHomeDashboardListModel>>(connString, "Get_MyProcessedListInHR", r => r.TranslateAsHRDashboardList(), param);

            SqlParameter[] countparam = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Method", 1),
                new SqlParameter("@P_Username", username),
                new SqlParameter("@P_Creator", creator),
                new SqlParameter("@P_Status", status),
                new SqlParameter("@P_RequestType", type),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_RequestDateFrom", dateFrom),
                new SqlParameter("@P_RequestDateTo", dateTo),
                new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_MyProcessedListInHR", countparam);

            Parallel.Invoke(
                   () => list.LookupsList = GetM_Lookups(connString, type, lang),
                   () => list.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));

            return list;
        }

        public HRHomeListModel GetMyOwnRequest(string connString, int pageNumber, int pageSize, string type, string username, string userID, string creator, string status, DateTime? dateFrom, DateTime? dateTo, string smartSearch, string lang)
        {
            HRHomeListModel list = new HRHomeListModel();

            SqlParameter[] param = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Method", 0),
                new SqlParameter("@P_Username", username),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_Creator", creator),
                new SqlParameter("@P_Status", status),
                new SqlParameter("@P_RequestType", type),
                new SqlParameter("@P_RequestDateFrom", dateFrom),
                new SqlParameter("@P_RequestDateTo", dateTo),
                new SqlParameter("@P_SmartSearch", smartSearch)
            };
            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<HRHomeDashboardListModel>>(connString, "Get_MyOwnRequestsInHR", r => r.TranslateAsHRDashboardList(), param);

            SqlParameter[] countparam = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Method", 1),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_Username", username),
                new SqlParameter("@P_Creator", creator),
                new SqlParameter("@P_Status", status),
                new SqlParameter("@P_RequestType", type),
                new SqlParameter("@P_RequestDateFrom", dateFrom),
                new SqlParameter("@P_RequestDateTo", dateTo),
                new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_MyOwnRequestsInHR", countparam);

            Parallel.Invoke(
                   () => list.LookupsList = GetM_Lookups(connString, type, lang),
                   () => list.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));

            return list;
        }

        public List<M_LookupsModel> GetM_Lookups(string connString, string type, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Type", 0),
                new SqlParameter("@P_Language", lang)
            };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }
    }
}
