using RulersCourt.Models.Meeting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Meeting
{
    public class MeetingInviteesClient
    {
        public void SaveInternalInvitees(string connString, List<MeetingInternalInviteesModel> internalInvitees, int? meetingId, int? userID, DateTime? date)
        {
            foreach (var item in internalInvitees)
            {
                if ((item.MeetingInternalInviteesID ?? 0) == 0)
                {
                    SqlParameter[] destinationparam = {
                 new SqlParameter("@P_MeetingInternalID", item.MeetingInternalInviteesID),
                 new SqlParameter("@P_DepartmentID", item.DepartmentID),
                 new SqlParameter("@P_MeetingID", meetingId),
                 new SqlParameter("@P_Name", item.UserID),
                 new SqlParameter("@P_UserID", userID),
                 new SqlParameter("@P_ActionDateTime", date)
            };
                    SqlHelper.ExecuteProcedureReturnString(connString, "Save_MeetingInternalInvitees", destinationparam);
                }

                if ((item.MeetingInternalInviteesID ?? 0) != 0)
                {
                    SqlParameter[] destinationparam = {
                 new SqlParameter("@P_MeetingInternalID", item.MeetingInternalInviteesID),
                 new SqlParameter("@P_DepartmentID", item.DepartmentID),
                 new SqlParameter("@P_MeetingID", meetingId),
                 new SqlParameter("@P_Name", item.UserID),
                 new SqlParameter("@P_DeleteFlag", 0),
                 new SqlParameter("@P_UserID", userID),
                 new SqlParameter("@P_ActionDateTime", date)
            };
                    SqlHelper.ExecuteProcedureReturnString(connString, "Save_MeetingInternalInvitees", destinationparam);
                }
            }
        }

        public void SaveExternalInvitees(string connString, List<MeetingExternalInviteesModel> internalInvitees, int? meetingId, int? userID, DateTime? date)
        {
            foreach (var item in internalInvitees)
            {
                if ((item.MeetingExternalInviteesID ?? 0) == 0)
                {
                    SqlParameter[] destinationparam = {
                 new SqlParameter("@P_MeetingExternalID", item.MeetingExternalInviteesID),
                 new SqlParameter("@P_Organization", item.Organization),
                 new SqlParameter("@P_MeetingID", meetingId),
                 new SqlParameter("@P_ContactPerson", item.ContactPerson),
                 new SqlParameter("@P_EmailID", item.EmailID),
                 new SqlParameter("@P_UserID", userID),
                 new SqlParameter("@P_ActionDateTime", date)
            };
                    SqlHelper.ExecuteProcedureReturnString(connString, "Save_MeetingExternalInvitees", destinationparam);
                }

                if ((item.MeetingExternalInviteesID ?? 0) != 0)
                {
                    SqlParameter[] destinationparam = {
                 new SqlParameter("@P_MeetingExternalID", item.MeetingExternalInviteesID),
                 new SqlParameter("@P_Organization", item.Organization),
                 new SqlParameter("@P_MeetingID", meetingId),
                 new SqlParameter("@P_ContactPerson", item.ContactPerson),
                 new SqlParameter("@P_EmailID", item.EmailID),
                 new SqlParameter("@P_DeleteFlag", 0),
                 new SqlParameter("@P_UserID", userID),
                 new SqlParameter("@P_ActionDateTime", date)
            };
                    SqlHelper.ExecuteProcedureReturnString(connString, "Save_MeetingExternalInvitees", destinationparam);
                }
            }
        }
    }
}
