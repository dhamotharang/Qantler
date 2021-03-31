using RulersCourt.Models.AdminManagement;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.AdminManagement
{
    public static class M_AdminManagementTranslator
    {
        public static M_AdminManagementGetModel TranslateAsGetAdminManagement(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var admin = new M_AdminManagementGetModel();

            if (reader.IsColumnExists("LookupsID"))
                admin.LookupsID = SqlHelper.GetNullableInt32(reader, "LookupsID");

            if (reader.IsColumnExists("DisplayName"))
                admin.DisplayName = SqlHelper.GetNullableString(reader, "DisplayName");

            if (reader.IsColumnExists("DisplayOrder"))
                admin.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return admin;
        }

        public static List<M_AdminManagementGetModel> TranslateAsAdminManagement(this SqlDataReader reader)
        {
            var admins = new List<M_AdminManagementGetModel>();
            while (reader.Read())
            {
                admins.Add(TranslateAsGetAdminManagement(reader, true));
            }

            return admins;
        }
    }
}
