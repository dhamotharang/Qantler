using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.HRComplaintSuggestions;
using RulersCourt.Translators;
using RulersCourt.Translators.HRComplaintSuggestions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.HRComplaintSuggestions
{
    public class HRComplaintSuggestionsClient
    {
        public HRComplaintSuggestionsGetModel GetHRComplaintSuggestionsByID(string connString, int hRComplaintSuggestionsID, int userID, string lang)
        {
            HRComplaintSuggestionsGetModel hRComplaintSuggestionsDetails = new HRComplaintSuggestionsGetModel();

            SqlParameter[] param = {
                new SqlParameter("@P_HRComplaintSuggestionsID", hRComplaintSuggestionsID)
            };

            if (hRComplaintSuggestionsID != 0)
            {
                hRComplaintSuggestionsDetails = SqlHelper.ExecuteProcedureReturnData<List<HRComplaintSuggestionsGetModel>>(connString, "Get_HRComplaintSuggestionsByID", r => r.TranslateAsHRComplaintSuggestionsList(), param).FirstOrDefault();
                hRComplaintSuggestionsDetails.HistoryLog = new HRComplaintSuggestionsHistroyLogClient().HRComplaintSuggestionsHistoryLogByHRComplaintSuggestionsID(connString, hRComplaintSuggestionsID, lang);

                SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", hRComplaintSuggestionsDetails.ReferenceNumber),
                    new SqlParameter("@P_Method", 0)
                };

                hRComplaintSuggestionsDetails.AssigneeId = SqlHelper.ExecuteProcedureReturnData<List<CurrentAssigneeModel>>(connString, "Get_HRComplaintSuggestionsByAssigneeandHRId", r => r.TranslateAsCurrentAssigneeList(), getAssignparam);

                SqlParameter[] getHRUserparam = {
                    new SqlParameter("@P_ReferenceNumber", hRComplaintSuggestionsDetails.ReferenceNumber),
                    new SqlParameter("@P_Method", 1)
                };

                hRComplaintSuggestionsDetails.HRHeadUsedId = SqlHelper.ExecuteProcedureReturnData<List<CurrentHRHeadModel>>(connString, "Get_HRComplaintSuggestionsByAssigneeandHRId", r => r.TranslateAsCurrentHRHeadList(), getHRUserparam);
                userID = hRComplaintSuggestionsDetails.CreatedBy.GetValueOrDefault();
            }

            Parallel.Invoke(
              () => hRComplaintSuggestionsDetails.M_OrganizationList = GetM_Organisation(connString, lang),
              () => hRComplaintSuggestionsDetails.M_LookupsList = GetM_Lookups(connString, lang));

            return hRComplaintSuggestionsDetails;
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
                new SqlParameter("@P_Type", "HRComplaintSuggestions"),
                new SqlParameter("@P_Language", lang)
            };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public HRComplaintSuggestionsWorkflowModel PostHRComplaintSuggestions(string connString, HRComplaintSuggestionsPostModel hRComplaintSuggestions)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Type", hRComplaintSuggestions.Type),
                new SqlParameter("@P_Subject", hRComplaintSuggestions.Subject),
                new SqlParameter("@P_Source", hRComplaintSuggestions.Source),
                new SqlParameter("@P_Details", hRComplaintSuggestions.Details),
                new SqlParameter("@P_RequestCreatedBy", hRComplaintSuggestions.RequestCreatedBy),
                new SqlParameter("@P_MailId", hRComplaintSuggestions.MailID),
                new SqlParameter("@P_PhoneNumber", hRComplaintSuggestions.PhoneNumber),
                new SqlParameter("@P_ActionTaken", hRComplaintSuggestions.ActionTaken),
                new SqlParameter("@P_CreatedBy", hRComplaintSuggestions.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", hRComplaintSuggestions.CreatedDateTime),
                new SqlParameter("@P_Action", hRComplaintSuggestions.Action),
                new SqlParameter("@P_Comment", hRComplaintSuggestions.Comments)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<HRComplaintSuggestionsWorkflowModel>(connString, "Save_HRComplaintSuggestions", r => r.TranslateAsHRComplaintSuggestionsSaveResponseList(), param);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "HRComplaintSuggestions"),
                new SqlParameter("@P_Action", hRComplaintSuggestions.Action)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));
            result.IsAnonymous = hRComplaintSuggestions.RequestCreatedBy == "0" ? true : false;
            result.FromID = hRComplaintSuggestions.CreatedBy ?? default(int);
            result.Action = hRComplaintSuggestions.Action;

            SqlParameter[] parama = { new SqlParameter("@P_Department", 9), new SqlParameter("@P_GetHead", 1) };
            result.HRHeadUsedId = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), parama);

            return result;
        }

        public HRComplaintSuggestionsWorkflowModel PutHRComplaintSuggestions(string connString, HRComplaintSuggestionsPutModel hRComplaintSuggestions)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_HRComplaintSuggestionsID", hRComplaintSuggestions.HRComplaintSuggestionsID),
                new SqlParameter("@P_Type", hRComplaintSuggestions.Type),
                new SqlParameter("@P_Subject", hRComplaintSuggestions.Subject),
                new SqlParameter("@P_Source", hRComplaintSuggestions.Source),
                new SqlParameter("@P_Details", hRComplaintSuggestions.Details),
                new SqlParameter("@P_RequestCreatedBy", hRComplaintSuggestions.RequestCreatedBy),
                new SqlParameter("@P_MailId", hRComplaintSuggestions.MailID),
                new SqlParameter("@P_PhoneNumber", hRComplaintSuggestions.PhoneNumber),
                new SqlParameter("@P_ActionTaken", hRComplaintSuggestions.ActionTaken),
                new SqlParameter("@P_UpdatedBy", hRComplaintSuggestions.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", hRComplaintSuggestions.UpdatedDateTime),
                new SqlParameter("@P_Action", hRComplaintSuggestions.Action),
                new SqlParameter("@P_Comment", hRComplaintSuggestions.Comments),
                new SqlParameter("@P_DeleteFlag", hRComplaintSuggestions.DeleteFlag)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<HRComplaintSuggestionsWorkflowModel>(connString, "Save_HRComplaintSuggestions", r => r.TranslateAsHRComplaintSuggestionsSaveResponseList(), param);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "HRComplaintSuggestions"),
                new SqlParameter("@P_Action", hRComplaintSuggestions.Action)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.FromID = hRComplaintSuggestions.UpdatedBy ?? default(int);
            result.Action = hRComplaintSuggestions.Action;

            SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber)
            };
            result.AssigneeId = hRComplaintSuggestions.AssigneeID;
            return result;
        }

        public HRComplaintSuggestionsPutModel GetPatchHRComplaintSuggestionsByID(string connString, int hRComplaintSuggestionsID)
        {
            HRComplaintSuggestionsPutModel hRComplaintSuggestionsDetails = new HRComplaintSuggestionsPutModel();

            SqlParameter[] param = {
                new SqlParameter("@P_HRComplaintSuggestionsID", hRComplaintSuggestionsID)
            };

            if (hRComplaintSuggestionsID != 0)
            {
                hRComplaintSuggestionsDetails = SqlHelper.ExecuteProcedureReturnData<List<HRComplaintSuggestionsPutModel>>(connString, "Get_HRComplaintSuggestionsByID", r => r.TranslateAsPutHRComplaintSuggestionsList(), param).FirstOrDefault();
            }

            return hRComplaintSuggestionsDetails;
        }

        internal HRComplaintSuggestionsWorkflowModel PatchHRComplaintSuggestions(string connString, int id, JsonPatchDocument<HRComplaintSuggestionsPutModel> value)
        {
            var result = GetPatchHRComplaintSuggestionsByID(connString, id);

            value.ApplyTo(result);
            var res = PutHRComplaintSuggestions(connString, result);
            return res;
        }
    }
}