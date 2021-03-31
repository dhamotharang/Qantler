using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.OfficalTaskCompensation.Compensation;
using RulersCourt.Models.OfficialTaskCompensation.Compensation;
using RulersCourt.Translators;
using RulersCourt.Translators.OfficalTaskCompensation.Compensation;
using RulersCourt.Translators.OfficialTaskCompensation.Compensation;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.OfficalTaskCompensation.Compensation
{
    public class CompensationClient
    {
        public CompensationGetModel GetCompensationByID(string connString, int compensationID, int userID, string lang)
        {
            CompensationGetModel compensationDetails = new CompensationGetModel();
            SqlParameter[] param = {
                new SqlParameter("@P_CompensationID", compensationID) };
            if (compensationID != 0)
            {
                compensationDetails = SqlHelper.ExecuteProcedureReturnData<List<CompensationGetModel>>(connString, "Get_CompensationByID", r => r.TranslateAsCompensationList(), param).FirstOrDefault();
                compensationDetails.CommunicationHistory = new CompensationCommunicationHistoryClient().CompensationCommunicationHistoryByCompensationID(connString, compensationID, lang);
            }

            Parallel.Invoke(
              () => compensationDetails.M_OrganizationList = GetM_Organisation(connString, lang),
              () => compensationDetails.M_LookupsList = GetM_Lookups(connString, lang));

            SqlParameter[] getApproverparam = {
                    new SqlParameter("@P_ReferenceNumber", compensationDetails.ReferenceNumber),
                    new SqlParameter("@P_UserID", userID) };

            compensationDetails.CurrentApprover = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_CompensationByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam);
            SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", compensationDetails.ReferenceNumber) };

            compensationDetails.AssigneeID = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_CompensationAssigneeID", getAssignparam));

            userID = compensationDetails.CreatedBy.GetValueOrDefault();
            SqlParameter[] getEmployee =
            {
                new SqlParameter("@P_CompensationID", compensationID),
                new SqlParameter("@P_Language", lang)
            };
            compensationDetails.EmployeeNameID = SqlHelper.ExecuteProcedureReturnData<List<CompensationEmployeeGetModel>>(connString, "Get_CompensationEmployeeDetails", r => r.TranslateAsEmployeeNameList(), getEmployee);
            return compensationDetails;
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
                new SqlParameter("@P_Type", "Compensation"), new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public CompensationWorkflowModel PostCompensation(string connString, CompensationPostModel compensation)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_OfficialTaskID", compensation.OfficialTaskID),
                new SqlParameter("@P_SourceOU", compensation.SourceOU),
                new SqlParameter("@P_SourceName", compensation.SourceName),
                new SqlParameter("@P_OfficialTaskType", compensation.OfficialTaskType),
                new SqlParameter("@P_StartDate", compensation.StartDate),
                new SqlParameter("@P_EndDate", compensation.EndDate),
                new SqlParameter("@P_NumberofDays", compensation.NumberofDays),
                new SqlParameter("@P_CompensationDescription", compensation.CompensationDescription),
                new SqlParameter("@P_NeedCompensation", compensation.NeedCompensation),
                new SqlParameter("@P_CreatedBy", compensation.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", compensation.CreatedDateTime),
                new SqlParameter("@P_Action", compensation.Action),
                new SqlParameter("@P_Comments", compensation.Comments)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<CompensationWorkflowModel>(connString, "Save_Compensation", r => r.TranslateAsCompensationSaveResponseList(), param);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Compensation"),
                new SqlParameter("@P_Action", compensation.Action) };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.ApproverID = compensation.ApproverID;
            result.ApproverDepartmentID = compensation.ApproverDepartmentID;
            result.FromID = compensation.CreatedBy ?? default(int);
            result.Action = compensation.Action;
            SqlParameter[] paramGetUser = {
                new SqlParameter("@P_CompensationID", compensation.OfficialTaskID)
              };

            result.TaskCreatorID = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_OfficialTaskCreator", paramGetUser));
            return result;
        }

        public CompensationWorkflowModel PutCompensation(string connString, CompensationPutModel compensation)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_CompensationID", compensation.CompensationID),
                new SqlParameter("@P_SourceOU", compensation.SourceOU),
                new SqlParameter("@P_SourceName", compensation.SourceName),
                new SqlParameter("@P_OfficialTaskType", compensation.OfficialTaskType),
                new SqlParameter("@P_StartDate", compensation.StartDate),
                new SqlParameter("@P_EndDate", compensation.EndDate),
                new SqlParameter("@P_NumberofDays", compensation.NumberofDays),
                new SqlParameter("@P_CompensationDescription", compensation.CompensationDescription),
                new SqlParameter("@P_NeedCompensation", compensation.NeedCompensation),
                new SqlParameter("@P_UpdatedBy", compensation.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", compensation.UpdatedDateTime),
                new SqlParameter("@P_Action", compensation.Action),
                new SqlParameter("@P_Comments", compensation.Comments),
                new SqlParameter("@P_DeleteFlag", compensation.DeleteFlag)
            };
            var result = SqlHelper.ExecuteProcedureReturnData<CompensationWorkflowModel>(connString, "Save_Compensation", r => r.TranslateAsCompensationSaveResponseList(), param);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Compensation"),
                new SqlParameter("@P_Action", compensation.Action) };
            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.ApproverID = compensation.ApproverID;
            result.ApproverDepartmentID = compensation.ApproverDepartmentID;
            result.FromID = compensation.UpdatedBy ?? default(int);
            result.Action = compensation.Action;
            SqlParameter[] paramGetUser = {
                new SqlParameter("@P_CompensationID", compensation.CompensationID),
                new SqlParameter("@P_Type", 1)
              };

            result.TaskCreatorID = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_OfficialTaskCreator", paramGetUser));

            SqlParameter[] param1 = {
                new SqlParameter("@P_CompensationID", result.CompensationID) };

            result.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<CompensationGetModel>>(connString, "Get_CompensationByID", r => r.TranslateAsCompensationList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };

            result.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_CompensationByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return result;
        }

        public CompensationPutModel GetPatchCompensationByID(string connString, int compensationID)
        {
            CompensationPutModel compensationDetails = new CompensationPutModel();
            SqlParameter[] param = {
                new SqlParameter("@P_CompensationID", compensationID) };
            if (compensationID != 0)
            {
                compensationDetails = SqlHelper.ExecuteProcedureReturnData<List<CompensationPutModel>>(connString, "Get_CompensationByID", r => r.TranslateAsPutCompensationList(), param).FirstOrDefault();
            }

            return compensationDetails;
        }

        public int SaveCommunicationChat(string connString, CompensationCommunicationHistoryModel chat)
        {
            SqlParameter[] param =
                    { new SqlParameter("@P_CommunicationID", chat.CommunicationID),
                    new SqlParameter("@P_CompensationID", chat.CompensationID),
                    new SqlParameter("@P_Message", chat.Message),
                    new SqlParameter("@P_ParentCommunicationID", chat.ParentCommunicationID),
                    new SqlParameter("@P_CreatedBy", chat.CreatedBy),
                    new SqlParameter("@P_CreatedDateTime", chat.CreatedDateTime)
                    };
            return int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Save_CompensationCommunicationHistory", param));
        }

        public CompensationPreviewModel GetCompensationPreview(string connString, int compensationID, string lang)
        {
            CompensationPreviewModel compensationDetails = new CompensationPreviewModel();
            SqlParameter[] param = {
                new SqlParameter("@P_CompensationID", compensationID),
                new SqlParameter("@P_Language", lang),
            };
            if (compensationID != 0)
            {
                compensationDetails = SqlHelper.ExecuteProcedureReturnData<CompensationPreviewModel>(connString, "Get_CompensationPreview", r => r.TranslateAsCompensationPreviewModel(), param);
            }

            return compensationDetails;
        }

        internal CompensationWorkflowModel PatchCompensation(string connString, int id, JsonPatchDocument<CompensationPutModel> value)
        {
            var result = GetPatchCompensationByID(connString, id);
            value.ApplyTo(result);
            var res = PutCompensation(connString, result);
            if (result.Action == "Escalate" || result.Action == "Redirect")
            {
                res.ApproverID = result.ApproverID;
            }

            SqlParameter[] param = { new SqlParameter("@P_Department", 9), new SqlParameter("@P_GetHead", 1) };
            res.AssigneeID = result.AssigneeID;
            res.HRHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), param);

            SqlParameter[] param1 = {
                new SqlParameter("@P_CompensationID", result.CompensationID) };

            res.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<CompensationGetModel>>(connString, "Get_CompensationByID", r => r.TranslateAsCompensationList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", res.ReferenceNumber) };

            res.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_CompensationByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            SqlParameter[] paramHR = {
                new SqlParameter("@P_CompensationID", id) };

            res.HRManagerUserID = SqlHelper.ExecuteProcedureReturnData<List<CompensationGetModel>>(connString, "Get_CompensationByID", r => r.TranslateAsCompensationList(), paramHR).FirstOrDefault().HRManagerUserID;

            return res;
        }
    }
}