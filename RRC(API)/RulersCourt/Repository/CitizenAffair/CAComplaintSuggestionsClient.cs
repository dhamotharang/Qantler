using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.CitizenAffair;
using RulersCourt.Models.CitizenAffairComplaintSuggestions;
using RulersCourt.Translators;
using RulersCourt.Translators.CAComplaintSuggestions;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.CAComplaintSuggestions
{
    public class CAComplaintSuggestionsClient
    {
        public CAComplaintSuggestionsGetModel GetCAComplaintSuggestionsByID(string connString, int caComplaintSuggestionsID, int userID, string lang)
        {
            CAComplaintSuggestionsGetModel caComplaintSuggestionsDetails = new CAComplaintSuggestionsGetModel();

            SqlParameter[] param = {
                new SqlParameter("@P_CAComplaintSuggestionsID", caComplaintSuggestionsID)
            };

            if (caComplaintSuggestionsID != 0)
            {
                caComplaintSuggestionsDetails = SqlHelper.ExecuteProcedureReturnData<List<CAComplaintSuggestionsGetModel>>(connString, "Get_CAComplaintSuggestionsByID", r => r.TranslateAsCAComplaintSuggestionsList(), param).FirstOrDefault();
                caComplaintSuggestionsDetails.HistoryLog = new CAComplaintSuggestionsHistoryLogClient().CAComplaintSuggestionsHistoryLogByCAComplaintSuggestionsID(connString, caComplaintSuggestionsID, lang);

                SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", caComplaintSuggestionsDetails.ReferenceNumber),
                    new SqlParameter("@P_Method", 0)
                };
                caComplaintSuggestionsDetails.AssigneeId = SqlHelper.ExecuteProcedureReturnData<List<CurrentCitizenAffairAssigneeModel>>(connString, "Get_CAComplaintSuggestionsByAssigneeandHRId", r => r.TranslateAsCurrentCitizenAffairAssigneeList(), getAssignparam);

                SqlParameter[] getCAUserparam = {
                    new SqlParameter("@P_ReferenceNumber", caComplaintSuggestionsDetails.ReferenceNumber),
                    new SqlParameter("@P_Method", 1)
                };
                caComplaintSuggestionsDetails.CAHeadUserId = SqlHelper.ExecuteProcedureReturnData<List<CurrentCitizenAffairHeadModel>>(connString, "Get_CAComplaintSuggestionsByAssigneeandHRId", r => r.TranslateAsCurrentCitizenAffairHeadList(), getCAUserparam);
                userID = caComplaintSuggestionsDetails.CreatedBy.GetValueOrDefault();
            }

            Parallel.Invoke(
              () => caComplaintSuggestionsDetails.M_OrganizationList = GetM_Organisation(connString),
              () => caComplaintSuggestionsDetails.M_LookupsList = GetM_Lookups(connString));

            return caComplaintSuggestionsDetails;
        }

        public List<OrganizationModel> GetM_Organisation(string connString)
        {
            var e = SqlHelper.ExecuteProcedureReturnData<List<OrganizationModel>>(connString, "Get_Organization", r => r.TranslateAsOrganizationList());
            return e;
        }

        public List<M_LookupsModel> GetM_Lookups(string connString)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Type", "CAComplaintSuggestions") };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public CAComplaintSuggestionsWorkflowModel PostCAComplaintSuggestions(string connString, CAComplaintSuggestionsPostModel caComplaintSuggestions)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Type", caComplaintSuggestions.Type),
                new SqlParameter("@P_Subject", caComplaintSuggestions.Subject),
                new SqlParameter("@P_Details", caComplaintSuggestions.Details),
                new SqlParameter("@P_Source", caComplaintSuggestions.Source),
                new SqlParameter("@P_RequestCreatedBy", caComplaintSuggestions.RequestCreatedBy),
                new SqlParameter("@P_MailId", caComplaintSuggestions.MailId),
                new SqlParameter("@P_PhoneNumber", caComplaintSuggestions.PhoneNumber),
                new SqlParameter("@P_ActionTaken", caComplaintSuggestions.ActionTaken),
                new SqlParameter("@P_CreatedBy", caComplaintSuggestions.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", caComplaintSuggestions.CreatedDateTime),
                new SqlParameter("@P_Action", caComplaintSuggestions.Action),
                new SqlParameter("@P_Comment", caComplaintSuggestions.Comments)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<CAComplaintSuggestionsWorkflowModel>(connString, "Save_CAComplaintSuggestions", r => r.TranslateAsCAComplaintSuggestionsSaveResponseList(), param);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "CAComplaintSuggestions"),
                new SqlParameter("@P_Action", caComplaintSuggestions.Action)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.FromID = caComplaintSuggestions.CreatedBy ?? default(int);
            result.Action = caComplaintSuggestions.Action;

            SqlParameter[] parama = { new SqlParameter("@P_Department", "5,6,7,8"), new SqlParameter("@P_GetHead", 1) };
            result.CAHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), parama);

            return result;
        }

        public CAComplaintSuggestionsWorkflowModel PutCAComplaintSuggestions(string connString, CAComplaintSuggestionsPutModel caComplaintSuggestions)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_CAComplaintSuggestionsID", caComplaintSuggestions.CAComplaintSuggestionsID),
                new SqlParameter("@P_Type", caComplaintSuggestions.Type),
                new SqlParameter("@P_Source", caComplaintSuggestions.Source),
                new SqlParameter("@P_Subject", caComplaintSuggestions.Subject),
                new SqlParameter("@P_Details", caComplaintSuggestions.Details),
                new SqlParameter("@P_RequestCreatedBy", caComplaintSuggestions.RequestCreatedBy),
                new SqlParameter("@P_MailId", caComplaintSuggestions.MailId),
                new SqlParameter("@P_PhoneNumber", caComplaintSuggestions.PhoneNumber),
                new SqlParameter("@P_ActionTaken", caComplaintSuggestions.ActionTaken),
                new SqlParameter("@P_UpdatedBy", caComplaintSuggestions.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", caComplaintSuggestions.UpdatedDateTime),
                new SqlParameter("@P_Action", caComplaintSuggestions.Action),
                new SqlParameter("@P_Comment", caComplaintSuggestions.Comments),
                new SqlParameter("@P_DeleteFlag", caComplaintSuggestions.DeleteFlag)
            };
            var result = SqlHelper.ExecuteProcedureReturnData<CAComplaintSuggestionsWorkflowModel>(connString, "Save_CAComplaintSuggestions", r => r.TranslateAsCAComplaintSuggestionsSaveResponseList(), param);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "CAComplaintSuggestions"),
                new SqlParameter("@P_Action", caComplaintSuggestions.Action)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.FromID = caComplaintSuggestions.UpdatedBy ?? default(int);
            result.Action = caComplaintSuggestions.Action;

            SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber)
            };
            result.AssigneeID = caComplaintSuggestions.AssigneeID;
            return result;
        }

        public CAComplaintSuggestionsPutModel GetPatchCAComplaintSuggestionsByID(string connString, int caComplaintSuggestionsID)
        {
            CAComplaintSuggestionsPutModel caComplaintSuggestionsDetails = new CAComplaintSuggestionsPutModel();

            SqlParameter[] param = {
                new SqlParameter("@P_CAComplaintSuggestionsID", caComplaintSuggestionsID)
            };

            if (caComplaintSuggestionsID != 0)
            {
                caComplaintSuggestionsDetails = SqlHelper.ExecuteProcedureReturnData<List<CAComplaintSuggestionsPutModel>>(connString, "Get_CAComplaintSuggestionsByID", r => r.TranslateAsPutCAComplaintSuggestionsList(), param).FirstOrDefault();
            }

            return caComplaintSuggestionsDetails;
        }

        internal CAComplaintSuggestionsWorkflowModel PatchCAComplaintSuggestions(string connString, int id, JsonPatchDocument<CAComplaintSuggestionsPutModel> value)
        {
            var result = GetPatchCAComplaintSuggestionsByID(connString, id);

            value.ApplyTo(result);
            var res = PutCAComplaintSuggestions(connString, result);

            return res;
        }
    }
}