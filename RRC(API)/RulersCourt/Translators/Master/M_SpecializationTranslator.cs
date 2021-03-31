using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_SpecializationTranslator
    {
        public static M_SpecializationModel TranslateAsGetSpecialization(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var specialization = new M_SpecializationModel();

            if (reader.IsColumnExists("SpecializationID"))
                specialization.SpecializationID = SqlHelper.GetNullableInt32(reader, "SpecializationID");

            if (reader.IsColumnExists("SpecializationName"))
                specialization.SpecializationName = SqlHelper.GetNullableString(reader, "SpecializationName");

            if (reader.IsColumnExists("DisplayOrder"))
                specialization.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return specialization;
        }

        public static List<M_SpecializationModel> TranslateAsSpecialization(this SqlDataReader reader)
        {
            var specializations = new List<M_SpecializationModel>();
            while (reader.Read())
            {
                specializations.Add(TranslateAsGetSpecialization(reader, true));
            }

            return specializations;
        }
    }
}
