using RulersCourt.Models.Contact;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Contact
{
    public static class ContactTranslator
    {
        public static ContactDashboardListModel ContactDashBoardListSet(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var contactModel = new ContactDashboardListModel();

            if (reader.IsColumnExists("ContactID"))
                contactModel.ContactID = SqlHelper.GetNullableInt32(reader, "ContactID");

            if (reader.IsColumnExists("Department"))
                contactModel.Department = SqlHelper.GetNullableString(reader, "Department");

            if (reader.IsColumnExists("UserName"))
                contactModel.UserName = SqlHelper.GetNullableString(reader, "UserName");

            if (reader.IsColumnExists("EntityName"))
                contactModel.EntityName = SqlHelper.GetNullableString(reader, "EntityName");

            if (reader.IsColumnExists("Designation"))
                contactModel.Designation = SqlHelper.GetNullableString(reader, "Designation");

            if (reader.IsColumnExists("EmailId"))
                contactModel.EmailId = SqlHelper.GetNullableString(reader, "EmailId");

            if (reader.IsColumnExists("PhoneNumber"))
                contactModel.PhoneNumber = SqlHelper.GetNullableString(reader, "PhoneNumber");

            if (reader.IsColumnExists("Photo"))
                contactModel.Photo = SqlHelper.GetNullableString(reader, "Photo");

            if (reader.IsColumnExists("ProfilePhotoID"))
                contactModel.ProfilePhotoID = SqlHelper.GetNullableString(reader, "ProfilePhotoID");

            if (reader.IsColumnExists("ProfilePhotoName"))
                contactModel.ProfilePhotoName = SqlHelper.GetNullableString(reader, "ProfilePhotoName");

            return contactModel;
        }

        public static List<ContactDashboardListModel> TranslateAsContactDashBoardList(this SqlDataReader reader)
        {
            var list = new List<ContactDashboardListModel>();
            while (reader.Read())
            {
                list.Add(ContactDashBoardListSet(reader, true));
            }

            return list;
        }

        public static ContactGetModel TranslateAsGetContact(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var contactModel = new ContactGetModel();

            if (reader.IsColumnExists("ContactID"))
                contactModel.ContactID = SqlHelper.GetNullableInt32(reader, "ContactID");

            if (reader.IsColumnExists("ReferenceNumber"))
                contactModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Type"))
                contactModel.Type = SqlHelper.GetNullableString(reader, "Type");

            if (reader.IsColumnExists("Department"))
                contactModel.Department = SqlHelper.GetNullableInt32(reader, "Department");

            if (reader.IsColumnExists("Section"))
                contactModel.Section = SqlHelper.GetNullableInt32(reader, "Section");

            if (reader.IsColumnExists("Unit"))
                contactModel.Unit = SqlHelper.GetNullableInt32(reader, "Unit");

            if (reader.IsColumnExists("PhoneNumberExtension"))
                contactModel.PhoneNumberExtension = SqlHelper.GetNullableString(reader, "PhoneNumberExtension");

            if (reader.IsColumnExists("UserName"))
                contactModel.UserName = SqlHelper.GetNullableString(reader, "UserName");

            if (reader.IsColumnExists("EntityName"))
                contactModel.EntityName = SqlHelper.GetNullableString(reader, "EntityName");

            if (reader.IsColumnExists("Designation"))
                contactModel.Designation = SqlHelper.GetNullableString(reader, "Designation");

            if (reader.IsColumnExists("EmailId"))
                contactModel.EmailId = SqlHelper.GetNullableString(reader, "EmailId");

            if (reader.IsColumnExists("PhoneNumber"))
                contactModel.PhoneNumber = SqlHelper.GetNullableString(reader, "PhoneNumber");

            if (reader.IsColumnExists("ProfilePhotoID"))
                contactModel.ProfilePhotoID = SqlHelper.GetNullableString(reader, "ProfilePhotoID");

            if (reader.IsColumnExists("ProfilePhotoName"))
                contactModel.ProfilePhotoName = SqlHelper.GetNullableString(reader, "ProfilePhotoName");

            if (reader.IsColumnExists("OfficialEntity"))
                contactModel.OfficialEntity = SqlHelper.GetBoolean(reader, "OfficialEntity");

            if (reader.IsColumnExists("CreatedBy"))
                contactModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                contactModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                contactModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                contactModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("DeleteFlag"))
                contactModel.DeleteFlag = SqlHelper.GetBoolean(reader, "DeleteFlag");
            return contactModel;
        }

        public static List<ContactGetModel> TranslateAsContactList(this SqlDataReader reader)
        {
            var contactList = new List<ContactGetModel>();
            while (reader.Read())
            {
                contactList.Add(TranslateAsGetContact(reader, true));
            }

            return contactList;
        }

        public static ContactPutModel TranslateAsPutContact(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var contactModel = new ContactPutModel();

            if (reader.IsColumnExists("ContactID"))
                contactModel.ContactID = SqlHelper.GetNullableInt32(reader, "ContactID");

            if (reader.IsColumnExists("Department"))
                contactModel.Department = SqlHelper.GetNullableInt32(reader, "Department");

            if (reader.IsColumnExists("Section"))
                contactModel.Section = SqlHelper.GetNullableInt32(reader, "Section");

            if (reader.IsColumnExists("Unit"))
                contactModel.Unit = SqlHelper.GetNullableInt32(reader, "Unit");

            if (reader.IsColumnExists("PhoneNumberExtension"))
                contactModel.PhoneNumberExtension = SqlHelper.GetNullableString(reader, "PhoneNumberExtension");

            if (reader.IsColumnExists("UserName"))
                contactModel.UserName = SqlHelper.GetNullableString(reader, "UserName");

            if (reader.IsColumnExists("EntityName"))
                contactModel.EntityName = SqlHelper.GetNullableString(reader, "EntityName");

            if (reader.IsColumnExists("EmailId"))
                contactModel.EmailId = SqlHelper.GetNullableString(reader, "EmailId");

            if (reader.IsColumnExists("PhoneNumber"))
                contactModel.PhoneNumber = SqlHelper.GetNullableString(reader, "PhoneNumber");

            if (reader.IsColumnExists("OfficialEntity"))
                contactModel.OfficialEntity = SqlHelper.GetBoolean(reader, "OfficialEntity");

            if (reader.IsColumnExists("UpdatedBy"))
                contactModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                contactModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Comments"))
                contactModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("ProfilePhotoID"))
                contactModel.ProfilePhotoID = SqlHelper.GetNullableString(reader, "ProfilePhotoID");

            if (reader.IsColumnExists("ProfilePhotoName"))
                contactModel.ProfilePhotoName = SqlHelper.GetNullableString(reader, "ProfilePhotoName");

            return contactModel;
        }

        public static List<ContactPutModel> TranslateAsPutContactList(this SqlDataReader reader)
        {
            var contactList = new List<ContactPutModel>();
            while (reader.Read())
            {
                contactList.Add(TranslateAsPutContact(reader, true));
            }

            return contactList;
        }
    }
}
