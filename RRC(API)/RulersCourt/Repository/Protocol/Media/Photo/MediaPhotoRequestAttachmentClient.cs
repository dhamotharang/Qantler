using RulersCourt.Models.Photo;
using RulersCourt.Translators.Protocol.Media.PhotoRequestTranslators;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RulersCourt.Repository.Protocol.Media.PhotoRequest
{
    public class MediaPhotoRequestAttachmentClient
    {
        public List<PhotoAttachmentGetModel> GetPhotoAttachmentById(string connString, int? serviceID, string type)
        {
            SqlParameter[] param = { new SqlParameter("@P_ServiceID", serviceID),
                                     new SqlParameter("@Type", type)
            };

            return SqlHelper.ExecuteProcedureReturnData<List<PhotoAttachmentGetModel>>(connString, "Get_AttachmentByID", r => r.TranslateAsPhotoAttachmentList(), param);
        }

        public string PostPhotoAttachments(string connString, string type, List<PhotoAttachmentGetModel> datas, int? serviceID)
        {
            string result = string.Empty;
            if (datas.Count != 0)
            {
                SqlParameter[] parama = { new SqlParameter("@P_Type", "Delete"),
                                         new SqlParameter("@P_ServiceType", type),
                                         new SqlParameter("@P_ServiceID", serviceID),
                                         new SqlParameter("@P_AttachmentGuid", string.Join(",", from item in datas select item.AttachmentGuid)),
                                         new SqlParameter("@P_AttachmentsName", string.Join(",", from item in datas select item.AttachmentsName)) };

                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_Attachments", parama);
            }
            else
            {
                SqlParameter[] parama1 = { new SqlParameter("@P_Type", "Delete"),
                                         new SqlParameter("@P_ServiceType", type),
                                         new SqlParameter("@P_ServiceID", serviceID) };
                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_Attachments", parama1);
            }

            foreach (PhotoAttachmentGetModel data in datas)
            {
                SqlParameter[] param = { new SqlParameter("@P_Type", type),
                                         new SqlParameter("@P_ServiceID", serviceID),
                                         new SqlParameter("@P_AttachmentGuid", data.AttachmentGuid),
                                         new SqlParameter("@P_AttachmentsName", data.AttachmentsName)
                                        };
                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_Attachments", param);
            }

            return result;
        }
    }
}
