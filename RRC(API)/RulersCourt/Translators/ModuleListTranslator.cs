using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class ModuleListTranslator
    {
        public static M_ModuleModel TranslateAsModule(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var model = new M_ModuleModel();

            if (reader.IsColumnExists("ModuleID"))
                model.ModuleID = SqlHelper.GetNullableInt32(reader, "ModuleID");

            if (reader.IsColumnExists("ModuleName"))
                model.ModuleName = SqlHelper.GetNullableString(reader, "ModuleName");

            return model;
        }

        public static List<M_ModuleModel> TranslateAsModuleList(this SqlDataReader reader)
        {
            var moduleList = new List<M_ModuleModel>();
            while (reader.Read())
            {
                moduleList.Add(TranslateAsModule(reader, true));
            }

            return moduleList;
        }
    }
}
