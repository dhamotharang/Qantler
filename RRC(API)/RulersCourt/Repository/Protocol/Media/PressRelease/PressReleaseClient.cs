using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.PressRelease;
using RulersCourt.Models.Protocol.Media.PressRelease;
using RulersCourt.Translators;
using RulersCourt.Translators.Protocol.Media.PressRelease;
using RulersCourt.Translators.Protocol.PressRelease;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Protocol.Media.PressRelease
{
    public class PressReleaseClient
    {
        public PressReleaseResponseModel PostPressRelease(string connString, PressReleasePostModel pressRelease)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Date", pressRelease.Date),
                new SqlParameter("@P_SourceOU", pressRelease.SourceOU),
                new SqlParameter("@P_SourceName", pressRelease.SourceName),
                new SqlParameter("@P_EventName", pressRelease.EventName),
                new SqlParameter("@P_Subject", pressRelease.Subject),
                new SqlParameter("@P_Type", pressRelease.Type),
                new SqlParameter("@P_Location", pressRelease.Location),
                new SqlParameter("@P_AttendedBy", pressRelease.AttendedBy),
                new SqlParameter("@P_Partners", pressRelease.Partners),
                new SqlParameter("@P_CreatedBy", pressRelease.CreatedBy),
                new SqlParameter("@P_ApproverDepartmentID", pressRelease.ApproverDepartmentID),
                new SqlParameter("@P_CreatedDateTime", pressRelease.CreatedDateTime),
                new SqlParameter("@P_Action", pressRelease.Action),
                new SqlParameter("@P_Comment", pressRelease.Comments)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<PressReleaseResponseModel>(connString, "Save_MediaNewPressReleaseRequest", r => r.TranslateAsPressReleaseSaveResponseList(), param);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "MediaNewPressReleaseRequest"),
                new SqlParameter("@P_Action", pressRelease.Action),
                new SqlParameter("@P_ApproverId", pressRelease.ApproverID) };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.ApproverID = pressRelease.ApproverID;
            result.ApproverDepartmentID = pressRelease.ApproverDepartmentID;
            result.FromID = pressRelease.CreatedBy ?? default(int);
            result.Action = pressRelease.Action;

            SqlParameter[] parama = { new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber), new SqlParameter("@P_UserID", pressRelease.CreatedBy) };
            result.MediaHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_MediaApprovers", r => r.TranslateAsUserList(), parama);

            return result;
        }

        public PressReleaseResponseModel PutPressRelease(string connString, PressReleasePutModel pressRelease)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_PressReleaseID", pressRelease.PressReleaseID),
                new SqlParameter("@P_Date", pressRelease.Date),
                new SqlParameter("@P_SourceOU", pressRelease.SourceOU),
                new SqlParameter("@P_SourceName", pressRelease.SourceName),
                new SqlParameter("@P_Subject", pressRelease.Subject),
                new SqlParameter("@P_Type", pressRelease.Type),
                new SqlParameter("@P_EventName", pressRelease.EventName),
                new SqlParameter("@P_AttendedBy", pressRelease.AttendedBy),
                new SqlParameter("@P_Partners", pressRelease.Partners),
                new SqlParameter("@P_Location", pressRelease.Location),
                new SqlParameter("@P_UpdatedBy", pressRelease.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", pressRelease.UpdatedDateTime),
                new SqlParameter("@P_Action", pressRelease.Action),
                new SqlParameter("@P_ApproverDepartmentID", pressRelease.ApproverDepartmentID),
                new SqlParameter("@P_Comment", pressRelease.Comments)
            };
            var result = SqlHelper.ExecuteProcedureReturnData<PressReleaseResponseModel>(connString, "Save_MediaNewPressReleaseRequest", r => r.TranslateAsPressReleaseSaveResponseList(), param);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "MediaNewPressReleaseRequest"),
                new SqlParameter("@P_Action", pressRelease.Action),
                new SqlParameter("@P_ApproverId", pressRelease.ApproverID)
            };
            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.ApproverID = pressRelease.ApproverID;
            result.ApproverDepartmentID = pressRelease.ApproverDepartmentID;
            result.FromID = pressRelease.UpdatedBy ?? default(int);
            result.Action = pressRelease.Action;

            SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };
            result.AssigneeID = pressRelease.AssigneeID;

            SqlParameter[] parama = { new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };
            result.MediaHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_MediaApprovers", r => r.TranslateAsUserList(), parama);
            SqlParameter[] param1 = {
            new SqlParameter("@P_PressReleaseID", result.PressReleaseID) };

            result.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<PressReleaseGetModel>>(connString, "Get_MediaNewPressReleaseRequestByID", r => r.TranslateAsPressReleaseList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };

            result.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_MediaNewPressReleaseRequestByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;
            return result;
        }

        public string DeletePressRelease(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_PressReleaseID", id)
             };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_MediaNewPressReleaseRequestByID", param);
        }

        public PressReleaseGetModel GetPressReleaseByID(string connstring, int id, int userID, string lang)
        {
            PressReleaseGetModel pressRelease = new PressReleaseGetModel();
            SqlParameter[] param = {
                new SqlParameter("@P_PressReleaseID", id) };
            if (id != 0)
            {
                pressRelease = SqlHelper.ExecuteProcedureReturnData<List<PressReleaseGetModel>>(connstring, "Get_MediaNewPressReleaseRequestByID", r => r.TranslateAsPressReleaseList(), param).FirstOrDefault();

                pressRelease.CommunicationHistory = new PressReleaseCommunicationHistoryClient().PressReleaseCommunicationHistoryByPhotographerID(connstring, id, lang);
            }

            Parallel.Invoke(
              () => pressRelease.M_OrganizationList = GetM_Organisation(connstring, lang),
              () => pressRelease.M_LookupsList = GetM_Lookups(connstring, lang),
              () => pressRelease.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connstring, lang));

            SqlParameter[] getApproverparam = {
                    new SqlParameter("@P_ReferenceNumber", pressRelease.ReferenceNumber),
                    new SqlParameter("@P_UserID", userID) };
            pressRelease.CurrentApprover = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connstring, "Get_MediaNewPressReleaseRequestByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam);

            SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", pressRelease.ReferenceNumber),
                    new SqlParameter("@P_Method", 0) };

            pressRelease.AssigneeID = SqlHelper.ExecuteProcedureReturnData<List<CurrentMediaAssigneeModel>>(connstring, "Get_MediaNewPressReleaseByAssigneeandMediaId", r => r.TranslateAsCurrentMediaAssigneeList(), getAssignparam);
            SqlParameter[] getMediaUserparam = {
                    new SqlParameter("@P_ReferenceNumber", pressRelease.ReferenceNumber),
                    new SqlParameter("@P_Method", 1) };
            pressRelease.MediaHeadUsedId = SqlHelper.ExecuteProcedureReturnData<List<CurrentMediaHeadModel>>(connstring, "Get_MediaNewPressReleaseByAssigneeandMediaId", r => r.TranslateAsCurrentMediaHeadList(), getMediaUserparam);
            userID = pressRelease.CreatedBy.GetValueOrDefault();
            return pressRelease;
        }

        public int SaveCommunicationChat(string connString, PressReleaseCommunicationHistory chat)
        {
            SqlParameter[] param =
                    { new SqlParameter("@P_CommunicationID", chat.CommunicationID),
                    new SqlParameter("@P_PressReleaseID", chat.PressReleaseID),
                    new SqlParameter("@P_Message", chat.Message),
                    new SqlParameter("@P_ParentCommunicationID", chat.ParentCommunicationID),
                    new SqlParameter("@P_Action", chat.Action),
                    new SqlParameter("@P_CreatedBy", chat.CreatedBy),
                    new SqlParameter("@P_CreatedDateTime", chat.CreatedDateTime)
                    };
            return int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Save_MediaNewPressReleaseRequestCommunicationHistory", param));
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
                new SqlParameter("@P_Type", "MediaNewPressReleaseRequest"), new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public PressReleasePutModel GetPatchPressReleaseByID(string connString, int pressReleaseID)
        {
            PressReleasePutModel pressRelease = new PressReleasePutModel();
            SqlParameter[] param = {
                new SqlParameter("@P_PressReleaseID", pressReleaseID) };
            if (pressReleaseID != 0)
            {
                pressRelease = SqlHelper.ExecuteProcedureReturnData<List<PressReleasePutModel>>(connString, "Get_MediaNewPressReleaseRequestByID", r => r.TranslateAsPutPressReleasesList(), param).FirstOrDefault();
            }

            return pressRelease;
        }

        internal PressReleaseResponseModel PatchPressRelease(string connString, int id, JsonPatchDocument<PressReleasePutModel> value)
        {
            var result = GetPatchPressReleaseByID(connString, id);
            value.ApplyTo(result);
            if ((result.Action == "Approve") || result.Action == "Reject" || result.Action == "Returnforinfo")
                result.ApproverID = result.UpdatedBy;
            var res = PutPressRelease(connString, result);
            if (result.Action == "Escalate" || result.Action == "Redirect")
            {
                res.ApproverID = result.ApproverID;
            }

            SqlParameter[] param = { new SqlParameter("@P_ReferenceNumber", res.ReferenceNumber) };
            res.AssigneeID = result.AssigneeID;
            res.MediaHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_MediaApprovers", r => r.TranslateAsUserList(), param);
            SqlParameter[] param1 = {
            new SqlParameter("@P_PressReleaseID", result.PressReleaseID) };

            res.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<PressReleaseGetModel>>(connString, "Get_MediaNewPressReleaseRequestByID", r => r.TranslateAsPressReleaseList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", res.ReferenceNumber) };

            res.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_MediaNewPressReleaseRequestByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return res;
        }
    }
}
