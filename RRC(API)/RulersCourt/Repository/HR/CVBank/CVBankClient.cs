using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models.CVBank;
using RulersCourt.Models.HR.CVBank;
using RulersCourt.Translators.HR.CVBank;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RulersCourt.Repository.HR.CVBank
{
    public class CVBankClient
    {
        public CVBankListModel GetCVBank(string connString, int pageNumber, int pageSize, string candidateName, string paramYearsofExperience, string paramSpecialization, string paramCountry, DateTime? paramDateFrom, DateTime? paramDateTo, string smartSearch, string lang)
        {
            CVBankListModel list = new CVBankListModel();

            SqlParameter[] param = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_Method", 0),
                new SqlParameter("@P_CandidateName", candidateName),
                new SqlParameter("@P_YearsofExperience", paramYearsofExperience),
                new SqlParameter("@P_Specialization", paramSpecialization),
                new SqlParameter("@P_Country", paramCountry),
                new SqlParameter("@P_DateFrom", paramDateFrom),
                new SqlParameter("@P_DateTo", paramDateTo),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<CVBankDashBoardListModel>>(connString, "Get_CVBankList", r => r.TranslateAsCVBankDashBoardList(), param);

            SqlParameter[] parama = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_Method", 1),
                new SqlParameter("@P_CandidateName", candidateName),
                new SqlParameter("@P_YearsofExperience", paramYearsofExperience),
                new SqlParameter("@P_Specialization", paramSpecialization),
                new SqlParameter("@P_Country", paramCountry),
                new SqlParameter("@P_DateFrom", paramDateFrom),
                new SqlParameter("@P_DateTo", paramDateTo),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_CVBankList", parama);

            list.CandidateName = SqlHelper.ExecuteProcedureReturnData<List<CVBankCandidateModel>>(connString, "Get_CVBankCandidateNameList", r => r.TranslateAsGetUserList());

            return list;
        }

        public CVBankGetModel GetCVBankByID(string connString, int cVBankId, int userID)
        {
            CVBankGetModel cVBankDetails = new CVBankGetModel();

            SqlParameter[] getparam = {
                new SqlParameter("@P_CVBankId", cVBankId) };
            if (cVBankId != 0)
            {
                cVBankDetails = SqlHelper.ExecuteProcedureReturnData<List<CVBankGetModel>>(connString, "Get_CVBankByID", r => r.TranslateAsCVBankList(), getparam).FirstOrDefault();

                if (cVBankDetails != null)
                {
                    cVBankDetails.Attachments = new CVBankAttachmentClient().GetCVBankAttachmentById(connString, cVBankDetails.CVBankId, "CVBank");

                    userID = cVBankDetails.CreatedBy.GetValueOrDefault();
                }
            }

            return cVBankDetails;
        }

        public CVBankSaveResponseModel PostCVBank(string connString, CVBankPostModel cVBank)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_CandidateName", cVBank.CandidateName),
                new SqlParameter("@P_EmailId", cVBank.EmailId),
                new SqlParameter("@P_JobTitle", cVBank.JobTitle),
                new SqlParameter("@P_Specialization", cVBank.Specialization),
                new SqlParameter("@P_EducationalQualification", cVBank.EducationalQualification),
                new SqlParameter("@P_Gender", cVBank.Gender),
                new SqlParameter("@P_YearsofExperience", cVBank.YearsofExperience),
                new SqlParameter("@P_AreaofExpertise", cVBank.AreaofExpertise),
                new SqlParameter("@P_CountryofResidence", cVBank.CountryofResidence),
                new SqlParameter("@P_CityofResidence", cVBank.CityofResidence),
                new SqlParameter("@P_Address", cVBank.Address),
                new SqlParameter("@P_CreatedBy", cVBank.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", cVBank.CreatedDateTime)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<CVBankSaveResponseModel>(connString, "Save_CVBank", r => r.TranslateAsCVBankSaveResponseList(), param);

            if (cVBank.Attachments != null)
                new CVBankAttachmentClient().PostCVBankAttachments(connString, "CVBank", cVBank.Attachments, result.CVBankId);

            return result;
        }

        public CVBankSaveResponseModel PutCVBank(string connString, CVBankPutModel cVBank)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_CVBankId", cVBank.CVBankId),
                new SqlParameter("@P_CandidateName", cVBank.CandidateName),
                new SqlParameter("@P_EmailId", cVBank.EmailId),
                new SqlParameter("@P_JobTitle", cVBank.JobTitle),
                new SqlParameter("@P_Specialization", cVBank.Specialization),
                new SqlParameter("@P_EducationalQualification", cVBank.EducationalQualification),
                new SqlParameter("@P_Gender", cVBank.Gender),
                new SqlParameter("@P_YearsofExperience", cVBank.YearsofExperience),
                new SqlParameter("@P_AreaofExpertise", cVBank.AreaofExpertise),
                new SqlParameter("@P_CountryofResidence", cVBank.CountryofResidence),
                new SqlParameter("@P_CityofResidence", cVBank.CityofResidence),
                new SqlParameter("@P_Address", cVBank.Address),
                new SqlParameter("@P_UpdatedBy", cVBank.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", cVBank.UpdatedDateTime),
                new SqlParameter("@P_DeleteFlag", cVBank.DeleteFlag)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<CVBankSaveResponseModel>(connString, "Save_CVBank", r => r.TranslateAsCVBankSaveResponseList(), param);

            if (cVBank.Attachments != null)
                new CVBankAttachmentClient().PostCVBankAttachments(connString, "CVBank", cVBank.Attachments, result.CVBankId);

            return result;
        }

        public string DeleteCVBank(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_CVBankId", id)
             };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_CVBankByID", param);
        }

        public CVBankPutModel GetPatchCVBankByID(string connString, int cVBankId)
        {
            CVBankPutModel cVBankDetails = new CVBankPutModel();

            SqlParameter[] getparam = {
                new SqlParameter("@P_CVBankId", cVBankId)
            };

            if (cVBankId != 0)
            {
                cVBankDetails = SqlHelper.ExecuteProcedureReturnData<List<CVBankPutModel>>(connString, "Get_CVBankByID", r => r.TranslateAsPutCVBankList(), getparam).FirstOrDefault();

                cVBankDetails.Attachments = new CVBankAttachmentClient().GetCVBankAttachmentById(connString, cVBankDetails.CVBankId, "CVBank");
            }

            return cVBankDetails;
        }

        internal CVBankSaveResponseModel PatchCVBank(string connString, int id, JsonPatchDocument<CVBankPutModel> value)
        {
            var result = GetPatchCVBankByID(connString, id);
            value.ApplyTo(result);
            var res = PutCVBank(connString, result);
            return res;
        }
    }
}
