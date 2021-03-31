using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.Certificate;
using RulersCourt.Translators;
using RulersCourt.Translators.Certificate;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Certificate
{
    public class CertificateClient
    {
        public CertificateGetModel GetCertificateByID(string connString, int certificateID, int userID, string lang)
        {
            CertificateGetModel certificateDetails = new CertificateGetModel();
            SqlParameter[] param = {
                new SqlParameter("@P_CertificateID", certificateID)
            };

            if (certificateID != 0)
            {
                certificateDetails = SqlHelper.ExecuteProcedureReturnData<List<CertificateGetModel>>(connString, "Get_CertificateByID", r => r.TranslateAsCertificateList(), param).FirstOrDefault();

                certificateDetails.HistoryLog = new CertificateHistoryLogClient().CertificateHistoryLogByCertificateID(connString, certificateID, lang);

                SqlParameter[] getAssigneeparam = {
                    new SqlParameter("@P_ReferenceNumber", certificateDetails.ReferenceNumber),
                    new SqlParameter("@P_Method", 0)
                };

                certificateDetails.AssigneeId = SqlHelper.ExecuteProcedureReturnData<List<CurrentAssigneeModel>>(connString, "Get_CertificateByAssigneeandHRId", r => r.TranslateAsCurrentAssigneeList(), getAssigneeparam);

                SqlParameter[] getHRDeptHeadparam = {
                    new SqlParameter("@P_ReferenceNumber", certificateDetails.ReferenceNumber),
                    new SqlParameter("@P_Method", 1)
                };

                certificateDetails.HRHeadUsedId = SqlHelper.ExecuteProcedureReturnData<List<CurrentHRHeadModel>>(connString, "Get_CertificateByAssigneeandHRId", r => r.TranslateAsCurrentHRHeadList(), getHRDeptHeadparam);

                userID = certificateDetails.CreatedBy.GetValueOrDefault();
            }

            Parallel.Invoke(
              () => certificateDetails.M_OrganizationList = GetM_Organisation(connString, lang),
              () => certificateDetails.M_LookupsList = GetM_Lookups(connString, lang));

            return certificateDetails;
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
                new SqlParameter("@P_Type", "Certificate"),
                new SqlParameter("@P_Language", lang)
            };

            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public CertificateWorkflowModel PostCertificate(string connString, CertificatePostModel certificate)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_SourceOU", certificate.SourceOU),
                new SqlParameter("@P_SourceName", certificate.SourceName),
                new SqlParameter("@P_Attention", certificate.Attention),
                new SqlParameter("@P_To", certificate.To),
                new SqlParameter("@P_SalaryCertificateClassification", certificate.SalaryCertificateClassification),
                new SqlParameter("@P_Reason", certificate.Reason),
                new SqlParameter("@P_CreatedBy", certificate.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", certificate.CreatedDateTime),
                new SqlParameter("@P_Action", certificate.Action),
                new SqlParameter("@P_Comment", certificate.Comments),
                new SqlParameter("@P_CertificateType", certificate.CertificateType)
            };
            var result = SqlHelper.ExecuteProcedureReturnData<CertificateWorkflowModel>(connString, "Save_Certificate", r => r.TranslateAsCertificateSaveResponseList(), param);

            SqlParameter[] paramStatus = {
               new SqlParameter("@P_Service", "Certificate"),
               new SqlParameter("@P_Action", certificate.Action)
            };

            if (certificate.Action != null)
                result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.Action = certificate.Action;
            result.FromID = certificate.CreatedBy ?? default(int);

            SqlParameter[] paramHRHead = {
                new SqlParameter("@P_Department", 9),
                new SqlParameter("@P_GetHead", 1)
            };

            result.HRHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), paramHRHead);
            return result;
        }

        public CertificateWorkflowModel PutCertificate(string connString, CertificatePutModel certificate)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_CertificateID", certificate.CertificateID),
                new SqlParameter("@P_Attention", certificate.Attention),
                new SqlParameter("@P_SourceOU", certificate.SourceOU),
                new SqlParameter("@P_SourceName", certificate.SourceName),
                new SqlParameter("@P_To", certificate.To),
                new SqlParameter("@P_SalaryCertificateClassification", certificate.SalaryCertificateClassification),
                new SqlParameter("@P_Reason", certificate.Reason),
                new SqlParameter("@P_UpdatedBy", certificate.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", certificate.UpdatedDateTime),
                new SqlParameter("@P_Action", certificate.Action),
                new SqlParameter("@P_Comment", certificate.Comments),
                new SqlParameter("@P_DeleteFlag", certificate.DeleteFlag)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<CertificateWorkflowModel>(connString, "Save_Certificate", r => r.TranslateAsCertificateSaveResponseList(), param);
            SqlParameter[] paramStatus = {
               new SqlParameter("@P_Service", "Certificate"),
               new SqlParameter("@P_Action", certificate.Action)
            };

            if (certificate.Action != null)
                result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));
            result.Action = certificate.Action;
            result.FromID = certificate.UpdatedBy ?? default(int);

            result.AssigneeID = certificate.AssigneeID;
            return result;
        }

        public string DeleteCertificate(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_CertificateID", id)
             };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_CertificateByID", param);
        }

        public CertificatePutModel GetPatchCertificateByID(string connString, int certificateID)
        {
            CertificatePutModel certificateDetails = new CertificatePutModel();
            SqlParameter[] param = {
                new SqlParameter("@P_CertificateID", certificateID)
            };

            if (certificateID != 0)
            {
                certificateDetails = SqlHelper.ExecuteProcedureReturnData<List<CertificatePutModel>>(connString, "Get_CertificateByID", r => r.TranslateAsPutCertificateList(), param).FirstOrDefault();
            }

            return certificateDetails;
        }

        internal CertificateWorkflowModel PatchCertificate(string connString, int id, JsonPatchDocument<CertificatePutModel> value)
        {
            var result = GetPatchCertificateByID(connString, id);
            value.ApplyTo(result);
            var res = PutCertificate(connString, result);
            if (result.Action.Equals("AssignToMe"))
                res.AssigneeID = result.UpdatedBy;
            return res;
        }
    }
}
