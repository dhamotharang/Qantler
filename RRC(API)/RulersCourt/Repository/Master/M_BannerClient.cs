using RulersCourt.Models.Master.M_Photos;
using RulersCourt.Translators.Master;
using System;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Master
{
    public class M_BannerClient
    {
        public int PostBanner(string connString, M_BannerPostModel banner)
        {
            SqlParameter[] parama = { new SqlParameter("@P_AttachmentGuid", banner.AttachmentGuid),
                                    new SqlParameter("@P_AttachmentName", banner.AttachmentName),
                                    new SqlParameter("@P_CreatedBy", banner.CreatedBy),
                                    new SqlParameter("@P_CreatedDateTime", banner.CreatedDateTime) };
            var result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_Banner", parama);
            return Convert.ToInt32(result);
        }

        public M_BannerGetModel GetBanner(string connString)
        {
            M_BannerGetModel photo = new M_BannerGetModel();
            photo = SqlHelper.ExecuteProcedureReturnData<M_BannerGetModel>(connString, "Get_Banner", r => r.TranslateAsBannerList());
            return photo;
        }
    }
}
