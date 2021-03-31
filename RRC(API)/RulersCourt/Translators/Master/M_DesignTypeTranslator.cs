using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_DesignTypeTranslator
    {
        public static M_DesignTypeModel TranslateAsGetDesignType(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var designType = new M_DesignTypeModel();

            if (reader.IsColumnExists("DesignTypeID"))
                designType.DesignTypeID = SqlHelper.GetNullableInt32(reader, "DesignTypeID");

            if (reader.IsColumnExists("DesignTypeName"))
                designType.DesignTypeName = SqlHelper.GetNullableString(reader, "DesignTypeName");

            if (reader.IsColumnExists("DisplayOrder"))
                designType.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return designType;
        }

        public static List<M_DesignTypeModel> TranslateAsDesignType(this SqlDataReader reader)
        {
            var designType = new List<M_DesignTypeModel>();
            while (reader.Read())
            {
                designType.Add(TranslateAsGetDesignType(reader, true));
            }

            return designType;
        }
    }
}
