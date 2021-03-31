using RulersCourt.Models.Protocol.Media.DiwanIdentity;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.Media.DiwanIdentity
{
    public static class DiwanIdentitySaveResponseTranslator
    {
        public static DiwanIdentityWorkflowModel TranslateAsDiwanIdentitySaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var diwanIdentitySave = new DiwanIdentityWorkflowModel();

            if (reader.IsColumnExists("DiwanIdentityID"))
                diwanIdentitySave.DiwanIdentityID = SqlHelper.GetNullableInt32(reader, "DiwanIdentityID");

            if (reader.IsColumnExists("ReferenceNumber"))
                diwanIdentitySave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                diwanIdentitySave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                diwanIdentitySave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return diwanIdentitySave;
        }

        public static DiwanIdentityWorkflowModel TranslateAsDiwanIdentitySaveResponseList(this SqlDataReader reader)
        {
            var diwanIdentitySaveResponse = new DiwanIdentityWorkflowModel();
            while (reader.Read())
            {
                diwanIdentitySaveResponse = TranslateAsDiwanIdentitySaveResponse(reader, true);
            }

            return diwanIdentitySaveResponse;
        }
    }
}
