using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.DutyTask;
using RulersCourt.Models.DutyTasks;
using RulersCourt.Translators;
using RulersCourt.Translators.DutyTask;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.DutyTask
{
    public class DutyTaskClient
    {
        public DutyTaskListModel GetDutyTasks(string connString, int pageNumber, int pageSize, string paramType, string paramUserID, string paramStatus, string paramCreator, string paramAssignee, string paramPriority, string paramLable, string paramLinkTo, DateTime? paramDueDateFrom, DateTime? paramDueDateTo, DateTime? paramCreationDateFrom, DateTime? paramCreationDateTo, string paramParticipants, string paramSmartSearch, string lang)
        {
            DutyTaskListModel list = new DutyTaskListModel();
            SqlParameter[] param = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_UserID", paramUserID),
                new SqlParameter("@P_Method", 0),
                new SqlParameter("@P_Status", paramStatus),
                new SqlParameter("@P_Creator", paramCreator),
                new SqlParameter("@P_Assignee", paramAssignee),
                new SqlParameter("@P_Priority", paramPriority),
                new SqlParameter("@P_DueDateFrom", paramDueDateFrom),
                new SqlParameter("@P_DueDateTo", paramDueDateTo),
                new SqlParameter("@P_CreationDateFrom", paramCreationDateFrom),
                new SqlParameter("@P_CreattionDateTo", paramCreationDateTo),
                new SqlParameter("@P_lable", paramLable),
                new SqlParameter("@P_LinkTo", paramLinkTo),
                new SqlParameter("@P_participants", paramParticipants),
                new SqlParameter("@P_SmartSearch", paramSmartSearch),
                new SqlParameter("@P_Type", paramType),
                new SqlParameter("@P_Language", lang)
            };

            SqlParameter[] parama = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_UserID", paramUserID),
                new SqlParameter("@P_Method", 1),
                new SqlParameter("@P_Status", paramStatus),
                new SqlParameter("@P_Creator", paramCreator),
                new SqlParameter("@P_Assignee", paramAssignee),
                new SqlParameter("@P_Priority", paramPriority),
                new SqlParameter("@P_DueDateFrom", paramDueDateFrom),
                new SqlParameter("@P_DueDateTo", paramDueDateTo),
                new SqlParameter("@P_CreationDateFrom", paramCreationDateFrom),
                new SqlParameter("@P_CreattionDateTo", paramCreationDateTo),
                new SqlParameter("@P_lable", paramLable),
                new SqlParameter("@P_LinkTo", paramLinkTo),
                new SqlParameter("@P_participants", paramParticipants),
                new SqlParameter("@P_Type", paramType),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_SmartSearch", paramSmartSearch)
            };

            SqlParameter[] paramGetCreator = {
                new SqlParameter("@P_UserId", paramUserID),
                new SqlParameter("@P_Type", 0),
                new SqlParameter("@P_Language", lang)
            };

            SqlParameter[] paramGetAssignee = {
                new SqlParameter("@P_UserId", paramUserID),
                new SqlParameter("@P_Type", 1),
                new SqlParameter("@P_Language", lang)
            };

            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskDashboardListModel>>(connString, "Get_DutyTaskList", r => r.TranslateAsList(), param);

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_DutyTaskList", parama);

            list.Assignee = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskAssigneeAndCreatorModel>>(connString, "Get_DutyTaskCreatorAndAssignee", r => r.TranslateAsGetUserList(), paramGetAssignee);

            list.Creator = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskAssigneeAndCreatorModel>>(connString, "Get_DutyTaskCreatorAndAssignee", r => r.TranslateAsGetUserList(), paramGetCreator);
            var temp = new M_LookupsModel();
            list.LookupsList = new List<M_LookupsModel>();
            if (lang == "EN")
            {
                temp.DisplayName = "All";
            }
            else
            {
                temp.DisplayName = "الكل";
            }

            list.LookupsList.Add(temp);
            foreach (M_LookupsModel m in GetM_Lookups(connString, lang))
            {
                list.LookupsList.Add(m);
            }

            return list;
        }

        public DutyTaskGetModel GetDutyTaskByID(string connString, int? taskID, int? userID, string lang)
        {
            DutyTaskGetModel dutyTask = new DutyTaskGetModel();

            SqlParameter[] param = {
                new SqlParameter("@P_DutyTaskID", taskID),
                new SqlParameter("@P_UserID", userID)
            };

            SqlParameter[] getResponsiblePersonparam = {
                    new SqlParameter("@P_DutyTaskID", taskID),
                    new SqlParameter("@P_Language", lang)
            };

            SqlParameter[] getResponsiblePersonparam1 = {
                    new SqlParameter("@P_DutyTaskID", taskID),
                    new SqlParameter("@P_Language", lang)
            };

            SqlParameter[] getResponsiblePersonDepartmentparam = {
                    new SqlParameter("@P_DutyTaskID", taskID)
            };

            SqlParameter[] attachmentparam = {
                new SqlParameter("@P_ServiceID", taskID),
                new SqlParameter("@Type", "DutyTask")
            };

            SqlParameter[] getLabelparam = {
                    new SqlParameter("@P_DutyTaskID", taskID)
            };

            if (taskID != 0)
            {
                dutyTask = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskGetModel>>(connString, "Get_DutyTaskByID", r => r.TranslateAsDutyTaskList(), param).FirstOrDefault();
                dutyTask.LinkToLetter = new DutyTaskLinkToLetterClient().GetLinkToLetter(connString, taskID);
                dutyTask.LinkToMemo = new DutyTaskLinkToMemoClient().GetLinkToMemo(connString, taskID);
                dutyTask.LinkToMeeting = new DutyTaskLinkToMeetingClient().GetLinkToMeeting(connString, taskID);
                dutyTask.CommuniationHistory = new DutyTaskCommunicationHistoryLogClient().DutyTaskCommunicationHistoryBytaskID(connString, taskID, lang);
                Parallel.Invoke(
                 () => dutyTask.ResponsibleUserId = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskResponsibleUsersModel>>(connString, "Get_DutyResponsibleUsersID", r => r.TranslateAsResponsibleUserList(), getResponsiblePersonparam),
                 () => dutyTask.TagList = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskResponsibleUsersModel>>(connString, "Get_DutyTaskResponsibleUsersID", r => r.TranslateAsResponsibleUserList(), getResponsiblePersonparam1),
                 () => dutyTask.ResponsibleDepartmentId = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskResponsibleDepartmentModel>>(connString, "Get_DutyResponsibleDepartmentID", r => r.TranslateAsResponsibleUserDepartmentList(), getResponsiblePersonDepartmentparam),
                 () => dutyTask.Attachments = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskAttachmentGetModel>>(connString, "Get_AttachmentByID", r => r.TranslateAsGetAttachmentList(), attachmentparam),
                 () => dutyTask.Labels = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskLablesModel>>(connString, "Get_DutyTaskLabels", r => r.TranslateAsLabelList(), getLabelparam));
            }

            Parallel.Invoke(
              () => dutyTask.M_OrganizationList = GetM_Organisation(connString, lang),
              () => dutyTask.M_LookupsList = GetM_Lookups(connString, lang),
              () => dutyTask.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));
            return dutyTask;
        }

        public List<OrganizationModel> GetM_Organisation(string connString, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Language", lang)
            };
            var e = SqlHelper.ExecuteProcedureReturnData<List<OrganizationModel>>(connString, "Get_Organization", r => r.TranslateAsOrganizationList(), param);
            return e;
        }

        public List<M_LookupsModel> GetM_Lookups(string connString, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Type", "DutyTask"),
                new SqlParameter("@P_Language", lang)
            };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public DutyTaskWorkflowModel PostDutyTask(string connString, DutyTaskPostModel dutyTask, string lang)
        {
            SqlParameter[] param = {
                 new SqlParameter("@P_SourceOU", dutyTask.SourceOU),
                 new SqlParameter("@P_SourceName", dutyTask.SourceName),
                 new SqlParameter("@P_Title", dutyTask.Title),
                 new SqlParameter("@P_StartDate", dutyTask.StartDate),
                 new SqlParameter("@P_EndDate", dutyTask.EndDate),
                 new SqlParameter("@P_TaskDetails", dutyTask.TaskDetails),
                 new SqlParameter("@P_Priority", dutyTask.Priority),
                 new SqlParameter("@P_RemindMeAt", dutyTask.RemindMeAt),
                 new SqlParameter("@P_CreatedBy", dutyTask.CreatedBy),
                 new SqlParameter("@P_CreatedDateTime", dutyTask.CreatedDateTime),
                 new SqlParameter("@P_Action", dutyTask.Action),
                 new SqlParameter("@P_Comment", dutyTask.Comments),
                 new SqlParameter("@P_AssigneeID", dutyTask.AssigneeUserId),
                 new SqlParameter("@P_AssigneeDepartmentId", dutyTask.AssigneeDepartmentId),
                 new SqlParameter("@P_Country", dutyTask.Country),
                 new SqlParameter("@P_City", dutyTask.City),
                 new SqlParameter("@P_Emirates", dutyTask.Emirates)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<DutyTaskWorkflowModel>(connString, "Save_DutyTask", r => r.TranslateAsDutyTaskSaveResponseList(), param);

            if (dutyTask.LinkToMemo != null)
                new DutyTaskLinkToMemoClient().PostLinkToMemo(connString, dutyTask.LinkToMemo, result.TaskID);

            if (dutyTask.LinkToMeeting != null)
                new DutyTaskLinkToMeetingClient().PostLinkToMeeting(connString, dutyTask.LinkToMeeting, result.TaskID);

            if (dutyTask.LinkToLetter != null)
                new DutyTaskLinkToLetterClient().PostLinkToMemo(connString, dutyTask.LinkToLetter, result.TaskID);

            if (dutyTask.Labels != null)
                new DutyTaskLabelClient().SaveUser(connString, dutyTask.Labels, result.TaskID, dutyTask.CreatedBy, dutyTask.CreatedDateTime);

            if (dutyTask.ResponsibleUserId != null)
                new DutyTaskResponsiblePersonClient().SaveResponsiblePersonUserID(connString, dutyTask.ResponsibleUserId, result.TaskID, dutyTask.CreatedBy, dutyTask.CreatedDateTime, lang);

            if (dutyTask.ResponsibleDepartmentId != null)
                new DutyTaskResponsiblePersonClient().SaveResponsibleDepartment(connString, dutyTask.ResponsibleDepartmentId, result.TaskID, dutyTask.CreatedBy, dutyTask.CreatedDateTime);

            if (dutyTask.Attachments != null)
                new DutyTaskAttachmentClient().PostAttachments(connString, "DutyTask", dutyTask.Attachments, result.TaskID);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "DutyTask"),
                new SqlParameter("@P_Action", dutyTask.Action)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.AssigneeUserId = dutyTask.AssigneeUserId;

            SqlParameter[] getDestinationUserparam = {
                    new SqlParameter("@P_DutyTaskID", result.TaskID),
                    new SqlParameter("@P_Language", lang)
            };

            result.ResponsibleUserId = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskResponsibleUsersModel>>(connString, "Get_DutyResponsibleUsersID", r => r.TranslateAsResponsibleUserList(), getDestinationUserparam);

            result.Action = dutyTask.Action;
            return result;
        }

        public DutyTaskWorkflowModel PutDutyTask(string connString, DutyTaskPutModel dutyTask, string lang)
        {
            SqlParameter[] param = {
                 new SqlParameter("@P_TaskID", dutyTask.TaskID),
                 new SqlParameter("@P_SourceOU", dutyTask.SourceOU),
                 new SqlParameter("@P_SourceName", dutyTask.SourceName),
                 new SqlParameter("@P_Title", dutyTask.Title),
                 new SqlParameter("@P_StartDate", dutyTask.StartDate),
                 new SqlParameter("@P_EndDate", dutyTask.EndDate),
                 new SqlParameter("@P_TaskDetails", dutyTask.TaskDetails),
                 new SqlParameter("@P_Priority", dutyTask.Priority),
                 new SqlParameter("@P_RemindMeAt", dutyTask.RemindMeAt),
                 new SqlParameter("@P_UpdatedBy", dutyTask.UpdatedBy),
                 new SqlParameter("@P_UpdatedDateTime", dutyTask.UpdatedDateTime),
                 new SqlParameter("@P_Action", dutyTask.Action),
                 new SqlParameter("@P_Comment", dutyTask.Comments),
                 new SqlParameter("@P_AssigneeID", dutyTask.AssigneeUserId),
                 new SqlParameter("@P_AssigneeDepartmentId", dutyTask.AssigneeDepartmentId),
                 new SqlParameter("@P_Country", dutyTask.Country),
                 new SqlParameter("@P_City", dutyTask.City),
                 new SqlParameter("@P_Emirates", dutyTask.Emirates)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<DutyTaskWorkflowModel>(connString, "Save_DutyTask", r => r.TranslateAsDutyTaskSaveResponseList(), param);

            if (dutyTask.LinkToMemo != null)
                new DutyTaskLinkToMemoClient().PostLinkToMemo(connString, dutyTask.LinkToMemo, result.TaskID);

            if (dutyTask.LinkToMeeting != null)
                new DutyTaskLinkToMeetingClient().PostLinkToMeeting(connString, dutyTask.LinkToMeeting, result.TaskID);

            if (dutyTask.LinkToLetter != null)
                new DutyTaskLinkToLetterClient().PostLinkToMemo(connString, dutyTask.LinkToLetter, result.TaskID);

            if (dutyTask.Labels != null)
                new DutyTaskLabelClient().SaveUser(connString, dutyTask.Labels, result.TaskID, dutyTask.UpdatedBy, dutyTask.UpdatedDateTime);

            if (dutyTask.ResponsibleUserId != null)
                new DutyTaskResponsiblePersonClient().SaveResponsiblePersonUserID(connString, dutyTask.ResponsibleUserId, result.TaskID, dutyTask.UpdatedBy, dutyTask.UpdatedDateTime, lang);

            if (dutyTask.ResponsibleDepartmentId != null)
                new DutyTaskResponsiblePersonClient().SaveResponsibleDepartment(connString, dutyTask.ResponsibleDepartmentId, result.TaskID, dutyTask.UpdatedBy, dutyTask.UpdatedDateTime);

            new DutyTaskAttachmentClient().PostAttachments(connString, "DutyTask", dutyTask.Attachments, result.TaskID);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "DutyTask"),
                new SqlParameter("@P_Action", dutyTask.Action)
            };

            if (dutyTask.Action != "Update")
            {
                result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));
            }
            else
            {
                SqlParameter[] putparam = {
                new SqlParameter("@P_DutyTaskID", dutyTask.TaskID),
                new SqlParameter("@P_UserID", dutyTask.UpdatedBy)
                };
                result.Status = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskGetModel>>(connString, "Get_DutyTaskByID", r => r.TranslateAsDutyTaskList(), putparam).FirstOrDefault().Status ?? 0;
            }

            result.AssigneeUserId = dutyTask.AssigneeUserId;

            SqlParameter[] getDestinationUserparam = {
                    new SqlParameter("@P_DutyTaskID", result.TaskID),
                    new SqlParameter("@P_Language", lang)
            };

            result.ResponsibleUserId = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskResponsibleUsersModel>>(connString, "Get_DutyResponsibleUsersID", r => r.TranslateAsResponsibleUserList(), getDestinationUserparam);

            result.Action = dutyTask.Action;
            result.FromID = dutyTask.UpdatedBy ?? default(int);
            result.PreviousAssigneeID = dutyTask.DelegateAssignee;

            return result;
        }

        public DutyTaskWorkflowModel PatchPutDutyTask(string connString, DutyTaskPutModel dutyTask, string lang)
        {
            SqlParameter[] param = {
                 new SqlParameter("@P_TaskID", dutyTask.TaskID),
                 new SqlParameter("@P_SourceOU", dutyTask.SourceOU),
                 new SqlParameter("@P_SourceName", dutyTask.SourceName),
                 new SqlParameter("@P_Title", dutyTask.Title),
                 new SqlParameter("@P_StartDate", dutyTask.StartDate),
                 new SqlParameter("@P_EndDate", dutyTask.EndDate),
                 new SqlParameter("@P_TaskDetails", dutyTask.TaskDetails),
                 new SqlParameter("@P_Priority", dutyTask.Priority),
                 new SqlParameter("@P_RemindMeAt", dutyTask.RemindMeAt),
                 new SqlParameter("@P_UpdatedBy", dutyTask.UpdatedBy),
                 new SqlParameter("@P_UpdatedDateTime", dutyTask.UpdatedDateTime),
                 new SqlParameter("@P_Action", dutyTask.Action),
                 new SqlParameter("@P_City", dutyTask.City),
                 new SqlParameter("@P_Country", dutyTask.Country),
                 new SqlParameter("@P_Emirates", dutyTask.Emirates),
                 new SqlParameter("@P_AssigneeDepartmentId", dutyTask.AssigneeDepartmentId),
                 new SqlParameter("@P_AssigneeID", dutyTask.AssigneeUserId),
                 new SqlParameter("@P_Comment", dutyTask.Comments)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<DutyTaskWorkflowModel>(connString, "Save_DutyTask", r => r.TranslateAsDutyTaskSaveResponseList(), param);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "DutyTask"),
                new SqlParameter("@P_Action", dutyTask.Action)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            SqlParameter[] getDestinationUserparam = {
                    new SqlParameter("@P_DutyTaskID", result.TaskID),
                    new SqlParameter("@P_Language", lang)
            };

            result.ResponsibleUserId = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskResponsibleUsersModel>>(connString, "Get_DutyResponsibleUsersID", r => r.TranslateAsResponsibleUserList(), getDestinationUserparam);

            result.Action = dutyTask.Action;

            result.PreviousAssigneeID = dutyTask.DelegateAssignee;
            result.FromID = dutyTask.UpdatedBy ?? default(int);
            SqlParameter[] param1 = {
                new SqlParameter("@P_DutyTaskID", result.TaskID) };
            result.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<DutyTaskGetModel>>(connString, "Get_DutyTaskByID", r => r.TranslateAsDutyTaskList(), param1).FirstOrDefault().Status);

            return result;
        }

        public DutyTaskPutModel GetPatchDutyTaskByID(string connString, int taskID)
        {
            DutyTaskPutModel dutyTask = new DutyTaskPutModel();

            SqlParameter[] param = {
                new SqlParameter("@P_DutyTaskID", taskID)
            };

            if (taskID != 0)
            {
                dutyTask = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskPutModel>>(connString, "Get_DutyTaskByID", r => r.TranslateAsPutDutyTaskList(), param).FirstOrDefault();
            }

            return dutyTask;
        }

        public int SaveCommunicationHistory(string connString, DutyTaskCommunicationHistoryModel chat)
        {
            SqlParameter[] param =
                    {
                    new SqlParameter("@P_CommunicationID", chat.CommunicationID),
                    new SqlParameter("@P_DutyTaskID", chat.TaskID),
                    new SqlParameter("@P_Message", chat.Message),
                    new SqlParameter("@P_CreatedBy", chat.CreatedBy),
                    new SqlParameter("@P_AttachmentGuid", chat.AttachmentGuid),
                    new SqlParameter("@P_AttachmentName", chat.AttachmentName),
                    new SqlParameter("@P_CreatedDateTime", chat.CreatedDateTime)
                    };
            return int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Save_DutyTaskCommunicationHistory", param));
        }

        public async Task<List<LinkToLetterModel>> GetLinkToLetter(string connString, int userID, string referenceNumber)
        {
            SqlParameter[] getLinkToLettersParam = {
                    new SqlParameter("@P_UserID", userID),
                    new SqlParameter("@P_ReferenceNumber", referenceNumber)
            };

            return await SqlHelper.ExecuteProcedureReturnDataAsync<List<LinkToLetterModel>>(connString, "Get_DutyTaskLinkToLetter", r => r.TranslateAsLinkToLetterList(), getLinkToLettersParam);
        }

        public async Task<List<LinkToMemoModel>> GetLinkToMemo(string connString, int userID, string referenceNumber)
        {
            SqlParameter[] getLinkToMemosParam = {
                    new SqlParameter("@P_UserID", userID),
                    new SqlParameter("@P_ReferenceNumber", referenceNumber)
            };

            return await SqlHelper.ExecuteProcedureReturnDataAsync<List<LinkToMemoModel>>(connString, "Get_DutyTaskLinkToMemo", r => r.TranslateAsLinkToMemoList(), getLinkToMemosParam);
        }

        public async Task<List<LinkToMeetingModel>> GetLinkToMeeting(string connString, int userID, string referenceNumber)
        {
            SqlParameter[] getLinkToMeetingsParam = {
                    new SqlParameter("@P_UserID", userID),
                    new SqlParameter("@P_ReferenceNumber", referenceNumber)
            };

            return await SqlHelper.ExecuteProcedureReturnDataAsync<List<LinkToMeetingModel>>(connString, "Get_DutyTaskLinkToMeeting", r => r.TranslateAsLinkToMeetingList(), getLinkToMeetingsParam);
        }

        public DutyTaskCountryAndEmiratesModel GetCountryAndEmirates(string connString, int userID, string lang, string module)
        {
            DutyTaskCountryAndEmiratesModel res = new DutyTaskCountryAndEmiratesModel();

            res.Country = DbClientFactory<M_CountryClient>.Instance.GetCountry(connString, userID, lang, module);
            res.Emirates = DbClientFactory<M_EmiratesClient>.Instance.GetEmirates(connString, userID, lang);

            return res;
        }

        public async Task<DutyTaskHomeModel> GetHomeCount(string connString, int userID)
        {
            SqlParameter[] getHomeCountParam = {
                    new SqlParameter("@P_UserID", userID) };

            return await SqlHelper.ExecuteProcedureReturnDataAsync<DutyTaskHomeModel>(connString, "Get_DutyTaskDashboardCount", r => r.TranslateAsHomeCount(), getHomeCountParam);
        }

        public DutyTaskWorkflowModel DeleteDutyTask(string connString, int taskID, int userID, string lang)
        {
            SqlParameter[] param1 = {
                    new SqlParameter("@P_DutyTaskID", taskID),
                    new SqlParameter("@P_UserID", userID) };

            var a = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Delete_DutyTask", param1));

            SqlParameter[] getDestinationUserparam = {
                    new SqlParameter("@P_DutyTaskID", taskID),
                    new SqlParameter("@P_Language", lang),
            };

            SqlParameter[] param = {
                new SqlParameter("@P_DutyTaskID", taskID),
                new SqlParameter("@P_UserID", 1)
            };

            SqlParameter[] param2 = {
                new SqlParameter("@P_DutyTaskID", taskID),
                new SqlParameter("@P_UserID", 1)
            };

            SqlParameter[] param3 = {
                new SqlParameter("@P_DutyTaskID", taskID),
                new SqlParameter("@P_UserID", 1)
            };

            var result = new DutyTaskWorkflowModel();

            result.AssigneeUserId = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskGetModel>>(connString, "Get_DutyTaskByID", r => r.TranslateAsDutyTaskList(), param).FirstOrDefault().AssigneeUserId;

            result.CreatorID = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskGetModel>>(connString, "Get_DutyTaskByID", r => r.TranslateAsDutyTaskList(), param2).FirstOrDefault().CreatedBy ?? 0;

            result.ReferenceNumber = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskGetModel>>(connString, "Get_DutyTaskByID", r => r.TranslateAsDutyTaskList(), param3).FirstOrDefault().TaskReferenceNumber;

            result.ResponsibleUserId = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskResponsibleUsersModel>>(connString, "Get_DutyResponsibleUsersID", r => r.TranslateAsResponsibleUserList(), getDestinationUserparam);

            result.Action = "Delete";

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "DutyTask"),
                new SqlParameter("@P_Action", "Delete")
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.TaskID = taskID;

            return result;
        }

        public List<DutyTaskReportListModel> GetDutyTasksReport(string connString, DutyTaskReportRequestModel value, string lang)
        {
            List<DutyTaskReportListModel> list = new List<DutyTaskReportListModel>();
            SqlParameter[] param = {
                new SqlParameter("@P_UserID", value.UserID),
                new SqlParameter("@P_Status", value.Status),
                new SqlParameter("@P_Creator", value.Creator),
                new SqlParameter("@P_Assignee", value.Assignee),
                new SqlParameter("@P_Priority", value.Priority),
                new SqlParameter("@P_DueDateFrom", value.DueDateFrom),
                new SqlParameter("@P_DueDateTo", value.DueDateTo),
                new SqlParameter("@P_CreationDateFrom", value.CreationDateFrom),
                new SqlParameter("@P_CreattionDateTo", value.CreationDateTo),
                new SqlParameter("@P_lable", value.Lable),
                new SqlParameter("@P_LinkTo", value.LinkTo),
                new SqlParameter("@P_participants", value.Participants),
                new SqlParameter("@P_SmartSearch", value.SmartSearch),
                new SqlParameter("@P_Language", lang)
            };
            list = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskReportListModel>>(connString, "Get_DutyTaskReport", r => r.TranslateAsDutyTaskReportList(), param);
            return list;
        }

        internal DutyTaskWorkflowModel PatchDutyTask(string connString, int id, JsonPatchDocument<DutyTaskPutModel> value, string lang)
        {
            var result = GetPatchDutyTaskByID(connString, id);
            value.ApplyTo(result);
            var res = PatchPutDutyTask(connString, result, lang);
            res.AssigneeUserId = result.AssigneeUserId;
            res.PreviousAssigneeID = result.DelegateAssignee;
            res.FromID = result.UpdatedBy ?? default(int);

            return res;
        }
    }
}
