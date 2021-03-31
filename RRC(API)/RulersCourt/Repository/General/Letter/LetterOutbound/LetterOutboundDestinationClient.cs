using RulersCourt.Models.Letter;
using RulersCourt.Translators.Letter;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RulersCourt.Repository
{
    public class LetterOutboundDestinationClient
    {
        public void SaveDepartment(string connString, List<LetterOutboundDestinationDepartmentModel> destinationEntity, int? letterId)
        {
            SqlParameter[] getDestinationparam = {
                    new SqlParameter("@P_LetterID", letterId) };
            var oldDestinationDepartment = SqlHelper.ExecuteProcedureReturnData<List<LetterOutboundDestinationDepartmentModel>>(connString, "Get_LetterOutboundDestinationEntity", r => r.TranslateAsLetterDestinationDepartmentList(), getDestinationparam);

            SqlParameter[] destinationDeleteparam = {
                 new SqlParameter("@P_AllLetterDestinationID", string.Join(",", from item in destinationEntity select item.LetterDestinationID)),
                 new SqlParameter("@P_LetterID", letterId),
                 new SqlParameter("@P_Type", 2) };
            SqlHelper.ExecuteProcedureReturnString(connString, "Save_LetterOutboundDestinationEntity", destinationDeleteparam);

            foreach (var item in destinationEntity)
            {
                SqlParameter[] destinationparam = {
                 new SqlParameter("@P_LetterDestinationID", item.LetterDestinationID),
                 new SqlParameter("@P_LetterID", letterId),
                 new SqlParameter("@P_EntityID", item.LetterDestinationEntityID),
                 new SqlParameter("@P_IsGovernmentEntity", item.IsGovernmentEntity),
                 new SqlParameter("@P_UserName", item.LetterDestinationUserName),
                 new SqlParameter("@P_SenderName", item.LetterDestinationUserName),
                 new SqlParameter("@P_Type", 1) };
                SqlHelper.ExecuteProcedureReturnString(connString, "Save_LetterOutboundDestinationEntity", destinationparam);
            }
        }
    }
}
