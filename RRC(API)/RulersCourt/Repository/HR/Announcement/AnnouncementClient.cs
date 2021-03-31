using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.Announcement;
using RulersCourt.Translators;
using RulersCourt.Translators.Announcement;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Announcement
{
    public class AnnouncementClient
    {
        public AnnouncementGetModel GetAnnouncementByID(string connString, int announcementID, int userID, string lang)
        {
            AnnouncementGetModel announcementDetails = new AnnouncementGetModel();

            SqlParameter[] param = {
                new SqlParameter("@P_AnnouncementID", announcementID)
            };

            if (announcementID != 0)
            {
                announcementDetails = SqlHelper.ExecuteProcedureReturnData<List<AnnouncementGetModel>>(connString, "Get_AnnouncementByID", r => r.TranslateAsAnnouncementList(), param).FirstOrDefault();

                announcementDetails.HistoryLog = new AnnouncementHistoryLogClient().AnnouncementHistoryLogByAnnouncementID(connString, announcementID, lang);

                announcementDetails.AnnouncementTypeAndDescription = SqlHelper.ExecuteProcedureReturnData<List<AnnouncementTypeAndDescriptionModel>>(connString, "Get_AnnouncementTypeAndDescription", r => r.TranslateAsAnnouncementTypeAndDescriptionList());

                SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", announcementDetails.ReferenceNumber),
                    new SqlParameter("@P_Method", 0)
                };

                announcementDetails.AssigneeId = SqlHelper.ExecuteProcedureReturnData<List<CurrentAssigneeModel>>(connString, "Get_AnnouncementByAssigneeandHRId", r => r.TranslateAsCurrentAssigneeList(), getAssignparam);

                SqlParameter[] getHRUserparam = {
                    new SqlParameter("@P_ReferenceNumber", announcementDetails.ReferenceNumber),
                    new SqlParameter("@P_Method", 1)
                };

                announcementDetails.HRHeadUsedId = SqlHelper.ExecuteProcedureReturnData<List<CurrentHRHeadModel>>(connString, "Get_AnnouncementByAssigneeandHRId", r => r.TranslateAsCurrentHRHeadList(), getHRUserparam);

                userID = announcementDetails.CreatedBy.GetValueOrDefault();
            }

            Parallel.Invoke(
              () => announcementDetails.M_OrganizationList = GetM_Organisation(connString, lang),
              () => announcementDetails.M_LookupsList = GetM_Lookups(connString, lang),
              () => announcementDetails.AnnouncementTypeAndDescription = SqlHelper.ExecuteProcedureReturnData<List<AnnouncementTypeAndDescriptionModel>>(connString, "Get_AnnouncementTypeAndDescription", r => r.TranslateAsAnnouncementTypeAndDescriptionList()));

            return announcementDetails;
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
                new SqlParameter("@P_Type", "Announcement"),
                new SqlParameter("@P_Language", lang)
            };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public AnnouncementWorkflowModel PostAnnouncement(string connString, AnnouncementPostModel announcement)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_SourceOU", announcement.SourceOU),
                new SqlParameter("@P_SourceName", announcement.SourceName),
                new SqlParameter("@P_AnnouncementType", announcement.AnnouncementType),
                new SqlParameter("@P_AnnouncementDescription", announcement.AnnouncementDescription),
                new SqlParameter("@P_CreatedBy", announcement.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", announcement.CreatedDateTime),
                new SqlParameter("@P_Action", announcement.Action),
                new SqlParameter("@P_Comment", announcement.Comments)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<AnnouncementWorkflowModel>(connString, "Save_Announcement", r => r.TranslateAsAnnouncementSaveResponseList(), param);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Announcement"),
                new SqlParameter("@P_Action", announcement.Action)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.FromID = announcement.CreatedBy ?? default(int);
            result.Action = announcement.Action;

            SqlParameter[] parama = {
                new SqlParameter("@P_Department", 9),
                new SqlParameter("@P_GetHead", 1)
            };
            result.HRHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), parama);

            return result;
        }

        public AnnouncementWorkflowModel PutAnnouncement(string connString, AnnouncementPutModel announcement)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_AnnouncementID", announcement.AnnouncementID),
                new SqlParameter("@P_SourceOU", announcement.SourceOU),
                new SqlParameter("@P_SourceName", announcement.SourceName),
                new SqlParameter("@P_AnnouncementType", announcement.AnnouncementType),
                new SqlParameter("@P_AnnouncementDescription", announcement.AnnouncementDescription),
                new SqlParameter("@P_UpdatedBy", announcement.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", announcement.UpdatedDateTime),
                new SqlParameter("@P_Action", announcement.Action),
                new SqlParameter("@P_Comment", announcement.Comments),
                new SqlParameter("@P_DeleteFlag", announcement.DeleteFlag)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<AnnouncementWorkflowModel>(connString, "Save_Announcement", r => r.TranslateAsAnnouncementSaveResponseList(), param);

            SqlParameter[] paramStatus = {
               new SqlParameter("@P_Service", "Announcement"),
               new SqlParameter("@P_Action", announcement.Action)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.FromID = announcement.UpdatedBy ?? default(int);
            result.Action = announcement.Action;

            SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber)
            };
            result.AssigneeID = announcement.AssigneeID;
            return result;
        }

        public AnnouncementPutModel GetPatchAnnouncementByID(string connString, int announcementID)
        {
            AnnouncementPutModel announcementDetails = new AnnouncementPutModel();

            SqlParameter[] param = {
                new SqlParameter("@P_AnnouncementID", announcementID)
            };

            if (announcementID != 0)
            {
                announcementDetails = SqlHelper.ExecuteProcedureReturnData<List<AnnouncementPutModel>>(connString, "Get_AnnouncementByID", r => r.TranslateAsPutAnnouncementList(), param).FirstOrDefault();
            }

            return announcementDetails;
        }

        internal AnnouncementWorkflowModel PatchAnnouncement(string connString, int id, JsonPatchDocument<AnnouncementPutModel> value)
        {
            var result = GetPatchAnnouncementByID(connString, id);

            value.ApplyTo(result);
            var res = PutAnnouncement(connString, result);

            return res;
        }
    }
}
