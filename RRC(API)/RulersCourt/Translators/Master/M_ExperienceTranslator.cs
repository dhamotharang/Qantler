using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_ExperienceTranslator
    {
        public static M_ExperienceModel TranslateAsGetExperience(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var experience = new M_ExperienceModel();

            if (reader.IsColumnExists("ExperienceID"))
                experience.ExperienceID = SqlHelper.GetNullableInt32(reader, "ExperienceID");

            if (reader.IsColumnExists("ExperienceName"))
                experience.ExperienceName = SqlHelper.GetNullableString(reader, "ExperienceName");

            if (reader.IsColumnExists("DisplayOrder"))
                experience.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return experience;
        }

        public static List<M_ExperienceModel> TranslateAsExperience(this SqlDataReader reader)
        {
            var experience = new List<M_ExperienceModel>();
            while (reader.Read())
            {
                experience.Add(TranslateAsGetExperience(reader, true));
            }

            return experience;
        }
    }
}
