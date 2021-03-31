using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.LeaveRequest;
using RulersCourt.Translators;
using RulersCourt.Translators.LeaveRequest;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Leave
{
    public class LeaveClient
    {
        public LeaveGetModel GetLeaveByID(string connString, int leaveID, int userID, string lang)
        {
            LeaveGetModel leaveDetails = new LeaveGetModel();
            SqlParameter[] param = {
                new SqlParameter("@P_LeaveID", leaveID)
            };

            if (leaveID != 0)
            {
                leaveDetails = SqlHelper.ExecuteProcedureReturnData<List<LeaveGetModel>>(connString, "Get_LeaveByID", r => r.TranslateAsLeaveList(), param).FirstOrDefault();
                leaveDetails.Attachments = new LeaveAttachmentClient().GetAttachmentById(connString, leaveDetails.LeaveID, "Leave");
                leaveDetails.CommunicationHistory = new LeaveCommunicationHistoryClient().LeaveCommunicationHistoryByLeaveID(connString, leaveID, lang);
            }

            Parallel.Invoke(
              () => leaveDetails.M_OrganizationList = GetM_Organisation(connString, lang),
              () => leaveDetails.M_LookupsList = GetM_Lookups(connString, lang),
              () => leaveDetails.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));

            SqlParameter[] getApproverparam = {
                    new SqlParameter("@P_ReferenceNumber", leaveDetails.ReferenceNumber),
                    new SqlParameter("@P_UserID", userID)
            };

            leaveDetails.CurrentApprover = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_LeaveByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam);

            SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", leaveDetails.ReferenceNumber)
            };

            leaveDetails.AssigneeID = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_LeaveAssigneeID", getAssignparam));

            userID = leaveDetails.CreatedBy.GetValueOrDefault();
            return leaveDetails;
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
                new SqlParameter("@P_Type", "Leave"),
                new SqlParameter("@P_Language", lang)
            };

            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public MemoListModel GetMemo(string connString, int pageNumber, int pageSize, string type, string username, string status, string sourceOU, string stringDestinationOU, string privated, string priority, DateTime? dateFrom, DateTime? dateTo, string smartSearch, string lang)
        {
            EnumMemo e = (EnumMemo)Enum.ToObject(typeof(EnumMemo), Convert.ToInt32(type));

            MemoListModel list = new MemoListModel();

            SqlParameter[] param = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_Type", e),
                new SqlParameter("@P_Username", username),
                new SqlParameter("@P_Method", 0),
                new SqlParameter("@P_Status", status),
                new SqlParameter("@P_SourceOU", sourceOU),
                new SqlParameter("@P_DestinationOU", stringDestinationOU),
                new SqlParameter("@P_Private", privated),
                new SqlParameter("@P_Priority", priority),
                new SqlParameter("@P_DateFrom", dateFrom),
                new SqlParameter("@P_DateTo", dateTo),
                new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<MemoGetModel>>(connString, "Get_MemoList", r => r.TranslateAsMemoList(), param);

            SqlParameter[] parama = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_Type", e),
                new SqlParameter("@P_Username", username),
                new SqlParameter("@P_Method", 1),
                new SqlParameter("@P_Status", status),
                new SqlParameter("@P_SourceOU", sourceOU),
                new SqlParameter("@P_DestinationOU", stringDestinationOU),
                new SqlParameter("@P_Private", privated),
                new SqlParameter("@P_Priority", priority),
                new SqlParameter("@P_DateFrom", dateFrom),
                new SqlParameter("@P_DateTo", dateTo),
                new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_MemoList", parama);

            Parallel.Invoke(
                  () => list.OrganizationList = GetM_Organisation(connString, lang),
                  () => list.LookupsList = GetM_Lookups(connString, lang));
            return list;
        }

        public LeaveWorkflowModel PostLeave(string connString, LeavePostModel leave)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_SourceOU", leave.SourceOU),
                new SqlParameter("@P_SourceName", leave.SourceName),
                new SqlParameter("@P_LeaveType", leave.LeaveType),
                new SqlParameter("@P_Reason", leave.Reason),
                new SqlParameter("@P_DOANameID", leave.DOANameID),
                new SqlParameter("@P_DOADepartmentID", leave.DOADepartmentID),
                new SqlParameter("@P_StartDate", leave.StartDate),
                new SqlParameter("@P_EndDate", leave.EndDate),
                new SqlParameter("@P_BalanceLeave", leave.BalanceLeave),
                new SqlParameter("@P_CreatedBy", leave.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", leave.CreatedDateTime),
                new SqlParameter("@P_Action", leave.Action),
                new SqlParameter("@P_ApproverDepartmentID", leave.ApproverDepartmentID),
                new SqlParameter("@P_LeaveTypeOther", leave.LeaveTypeOther),
                new SqlParameter("@P_Comment", leave.Comments)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<LeaveWorkflowModel>(connString, "Save_Leave", r => r.TranslateAsLeaveSaveResponseList(), param);

            if (leave.Attachments != null)
                new LeaveAttachmentClient().PostAttachments(connString, "Leave", leave.Attachments, result.LeaveID);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Leave"),
                new SqlParameter("@P_Action", leave.Action)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));
            result.ApproverID = leave.ApproverID;
            result.ApproverDepartmentID = leave.ApproverDepartmentID;
            result.FromID = leave.CreatedBy ?? default(int);
            result.Action = leave.Action;
            return result;
        }

        public LeaveWorkflowModel PutLeave(string connString, LeavePutModel leave)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_LeaveID", leave.LeaveID),
                new SqlParameter("@P_SourceOU", leave.SourceOU),
                new SqlParameter("@P_SourceName", leave.SourceName),
                new SqlParameter("@P_LeaveType", leave.LeaveType),
                new SqlParameter("@P_Reason", leave.Reason),
                new SqlParameter("@P_DOANameID", leave.DOANameID),
                new SqlParameter("@P_DOADepartmentID", leave.DOADepartmentID),
                new SqlParameter("@P_StartDate", leave.StartDate),
                new SqlParameter("@P_EndDate", leave.EndDate),
                new SqlParameter("@P_BalanceLeave", leave.BalanceLeave),
                new SqlParameter("@P_UpdatedBy", leave.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", leave.UpdatedDateTime),
                new SqlParameter("@P_Action", leave.Action),
                new SqlParameter("@P_Comment", leave.Comments),
                new SqlParameter("@P_ApproverDepartmentID", leave.ApproverDepartmentID),
                new SqlParameter("@P_LeaveTypeOther", leave.LeaveTypeOther),
                new SqlParameter("@P_DeleteFlag", leave.DeleteFlag)
            };
            var result = SqlHelper.ExecuteProcedureReturnData<LeaveWorkflowModel>(connString, "Save_Leave", r => r.TranslateAsLeaveSaveResponseList(), param);

            if (leave.Attachments != null && leave.Attachments.Count > 0)
                new LeaveAttachmentClient().PostAttachments(connString, "Leave", leave.Attachments, result.LeaveID);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Leave"),
                new SqlParameter("@P_Action", leave.Action)
            };
            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.ApproverID = leave.ApproverID;
            result.ApproverDepartmentID = leave.ApproverDepartmentID;
            result.FromID = leave.UpdatedBy ?? default(int);
            result.Action = leave.Action;
            SqlParameter[] param1 = {
                new SqlParameter("@P_LeaveID", result.LeaveID)
            };

            result.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<LeaveGetModel>>(connString, "Get_LeaveByID", r => r.TranslateAsLeaveList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber)
            };

            result.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_LeaveByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return result;
        }

        public string DeleteLeave(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_LeaveID", id)
             };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_LeaveByID", param);
        }

        public LeavePutModel GetPatchLeaveByID(string connString, int leaveID)
        {
            LeavePutModel leaveDetails = new LeavePutModel();
            SqlParameter[] param = {
                new SqlParameter("@P_LeaveID", leaveID)
            };
            if (leaveID != 0)
            {
                leaveDetails = SqlHelper.ExecuteProcedureReturnData<List<LeavePutModel>>(connString, "Get_LeaveByID", r => r.TranslateAsPutLeaveList(), param).FirstOrDefault();
                leaveDetails.Attachments = new LeaveAttachmentClient().GetAttachmentById(connString, leaveDetails.LeaveID, "Leave");
            }

            return leaveDetails;
        }

        public int SaveCommunicationChat(string connString, LeaveCommunicationHistory chat)
        {
            SqlParameter[] param = {
                    new SqlParameter("@P_CommunicationID", chat.CommunicationID),
                    new SqlParameter("@P_LeaveID", chat.LeaveID),
                    new SqlParameter("@P_Message", chat.Message),
                    new SqlParameter("@P_ParentCommunicationID", chat.ParentCommunicationID),
                    new SqlParameter("@P_CreatedBy", chat.CreatedBy),
                    new SqlParameter("@P_CreatedDateTime", chat.CreatedDateTime)
            };
            return int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Save_LeaveCommunicationHistory", param));
        }

        internal LeaveWorkflowModel PatchLeave(string connString, int id, JsonPatchDocument<LeavePutModel> value)
        {
            var result = GetPatchLeaveByID(connString, id);
            value.ApplyTo(result);
            var res = PutLeave(connString, result);
            if (result.Action == "Escalate" || result.Action == "Redirect")
            {
                res.ApproverID = result.ApproverID;
            }

            SqlParameter[] param = { new SqlParameter("@P_Department", 9), new SqlParameter("@P_GetHead", 1) };
            res.AssigneeID = result.AssigneeID;
            res.HRHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), param);

            SqlParameter[] param1 = {
                new SqlParameter("@P_LeaveID", result.LeaveID)
            };

            SqlParameter[] param2 = {
                new SqlParameter("@P_LeaveID", result.LeaveID)
            };

            res.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<LeaveGetModel>>(connString, "Get_LeaveByID", r => r.TranslateAsLeaveList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", res.ReferenceNumber)
            };

            res.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_LeaveByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            res.HRManagerUserID = SqlHelper.ExecuteProcedureReturnData<List<LeaveGetModel>>(connString, "Get_LeaveByID", r => r.TranslateAsLeaveList(), param2).FirstOrDefault().HRManagerUserID;

            return res;
        }
    }
}
