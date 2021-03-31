using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_EducationTranslator
    {
        public static M_EducationModel TranslateAsGetEducation(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var education = new M_EducationModel();

            if (reader.IsColumnExists("EducationID"))
                education.EducationID = SqlHelper.GetNullableInt32(reader, "EducationID");

            if (reader.IsColumnExists("EducationName"))
                education.EducationName = SqlHelper.GetNullableString(reader, "EducationName");

            if (reader.IsColumnExists("DisplayOrder"))
                education.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return education;
        }

        public static List<M_EducationModel> TranslateAsEducation(this SqlDataReader reader)
        {
            var education = new List<M_EducationModel>();
            while (reader.Read())
            {
                education.Add(TranslateAsGetEducation(reader, true));
            }

            return education;
        }
    }
}
