using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.ITSupport;
using RulersCourt.Translators;
using RulersCourt.Translators.ITSupportTranslators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.ITSupportClient
{
    public class ITSupportClient
    {
        public ITSupportTranslatorModel PostITSupport(string connString, ITSupportPostModel iTSupport)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Date", iTSupport.Date),
                new SqlParameter("@P_SourceOU", iTSupport.SourceOU),
                new SqlParameter("@P_SourceName", iTSupport.SourceName),
                new SqlParameter("@P_Subject", iTSupport.Subject),
                new SqlParameter("@P_RequestorDepartment", iTSupport.RequestorDepartment),
                new SqlParameter("@P_RequestorName", iTSupport.RequestorName),
                new SqlParameter("@P_RequestType", iTSupport.RequestType),
                new SqlParameter("@P_RequestDetails", iTSupport.RequestDetails),
                new SqlParameter("@P_Priority", iTSupport.Priority),
                new SqlParameter("@P_Action", iTSupport.Action),
                new SqlParameter("@P_Comments", iTSupport.Comments),
                new SqlParameter("@P_CreatedBy", iTSupport.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", iTSupport.CreatedDateTime)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<ITSupportTranslatorModel>(connString, "Save_ITSupport", r => r.TranslateAsITSupportSaveResponseList(), param);

            if (iTSupport.Attachments != null)                new ITSupportAttachmentClient().PostITSupportAttachments(connString, "ITSupport", iTSupport.Attachments, result.ITSupportID);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "ITSupport"),
                new SqlParameter("@P_Action", iTSupport.Action)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));
            result.FromID = iTSupport.CreatedBy ?? default(int);
            result.Action = iTSupport.Action;

            SqlParameter[] parama = {
                new SqlParameter("@P_Department", 11),
                new SqlParameter("@P_GetHead", 1)
            };

            result.HRITSupportUsedId = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), parama);

            return result;
        }

        public ITSupportTranslatorModel PutITSupport(string connString, ITSupportPutModel iTSupport)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Date", iTSupport.Date),
                new SqlParameter("@P_SourceOU", iTSupport.SourceOU),
                new SqlParameter("@P_SourceName", iTSupport.SourceName),
                new SqlParameter("@P_Subject", iTSupport.Subject),
                new SqlParameter("@P_RequestorDepartment", iTSupport.RequestorDepartment),
                new SqlParameter("@P_RequestorName", iTSupport.RequestorName),
                new SqlParameter("@P_RequestType", iTSupport.RequestType),
                new SqlParameter("@P_RequestDetails", iTSupport.RequestDetails),
                new SqlParameter("@P_Priority", iTSupport.Priority),
                new SqlParameter("@P_Action", iTSupport.Action),
                new SqlParameter("@P_Comments", iTSupport.Comments),
                new SqlParameter("@P_UpdatedBy", iTSupport.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", iTSupport.UpdatedDateTime),
                new SqlParameter("@P_ITSupportID", iTSupport.ITSupportID)
            };
            var result = SqlHelper.ExecuteProcedureReturnData<ITSupportTranslatorModel>(connString, "Save_ITSupport", r => r.TranslateAsITSupportSaveResponseList(), param);

            if (iTSupport.Attachments != null)                new ITSupportAttachmentClient().PostITSupportAttachments(connString, "ITSupport", iTSupport.Attachments, result.ITSupportID);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "ITSupport"),
                new SqlParameter("@P_Action", iTSupport.Action)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));
            result.FromID = iTSupport.UpdatedBy ?? default(int);
            result.Action = iTSupport.Action;

            SqlParameter[] parama = {
                new SqlParameter("@P_Department", 11),
                new SqlParameter("@P_GetHead", 1)
            };

            result.HRITSupportUsedId = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), parama);

            return result;
        }

        public ITSupportPutModel GetPatchITSupportByID(string connString, int iTSupportID)
        {
            ITSupportPutModel iTSupportDetails = new ITSupportPutModel();

            SqlParameter[] param = {
                new SqlParameter("@P_ITSupportID", iTSupportID)
            };

            if (iTSupportID != 0)
            {
                iTSupportDetails = SqlHelper.ExecuteProcedureReturnData<List<ITSupportPutModel>>(connString, "Get_ITSupportByID", r => r.TranslateAsPutITSupportList(), param).FirstOrDefault();
                iTSupportDetails.Attachments = new ITSupportAttachmentClient().GetITSupportAttachmentById(connString, iTSupportDetails.ITSupportID, "ITSupport");
            }

            return iTSupportDetails;
        }

        public ITSupportGetModel GetITSupportID(string connString, int iTSupportID, int userID, string lang)
        {
            ITSupportGetModel getITSupportIDDetails = new ITSupportGetModel();

            SqlParameter[] param = {
               new SqlParameter("@P_ITSupportID", iTSupportID)
            };

            if (iTSupportID != 0)
            {
                getITSupportIDDetails = SqlHelper.ExecuteProcedureReturnData<List<ITSupportGetModel>>(connString, "Get_ITSupportByID", r => r.TranslatePhotoGetModelList(), param).FirstOrDefault();
                getITSupportIDDetails.Attachments = new ITSupportAttachmentClient().GetITSupportAttachmentById(connString, getITSupportIDDetails.ITSupportID, "ITSupport");
                getITSupportIDDetails.HistoryLog = new ITSupportHistoryLogClientcs().ITSupportHistoryLogModelByID(connString, iTSupportID, lang);
                userID = getITSupportIDDetails.CreatedBy.GetValueOrDefault();
            }

            Parallel.Invoke(
            () => getITSupportIDDetails.M_OrganizationList = GetM_Organisation(connString, lang),
            () => getITSupportIDDetails.M_LookupsList = GetM_Lookups(connString, lang));

            return getITSupportIDDetails;
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
                new SqlParameter("@P_Type", "ITSupport"),
                new SqlParameter("@P_Language", lang)
            };

            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public string GetITSupportLastSync(string connString)
        {
            return SqlHelper.ExecuteProcedureReturnString(connString, "Get_M_ITLastSyncDate");
        }

        internal ITSupportTranslatorModel PatchITSupport(string connString, int id, JsonPatchDocument<ITSupportPutModel> value)
        {
            var result = GetPatchITSupportByID(connString, id);

            value.ApplyTo(result);

            var res = PutITSupport(connString, result);

            if (result.Action == "Close")
            {
                res.ITSupportID = result.ITSupportID;
            }

            SqlParameter[] param = {
                new SqlParameter("@P_Department", 11),
                new SqlParameter("@P_GetHead", 1)
            };

            res.HRITSupportUsedId = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), param);
            return res;
        }
    }
}
