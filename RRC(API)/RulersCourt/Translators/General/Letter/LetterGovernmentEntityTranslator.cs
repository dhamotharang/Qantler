using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.General.Letter
{
    public static class LetterGovernmentEntityTranslator
    {
        public static GovernmentEntityModel TranslateAsGetEntity(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var releatedLetter = new GovernmentEntityModel();

            if (reader.IsColumnExists("EntityName"))
                releatedLetter.EntityName = SqlHelper.GetNullableString(reader, "EntityName");

            if (reader.IsColumnExists("UserName"))
                releatedLetter.UserName = SqlHelper.GetNullableString(reader, "UserName");

            if (reader.IsColumnExists("ID"))
                releatedLetter.ID = SqlHelper.GetNullableInt32(reader, "ID");

            return releatedLetter;
        }

        public static List<GovernmentEntityModel> TranslateAsEntity(this SqlDataReader reader)
        {
            var entityList = new List<GovernmentEntityModel>();
            while (reader.Read())
            {
                entityList.Add(TranslateAsGetEntity(reader, true));
            }

            return entityList;
        }
    }
}
