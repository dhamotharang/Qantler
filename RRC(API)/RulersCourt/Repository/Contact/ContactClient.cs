using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.Contact;
using RulersCourt.Translators;
using RulersCourt.Translators.Contact;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Contact
{
    public class ContactClient
    {
        public ContactListModel GetInternalContact(string connString, int pageNumber, int pageSize, string type, string department, string userName, string entityName, string designation, string emailId, string phoneNumber, string govermentEntity, string lang)
        {
            ContactListModel list = new ContactListModel();
            SqlParameter[] param = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_Method", 0),
                new SqlParameter("@P_Type", type),
                new SqlParameter("@P_Department", department),
                new SqlParameter("@P_UserName", userName),
                new SqlParameter("@P_EntityName", entityName),
                new SqlParameter("@P_Designation", designation),
                new SqlParameter("@P_GovernmentEntity", govermentEntity),
                new SqlParameter("@P_EmailId", emailId),
                new SqlParameter("@P_PhoneNumber", phoneNumber)
            };

            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<ContactDashboardListModel>>(connString, "Get_ContactList", r => r.TranslateAsContactDashBoardList(), param);

            SqlParameter[] parama = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_Method", 1),
                new SqlParameter("@P_Type", type),
                new SqlParameter("@P_Department", department),
                new SqlParameter("@P_UserName", userName),
                new SqlParameter("@P_EntityName", entityName),
                new SqlParameter("@P_Designation", designation),
                new SqlParameter("@P_GovernmentEntity", govermentEntity),
                new SqlParameter("@P_EmailId", emailId),
                new SqlParameter("@P_PhoneNumber", phoneNumber)
            };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_ContactList", parama);

            Parallel.Invoke(
              () => list.OrganizationList = GetM_Organisation(connString, lang),
              () => list.LookupsList = GetM_Lookups(connString, lang));

            return list;
        }

        public ContactGetModel GetContactByID(string connString, int contactID, int userID, string lang)
        {
            ContactGetModel contactDetails = new ContactGetModel();

            SqlParameter[] getparam = {
                new SqlParameter("@P_ContactID", contactID)
            };
            if (contactID != 0)
            {
                contactDetails = SqlHelper.ExecuteProcedureReturnData<List<ContactGetModel>>(connString, "Get_ContactByID", r => r.TranslateAsContactList(), getparam).FirstOrDefault();
                contactDetails.HistoryLog = new ContactHistroyLogClient().ContactHistoryLogByContactID(connString, contactID);
                if (contactDetails != null)
                {
                    contactDetails.Attachments = new ContactAttachmentClient().GetContactAttachmentById(connString, contactDetails.ContactID, "Contact");
                    userID = contactDetails.CreatedBy.GetValueOrDefault();
                }
            }

            Parallel.Invoke(
              () => contactDetails.M_OrganizationList = GetM_Organisation(connString, lang),
              () => contactDetails.M_LookupsList = GetM_Lookups(connString, lang));

            return contactDetails;
        }

        public List<OrganizationModel> GetM_Organisation(string connString, string lang)
        {
            SqlParameter[] orgParam = {
                new SqlParameter("@P_Language", lang)
            };
            var e = SqlHelper.ExecuteProcedureReturnData<List<OrganizationModel>>(connString, "Get_Organization", r => r.TranslateAsOrganizationList(), orgParam);
            return e;
        }

        public List<M_LookupsModel> GetM_Lookups(string connString, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Type", "Contact"),
                new SqlParameter("@P_Language", lang)
            };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public ContactSaveResponseModel PostContact(string connString, ContactPostModel contact)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Type", contact.Type),
                new SqlParameter("@P_Department", contact.Department),
                new SqlParameter("@P_UserName", contact.UserName),
                new SqlParameter("@P_EntityName", contact.EntityName),
                new SqlParameter("@P_Designation", contact.Designation),
                new SqlParameter("@P_Section", contact.Section),
                new SqlParameter("@P_Unit", contact.Unit),
                new SqlParameter("@P_PhoneNumberExtension", contact.PhoneNumberExtension),
                new SqlParameter("@P_EmailId", contact.EmailId),
                new SqlParameter("@P_PhoneNumber", contact.PhoneNumber),
                new SqlParameter("@P_OfficialEntity", contact.OfficialEntity),
                new SqlParameter("@P_Comment", contact.Comments),
                new SqlParameter("@P_CreatedBy", contact.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", contact.CreatedDateTime),
                new SqlParameter("@P_ProfilePhotoID", contact.ProfilePhotoID),
                new SqlParameter("@P_ProfilePhotoName", contact.ProfilePhotoName)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<ContactSaveResponseModel>(connString, "Save_Contact", r => r.TranslateAsContactSaveResponseList(), param);

            if (contact.Attachments != null)
                new ContactAttachmentClient().PostContactAttachments(connString, "Contact", contact.Attachments, result.ContactID);

            return result;
        }

        public ContactSaveResponseModel PutContact(string connString, ContactPutModel contact)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_ContactID", contact.ContactID),
                new SqlParameter("@P_Department", contact.Department),
                new SqlParameter("@P_UserName", contact.UserName),
                new SqlParameter("@P_EntityName", contact.EntityName),
                new SqlParameter("@P_Section", contact.Section),
                new SqlParameter("@P_Unit", contact.Unit),
                new SqlParameter("@P_PhoneNumberExtension", contact.PhoneNumberExtension),
                new SqlParameter("@P_Designation", contact.Designation),
                new SqlParameter("@P_EmailId", contact.EmailId),
                new SqlParameter("@P_PhoneNumber", contact.PhoneNumber),
                new SqlParameter("@P_OfficialEntity", contact.OfficialEntity),
                new SqlParameter("@P_Comment", contact.Comments),
                new SqlParameter("@P_UpdatedBy", contact.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", contact.UpdatedDateTime),
                new SqlParameter("@P_ProfilePhotoID", contact.ProfilePhotoID),
                new SqlParameter("@P_ProfilePhotoName", contact.ProfilePhotoName)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<ContactSaveResponseModel>(connString, "Save_Contact", r => r.TranslateAsContactSaveResponseList(), param);

            if (contact.Attachments != null)
                new ContactAttachmentClient().PostContactAttachments(connString, "Contact", contact.Attachments, result.ContactID);

            return result;
        }

        public string DeleteContact(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_ContactID", id)
             };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_ContactByID", param);
        }

        public ContactPutModel GetPatchContactByID(string connString, int contactID)
        {
            ContactPutModel contactDetails = new ContactPutModel();

            SqlParameter[] getparam = {
                new SqlParameter("@P_ContactID", contactID)
            };
            if (contactID != 0)
            {
                contactDetails = SqlHelper.ExecuteProcedureReturnData<List<ContactPutModel>>(connString, "Get_ContactByID", r => r.TranslateAsPutContactList(), getparam).FirstOrDefault();

                contactDetails.Attachments = new ContactAttachmentClient().GetContactAttachmentById(connString, contactDetails.ContactID, "Contact");
            }

            return contactDetails;
        }

        internal ContactSaveResponseModel PatchContact(string connString, int id, JsonPatchDocument<ContactPutModel> value)
        {
            var result = GetPatchContactByID(connString, id);
            value.ApplyTo(result);
            var res = PutContact(connString, result);
            return res;
        }
    }
}
