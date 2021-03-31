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
using OERService.Helpers;

namespace OERService.DataAccess
{
    public class ProfileDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

		private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public ProfileDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<DatabaseResponse> UpdateUserProfileStatusAll(Boolean Status)
        {
            try
            {
                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Status",  SqlDbType.Bit )
                };



                parameters[0].Value = Status;

                _DataHelper = new DataAccessHelper("spu_UserMasterAllStatus", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);




                return new DatabaseResponse { ResponseCode = result };
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
        public async Task<DatabaseResponse> UpdateUserProfileStatus(int UserID, Boolean Status)
        {
            try
            {
                SqlParameter[] parameters =
               {
                    new SqlParameter( "@UserId",  SqlDbType.Int ),
                    new SqlParameter( "@Status",  SqlDbType.Bit )
                };


                parameters[0].Value = UserID;
                parameters[1].Value = Status;

                _DataHelper = new DataAccessHelper("spu_UserMasterStatus", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);


             

                return new DatabaseResponse { ResponseCode = result};
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
        public async Task<DatabaseResponse> GetEmailNotificationStatus(int UserID)
        {
            try
            {
                SqlParameter[] parameters =
               {
                    new SqlParameter( "@UserId",  SqlDbType.Int )
                };


                parameters[0].Value = UserID;
               

                _DataHelper = new DataAccessHelper("sps_EmailNotificationStatus", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                UserEmailInfo user = new UserEmailInfo();

                if (dt != null && dt.Rows.Count > 0)
                {

                    user = (from model in dt.AsEnumerable()
                            select new UserEmailInfo()
                            {
                                Id = model.Field<int>("Id"),
                                IsEmailNotification = model.Field<bool>("IsEmailNotification")
                            }).FirstOrDefault();
                }

                return new DatabaseResponse { ResponseCode = result, Results = user };
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
        public async Task<DatabaseResponse> CreateProfile(CreateUserProfileRequest profileRequest)
        {
            try
            {
                SqlParameter[] parameters =
               {
                    new SqlParameter( "@TitleId",  SqlDbType.Int ),
                    new SqlParameter( "@FirstName",  SqlDbType.NVarChar ),
                    new SqlParameter( "@MiddleName",  SqlDbType.NVarChar ),
                    new SqlParameter( "@LastName",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CountryId",  SqlDbType.Int ),
                    new SqlParameter( "@StateId",  SqlDbType.Int ),
                    new SqlParameter( "@Gender",  SqlDbType.Int ),
                    new SqlParameter( "@Email",  SqlDbType.NVarChar ),
                    new SqlParameter( "@PortalLanguageId",  SqlDbType.Int ),                
                    new SqlParameter( "@DateOfBirth",  SqlDbType.DateTime ),
                    new SqlParameter( "@Photo",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ProfileDescription",  SqlDbType.NVarChar ),
                    new SqlParameter( "@SubjectsInterested",  SqlDbType.NVarChar ),
                    new SqlParameter( "@IsContributor",  SqlDbType.Bit ),
                    new SqlParameter( "@UserCertifications",  SqlDbType.NVarChar ),
                    new SqlParameter( "@UserEducations",  SqlDbType.NVarChar ),
                    new SqlParameter( "@UserExperiences",  SqlDbType.NVarChar ),
                    new SqlParameter( "@UserLanguages",  SqlDbType.NVarChar ),
                    new SqlParameter( "@UserSocialMedias",  SqlDbType.NVarChar ),
                };

                CommonHelper helper = new CommonHelper();

                parameters[0].Value =  profileRequest.TitleId==null || profileRequest.TitleId==0 ? null: profileRequest.TitleId;
                parameters[1].Value =  profileRequest.FirstName;
                parameters[2].Value =  profileRequest.MiddleName;
                parameters[3].Value =  profileRequest.LastName;
                parameters[4].Value =  profileRequest.CountryId==null || profileRequest.CountryId==0?null: profileRequest.CountryId;
                parameters[5].Value =  profileRequest.StateId == null || profileRequest.StateId == 0 ? null : profileRequest.StateId; 
                parameters[6].Value =  profileRequest.Gender;
                parameters[7].Value =  profileRequest.Email;
                parameters[8].Value =  profileRequest.PortalLanguageId == null || profileRequest.PortalLanguageId == 0 ? null : profileRequest.PortalLanguageId;             
                parameters[9].Value =  profileRequest.DateOfBirth;
                parameters[10].Value = profileRequest.Photo;
                parameters[11].Value = profileRequest.ProfileDescription;
                parameters[12].Value = profileRequest.SubjectsInterested;
                parameters[13].Value = profileRequest.IsContributor;
                parameters[14].Value = profileRequest.UserCertifications!=null? helper.GetJsonString(profileRequest.UserCertifications):null;
                parameters[15].Value = profileRequest.UserEducations!=null?helper.GetJsonString(profileRequest.UserEducations):null;
                parameters[16].Value = profileRequest.UserExperiences!=null?helper.GetJsonString(profileRequest.UserExperiences):null;
                parameters[17].Value = profileRequest.UserLanguages!=null? helper.GetJsonString(profileRequest.UserLanguages):null;
                parameters[18].Value = profileRequest.UserSocialMedias!=null? helper.GetJsonString(profileRequest.UserSocialMedias):null;

                _DataHelper = new DataAccessHelper("CreateUserProfile", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                UserMaster user = new UserMaster();

                if (dt != null && dt.Rows.Count > 0)
                {

                    user = (from model in dt.AsEnumerable()
                                  select new UserMaster()
                                  {
                                      Id = model.Field<int>("Id")                                     
                                  }).FirstOrDefault();
                }

                return new DatabaseResponse { ResponseCode = result, Results = user };
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
        public async Task<DatabaseResponse> GetUserProfile(string email)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Email",  SqlDbType.NVarChar )

                };

                parameters[0].Value = email;

                _DataHelper = new DataAccessHelper("GetUserProfileByEmail", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                UserProfile profile = new UserProfile();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0]!=null && ds.Tables[0].Rows.Count > 0)
                {

                    profile = (from model in ds.Tables[0].AsEnumerable()
                                  select new UserProfile()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Title = model.Field<int?>("TitleId") == null ? null : new Title { Id = model.Field<int>("TitleId"), Name = model.Field<string>("Title") },
                                      FirstName = model.Field<string>("FirstName"),
                                      MiddleName = model.Field<string>("MiddleName"),
                                      LastName = model.Field<string>("LastName"),
                                      Country = model.Field<int?>("CountryId") == null ? null : new Country { Id = model.Field<int>("CountryId"), Name = model.Field<string>("Country") },
                                      State = model.Field<int?>("StateId") == null ? null : new State { Id = model.Field<int>("StateId"), Name = model.Field<string>("State") },
                                      Gender = model.Field<int?>("Gender") == 0 ? null : new Gender { Id = model.Field<int>("Gender"), Name = ((GenderType)model.Field<int>("Gender")).ToString() },
                                      Email = model.Field<string>("Email"),
                                      PortalLanguage = model.Field<int?>("PortalLanguageId") == null ? null : new PortalLanguage { Id = model.Field<int>("PortalLanguageId"), Name = ((PortalLanguageType)model.Field<int>("PortalLanguageId")).ToString() },
                                      // Department = new Department { Id = model.Field<int>("DepartmentId"), Name = model.Field<string>("Department") },
                                      // Designation = new Designation { Id = model.Field<int>("DesignationId"), Name = model.Field<string>("Designation") },
                                      DateOfBirth = model.Field<DateTime?>("DateOfBirth"),
                                      Photo = model.Field<string>("Photo"),
                                      ProfileDescription = model.Field<string>("ProfileDescription"),
                                      SubjectsInterested = model.Field<string>("SubjectsInterested"),
                                      ApprovalStatus = model.Field<bool?>("ApprovalStatus"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                      UpdatedOn = model.Field<DateTime?>("UpdatedOn"),
                                      Active = model.Field<bool?>("Active"),
                                      IsContributor = model.Field<bool?>("IsContributor"),
                                      IsAdmin = model.Field<bool?>("IsAdmin"),
                                      Theme = model.Field<string>("Theme")
                                  }).FirstOrDefault();
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {

                   profile.UserCertifications = (from model in ds.Tables[1].AsEnumerable()
                               select new UserCertification()
                               {
                                   Id = model.Field<decimal>("Id"),                                    
                                   CertificationName = model.Field<string>("CertificationName"),
                                   CreatedOn = model.Field<DateTime>("CreatedOn"),
                                   Year= model.Field<int>("Year"),
                               }).ToList();
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {

                    profile.UserEducations = (from model in ds.Tables[2].AsEnumerable()
                                                  select new UserEducation()
                                                  {
                                                      Id = model.Field<decimal>("Id"),
                                                      UniversitySchool = model.Field<string>("UniversitySchool"),
                                                      Major = model.Field<string>("Major"),
                                                      Grade= model.Field<string>("Grade"),
                                                      FromDate =model.Field<DateTime>("FromDate"),
                                                      ToDate = model.Field<DateTime>("ToDate"),
                                                      CreatedOn = model.Field<DateTime>("CreatedOn")
                                                    
                                                  }).ToList();
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                {

                    profile.UserLanguages = (from model in ds.Tables[3].AsEnumerable()
                                              select new UserLanguage()
                                              {
                                                   Id = model.Field<decimal>("Id"),
                                                   Language = new Language { Id = model.Field<int>("LanguageId"), Name = model.Field<string>("Language") },
                                                   IsRead = model.Field<bool>("IsRead"),
                                                   IsSpeak = model.Field<bool>("IsSpeak"),
                                                   IsWrite = model.Field<bool>("IsWrite"),
                                                   CreatedOn = model.Field<DateTime>("CreatedOn")

                                              }).ToList();
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                {

                    profile.UserExperiences = (from model in ds.Tables[4].AsEnumerable()
                                             select new UserExperience()
                                             {
                                                 Id = model.Field<decimal>("Id"),
                                                 Designation = model.Field<string>("Designation"),
                                                 OrganizationName = model.Field<string>("OrganizationName"),
                                                 FromDate = model.Field<DateTime?>("FromDate"),
                                                 ToDate = model.Field<DateTime?>("ToDate"),
                                                 CreatedOn = model.Field<DateTime>("CreatedOn")

                                             }).ToList();
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[5] != null && ds.Tables[5].Rows.Count > 0)
                {

                    profile.UserSocialMedias = (from model in ds.Tables[5].AsEnumerable()
                                               select new UserSocialMedia()
                                               {
                                                   Id = model.Field<decimal>("Id"),
                                                   SocialMedia = new SocialMedia { Id = model.Field<int>("SocialMediaId"), Name = model.Field<string>("SocialMedia") },
                                                   URL = model.Field<string>("URL"),                                                  
                                                   CreatedOn = model.Field<DateTime>("CreatedOn")

                                               }).ToList();
                }
                return new DatabaseResponse { ResponseCode = result, Results = profile };
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
        public async Task<DatabaseResponse> GetUserById(int Id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Int )

                };

                parameters[0].Value = Id;

                _DataHelper = new DataAccessHelper("sps_UserById", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                UserProfile profile = new UserProfile();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {

                    profile = (from model in ds.Tables[0].AsEnumerable()
                               select new UserProfile()
                               {
                                   Id = model.Field<int>("Id"),
                                   Title = model.Field<int?>("TitleId") == null ? null : new Title { Id = model.Field<int>("TitleId"), Name = model.Field<string>("Title") },
                                   FirstName = model.Field<string>("FirstName"),
                                   MiddleName = model.Field<string>("MiddleName"),
                                   LastName = model.Field<string>("LastName"),
                                   Country = model.Field<int?>("CountryId") == null ? null : new Country { Id = model.Field<int>("CountryId"), Name = model.Field<string>("Country"), Name_Ar = model.Field<string>("Country_Ar") },
                                   State = model.Field<int?>("StateId") == null ? null : new State { Id = model.Field<int>("StateId"), Name = model.Field<string>("State"), Name_Ar = model.Field<string>("State_Ar") },
                                   Gender = model.Field<int?>("Gender") == 0 ? null : new Gender { Id = model.Field<int>("Gender"), Name = ((GenderType)model.Field<int>("Gender")).ToString() },
                                   Email = model.Field<string>("Email"),
                                   PortalLanguage = model.Field<int?>("PortalLanguageId") == null ? null : new PortalLanguage { Id = model.Field<int>("PortalLanguageId"), Name = ((PortalLanguageType)model.Field<int>("PortalLanguageId")).ToString() },
                                   // Department = new Department { Id = model.Field<int>("DepartmentId"), Name = model.Field<string>("Department") },
                                   // Designation = new Designation { Id = model.Field<int>("DesignationId"), Name = model.Field<string>("Designation") },
                                   DateOfBirth = model.Field<DateTime?>("DateOfBirth"),
                                   Photo = model.Field<string>("Photo"),
                                   ProfileDescription = model.Field<string>("ProfileDescription"),
                                   SubjectsInterested = model.Field<string>("SubjectsInterested"),
                                   ApprovalStatus = model.Field<bool?>("ApprovalStatus"),
                                   CreatedOn = model.Field<DateTime>("CreatedOn"),
                                   UpdatedOn = model.Field<DateTime?>("UpdatedOn"),
                                   Active = model.Field<bool?>("Active"),
                                   IsContributor = model.Field<bool?>("IsContributor"),
                                   IsAdmin = model.Field<bool?>("IsAdmin")
                               }).FirstOrDefault();
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {

                    profile.UserCertifications = (from model in ds.Tables[1].AsEnumerable()
                                                  select new UserCertification()
                                                  {
                                                      Id = model.Field<decimal>("Id"),
                                                      CertificationName = model.Field<string>("CertificationName"),
                                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                                      Year = model.Field<int>("Year"),
                                                  }).ToList();
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {

                    profile.UserEducations = (from model in ds.Tables[2].AsEnumerable()
                                              select new UserEducation()
                                              {
                                                  Id = model.Field<decimal>("Id"),
                                                  UniversitySchool = model.Field<string>("UniversitySchool"),
                                                  Major = model.Field<string>("Major"),
                                                  Grade = model.Field<string>("Grade"),
                                                  FromDate = model.Field<DateTime>("FromDate"),
                                                  ToDate = model.Field<DateTime>("ToDate"),
                                                  CreatedOn = model.Field<DateTime>("CreatedOn")

                                              }).ToList();
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                {

                    profile.UserLanguages = (from model in ds.Tables[3].AsEnumerable()
                                             select new UserLanguage()
                                             {
                                                 Id = model.Field<decimal>("Id"),
                                                 Language = new Language { Id = model.Field<int>("LanguageId"), Name = model.Field<string>("Language") },
                                                 IsRead = model.Field<bool>("IsRead"),
                                                 IsSpeak = model.Field<bool>("IsSpeak"),
                                                 IsWrite = model.Field<bool>("IsWrite"),
                                                 CreatedOn = model.Field<DateTime>("CreatedOn")

                                             }).ToList();
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                {

                    profile.UserExperiences = (from model in ds.Tables[4].AsEnumerable()
                                               select new UserExperience()
                                               {
                                                   Id = model.Field<decimal>("Id"),
                                                   Designation = model.Field<string>("Designation"),
                                                   OrganizationName = model.Field<string>("OrganizationName"),
                                                   FromDate = model.Field<DateTime?>("FromDate"),
                                                   ToDate = model.Field<DateTime?>("ToDate"),
                                                   CreatedOn = model.Field<DateTime>("CreatedOn")

                                               }).ToList();
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[5] != null && ds.Tables[5].Rows.Count > 0)
                {

                    profile.UserSocialMedias = (from model in ds.Tables[5].AsEnumerable()
                                                select new UserSocialMedia()
                                                {
                                                    Id = model.Field<decimal>("Id"),
                                                    SocialMedia = new SocialMedia { Id = model.Field<int>("SocialMediaId"), Name = model.Field<string>("SocialMedia"), Name_Ar= model.Field<string>("Name_Ar") },
                                                    URL = model.Field<string>("URL"),
                                                    CreatedOn = model.Field<DateTime>("CreatedOn")

                                                }).ToList();
                }
                return new DatabaseResponse { ResponseCode = result, Results = profile };
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
        public async Task<DatabaseResponse> GetUserProfileById(int  id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Int )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("GetUserProfileById", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                UserProfile profile = new UserProfile();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {

                    profile = (from model in ds.Tables[0].AsEnumerable()
                               select new UserProfile()
                               {
                                   Id = model.Field<int>("Id"),
                                   Title = model.Field<int?>("TitleId") == null ? null : new Title { Id = model.Field<int>("TitleId"), Name = model.Field<string>("Title") },
                                   FirstName = model.Field<string>("FirstName"),
                                   MiddleName = model.Field<string>("MiddleName"),
                                   LastName = model.Field<string>("LastName"),
                                   Country = model.Field<int?>("CountryId") == null ? null : new Country { Id = model.Field<int?>("CountryId"), Name = model.Field<string>("Country") },
                                   State = model.Field<int?>("StateId") == null ? null : new State { Id = model.Field<int?>("StateId"), Name = model.Field<string>("State") },
                                   Gender = model.Field<int?>("Gender")==0?null: new Gender { Id = model.Field<int>("Gender"), Name = ((GenderType)model.Field<int>("Gender")).ToString() },
                                   Email = model.Field<string>("Email"),
                                   PortalLanguage = model.Field<int?>("PortalLanguageId") == null ? null : new PortalLanguage { Id = model.Field<int>("PortalLanguageId"), Name = ((PortalLanguageType)model.Field<int>("PortalLanguageId")).ToString() },
                                   // Department = new Department { Id = model.Field<int>("DepartmentId"), Name = model.Field<string>("Department") },
                                   // Designation = new Designation { Id = model.Field<int>("DesignationId"), Name = model.Field<string>("Designation") },
                                   DateOfBirth = model.Field<DateTime?>("DateOfBirth"),
                                   Photo = model.Field<string>("Photo"),
                                   ProfileDescription = model.Field<string>("ProfileDescription"),
                                   SubjectsInterested = model.Field<string>("SubjectsInterested"),
                                   ApprovalStatus = model.Field<bool>("ApprovalStatus"),
                                   CreatedOn = model.Field<DateTime>("CreatedOn"),
                                   UpdatedOn = model.Field<DateTime?>("UpdatedOn"),
                                   Active = model.Field<bool?>("Active"),
                                   IsContributor = model.Field<bool?>("IsContributor"),
                                   IsAdmin = model.Field<bool?>("IsAdmin")
                               }).FirstOrDefault();
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {

                    profile.UserCertifications = (from model in ds.Tables[1].AsEnumerable()
                                                  select new UserCertification()
                                                  {
                                                      Id = model.Field<decimal>("Id"),
                                                      CertificationName = model.Field<string>("CertificationName"),
                                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                                      Year = model.Field<int>("Year"),
                                                  }).ToList();
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {

                    profile.UserEducations = (from model in ds.Tables[2].AsEnumerable()
                                              select new UserEducation()
                                              {
                                                  Id = model.Field<decimal>("Id"),
                                                  UniversitySchool = model.Field<string>("UniversitySchool"),
                                                  Major = model.Field<string>("Major"),
                                                  Grade = model.Field<string>("Grade"),
                                                  FromDate = model.Field<DateTime>("FromDate"),
                                                  ToDate = model.Field<DateTime>("ToDate"),
                                                  CreatedOn = model.Field<DateTime>("CreatedOn")

                                              }).ToList();
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                {

                    profile.UserLanguages = (from model in ds.Tables[3].AsEnumerable()
                                             select new UserLanguage()
                                             {
                                                 Id = model.Field<decimal>("Id"),
                                                 Language = new Language { Id = model.Field<int>("LanguageId"), Name = model.Field<string>("Language") },
                                                 IsRead = model.Field<bool>("IsRead"),
                                                 IsSpeak = model.Field<bool>("IsSpeak"),
                                                 IsWrite = model.Field<bool>("IsWrite"),
                                                 CreatedOn = model.Field<DateTime>("CreatedOn")

                                             }).ToList();
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                {

                    profile.UserExperiences = (from model in ds.Tables[4].AsEnumerable()
                                               select new UserExperience()
                                               {
                                                   Id = model.Field<decimal>("Id"),
                                                   Designation = model.Field<string>("Designation"),
                                                   OrganizationName = model.Field<string>("OrganizationName"),
                                                   FromDate = model.Field<DateTime?>("FromDate"),
                                                   ToDate = model.Field<DateTime?>("ToDate"),
                                                   CreatedOn = model.Field<DateTime>("CreatedOn")

                                               }).ToList();
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[5] != null && ds.Tables[5].Rows.Count > 0)
                {

                    profile.UserSocialMedias = (from model in ds.Tables[5].AsEnumerable()
                                                select new UserSocialMedia()
                                                {
                                                    Id = model.Field<decimal>("Id"),
                                                    SocialMedia = new SocialMedia { Id = model.Field<int>("SocialMediaId"), Name = model.Field<string>("SocialMedia") },
                                                    URL = model.Field<string>("URL"),
                                                    CreatedOn = model.Field<DateTime>("CreatedOn")

                                                }).ToList();
                }
                return new DatabaseResponse { ResponseCode = result, Results = profile };
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
        public async Task<DatabaseResponse> UpdateLastLogin(int userId)
        {
            try
            {

                SqlParameter[] parameters =
                {
                    new SqlParameter( "@UserId",  SqlDbType.Int )
                };


                parameters[0].Value = userId;
                _DataHelper = new DataAccessHelper("spu_UserLastLogin", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);
                return new DatabaseResponse { ResponseCode = result};

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
        public async Task<DatabaseResponse> UpdateTheme(int UserId, string Theme)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@UserId",  SqlDbType.Int ),
                    new SqlParameter( "@Theme",  SqlDbType.NVarChar )
                };


                parameters[0].Value = UserId;
                parameters[1].Value = Theme;
                _DataHelper = new DataAccessHelper("spu_AppTheme", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);
                return new DatabaseResponse { ResponseCode = result};
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

        public async Task<DatabaseResponse> UpdateProfile(UpdateUserProfileRequest profileRequest)
        {
            try
            {

                SqlParameter[] parameters =
                {
                    new SqlParameter( "@TitleId",  SqlDbType.Int ),
                    new SqlParameter( "@FirstName",  SqlDbType.NVarChar ),
                    new SqlParameter( "@MiddleName",  SqlDbType.NVarChar ),
                    new SqlParameter( "@LastName",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CountryId",  SqlDbType.Int ),
                    new SqlParameter( "@StateId",  SqlDbType.Int ),
                    new SqlParameter( "@Gender",  SqlDbType.Int ),
                    new SqlParameter( "@Email",  SqlDbType.NVarChar ),
                    new SqlParameter( "@PortalLanguageId",  SqlDbType.Int ),                 
                    new SqlParameter( "@DateOfBirth",  SqlDbType.DateTime ),
                    new SqlParameter( "@Photo",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ProfileDescription",  SqlDbType.NVarChar ),
                    new SqlParameter( "@SubjectsInterested",  SqlDbType.NVarChar ),
                    new SqlParameter( "@IsContributor",  SqlDbType.Bit ),
                    new SqlParameter( "@UserCertifications",  SqlDbType.NVarChar ),
                    new SqlParameter( "@UserEducations",  SqlDbType.NVarChar ),
                    new SqlParameter( "@UserExperiences",  SqlDbType.NVarChar ),
                    new SqlParameter( "@UserLanguages",  SqlDbType.NVarChar ),
                    new SqlParameter( "@UserSocialMedias",  SqlDbType.NVarChar ),
                     new SqlParameter( "@Id",  SqlDbType.Int ),
                     new SqlParameter( "@EmailUrl",  SqlDbType.NVarChar ),
                };

                CommonHelper helper = new CommonHelper();

                parameters[0].Value = profileRequest.TitleId == null || profileRequest.TitleId == 0 ? null : profileRequest.TitleId;
                parameters[1].Value = profileRequest.FirstName;
                parameters[2].Value = profileRequest.MiddleName;
                parameters[3].Value = profileRequest.LastName;
                parameters[4].Value = profileRequest.CountryId == null || profileRequest.CountryId == 0 ? null : profileRequest.CountryId;
                parameters[5].Value = profileRequest.StateId == null || profileRequest.StateId == 0 ? null : profileRequest.StateId;
                parameters[6].Value = profileRequest.Gender;
                parameters[7].Value = profileRequest.Email;
                parameters[8].Value = profileRequest.PortalLanguageId == null || profileRequest.PortalLanguageId == 0 ? null : profileRequest.PortalLanguageId;
                parameters[9].Value = profileRequest.DateOfBirth;
                parameters[10].Value = profileRequest.Photo;
                parameters[11].Value = profileRequest.ProfileDescription;
                parameters[12].Value = profileRequest.SubjectsInterested;
                parameters[13].Value = profileRequest.IsContributor;
                parameters[14].Value = profileRequest.UserCertifications != null ? helper.GetJsonString(profileRequest.UserCertifications) : null;
                parameters[15].Value = profileRequest.UserEducations != null ? helper.GetJsonString(profileRequest.UserEducations) : null;
                parameters[16].Value = profileRequest.UserExperiences != null ? helper.GetJsonString(profileRequest.UserExperiences) : null;
                parameters[17].Value = profileRequest.UserLanguages != null ? helper.GetJsonString(profileRequest.UserLanguages) : null;
                parameters[18].Value = profileRequest.UserSocialMedias != null ? helper.GetJsonString(profileRequest.UserSocialMedias) : null;
                parameters[19].Value = profileRequest.Id;
                parameters[20].Value = profileRequest.EmailUrl;
                _DataHelper = new DataAccessHelper("UpdateUserProfile", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);
                UserMaster user = new UserMaster();

                if (dt != null && dt.Rows.Count > 0)
                {

                    user = (from model in dt.AsEnumerable()
                            select new UserMaster()
                            {
                                Id = model.Field<int>("Id"),
                                IsEmailNotification= model.Field<bool>("IsEmailNotification"),
                                IsContributor = model.Field<bool>("IsContributor"),
                                PortalLanguageId = model.Field<int?>("PortalLanguageId")
                            }).FirstOrDefault();
                    if (user.IsEmailNotification && profileRequest.IsContributor && Convert.ToInt32(dt.Rows[0]["IsAlreadyContributer"]) == 0)
                    {
                        UserEmail userEmail = new UserEmail();
                        userEmail.Email = profileRequest.Email;
                        string text = string.Empty;
                        string buttonText = string.Empty;
                        if (user.PortalLanguageId == 2)
                        {
                            text = "You are now a contributor. You can create or review resources and courses.";
                            buttonText = "View";
                            userEmail.Subject = "Contributor Access provided";
                        }
                        else
                        {
                            text = "أنت الاّن مشارك من مشاركين منصة منارة، يمكنك إنشاء أومراجعة المصادر والمساقات التعليمية";
                            buttonText = "عرض";
                            userEmail.Subject = "منح صلاحية المشاركة";
                        }
                        userEmail.Body = Emailer.CreateEmailBody(profileRequest.FirstName + " " + profileRequest.LastName, profileRequest.EmailUrl, text, buttonText, user.PortalLanguageId, _configuration);
                       
                        await Emailer.SendEmailAsync(userEmail,_configuration);
                    }
                }

                return new DatabaseResponse { ResponseCode = result, Results = user };

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

        public async Task<DatabaseResponse> CreateInitialProfile(CreateInitialProfileRequest profileRequest)
        {
            try
            {

                SqlParameter[] parameters =
               {                 
                    new SqlParameter( "@FirstName",  SqlDbType.NVarChar ),                  
                    new SqlParameter( "@LastName",  SqlDbType.NVarChar ),                   
                    new SqlParameter( "@Email",  SqlDbType.NVarChar ),                   
                    new SqlParameter( "@IsContributor",  SqlDbType.Bit ),
                    new SqlParameter( "@IsAdmin",  SqlDbType.Bit )
                };


                parameters[0].Value = profileRequest.FirstName;
                parameters[1].Value = profileRequest.LastName;
                parameters[2].Value = profileRequest.Email;
                parameters[3].Value = profileRequest.IsContributor;
                parameters[4].Value = profileRequest.IsAdmin;
              

                _DataHelper = new DataAccessHelper("CreateUserInitialProfile", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                UserMaster user = new UserMaster();

                if (dt != null && dt.Rows.Count > 0)
                {

                    user = (from model in dt.AsEnumerable()
                            select new UserMaster()
                            {
                                Id = model.Field<int>("Id")
                            }).FirstOrDefault();
                }

                return new DatabaseResponse { ResponseCode = result, Results = user };
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
        
        public async Task<DatabaseResponse> UpdateUserRole(int userId, bool? IsContributor, bool? IsAdmin, int? portalLanguageId)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@UserID",  SqlDbType.Int ),
                    new SqlParameter( "@IsContributor",  SqlDbType.Bit ),
                    new SqlParameter( "@IsAdmin",  SqlDbType.Bit ),
                    new SqlParameter( "@portalLanguageId",  SqlDbType.Int )
               };


                parameters[0].Value = userId;
                parameters[1].Value = IsContributor;
                parameters[2].Value = IsAdmin;
                parameters[3].Value = portalLanguageId;

                _DataHelper = new DataAccessHelper("UpdateUserRole", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);               

                return new DatabaseResponse { ResponseCode = result};
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
        public async Task<DatabaseResponse> UpdateEmailNotification(UserNotification userNotification)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@UserID",  SqlDbType.Int ),
                    new SqlParameter( "@IsEmailNotification",  SqlDbType.Bit )

               };


                parameters[0].Value = userNotification.UserId;
                parameters[1].Value = userNotification.IsEmailNotificaiton;

                _DataHelper = new DataAccessHelper("spu_EmailNotification", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                return new DatabaseResponse { ResponseCode = result };
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

        public async Task<DatabaseResponse> GetUserFavouritesByContentID(int UserId, int ContentId, int ContentType)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@UserID",  SqlDbType.Int ),
                    new SqlParameter( "@ContentId",  SqlDbType.Int ),
                    new SqlParameter( "@ContentType",  SqlDbType.Int )
               };


                parameters[0].Value = UserId;
                parameters[1].Value = ContentId;
                parameters[2].Value = ContentType;

                _DataHelper = new DataAccessHelper("sps_GetUserFavouritesByContentID", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                return new DatabaseResponse { ResponseCode = result};
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

        public async Task<DatabaseResponse> GetUserBookmarkedContent(int UserId)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@UserID",  SqlDbType.Int ),

               };


                parameters[0].Value = UserId;

                _DataHelper = new DataAccessHelper("sps_GetUserBookmarkedContent", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<UserBookMarks> bookmarks = new List<UserBookMarks>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    bookmarks = (from model in dt.AsEnumerable()
                                 select new UserBookMarks()
                                 {
                                     Id = model.Field<int>("Id"),
                                     ContentId = model.Field<int>("ContentId"),
                                     ContentType = model.Field<int>("ContentType"),
                                     Title = model.Field<string>("Title"),
                                     Description = model.Field<string>("Description"),
                                     Thumbnail =model.Field<string>("Thumbnail")
                                 }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result , Results = bookmarks };
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

        public async Task<DatabaseResponse> AddUserBookmarkedContent(AddUserBookMarks userBookMarks)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@UserID",  SqlDbType.Int ),
                    new SqlParameter( "@ContentId",  SqlDbType.Int ),
                    new SqlParameter( "@ContentType",  SqlDbType.Int ),

               };


                parameters[0].Value = userBookMarks.UserId;
                parameters[1].Value = userBookMarks.ContentId;
                parameters[2].Value = userBookMarks.ContentType;

                _DataHelper = new DataAccessHelper("spi_AddUserBookmarkedContent", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<UserBookMarks> bookmarks = new List<UserBookMarks>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    bookmarks = (from model in dt.AsEnumerable()
                                 select new UserBookMarks()
                                 {
                                     Id = model.Field<int>("Id"),
                                     ContentId = model.Field<int>("ContentId"),
                                     ContentType = model.Field<int>("ContentType") ,
                                     Title = model.Field<string>("Title"),
                                     Description = model.Field<string>("Description"),
                                     Thumbnail = model.Field<string>("Thumbnail")

                                 }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = bookmarks };
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

        public async Task<DatabaseResponse> DeleteUserBookmarkedContent(DeleteUserBookMarks userBookMarks)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@UserID",  SqlDbType.Int ),
                    new SqlParameter( "@ContentId",  SqlDbType.Int ),
                    new SqlParameter( "@ContentType",  SqlDbType.Int ),

               };


                parameters[0].Value = userBookMarks.UserId;
                parameters[1].Value = userBookMarks.ContentId;
                parameters[2].Value = userBookMarks.ContentType;

                _DataHelper = new DataAccessHelper("spi_DeleteUserBookmarkedContent", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<UserBookMarks> bookmarks = new List<UserBookMarks>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    bookmarks = (from model in dt.AsEnumerable()
                                 select new UserBookMarks()
                                 {
                                     Id = model.Field<int>("Id"),
                                     ContentId = model.Field<int>("ContentId"),
                                     ContentType = model.Field<int>("ContentType"),
                                     Title = model.Field<string>("Title"),
                                     Description = model.Field<string>("Description"),
                                     Thumbnail = model.Field<string>("Thumbnail")

                                 }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = bookmarks };
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
