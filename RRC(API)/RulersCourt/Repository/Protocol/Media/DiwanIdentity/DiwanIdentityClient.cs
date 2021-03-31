using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.DiwanIdentity;
using RulersCourt.Models.Protocol.Media.DiwanIdentity;
using RulersCourt.Translators;
using RulersCourt.Translators.Protocol.Media.DiwanIdentity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Protocol.Media.DiwanIdentity
{
    public class DiwanIdentityClient
    {
        public DiwanIdentityGetModel GetDiwanIdentityByID(string connString, int diwanIdentityID, int userID, string lang)
        {
            DiwanIdentityGetModel diwanIdentityDetails = new DiwanIdentityGetModel();
            SqlParameter[] param = {
                new SqlParameter("@P_DiwanIdentityID", diwanIdentityID) };
            if (diwanIdentityID != 0)
            {
                diwanIdentityDetails = SqlHelper.ExecuteProcedureReturnData<List<DiwanIdentityGetModel>>(connString, "Get_DiwanIdentityByID", r => r.TranslateAsDiwanIdentityList(), param).FirstOrDefault();

                diwanIdentityDetails.CommunicationHistory = new DiwanIdentityCommunicationHistoryClient().DiwanIdentityCommunicationHistoryByDiwanIdentityID(connString, diwanIdentityID, lang);
            }

            Parallel.Invoke(
              () => diwanIdentityDetails.M_OrganizationList = GetM_Organisation(connString, lang),
              () => diwanIdentityDetails.M_LookupsList = GetM_Lookups(connString, lang),
              () => diwanIdentityDetails.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));

            SqlParameter[] getApproverparam = {
                    new SqlParameter("@P_ReferenceNumber", diwanIdentityDetails.ReferenceNumber),
                    new SqlParameter("@P_UserID", userID) };
            diwanIdentityDetails.CurrentApprover = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_DiwanIdentityByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam);

            SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", diwanIdentityDetails.ReferenceNumber),
                    new SqlParameter("@P_Method", 0) };
            diwanIdentityDetails.AssigneeId = SqlHelper.ExecuteProcedureReturnData<List<CurrentMediaAssigneeModel>>(connString, "Get_DiwanIdentityByAssigneeandMediaId", r => r.TranslateAsCurrentMediaAssigneeList(), getAssignparam);

            SqlParameter[] getMediaUserparam = {
                    new SqlParameter("@P_ReferenceNumber", diwanIdentityDetails.ReferenceNumber),
                    new SqlParameter("@P_Method", 1) };
            diwanIdentityDetails.MediaHeadUsedId = SqlHelper.ExecuteProcedureReturnData<List<CurrentMediaHeadModel>>(connString, "Get_DiwanIdentityByAssigneeandMediaId", r => r.TranslateAsCurrentMediaHeadList(), getMediaUserparam);
            userID = diwanIdentityDetails.CreatedBy.GetValueOrDefault();
            return diwanIdentityDetails;
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
                new SqlParameter("@P_Type", "DiwanIdentity"), new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public DiwanIdentityWorkflowModel PostDiwanIdentity(string connString, DiwanIdentityPostModel diwanIdentity)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_SourceOU", diwanIdentity.SourceOU),
                new SqlParameter("@P_SourceName", diwanIdentity.SourceName),
                new SqlParameter("@P_PurposeofUse", diwanIdentity.PurposeofUse),
                new SqlParameter("@P_CreatedBy", diwanIdentity.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", diwanIdentity.CreatedDateTime),
                new SqlParameter("@P_Action", diwanIdentity.Action),
                new SqlParameter("@P_ApproverDepartmentID", diwanIdentity.ApproverDepartmentID),
                new SqlParameter("@P_Comment", diwanIdentity.Comments)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<DiwanIdentityWorkflowModel>(connString, "Save_DiwanIdentity", r => r.TranslateAsDiwanIdentitySaveResponseList(), param);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "DiwanIdentity"),
                new SqlParameter("@P_Action", diwanIdentity.Action),
                new SqlParameter("@P_ApproverId", diwanIdentity.ApproverID) };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.ApproverID = diwanIdentity.ApproverID;
            result.ApproverDepartmentID = diwanIdentity.ApproverDepartmentID;
            result.FromID = diwanIdentity.CreatedBy ?? default(int);
            result.Action = diwanIdentity.Action;

            SqlParameter[] parama = { new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber), new SqlParameter("@P_UserID", diwanIdentity.CreatedBy) };

            result.MediaHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_MediaApprovers", r => r.TranslateAsUserList(), parama);

            return result;
        }

        public DiwanIdentityWorkflowModel PutDiwanIdentity(string connString, DiwanIdentityPutModel diwanIdentity)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Date", diwanIdentity.Date),
                new SqlParameter("@P_DiwanIdentityID", diwanIdentity.DiwanIdentityID),
                new SqlParameter("@P_SourceOU", diwanIdentity.SourceOU),
                new SqlParameter("@P_SourceName", diwanIdentity.SourceName),
                new SqlParameter("@P_PurposeofUse", diwanIdentity.PurposeofUse),
                new SqlParameter("@P_UpdatedBy", diwanIdentity.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", diwanIdentity.UpdatedDateTime),
                new SqlParameter("@P_Action", diwanIdentity.Action),
                new SqlParameter("@P_ApproverDepartmentID", diwanIdentity.ApproverDepartmentID),
                new SqlParameter("@P_Comment", diwanIdentity.Comments),
                new SqlParameter("@P_DeleteFlag", diwanIdentity.DeleteFlag)
            };
            var result = SqlHelper.ExecuteProcedureReturnData<DiwanIdentityWorkflowModel>(connString, "Save_DiwanIdentity", r => r.TranslateAsDiwanIdentitySaveResponseList(), param);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "DiwanIdentity"),
                new SqlParameter("@P_Action", diwanIdentity.Action),
                new SqlParameter("@P_ApproverId", diwanIdentity.ApproverID)
            };
            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.ApproverID = diwanIdentity.ApproverID;
            result.ApproverDepartmentID = diwanIdentity.ApproverDepartmentID;
            result.FromID = diwanIdentity.UpdatedBy ?? default(int);
            result.Action = diwanIdentity.Action;

            SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };
            result.AssigneeID = diwanIdentity.AssigneeID;

            SqlParameter[] parama = { new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };
            result.MediaHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_MediaApprovers", r => r.TranslateAsUserList(), parama);

            SqlParameter[] param1 = {
                new SqlParameter("@P_DiwanIdentityID", result.DiwanIdentityID) };

            result.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<DiwanIdentityGetModel>>(connString, "Get_DiwanIdentityByID", r => r.TranslateAsDiwanIdentityList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };

            result.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_DiwanIdentityByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return result;
        }

        public DiwanIdentityPutModel GetPatchDiwanIdentityByID(string connString, int diwanIdentityID)
        {
            DiwanIdentityPutModel diwanIdentityDetails = new DiwanIdentityPutModel();
            SqlParameter[] param = {
                new SqlParameter("@P_DiwanIdentityID", diwanIdentityID) };
            if (diwanIdentityID != 0)
            {
                diwanIdentityDetails = SqlHelper.ExecuteProcedureReturnData<List<DiwanIdentityPutModel>>(connString, "Get_DiwanIdentityByID", r => r.TranslateAsPutDiwanIdentityList(), param).FirstOrDefault();
            }

            return diwanIdentityDetails;
        }

        public int SaveCommunicationChat(string connString, DiwanIdentityCommunicationHistory chat)
        {
            SqlParameter[] param =
                    { new SqlParameter("@P_CommunicationID", chat.CommunicationID),
                    new SqlParameter("@P_DiwanIdentityID", chat.DiwanIdentityID),
                    new SqlParameter("@P_Message", chat.Message),
                    new SqlParameter("@P_ParentCommunicationID", chat.ParentCommunicationID),
                    new SqlParameter("@P_CreatedBy", chat.CreatedBy),
                    new SqlParameter("@P_CreatedDateTime", chat.CreatedDateTime)
                    };
            return int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Save_DiwanIdentityCommunicationHistory", param));
        }

        internal DiwanIdentityWorkflowModel PatchDiwanIdentity(string connString, int id, JsonPatchDocument<DiwanIdentityPutModel> value)
        {
            var result = GetPatchDiwanIdentityByID(connString, id);
            value.ApplyTo(result);
            var res = PutDiwanIdentity(connString, result);
            if (result.Action == "Escalate" || result.Action == "Redirect")
            {
                res.ApproverID = result.ApproverID;
            }

            SqlParameter[] param = { new SqlParameter("@P_ReferenceNumber", res.ReferenceNumber) };
            res.AssigneeID = result.AssigneeID;
            res.MediaHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_MediaApprovers", r => r.TranslateAsUserList(), param);

            SqlParameter[] param1 = {
                new SqlParameter("@P_DiwanIdentityID", result.DiwanIdentityID) };

            res.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<DiwanIdentityGetModel>>(connString, "Get_DiwanIdentityByID", r => r.TranslateAsDiwanIdentityList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", res.ReferenceNumber) };

            res.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_DiwanIdentityByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return res;
        }
    }
}
