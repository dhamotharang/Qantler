using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.BabyAddition;
using RulersCourt.Translators;
using RulersCourt.Translators.BabyAddition;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.BabyAddition
{
    public class BabyAdditionClient
    {
        public BabyAdditionGetModel GetBabyAdditionByID(string connString, int babyAdditionID, int userID, string lang)
        {
            BabyAdditionGetModel babyAdditionDetails = new BabyAdditionGetModel();

            SqlParameter[] param = {
                new SqlParameter("@P_BabyAdditionID", babyAdditionID)
            };

            if (babyAdditionID != 0)
            {
                babyAdditionDetails = SqlHelper.ExecuteProcedureReturnData<List<BabyAdditionGetModel>>(connString, "Get_BabyAdditionByID", r => r.TranslateAsBabyAdditionList(), param).FirstOrDefault();
                babyAdditionDetails.Attachments = new BabyAdditionAttachmentClient().GetAttachmentById(connString, babyAdditionDetails.BabyAdditionID, "BabyAddition");
                babyAdditionDetails.HistoryLog = new BabyAdditionHistoryLogClient().BabyAdditionHistoryLogByBabyAdditionID(connString, babyAdditionID, lang);

                SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", babyAdditionDetails.ReferenceNumber)
                };

                babyAdditionDetails.AssigneeId = SqlHelper.ExecuteProcedureReturnData<List<CurrentAssigneeModel>>(connString, "Get_BabyAdditionByAssigneeandHRId", r => r.TranslateAsCurrentAssigneeList(), getAssignparam);

                SqlParameter[] getHRUserparam = {
                    new SqlParameter("@P_ReferenceNumber", babyAdditionDetails.ReferenceNumber)
                };

                babyAdditionDetails.HRHeadUsedId = SqlHelper.ExecuteProcedureReturnData<List<CurrentHRHeadModel>>(connString, "Get_BabyAdditionByAssigneeandHRId", r => r.TranslateAsCurrentHRHeadList(), getHRUserparam);
                userID = babyAdditionDetails.CreatedBy.GetValueOrDefault();
            }

            Parallel.Invoke(
              () => babyAdditionDetails.M_OrganizationList = GetM_Organisation(connString, lang),
              () => babyAdditionDetails.M_LookupsList = GetM_Lookups(connString, lang));
            return babyAdditionDetails;
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
                new SqlParameter("@P_Type", "BabyAddition"),
                new SqlParameter("@P_Language", lang)
            };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public BabyAdditionWorkflowModel PostBabyAddition(string connString, BabyAdditionPostModel babyAddition)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_SourceOU", babyAddition.SourceOU),
                new SqlParameter("@P_SourceName", babyAddition.SourceName),
                new SqlParameter("@P_BabyName", babyAddition.BabyName),
                new SqlParameter("@P_Gender", babyAddition.Gender),
                new SqlParameter("@P_Birthday", babyAddition.Birthday),
                new SqlParameter("@P_HospitalName", babyAddition.HospitalName),
                new SqlParameter("@P_CountryofBirth", babyAddition.CountryOfBirth),
                new SqlParameter("@P_CityofBirth", babyAddition.CityOfBirth),
                new SqlParameter("@P_CreatedBy", babyAddition.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", babyAddition.CreatedDateTime),
                new SqlParameter("@P_Action", babyAddition.Action),
                new SqlParameter("@P_Comment", babyAddition.Comments)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<BabyAdditionWorkflowModel>(connString, "Save_BabyAddition", r => r.TranslateAsBabyAdditionSaveResponseList(), param);

            if (babyAddition.Attachments != null)
                new BabyAdditionAttachmentClient().PostAttachments(connString, "BabyAddition", babyAddition.Attachments, result.BabyAdditionID);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "BabyAddition"),
                new SqlParameter("@P_Action", babyAddition.Action)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));
            result.FromID = babyAddition.CreatedBy ?? default(int);
            result.Action = babyAddition.Action;

            SqlParameter[] parama = { new SqlParameter("@P_Department", 9), new SqlParameter("@P_GetHead", 1) };
            result.HRHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), parama);

            return result;
        }

        public BabyAdditionWorkflowModel PutBabyAddition(string connString, BabyAdditionPutModel babyAddition)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_BabyAdditionID", babyAddition.BabyAdditionID),
                new SqlParameter("@P_SourceOU", babyAddition.SourceOU),
                new SqlParameter("@P_SourceName", babyAddition.SourceName),
                new SqlParameter("@P_BabyName", babyAddition.BabyName),
                new SqlParameter("@P_Gender", babyAddition.Gender),
                new SqlParameter("@P_Birthday", babyAddition.Birthday),
                new SqlParameter("@P_HospitalName", babyAddition.HospitalName),
                new SqlParameter("@P_CountryofBirth", babyAddition.CountryOfBirth),
                new SqlParameter("@P_CityofBirth", babyAddition.CityOfBirth),
                new SqlParameter("@P_UpdatedBy", babyAddition.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", babyAddition.UpdatedDateTime),
                new SqlParameter("@P_Action", babyAddition.Action),
                new SqlParameter("@P_Comment", babyAddition.Comments),
                new SqlParameter("@P_DeleteFlag", babyAddition.DeleteFlag)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<BabyAdditionWorkflowModel>(connString, "Save_BabyAddition", r => r.TranslateAsBabyAdditionSaveResponseList(), param);

            if (babyAddition.Attachments != null)
                new BabyAdditionAttachmentClient().PostAttachments(connString, "BabyAddition", babyAddition.Attachments, result.BabyAdditionID);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "BabyAddition"),
                new SqlParameter("@P_Action", babyAddition.Action)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.FromID = babyAddition.UpdatedBy ?? default(int);
            result.Action = babyAddition.Action;

            SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber)
            };
            result.AssigneeID = babyAddition.AssigneeID;
            return result;
        }

        public BabyAdditionPutModel GetPatchBabyAdditionByID(string connString, int babyAdditionID)
        {
            BabyAdditionPutModel babyAdditionDetails = new BabyAdditionPutModel();

            SqlParameter[] param = {
                new SqlParameter("@P_BabyAdditionID", babyAdditionID)
            };

            if (babyAdditionID != 0)
            {
                babyAdditionDetails = SqlHelper.ExecuteProcedureReturnData<List<BabyAdditionPutModel>>(connString, "Get_BabyAdditionByID", r => r.TranslateAsPutBabyAdditionList(), param).FirstOrDefault();
                babyAdditionDetails.Attachments = new BabyAdditionAttachmentClient().GetAttachmentById(connString, babyAdditionDetails.BabyAdditionID, "BabyAddition");
            }

            return babyAdditionDetails;
        }

        internal BabyAdditionWorkflowModel PatchBabyAddition(string connString, int id, JsonPatchDocument<BabyAdditionPutModel> value)
        {
            var result = GetPatchBabyAdditionByID(connString, id);

            value.ApplyTo(result);
            var res = PutBabyAddition(connString, result);

            return res;
        }
    }
}
