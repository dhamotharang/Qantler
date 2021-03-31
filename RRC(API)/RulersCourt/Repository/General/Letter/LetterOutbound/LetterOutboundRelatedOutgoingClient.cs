using RulersCourt.Models.Letter;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RulersCourt.Repository
{
    public class LetterOutboundRelatedOutgoingClient
    {
        public void Save(string connString, List<LetterOutboundRelatedOutgoingModel> outgoingNumber, int? letterId, int letterType)
        {
            var result = string.Empty;

            if (outgoingNumber.Count != 0)
            {
                string referenceNumber = string.Join(",", outgoingNumber.Select(w => w.OutgoingLetterReferenceNo).ToArray());
                SqlParameter[] parama = { new SqlParameter("@P_LetterID", letterId),
                                        new SqlParameter("@P_LetterType", letterType),
                                        new SqlParameter("@P_OutgoingLetterReferenceNo", referenceNumber),
                                        new SqlParameter("@P_Type", "Delete") };

                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_LetterOutbound_RelatedOutgoing", parama);
            }
            else
            {
                SqlParameter[] parama1 = { new SqlParameter("@P_Type", "Delete"),
                                            new SqlParameter("@P_LetterID", letterId),
                                            new SqlParameter("@P_LetterType", letterType) };
                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_LetterOutbound_RelatedOutgoing", parama1);
            }

            foreach (LetterOutboundRelatedOutgoingModel data in outgoingNumber)
            {
                SqlParameter[] param2 = { new SqlParameter("@P_LetterID", letterId),
                                        new SqlParameter("@P_LetterType", letterType),
                                        new SqlParameter("@P_OutgoingLetterReferenceNo", data.OutgoingLetterReferenceNo) };
                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_LetterOutbound_RelatedOutgoing", param2);
            }
        }
    }
}
