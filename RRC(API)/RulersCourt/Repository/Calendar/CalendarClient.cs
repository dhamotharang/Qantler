using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.Calendar;
using RulersCourt.Translators;
using RulersCourt.Translators.Calendar;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Calendar
{
    public class CalendarClient
    {
        public CalendarGetModel GetCalendarByID(string connString, int id, int userID, string lang)
        {
            CalendarGetModel calendar = new CalendarGetModel();
            SqlParameter[] param = {
                new SqlParameter("@P_CalendarID", id),
                new SqlParameter("@P_UserID", userID)
            };
            if (id != 0)
            {
                calendar = SqlHelper.ExecuteProcedureReturnData<List<CalendarGetModel>>(connString, "Get_CalendarByID", r => r.TranslateAsCalendarList(), param).FirstOrDefault();
                calendar.CommunicationHistory = new CalendarHistoryClient().CalendarHistoryLogByMeetingID(connString, id, lang);
            }

            Parallel.Invoke(
              () => calendar.M_OrganizationList = GetM_Organisation(connString, lang),
              () => calendar.M_LookupsList = GetM_Lookups(connString, lang));

            SqlParameter[] getApproverparam = {
             new SqlParameter("@P_UserID", userID),
             new SqlParameter("@P_ID", calendar.CalendarID)
            };
            calendar.CurrentApprover = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_CalendarByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam);

            userID = calendar.CreatedBy.GetValueOrDefault();

            return calendar;
        }

        public List<OrganizationModel> GetM_Organisation(string connString, string lang)
        {
            SqlParameter[] param = { new SqlParameter("@P_Language", lang) };
            var e = SqlHelper.ExecuteProcedureReturnData<List<OrganizationModel>>(connString, "Get_Organization", r => r.TranslateAsOrganizationList(), param);
            return e;
        }

        public List<M_LookupsModel> GetM_Lookups(string connString, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Type", "Calendar"),
                new SqlParameter("@P_Language", lang)
            };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public CalendarWorkFlowModel PostCalendar(string connString, CalendarPostModel value)
        {
            SqlParameter[] param = { new SqlParameter("@P_EventRequestor", value.EventRequestor),
                                     new SqlParameter("@P_EventType", value.EventType),
                                     new SqlParameter("@P_EventDetails", value.EventDetails),
                                     new SqlParameter("@P_DateFrom", value.DateFrom),
                                     new SqlParameter("@P_DateTo", value.DateTo),
                                     new SqlParameter("@P_AllDayEvents", value.AllDayEvents),
                                     new SqlParameter("@P_Location", value.Location),
                                     new SqlParameter("@P_City", value.City),
                                     new SqlParameter("@P_ParentReferenceNumber", value.ParentReferenceNumber),
                                     new SqlParameter("@P_CreatedBy", value.CreatedBy),
                                     new SqlParameter("@P_CreatedDateTime", value.CreatedDateTime),
                                     new SqlParameter("@P_Action", value.Action),
                                     new SqlParameter("@P_Comment", value.Comments)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<CalendarWorkFlowModel>(connString, "Save_Calendar", r => r.TranslateAsSaveResponseList(), param);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Calendar"),
                new SqlParameter("@P_Action", value.Action),
                new SqlParameter("@P_ApproverId", value.ApproverID)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.ApproverID = value.ApproverID;

            result.FromID = value.CreatedBy ?? default(int);
            result.Action = value.Action;
            result.ParentReferenceNumber = value.ParentReferenceNumber;
            SqlParameter[] parama = { new SqlParameter("@P_Department", 4), new SqlParameter("@P_GetHead", 1) };

            result.MediaHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), parama);

            return result;
        }

        public CalendarWorkFlowModel UpdateCalendar(string connString, CalendarPutModel value)
        {
            SqlParameter[] param = {
                                     new SqlParameter("@P_CalendarID", value.CalendarID),
                                     new SqlParameter("@P_EventRequestor", value.EventRequestor),
                                     new SqlParameter("@P_EventType", value.EventType),
                                     new SqlParameter("@P_EventDetails", value.EventDetails),
                                     new SqlParameter("@P_DateFrom", value.DateFrom),
                                     new SqlParameter("@P_DateTo", value.DateTo),
                                     new SqlParameter("@P_AllDayEvents", value.AllDayEvents),
                                     new SqlParameter("@P_Location", value.Location),
                                     new SqlParameter("@P_City", value.City),
                                     new SqlParameter("@P_UpdatedBy", value.UpdatedBy),
                                     new SqlParameter("@P_UpdatedDateTime", value.UpdatedDateTime),
                                     new SqlParameter("@P_Action", value.Action),
                                     new SqlParameter("@P_Comment", value.Comments),
                                     new SqlParameter("@P_IsApologySent", value.IsApologySent)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<CalendarWorkFlowModel>(connString, "Save_Calendar", r => r.TranslateAsSaveResponseList(), param);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Calendar"),
                new SqlParameter("@P_Action", value.Action),
                new SqlParameter("@P_ApproverId", value.ApproverID)
            };
            if (value.Action != "Apology")
                result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.ApproverID = value.ApproverID;

            result.FromID = value.UpdatedBy ?? default(int);
            result.Action = value.Action;

            SqlParameter[] parama = { new SqlParameter("@P_Department", 4), new SqlParameter("@P_GetHead", 1) };

            result.MediaHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), parama);

            SqlParameter[] param1 = {
                new SqlParameter("@P_CalendarID", result.CalendarID)
            };

            result.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<CalendarGetModel>>(connString, "Get_CalendarByID", r => r.TranslateAsCalendarList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber)
            };

            result.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_CalendarByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return result;
        }

        public CalendarPutModel GetPatchCalendarByID(string connString, int id)
        {
            CalendarPutModel calendar = new CalendarPutModel();

            SqlParameter[] param = {
                new SqlParameter("@P_CalendarID", id),
                new SqlParameter("@P_UserID", 0)
            };

            if (id != 0)
            {
                calendar = SqlHelper.ExecuteProcedureReturnData<List<CalendarPutModel>>(connString, "Get_CalendarById", r => r.TranslateAsCalendarPatchList(), param).FirstOrDefault();
            }

            return calendar;
        }

        public CalendarListModel GetCalendarList(string connString, int pageNumber, int pageSize, string paramUserID, string paramRefNo, string paramType, string paramEventType, string paramEventRequestor, DateTime? paramDateFrom, DateTime? paramDateTo, string paramStatus, string paramSmartSearch, string lang)
        {
            CalendarListModel list = new CalendarListModel();
            SqlParameter[] param = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_UserID", paramUserID),
                new SqlParameter("@P_Method", 0),
                new SqlParameter("@P_ReferenceNumber", paramRefNo),
                new SqlParameter("@P_Type", paramType),
                new SqlParameter("@P_EventType", paramEventType),
                new SqlParameter("@P_EventRequestor", paramEventRequestor),
                new SqlParameter("@P_DateFrom", paramDateFrom),
                new SqlParameter("@P_DateTo", paramDateTo),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_Status", paramStatus),
                new SqlParameter("@P_SmartSearch", paramSmartSearch)
            };
            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<CalendarListViewModel>>(connString, "Get_CalendarList", r => r.TranslateAsCalendarDashboardList(), param);

            SqlParameter[] parama = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_UserID", paramUserID),
                new SqlParameter("@P_Method", 1),
                new SqlParameter("@P_ReferenceNumber", paramRefNo),
                new SqlParameter("@P_Type", paramType),
                new SqlParameter("@P_EventRequestor", paramEventRequestor),
                new SqlParameter("@P_EventType", paramEventType),
                new SqlParameter("@P_DateFrom", paramDateFrom),
                new SqlParameter("@P_DateTo", paramDateTo),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_Status", paramStatus),
                new SqlParameter("@P_SmartSearch", paramSmartSearch)
            };
            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_CalendarList", parama);

            Parallel.Invoke(
                () => list.M_OrganizationList = GetM_Organisation(connString, lang),
                () => list.M_LookupsList = GetM_Lookups(connString, lang));
            return list;
        }

        public CalendarViewModel GetCalenderViewList(string connString, int paramUserID, string paramMonth, string paramYear, string paramRefNo, string paramEventType, string paramEventRequestor, DateTime? paramDateFrom, DateTime? paramDateTo, string lang, string paramSmartSearch)
        {
            CalendarViewModel list = new CalendarViewModel();
            SqlParameter[] param = {
                new SqlParameter("@P_UserID", paramUserID),
                new SqlParameter("@P_Month", paramMonth),
                new SqlParameter("@P_Year", paramYear),
                new SqlParameter("@P_ReferenceNumber", paramRefNo),
                new SqlParameter("@P_EventRequestor", paramEventRequestor),
                new SqlParameter("@P_EventType", paramEventType),
                new SqlParameter("@P_DateFrom", paramDateFrom),
                new SqlParameter("@P_DateTo", paramDateTo),
                new SqlParameter("@P_SmartSearch", paramSmartSearch)
            };
            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<CalendarViewListModel>>(connString, "Get_CalenderViewList", r => r.TranslateAsCalenderViewDashboardList(), param);

            Parallel.Invoke(
              () => list.M_LookupsList = GetM_Lookups(connString, lang));

            return list;
        }

        public CalendarBulkList GetCalendarByBulkID(string connString, int userID, string referenceNumber, string lang)
        {
            CalendarBulkList calendar = new CalendarBulkList();
            SqlParameter[] param = {
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_ReferenceNumber", referenceNumber),
                new SqlParameter("@P_Language", lang)
            };

            SqlParameter[] parama = {
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_ReferenceNumber", referenceNumber)
            };

            calendar.Collection = SqlHelper.ExecuteProcedureReturnData<List<CalendarBulkModel>>(connString, "Get_CalendarByBulkId", r => r.TranslateAsCalendarBulkList(), param);

            calendar.ReferenceNumber = referenceNumber;
            calendar.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_CalendarByBulkIdStatus", parama));
            calendar.CreatedBy = calendar.Collection.FirstOrDefault().CreatedBy;
            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", calendar.ReferenceNumber)
            };

            calendar.ApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_CalendarByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            Parallel.Invoke(
              () => calendar.M_OrganizationList = GetM_Organisation(connString, lang),
              () => calendar.M_LookupsList = GetM_Lookups(connString, lang));
            return calendar;
        }

        public List<CalendarReport> GetCalendarReportExportList(string connString, CalendarReportModel report, string paramSmartsearch, string lang)
        {
            List<CalendarReport> list = new List<CalendarReport>();

            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", 1),
            new SqlParameter("@P_PageSize", 10),
            new SqlParameter("@P_Method", 2),
            new SqlParameter("@P_CalendarID", report.CalendarID),
            new SqlParameter("@P_ReferenceNumber", report.ReferenceNumber),
            new SqlParameter("@P_EventRequestor", report.EventRequestor),
            new SqlParameter("@P_Location", report.Location),
            new SqlParameter("@P_DateFrom", report.DateFrom),
            new SqlParameter("@P_DateTo", report.DateTo),
            new SqlParameter("@P_EventType", report.EventType),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_SmartSearch", report.SmartSearch),
            new SqlParameter("@P_Language", lang)
            };

            list = SqlHelper.ExecuteProcedureReturnData<List<CalendarReport>>(connString, "CalendarReport", r => r.TranslateAsCalendarReportList(), param);

            return list;
        }

        public CalendarWorkFlowModel GetCalendarBulkWorkflowByID(string connString, int calendarID, CalendarBulkApprovalModel value)
        {
            CalendarPutModel model = new CalendarPutModel();

            model = GetPatchCalendarByID(connString, calendarID);

            model.Action = value.Action;
            model.UpdatedBy = value.ActionBy;
            model.UpdatedDateTime = value.ActionDateTime;
            model.Comments = value.Comments;

            if (model.Action == "Escalate")
                model.ApproverID = value.ApproverID;

            var res = UpdateCalendar(connString, model);

            if ((model.Action == "Approve") || model.Action == "Reject" || model.Action == "Returnforinfo")
                model.ApproverID = model.UpdatedBy;
            SqlParameter[] param1 = {
                new SqlParameter("@P_CalendarID", model.CalendarID)
            };

            res.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<CalendarGetModel>>(connString, "Get_CalendarByID", r => r.TranslateAsCalendarList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", res.ReferenceNumber)
            };

            res.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_CalendarByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return res;
        }

        public string BulkApology(string connString, int calendarID, CalendarBulkApprovalModel value)
        {
            var result = string.Empty;
            foreach (CalendarBulkID item in value.CalendarID)
            {
                SqlParameter[] apologyparam = {
                new SqlParameter("@P_CalendarID", item.CalendarID),
                new SqlParameter("@P_UpdatedBy", value.ApproverID),
                new SqlParameter("@P_IsApologySent", value.IsApologySent),
                new SqlParameter("@P_UpdatedDatetime", value.ActionDateTime)
                };

                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_CalendarApology", apologyparam);
            }

            return result;
        }

        public string SaveCommunicationChat(string connString, CalendarHistoryPostModel value)
        {
            var result = string.Empty;
            SqlParameter[] communicationParam = {
                new SqlParameter("@P_CalendarID", value.CalendarID),
                new SqlParameter("@P_Message", value.Message),
                new SqlParameter("@P_ParentCommunicationID", value.ParentCommunicationID),
                new SqlParameter("@P_CreatedBy", value.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", value.CreatedDateTime)
            };

            result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_CalendarCommunicationHistory", communicationParam);

            return result;
        }

        public CalendarcountModel GetAllModulesPendingTasksCount(string connString, int userID)
        {
            CalendarcountModel list = new CalendarcountModel();
            SqlParameter[] myRequestparam = {
                       new SqlParameter("@P_UserID", userID),
                        };
            list = SqlHelper.ExecuteProcedureReturnData<CalendarcountModel>(connString, "Get_CalendarCount", r => r.TranslateasCalendarDashboardcount(), myRequestparam);
            return list;
        }

        internal CalendarWorkFlowModel PatchCalendar(string connString, int id, JsonPatchDocument<CalendarPutModel> value)
        {
            var result = GetPatchCalendarByID(connString, id);

            value.ApplyTo(result);
            if ((result.Action == "Approve") || result.Action == "Reject" || result.Action == "Returnforinfo")
                result.ApproverID = result.UpdatedBy;

            var res = UpdateCalendar(connString, result);
            if (result.Action == "Escalate" || result.Action == "Redirect")
            {
                res.ApproverID = result.ApproverID;
            }

            if ((result.Action == "Apology") && result.IsApologySent == true)
                result.IsApologySent = true;
            if ((result.Action == "Apology") && result.IsApologySent == false)
                result.IsApologySent = false;

            SqlParameter[] param = { new SqlParameter("@P_Department", 4), new SqlParameter("@P_GetHead", 1) };

            res.MediaHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), param);

            SqlParameter[] param1 = {
                new SqlParameter("@P_CalendarID", result.CalendarID)
            };

            res.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<CalendarGetModel>>(connString, "Get_CalendarByID", r => r.TranslateAsCalendarList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", res.ReferenceNumber)
            };

            res.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_CalendarByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return res;
        }
    }
}
