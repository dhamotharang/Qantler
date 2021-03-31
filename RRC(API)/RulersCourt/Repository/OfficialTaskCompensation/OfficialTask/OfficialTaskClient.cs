using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.OfficalTask;
using RulersCourt.Models.OfficialTask;
using RulersCourt.Models.OfficialTaskCompensation.OfficialTask;
using RulersCourt.Repository.OfficialTaskCompensation.OfficialTask;
using RulersCourt.Translators;
using RulersCourt.Translators.OfficalTaskCompensation.OfficalTask;
using RulersCourt.Translators.OfficialTaskCompensation.OfficialTask;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.OfficialTaskCompensation.OfficalTask
{
    public class OfficialTaskClient
    {
        public OfficialTaskGetModel GetOfficialTaskByID(string connString, int officialTaskID, int userID, string lang)
        {
            OfficialTaskGetModel officialTaskDetails = new OfficialTaskGetModel();
            SqlParameter[] param = {
                new SqlParameter("@P_OfficialTaskID", officialTaskID),
                new SqlParameter("@P_UserID", userID) };

            SqlParameter[] getEmployeeNameparam = {
                new SqlParameter("@P_OfficialTaskID", officialTaskID),
                new SqlParameter("@P_Language", lang)
            };

            SqlParameter[] getEmployeeDepartmentparam = {
                    new SqlParameter("@P_OfficialTaskID", officialTaskID) };

            if (officialTaskID != 0)
            {
                officialTaskDetails = SqlHelper.ExecuteProcedureReturnData<List<OfficialTaskGetModel>>(connString, "Get_OfficialTaskByID", r => r.TranslateAsOfficialTaskList(), param).FirstOrDefault();

                officialTaskDetails.CommunicationHistory = new OfficialTaskCommunicationHistoryClient().OfficialTaskCommunicationHistoryByOfficalTaskID(connString, officialTaskID, lang);
                userID = officialTaskDetails.CreatedBy.GetValueOrDefault();

                officialTaskDetails.EmployeeNameID = SqlHelper.ExecuteProcedureReturnData<List<OfficialTaskEmployeeGetModel>>(connString, "Get_OfficialTaskEmployeeDetails", r => r.TranslateAsEmployeeNameList(), getEmployeeNameparam);
            }

            Parallel.Invoke(
              () => officialTaskDetails.M_OrganizationList = GetM_Organisation(connString, lang),
              () => officialTaskDetails.M_LookupsList = GetM_Lookups(connString, lang));

            return officialTaskDetails;
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
                new SqlParameter("@P_Type", "OfficialTask"), new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public OfficialTaskWorkflowModel PostOfficialTask(string connString, OfficialTaskPostModel officialTask)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Date", officialTask.Date),
                new SqlParameter("@P_SourceOU", officialTask.SourceOU),
                new SqlParameter("@P_SourceName", officialTask.SourceName),
                new SqlParameter("@P_OfficialTaskType", officialTask.OfficialTaskType),
                new SqlParameter("@P_StartDate", officialTask.StartDate),
                new SqlParameter("@P_EndDate", officialTask.EndDate),
                new SqlParameter("@P_NumberofDays", officialTask.NumberofDays),
                new SqlParameter("@P_OfficialTaskDescription", officialTask.OfficialTaskDescription),
                new SqlParameter("@P_CreatedBy", officialTask.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", officialTask.CreatedDateTime),
                new SqlParameter("@P_Action", officialTask.Action),
                new SqlParameter("@P_Comments", officialTask.Comments)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<OfficialTaskWorkflowModel>(connString, "Save_OfficialTask", r => r.TranslateAsOfficialTaskSaveResponseList(), param);

            if (officialTask.EmployeeNameID != null)
                new OfficialEmployeeClient().SaveUser(connString, officialTask.EmployeeNameID, result.OfficialTaskID);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "OfficialTask"),
                new SqlParameter("@P_Action", officialTask.Action) };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            SqlParameter[] paramgetuser = { new SqlParameter("@P_Department", 9), new SqlParameter("@P_GetHead", 0) };

            result.AssigneeID = result.AssigneeID;
            result.FromID = officialTask.CreatedBy ?? default(int);
            result.Action = officialTask.Action;
            result.EmployeeNameID = officialTask.EmployeeNameID;
            return result;
        }

        public OfficialTaskWorkflowModel PutOfficialTask(string connString, OfficialTaskPutModel officialTask)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_OfficialTaskID", officialTask.OfficialTaskID),
                new SqlParameter("@P_Date", officialTask.Date),
                new SqlParameter("@P_SourceOU", officialTask.SourceOU),
                new SqlParameter("@P_SourceName", officialTask.SourceName),
                new SqlParameter("@P_OfficialTaskType", officialTask.OfficialTaskType),
                new SqlParameter("@P_StartDate", officialTask.StartDate),
                new SqlParameter("@P_EndDate", officialTask.EndDate),
                new SqlParameter("@P_NumberofDays", officialTask.NumberofDays),
                new SqlParameter("@P_OfficialTaskDescription", officialTask.OfficialTaskDescription),
                new SqlParameter("@P_UpdatedBy", officialTask.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", officialTask.UpdatedDateTime),
                new SqlParameter("@P_Action", officialTask.Action),
                new SqlParameter("@P_Comments", officialTask.Comments),
                new SqlParameter("@P_DeleteFlag", officialTask.DeleteFlag)
            };
            var result = SqlHelper.ExecuteProcedureReturnData<OfficialTaskWorkflowModel>(connString, "Save_OfficialTask", r => r.TranslateAsOfficialTaskSaveResponseList(), param);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "OfficialTask"),
                new SqlParameter("@P_Action", officialTask.Action) };
            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            SqlParameter[] paramgetuser = { new SqlParameter("@P_Department", 9), new SqlParameter("@P_GetHead", 0) };
            result.AssigneeID = result.AssigneeID;
            result.Action = officialTask.Action;
            return result;
        }

        public OfficialTaskPutModel GetPatchOfficialTaskByID(string connString, int officialTaskID)
        {
            OfficialTaskPutModel officialTaskDetails = new OfficialTaskPutModel();
            SqlParameter[] param = {
                new SqlParameter("@P_OfficialTaskID", officialTaskID) };
            if (officialTaskID != 0)
            {
                officialTaskDetails = SqlHelper.ExecuteProcedureReturnData<List<OfficialTaskPutModel>>(connString, "Get_OfficialTaskByID", r => r.TranslateAsPutOfficialTaskList(), param).FirstOrDefault();
            }

            return officialTaskDetails;
        }

        public int SaveCommunicationChat(string connString, OfficialTaskCommunicationHistoryModel chat)
        {
            SqlParameter[] param =
                    { new SqlParameter("@P_CommunicationID", chat.CommunicationID),
                    new SqlParameter("@P_OfficialTaskID", chat.OfficialTaskID),
                    new SqlParameter("@P_Message", chat.Message),
                    new SqlParameter("@P_ParentCommunicationID", chat.ParentCommunicationID),
                    new SqlParameter("@P_CreatedBy", chat.CreatedBy),
                    new SqlParameter("@P_CreatedDateTime", chat.CreatedDateTime)
                    };
            return int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Save_OfficialTaskCommunicationHistory", param));
        }

        public List<UserModel> GetOfficialTaskUser(string connString, string department, string userID, DateTime? startDate, DateTime? endDate)
        {
            SqlParameter[] param = {
                   new SqlParameter("@P_Department", department),
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_StartDate", startDate),
                   new SqlParameter("@P_EndDate", endDate) };

            return SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_OfficialTaskUser", r => r.TranslateAsUserList(), param);
        }

        public bool? UserAvailability(string connString, string userID, string startDate, string endDate, List<OfficialTaskEmployeeNameModel> employee)
        {
            SqlParameter[] param = {
                   new SqlParameter("@P_UserID", string.Join(",", from item in employee select item.OfficialTaskEmployeeID)),
                   new SqlParameter("@P_StartDate", Convert.ToDateTime(startDate)),
                   new SqlParameter("@P_EndDate", Convert.ToDateTime(endDate)) };

            var res = SqlHelper.ExecuteProcedureReturnString(connString, "Get_OfficialTaskUserAvailability", param);
            return Convert.ToBoolean(res);
        }

        internal OfficialTaskWorkflowModel PatchOfficialTask(string connString, int id, JsonPatchDocument<OfficialTaskPutModel> value)
        {
            var result = GetPatchOfficialTaskByID(connString, id);
            value.ApplyTo(result);
            var res = PutOfficialTask(connString, result);

            SqlParameter[] param = { new SqlParameter("@P_Department", 9), new SqlParameter("@P_GetHead", 1) };

            res.HRHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), param);
            return res;
        }
    }
}