using RulersCourt.Models;
using RulersCourt.Repository.Master;
using RulersCourt.Repository.Master.Vehicle;
using RulersCourt.Translators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RulersCourt.Repository
{
    public class M_MasterLookupClient
    {
        public List<M_MasterLookupsGetModel> GetMasterLookups(string connString, int userID, int? type, string search, string language)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Category", type),
                                    new SqlParameter("@P_Search", search),
                                    new SqlParameter("@P_Language", language) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_MasterLookupsGetModel>>(connString, "Get_MasterAdminManagement", r => r.TranslateAsMasterLookups(), parama);
        }

        public string PostMasterLookup(string connString, int userID, M_MasterLookupsPostModel data, int countryID, int? type, string language, int emirates)
        {
            switch (type)
            {
                case 1:
                    return new M_SpecializationClient().PostSpecialization(connString, userID, data, language);
                case 2:
                    return new M_ReligionClient().PostReligion(connString, userID, data, language);
                case 3:
                    return new M_OfficialTaskRequestClient().PostOfficialTaskRequest(connString, userID, data, language);
                case 4:
                    return new M_NationalityClient().PostNationality(connString, userID, data, language);
                case 5:
                    return new M_MediaChannelClient().PostMediaChannel(connString, userID, data, language);
                case 6:
                    return new M_LocationClient().PostLocation(connString, userID, data, language);
                case 7:
                    return new M_LanguageClient().PostLanguage(connString, userID, data, language);
                case 8:
                    return new M_ExperienceClient().PostExperience(connString, userID, data, language);
                case 9:
                    return new M_EmployeeStatusClient().PostEmployeeStatus(connString, userID, data, language);
                case 10:
                    return new M_EmiratesClient().PostEmirates(connString, userID, data, language);
                case 11:
                    return new M_EducationClient().PostEducation(connString, userID, data, language);
                case 12:
                    return new M_DesignTypeClient().PostDesignType(connString, userID, data, language);
                case 13:
                    return new M_CountryClient().PostCountry(connString, userID, data, language);
                case 14:
                    return new M_CityClient().PostCity(connString, userID, data, countryID, language, emirates);
                case 15:
                    return new M_EventTypeClient().PostEventType(connString, userID, data, language);
                case 16:
                    return new M_MeetingClient().PostMeetingType(connString, userID, data, language);
                case 17:
                    return new M_LeaveTypeClient().PostLeaveType(connString, userID, data, language);

                case 18:
                    return new M_TripTypeClient().PostTripType(connString, userID, data, language);

                case 19:
                    return new M_VehicleModelClient().PostVehicleModel(connString, userID, data, language);

                case 20:
                    return new M_AnnouncementTypeClient().PostAnnouncementTypeModel(connString, userID, data, language);

                default:
                    return "Error";
            }
        }

        public string PutMasterlookup(string connString, int userID, M_MasterLookupsPutModel data, int countryID, int? type, string language, int emirates)
        {
            switch (type)
            {
                case 1:
                    return new M_SpecializationClient().PutSpecialization(connString, userID, data, language);
                case 2:
                    return new M_ReligionClient().PutReligion(connString, userID, data, language);
                case 3:
                    return new M_OfficialTaskRequestClient().PutOfficialTaskRequest(connString, userID, data, language);
                case 4:
                    return new M_NationalityClient().PutNationality(connString, userID, data, language);
                case 5:
                    return new M_MediaChannelClient().PutMediaChannel(connString, userID, data, language);
                case 6:
                    return new M_LocationClient().PutLocation(connString, userID, data, language);
                case 7:
                    return new M_LanguageClient().PutLanguage(connString, userID, data, language);
                case 8:
                    return new M_ExperienceClient().PutExperience(connString, userID, data, language);
                case 9:
                    return new M_EmployeeStatusClient().PutEmployeeStatus(connString, userID, data, language);
                case 10:
                    return new M_EmiratesClient().PutEmirates(connString, userID, data, language);
                case 11:
                    return new M_EducationClient().PutEducation(connString, userID, data, language);
                case 12:
                    return new M_DesignTypeClient().PutDesignType(connString, userID, data, language);
                case 13:
                    return new M_CountryClient().PutCountry(connString, userID, data, language);
                case 14:
                    return new M_CityClient().PutCity(connString, userID, data, countryID, language, emirates);
                case 15:
                    return new M_EventTypeClient().PutEventType(connString, userID, data, language);
                case 16:
                    return new M_MeetingClient().PutMeetingType(connString, userID, data, language);
                case 17:
                    return new M_LeaveTypeClient().PutLeaveType(connString, userID, data, language);

                case 18:
                    return new M_TripTypeClient().PutTripType(connString, userID, data, language);
                case 19:
                    return new M_VehicleModelClient().PutVehicleModel(connString, userID, data, language);
                case 20:
                    return new M_AnnouncementTypeClient().PutAnnouncementTypeModel(connString, userID, data, language);
                case 21:
                    return new M_AnnouncementTypeClient().PutAnnouncementTypeDescModel(connString, userID, data, language);

                default:
                    return "Error";
            }
        }

        public string DeleteMasterlookup(string connString, int userID, int lookupID, int? type)
        {
            switch (type)
            {
                case 1:
                    return new M_SpecializationClient().DeleteSpecialization(connString, userID, lookupID);
                case 2:
                    return new M_ReligionClient().DeleteReligion(connString, userID, lookupID);
                case 3:
                    return new M_OfficialTaskRequestClient().DeleteOfficialTaskRequest(connString, userID, lookupID);
                case 4:
                    return new M_NationalityClient().DeleteNationality(connString, userID, lookupID);
                case 5:
                    return new M_MediaChannelClient().DeleteMediaChannel(connString, userID, lookupID);
                case 6:
                    return new M_LocationClient().DeleteLocation(connString, userID, lookupID);
                case 7:
                    return new M_LanguageClient().DeleteLanguage(connString, userID, lookupID);
                case 8:
                    return new M_ExperienceClient().DeleteEmployeeStatus(connString, userID, lookupID);
                case 9:
                    return new M_EmployeeStatusClient().DeleteEmployeeStatus(connString, userID, lookupID);
                case 10:
                    return new M_EmiratesClient().DeleteEmirates(connString, userID, lookupID);
                case 11:
                    return new M_EducationClient().DeleteEducation(connString, userID, lookupID);
                case 12:
                    return new M_DesignTypeClient().DeleteDesignType(connString, userID, lookupID);
                case 13:
                    return new M_CountryClient().DeleteCountry(connString, userID, lookupID);
                case 14:
                    return new M_CityClient().DeleteCity(connString, userID, lookupID);
                case 15:
                    return new M_EventTypeClient().DeleteEventType(connString, userID, lookupID);
                case 16:
                    return new M_MeetingClient().DeleteMeetingType(connString, userID, lookupID);
                case 17:
                    return new M_LeaveTypeClient().DeleteLeaveType(connString, userID, lookupID);
                case 18:
                    return new M_TripTypeClient().DeleteTripType(connString, userID, lookupID);
                case 19:
                    return new M_VehicleModelClient().DeleteVehicleModel(connString, userID, lookupID);
                case 20:
                    return new M_AnnouncementTypeClient().DeleteAnnouncementTypeModel(connString, userID, lookupID);
                case 21:
                    return new M_AnnouncementTypeClient().DeleteAnnouncementTypeDescModel(connString, userID, lookupID);
                default:
                    return "Error";
            }
        }

        public List<int?> GetApprovers(string connString, int userID, int? department)
        {
            SqlParameter[] parama = {
                new SqlParameter("@P_Department", department) };
            return SqlHelper.ExecuteProcedureReturnData<List<int?>>(connString, "Get_M_ApproverConfiguration", r => r.TranslateAsApproverConfiguration(), parama);
        }

        public bool CanApproverRemoved(string connString, int userID, M_ApproverBindingModel data)
        {
            string approverID = string.Join(";", data.ApproverID);
            SqlParameter[] parama = {
                new SqlParameter("@P_ApproverID", approverID)
            };
            var res = SqlHelper.ExecuteProcedureReturnString(connString, "Get_CanApproverRemoved", parama);
            return Convert.ToBoolean(res);
        }

        public string SaveApprovers(string connString, int userID, int? department, M_ApproverConfigurationModel data)
        {
            var approver = string.Join(";", from item in data.ApproverID select item);
            SqlParameter[] parama = { new SqlParameter("@P_Department", department),
                                    new SqlParameter("@P_ApproverID", approver),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_ApproverConfiguration", parama);
        }

        public List<M_DepartmentModel> GetDepartments(string connString, int userID, string language)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", language) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_DepartmentModel>>(connString, "Get_M_ApproverDepartments", r => r.TranslateAsDepartments(), parama);
        }

        public List<M_SectionModel> GetSections(string connString, int userID, string language)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", language) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_SectionModel>>(connString, "Get_M_Section", r => r.TranslateAsSections(), parama);
        }

        public List<M_UnitModel> GetUnits(string connString, int userID, string language)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", language) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_UnitModel>>(connString, "Get_M_Unit", r => r.TranslateAsUnits(), parama);
        }

        public List<M_UserModel> GetUsers(string connString, int userID, string search, int departmentID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_search", search),
                                     new SqlParameter("@P_DepartmentID", departmentID),
                                     new SqlParameter("@P_Language", lang),
            };
            return SqlHelper.ExecuteProcedureReturnData<List<M_UserModel>>(connString, "Get_M_User", r => r.TranslateAsUsers(), parama);
        }

        public M_UserManagementListModel GetUserManagement(string connString, int userID, string search, int pageNumber, int pagesize, string lang)
        {
            M_UserManagementListModel list = new M_UserManagementListModel();
            SqlParameter[] parama = { new SqlParameter("@P_search", search),
                                    new SqlParameter("@P_Method", 1),
                                    new SqlParameter("@P_PageNumber", pageNumber),
                                    new SqlParameter("@P_PageSize", pagesize),
                                    new SqlParameter("@P_Language", lang)};
            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<M_UserManagementModel>>(connString, "Get_M_UserManagement", r => r.TranslateAsUserManagement(), parama);
            SqlParameter[] countparama = { new SqlParameter("@P_search", search),
                                    new SqlParameter("@P_Method", 0),
                                    new SqlParameter("@P_Language", lang),
            };
            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_M_UserManagement", countparama);
            return list;
        }

        public string SaveUsers(string connString, int userID, M_UserManagementModel data)
        {
            SqlParameter[] parama = {
                new SqlParameter("@P_UserProfileID", data.UserProfileID),
                new SqlParameter("@P_EmployeeName", data.EmployeeName),
                new SqlParameter("@P_DepartmentID", data.DepartmentID),
                new SqlParameter("@P_SectionID", data.SectionID),
                new SqlParameter("@P_UnitID", data.UnitID),
                new SqlParameter("@P_HOD", data.HOD),
                new SqlParameter("@P_HOS", data.HOS),
                new SqlParameter("@P_HOU", data.HOU),
                new SqlParameter("@P_CanRaiseOfficalRequest", data.CanRaiseOfficalRequest),
                new SqlParameter("@P_CanManageNews", data.CanManageNews),
                new SqlParameter("@P_UpdateBy", userID),
                new SqlParameter("@P_balanceLeave", data.BalanceLeave),
                new SqlParameter("@P_CanEditContact", data.CanEditContact) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_UserManagement", parama);
        }

        public string SaveMailremainders(string connString, int userID, M_MailRemainderModel data)
        {
            SqlParameter[] parama = {
                new SqlParameter("@P_Value", data.MailRemainder),
                new SqlParameter("@P_CreatedBy", userID),
                new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_MailRemainder", parama);
        }

        public M_MailRemainderModel GetMailremainders(string connString, int userID)
        {
            string res = SqlHelper.ExecuteProcedureReturnString(connString, "Get_M_MailRemainder");
            M_MailRemainderModel result = new M_MailRemainderModel();
            if (res != null)
                result.MailRemainder = res;
            return result;
        }
    }
}