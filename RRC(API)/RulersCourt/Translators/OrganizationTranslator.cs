using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class OrganizationTranslator
    {
        public static OrganizationModel TranslateAsOrganization(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var organizationModel = new OrganizationModel();

            if (reader.IsColumnExists("OrganizationID"))
                organizationModel.OrganizationID = SqlHelper.GetNullableInt32(reader, "OrganizationID");

            if (reader.IsColumnExists("OrganizationUnits"))
                organizationModel.OrganizationUnits = SqlHelper.GetNullableString(reader, "OrganizationUnits");

            return organizationModel;
        }

        public static List<OrganizationModel> TranslateAsOrganizationList(this SqlDataReader reader)
        {
            var organizationList = new List<OrganizationModel>();
            while (reader.Read())
            {
                organizationList.Add(TranslateAsOrganization(reader, true));
            }

            return organizationList;
        }
    }
}
