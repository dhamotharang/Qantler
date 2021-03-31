using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_GradeTranslator
    {
        public static M_GradeModel TranslateAsGetGrade(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var grade = new M_GradeModel();

            if (reader.IsColumnExists("GradeID"))
                grade.GradeID = SqlHelper.GetNullableInt32(reader, "GradeID");

            if (reader.IsColumnExists("GradeName"))
                grade.GradeName = SqlHelper.GetNullableString(reader, "GradeName");

            if (reader.IsColumnExists("DisplayOrder"))
                grade.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return grade;
        }

        public static List<M_GradeModel> TranslateAsGrade(this SqlDataReader reader)
        {
            var gradeList = new List<M_GradeModel>();
            while (reader.Read())
            {
                gradeList.Add(TranslateAsGetGrade(reader, true));
            }

            return gradeList;
        }
    }
}
