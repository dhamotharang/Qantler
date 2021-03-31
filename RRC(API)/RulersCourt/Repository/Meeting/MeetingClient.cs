using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.Meeting;
using RulersCourt.Translators;
using RulersCourt.Translators.Letter;
using RulersCourt.Translators.Meeting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Meeting
{
    public class MeetingClient
    {
        public MeetingResponseModel PostMeeting(string connString, MeetingPostModel value)
        {
            SqlParameter[] param = { new SqlParameter("@P_OrganizerName", value.OrganizerDepartmentID),
                                     new SqlParameter("@P_OrganizerDepartment", value.OrganizerUserID),
                                     new SqlParameter("@P_Subject", value.Subject),
                                     new SqlParameter("@P_Location", value.Location),
                                     new SqlParameter("@P_StartDateTime", value.StartDateTime),
                                     new SqlParameter("@P_EndDateTime", value.EndDateTime),
                                     new SqlParameter("@P_MeetingType", value.MeetingType),
                                     new SqlParameter("@P_InternalInvitees", value.IsInternalInvitees),
                                     new SqlParameter("@P_ExternalInvitees", value.IsExternalInvitees),
                                     new SqlParameter("@P_CreatedBy", value.CreatedBy),
                                     new SqlParameter("@P_CreatedDateTime", value.CreatedDateTime),
                                     new SqlParameter("@P_Action", value.Action),
                                     new SqlParameter("@P_Comment", value.Comments)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<MeetingResponseModel>(connString, "Save_Meeting", r => r.TranslateAsMeetingSaveResponseList(), param);

            if (value.Attachments != null)
                new MeetingAttachmentClient().PostMeetingAttachments(connString, "Meeting", value.Attachments, result.MeetingID);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Meeting"),
                new SqlParameter("@P_Action", value.Action) };
            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));
            if (value.InternalInvitees != null)
                new MeetingInviteesClient().SaveInternalInvitees(connString, value.InternalInvitees, result.MeetingID, value.CreatedBy, value.CreatedDateTime);

            if (value.ExternalInvitees != null)
                new MeetingInviteesClient().SaveExternalInvitees(connString, value.ExternalInvitees, result.MeetingID, value.CreatedBy, value.CreatedDateTime);

            if (value.RemindMeAt != null)
                new MeetingRemindMeClient().SaveRemindMeAt(connString, value.RemindMeAt, result.MeetingID, value.CreatedBy, value.CreatedDateTime);
            result.InternalInvitees = value.InternalInvitees;
            result.ExternalInvitees = value.ExternalInvitees;
            if (value.Action == "Submit" || value.Action == "Reschedule" || value.Action == "cancel")
            {
                result.Action = value.Action;
            }
            else
            {
                result.Action = "Add Comment";
            }

            return result;
        }

        public MeetingGetModel GetMeetingByID(string connString, int? id, int? userID, string lang)
        {
            MeetingGetModel meeting = new MeetingGetModel();
            SqlParameter[] param = {
                new SqlParameter("@P_MeetingID", id), new SqlParameter("@P_UserID", userID) };
            SqlParameter[] iternalInvitees = {
                    new SqlParameter("@P_MeetingID", id) };
            SqlParameter[] externalInvitees = {
                    new SqlParameter("@P_MeetingID", id) };
            SqlParameter[] remindMeAt = {
                    new SqlParameter("@P_MeetingID", id) };

            if (id != 0)
            {
                meeting = SqlHelper.ExecuteProcedureReturnData<List<MeetingGetModel>>(connString, "Get_MeetingByID", r => r.TranslateAsMeetingList(), param).FirstOrDefault();

                meeting.ExternalInvitiees = SqlHelper.ExecuteProcedureReturnData<List<MeetingExternalInviteesModel>>(connString, "Get_MeetingExternalInviteesByID", r => r.TranslateAsMeetingExternalInviteesList(), externalInvitees);

                meeting.InternalInvitees = SqlHelper.ExecuteProcedureReturnData<List<MeetingInternalInviteesModel>>(connString, "Get_MeetingInternalInviteesByID", r => r.TranslateAsMeetingInternalInviteesList(), iternalInvitees);

                meeting.RemindMeAt = SqlHelper.ExecuteProcedureReturnData<List<MeetingRemindMeAtModel>>(connString, "Get_MeetingRemindMeAtByID", r => r.TranslateAsMeetingRemindMeAtList(), remindMeAt);

                meeting.Attachments = new MeetingAttachmentClient().GetMeetingAttachmentById(connString, meeting.MeetingID, "Meeting");

                meeting.CommunicationHistory = new MeetingHistoryClient().MeetingHistoryLogByMeetingID(connString, id, lang);

                userID = meeting.CreatedBy.GetValueOrDefault();
            }

            Parallel.Invoke(
              () => meeting.M_OrganizationList = GetM_Organisation(connString, lang),
              () => meeting.M_LookupsList = GetM_Lookups(connString, lang));
            return meeting;
        }

        public List<OrganizationModel> GetM_Organisation(string connString, string lang)
        {
            SqlParameter[] orgParam = {
                new SqlParameter("@P_Language", lang) };
            var e = SqlHelper.ExecuteProcedureReturnData<List<OrganizationModel>>(connString, "Get_Organization", r => r.TranslateAsOrganizationList(), orgParam);
            return e;
        }

        public List<M_LookupsModel> GetM_Lookups(string connString, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Type", "Meeting"),
                new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsL_LookupsList(), param);
        }

        public MeetingResponseModel PutMeeting(string connString, MeetingPutModel value)
        {
            SqlParameter[] param = {
                                     new SqlParameter("@P_MeetingID", value.MeetingID),
                                     new SqlParameter("@P_OrganizerName", value.OrganizerDepartmentID),
                                     new SqlParameter("@P_OrganizerDepartment", value.OrganizerUserID),
                                     new SqlParameter("@P_Subject", value.Subject),
                                     new SqlParameter("@P_Location", value.Location),
                                     new SqlParameter("@P_StartDateTime", value.StartDateTime),
                                     new SqlParameter("@P_EndDateTime", value.EndDateTime),
                                     new SqlParameter("@P_MeetingType", value.MeetingType),
                                     new SqlParameter("@P_InternalInvitees", value.IsInternalInvitees),
                                     new SqlParameter("@P_ExternalInvitees", value.IsExternalInvitees),
                                     new SqlParameter("@P_UpdatedBy", value.UpdatedBy),
                                     new SqlParameter("@P_UpdatedDateTime", value.UpdatedDateTime),
                                     new SqlParameter("@P_Action", value.Action),
                                     new SqlParameter("@P_Comment", value.Comments)
            };
            var result = SqlHelper.ExecuteProcedureReturnData<MeetingResponseModel>(connString, "Save_Meeting", r => r.TranslateAsMeetingSaveResponseList(), param);

            if (value.Attachments != null)
                new MeetingAttachmentClient().PostMeetingAttachments(connString, "Meeting", value.Attachments, result.MeetingID);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Meeting"),
                new SqlParameter("@P_Action", value.Action) };
            if (value.Action == "Submit" || value.Action == "Reschedule" || value.Action == "cancel")
            {
                result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));
            }

            if (value.InternalInvitees != null)
                new MeetingInviteesClient().SaveInternalInvitees(connString, value.InternalInvitees, result.MeetingID, value.UpdatedBy, value.UpdatedDateTime);

            if (value.ExternalInvitees != null)
                new MeetingInviteesClient().SaveExternalInvitees(connString, value.ExternalInvitees, result.MeetingID, value.UpdatedBy, value.UpdatedDateTime);

            if (value.RemindMeAt != null)
                new MeetingRemindMeClient().SaveRemindMeAt(connString, value.RemindMeAt, result.MeetingID, value.UpdatedBy, value.UpdatedDateTime);

            if (value.Action == "Submit" || value.Action == "Reschedule" || value.Action == "cancel")
            {
                result.Action = value.Action;
            }
            else
            {
                result.Action = "Add Comment";
            }

            result.InternalInvitees = value.InternalInvitees;
            result.ExternalInvitees = value.ExternalInvitees;

            return result;
        }

        public MeetingPutModel GetPatchMeetingByID(string connString, int meetingID)
        {
            MeetingPutModel meeting = new MeetingPutModel();

            SqlParameter[] param = {
                new SqlParameter("@P_MeetingID", meetingID), new SqlParameter("@P_UserID", 0) };

            SqlParameter[] externalInvitees = {
                    new SqlParameter("@P_MeetingID", meetingID) };

            SqlParameter[] internalInvitees = {
                    new SqlParameter("@P_MeetingID", meetingID) };

            if (meetingID != 0)
            {
                meeting = SqlHelper.ExecuteProcedureReturnData<List<MeetingPutModel>>(connString, "Get_MeetingByID", r => r.TranslateAsPutMeetingList(), param).FirstOrDefault();

                meeting.ExternalInvitees = SqlHelper.ExecuteProcedureReturnData<List<MeetingExternalInviteesModel>>(connString, "Get_MeetingExternalInviteesByID", r => r.TranslateAsMeetingExternalInviteesList(), externalInvitees);

                meeting.InternalInvitees = SqlHelper.ExecuteProcedureReturnData<List<MeetingInternalInviteesModel>>(connString, "Get_MeetingInternalInviteesByID", r => r.TranslateAsMeetingInternalInviteesList(), internalInvitees);

                meeting.Attachments = new MeetingAttachmentClient().GetMeetingAttachmentById(connString, meeting.MeetingID, "Meeting");
            }

            return meeting;
        }

        public MeetingResponseModel PatchMeeting(string connString, int id, JsonPatchDocument<MeetingPutModel> value)
        {
            var result = GetPatchMeetingByID(connString, id);
            value.ApplyTo(result);
            var res = PutMeeting(connString, result);
            if (result.Action == "Submit" || result.Action == "Reschedule" || result.Action == "cancel")
            {
                res.Action = result.Action;
            }
            else
            {
                res.Action = "Add Comment";
            }

            res.ExternalInvitees = result.ExternalInvitees;
            res.InternalInvitees = result.InternalInvitees;
            return res;
        }

        public MeetingResponseModel PostMOM(string connString, MOMPostModel value)
        {
            SqlParameter[] param = { new SqlParameter("@P_MeetingID", value.MeetingID),
                                     new SqlParameter("@P_PointsDiscussed", value.PointsDiscussed),
                                     new SqlParameter("@P_PendingPoints", value.PendingPoints),
                                     new SqlParameter("@P_Suggestion", value.Suggestion),
                                     new SqlParameter("@P_CreatedBy", value.CreatedBy),
                                     new SqlParameter("@P_CreatedDateTime", value.CreatedDateTime) };
            SqlParameter[] internalInvitees = {
                    new SqlParameter("@P_MeetingID", value.MeetingID) };
            SqlParameter[] externalInvitees = {
                    new SqlParameter("@P_MeetingID", value.MeetingID) };
            var result = SqlHelper.ExecuteProcedureReturnData<MeetingResponseModel>(connString, "Save_MOM", r => r.TranslateAsMOMSaveResponseList(), param);
            result.ExternalInvitees = SqlHelper.ExecuteProcedureReturnData<List<MeetingExternalInviteesModel>>(connString, "Get_MeetingExternalInviteesByID", r => r.TranslateAsMeetingExternalInviteesList(), externalInvitees);

            result.InternalInvitees = SqlHelper.ExecuteProcedureReturnData<List<MeetingInternalInviteesModel>>(connString, "Get_MeetingInternalInviteesByID", r => r.TranslateAsMeetingInternalInviteesList(), internalInvitees);

            result.MeetingID = value.MeetingID;
            result.Action = "Create";

            return result;
        }

        public int SaveCommunicationChat(string connString, MeetingCommunicationHistoryModel chat)
        {
            SqlParameter[] param =
                    { new SqlParameter("@P_CommunicationID", chat.CommunicationID),
                    new SqlParameter("@P_MeetingID", chat.MeetingID),
                    new SqlParameter("@P_Message", chat.Message),
                    new SqlParameter("@P_ParentCommunicationID", chat.ParentCommunicationID),
                    new SqlParameter("@P_CreatedBy", chat.CreatedBy),
                    new SqlParameter("@P_CreatedDateTime", chat.CreatedDateTime)
                    };

            return int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Save_MeetingCommunicationHistory", param));
        }

        public MOMGetModel GetMOMByID(string connString, int id, int userID, string lang)
        {
            MOMGetModel meeting = new MOMGetModel();
            SqlParameter[] param = {
                new SqlParameter("@P_MeetingID", id),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Language", lang)
            };

            if (id != 0)
            {
                meeting = SqlHelper.ExecuteProcedureReturnData<List<MOMGetModel>>(connString, "Get_MOMByID", r => r.TranslateAsMOMList(), param).FirstOrDefault();
                meeting.CommunicationHistory = new MeetingHistoryClient().MOMHistoryLogByMeetingID(connString, id, lang);
                userID = meeting.CreatedBy.GetValueOrDefault();
            }

            return meeting;
        }

        public MeetingListModel GetMeetingList(string connString, int pageNumber, int pageSize, string paramUserID, string paramRefNo, string paramSubject, string paramLocation, string paramMeetingType, DateTime? paramStartDate, DateTime? paramEndDate, string paramInvitees, string paramSmartSearch, string lang)
        {
            MeetingListModel list = new MeetingListModel();
            SqlParameter[] param = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_UserID", paramUserID),
                new SqlParameter("@P_Method", 0),
                new SqlParameter("@P_ReferenceNumber", paramRefNo),
                new SqlParameter("@P_Subject", paramSubject),
                new SqlParameter("@P_Location", paramLocation),
                new SqlParameter("@P_MeetingType", paramMeetingType),
                new SqlParameter("@P_StartDateTime", paramStartDate),
                new SqlParameter("@P_EndDateTime", paramEndDate),
                new SqlParameter("@P_Invitees", paramInvitees),
                new SqlParameter("@P_SmartSearch", paramSmartSearch),
                new SqlParameter("@P_Language", lang) };
            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<MeetingListViewModel>>(connString, "Get_MeetingList", r => r.TranslateAsMeetingDashboardList(), param);

            SqlParameter[] parama = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_UserID", paramUserID),
                new SqlParameter("@P_Method", 1),
                new SqlParameter("@P_ReferenceNumber", paramRefNo),
                new SqlParameter("@P_Subject", paramSubject),
                new SqlParameter("@P_Location", paramLocation),
                new SqlParameter("@P_MeetingType", paramMeetingType),
                new SqlParameter("@P_StartDateTime", paramStartDate),
                new SqlParameter("@P_EndDateTime", paramEndDate),
                new SqlParameter("@P_Invitees", paramInvitees),
                new SqlParameter("@P_SmartSearch", paramSmartSearch),
                new SqlParameter("@P_Language", lang)
            };
            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_MeetingList", parama);

            Parallel.Invoke(
                () => list.M_OrganizationList = GetM_Organisation(connString, lang),
                () => list.M_LookupsList = GetM_Lookups(connString, lang));
            return list;
        }

        public MeetingCalendarModel GetMeetingCalenderList(string connString, int paramUserID, string paramMonth, string paramYear, string paramRefNo, string paramSubject, string paramLocation, string paramMeetingType, DateTime? paramStartDate, DateTime? paramEndDate, string paramSmartSearch, string lang)
        {
            MeetingCalendarModel list = new MeetingCalendarModel();
            SqlParameter[] param = {
                new SqlParameter("@P_UserID", paramUserID),
                new SqlParameter("@P_Month", paramMonth),
                new SqlParameter("@P_Year", paramYear),
                new SqlParameter("@P_ReferenceNumber", paramRefNo),
                new SqlParameter("@P_Subject", paramSubject),
                new SqlParameter("@P_Location", paramLocation),
                new SqlParameter("@P_MeetingType", paramMeetingType),
                new SqlParameter("@P_StartDateTime", paramStartDate),
                new SqlParameter("@P_EndDateTime", paramEndDate),
                new SqlParameter("@P_SmartSearch", paramSmartSearch),
                new SqlParameter("@P_Language", lang),
            };
            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<MeetingCalendarViewModel>>(connString, "Get_MeetingCalenderList", r => r.TranslateAsMeetingCalenderDashboardList(), param);

            return list;
        }

        public MeetingPreviewModel GetMeetingPreview(string connString, int id, int userID, string lang)
        {
            MeetingPreviewModel meeting = new MeetingPreviewModel();
            SqlParameter[] param = {
                new SqlParameter("@P_MeetingID", id),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Language", lang)
            };
            if (id != 0)
            {
                meeting = SqlHelper.ExecuteProcedureReturnData<List<MeetingPreviewModel>>(connString, "Get_MeetingPreview", r => r.TranslateAsMeetingPreviewList(), param).FirstOrDefault();

                userID = meeting.CreatedBy.GetValueOrDefault();
            }

            return meeting;
        }

        public List<MeetingReportModel> GetMeetingReportExportList(string connString, MeetingReport report, string lang)
        {
            List<MeetingReportModel> list = new List<MeetingReportModel>();

            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", 1),
            new SqlParameter("@P_PageSize", 10),
            new SqlParameter("@P_Method", 2),
            new SqlParameter("@P_Invitees", report.Invitees),
            new SqlParameter("@P_ReferenceNumber", report.ReferenceNumber),
            new SqlParameter("@P_Subject", report.Subject),
            new SqlParameter("@P_Location", report.Location),
            new SqlParameter("@P_MeetingType", report.MeetingType),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_SmartSearch", report.SmartSearch),
            new SqlParameter("@P_Language", lang)
            };

            list = SqlHelper.ExecuteProcedureReturnData<List<MeetingReportModel>>(connString, "MeetingReport", r => r.TranslateAsMeetingReportList(), param);

            return list;
        }
    }
}
