using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Core.Helpers;
using Microsoft.Extensions.Configuration;
using Core.Models;
using Core.Enums;
using OERService.Models;
using Serilog;

namespace OERService.DataAccess
{
    public class AppDataDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

		private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public AppDataDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<Gender> GetGenders()
        {
         
            List<Gender> genders = new List<Gender>();
            Gender gender = new Gender();
            gender.Id = 1;
            gender.Name = "Male";
            gender.Name_Ar = "ذكر";
            genders.Add(gender);
            gender = new Gender();
            gender.Id = 2;
            gender.Name = "Female";
            gender.Name_Ar = "أنثى";
            genders.Add(gender);
            return genders;
        }
        public async Task<DatabaseResponse> GetApprovalCount()
        {
            try
            {

                _DataHelper = new DataAccessHelper("sps_GetCommunityApproveRejectCount", _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);
                CommunityApproveRejectCount communityApproveRejectCount = new CommunityApproveRejectCount();

                if (dt != null && dt.Rows.Count > 0)
                {
                    //titles
                    communityApproveRejectCount = (from model in dt.AsEnumerable()
                                                   select new CommunityApproveRejectCount()
                                                   {
                                                       ApproveCount = model.Field<int?>("ApproveCount"),
                                                       RejectCount = model.Field<int?>("RejectCount"),
                                                       LastUpdatedBy = model.Field<int?>("LastUpdatedBy"),
                                                       LastUpdatedOn = model.Field<DateTime?>("LastUpdatedOn"),
                                                   }).FirstOrDefault();
                    
                }
                return new DatabaseResponse { ResponseCode = result, Results = communityApproveRejectCount };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));
                throw;
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> UpdateCommunityApprovalCount(UpdateCount updateCount)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@ApproveCount",  SqlDbType.Int ),
                    new SqlParameter( "@RejectCount",  SqlDbType.Int ),
                    new SqlParameter( "@UserId",  SqlDbType.Int )

                };

                parameters[0].Value = updateCount.ApproveCount;
                parameters[1].Value = updateCount.RejectCount;
                parameters[2].Value = updateCount.UserId;
                _DataHelper = new DataAccessHelper("spu_CommunityApproveRejectCount", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);
                return new DatabaseResponse { ResponseCode = result, Results = null };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));
                throw;
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }
        public async Task <DatabaseResponse> GetProfileAppData()
        {
            try
            {             

                _DataHelper = new DataAccessHelper("GetProfileAppData", _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                List<Title> titles = new List<Title>();

                List<Country> countries = new List<Country>();

                List<State> states = new List<State>();

                List<SocialMedia> socialMedias = new List<SocialMedia>();

                if (ds != null && ds.Tables.Count> 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count>0)
                {
                    //titles
                    titles = (from model in ds.Tables[0].AsEnumerable()
                                  select new Title()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Name = model.Field<string>("Name"),
                                      Name_Ar = model.Field<string>("Name_Ar"),
                                  }).ToList();
                }



                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    // country
                    countries = (from model in ds.Tables[1].AsEnumerable()
                              select new Country()
                              {
                                  Id = model.Field<int>("Id"),
                                  Name = model.Field<string>("Name"),
                                  Name_Ar = model.Field<string>("Name_Ar"),
                              }).ToList();
                }



                if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {
                    // state
                    states = (from model in ds.Tables[2].AsEnumerable()
                              select new State()
                              {
                                  Id = model.Field<int>("Id"),
                                  CountryId = model.Field<int>("CountryId"),
                                  Name = model.Field<string>("Name"),
                                  Name_Ar = model.Field<string>("Name_Ar"),
                              }).ToList();
                }



                if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                {
                    // socialmedia
                    socialMedias = (from model in ds.Tables[3].AsEnumerable()
                              select new SocialMedia()
                              {
                                  Id = model.Field<int>("Id"),
                                  Name = model.Field<string>("Name"),
                                  Name_Ar = model.Field<string>("Name_Ar")
                              }).ToList();
                }

                List<Gender> genders = this.GetGenders();

                List<PortalLanguage> portalLanguages = ((PortalLanguageType[])Enum.GetValues(typeof(PortalLanguageType))).Select(c => new PortalLanguage() { Id = (int)c, Name = c.ToString() }).ToList();

                ProfileAppData appData = new ProfileAppData { Titles=titles, Countries=countries, States=states, SocialMedias=socialMedias, Genders=genders, PortalLanguages=portalLanguages};
                
                return new DatabaseResponse { ResponseCode = result, Results = appData };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));
                throw;
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }
    }
}
