using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Letter
{
    public static class LetterOutboundOrganizationEntityTranslator
    {
        public static OrganisationEntityModel TranslateAsGetOrganizationEntity(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var organizationEntity = new OrganisationEntityModel();

            if (reader.IsColumnExists("ContactID"))
                organizationEntity.ContactID = SqlHelper.GetNullableInt32(reader, "ContactID");

            if (reader.IsColumnExists("UserID"))
                organizationEntity.UserID = SqlHelper.GetNullableString(reader, "UserID");

            if (reader.IsColumnExists("UserName"))
                organizationEntity.UserName = SqlHelper.GetNullableString(reader, "UserName");

            if (reader.IsColumnExists("Email"))
                organizationEntity.Email = SqlHelper.GetNullableString(reader, "Email");

            return organizationEntity;
        }

        public static List<OrganisationEntityModel> TranslateAsGetOrganizationEntityList(this SqlDataReader reader)
        {
            var organizationEntityList = new List<OrganisationEntityModel>();
            while (reader.Read())
            {
                organizationEntityList.Add(TranslateAsGetOrganizationEntity(reader, true));
            }

            return organizationEntityList;
        }
    }
}
