using Core.Enums;
using Core.Helpers;
using Core.Models;
using Microsoft.Extensions.Configuration;
using OERService.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OERService.DataAccess
{
    public class ReportDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

		private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public ReportDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<DatabaseResponse> GetDashboardReport()
        {
            try
            {
                _DataHelper = new DataAccessHelper("sps_OerDashboardReport", _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);


                Reports reports = new Reports();
                List<TopReviewers> topReviewers = new List<TopReviewers>();
                List<TopContributors> topContributors = new List<TopContributors>();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                    reports = (from model in ds.Tables[0].AsEnumerable()
                            select new Reports()
                            {
                                Contributors = model.Field<int>("Contributors"),
                                Resources = model.Field<int>("Resources"),
                                Courses = model.Field<int>("Courses"),
                                TotalVisit= model.Field<int?>("TotalVisit"),
                            }).FirstOrDefault();
                }
                if (ds != null && ds.Tables[2].Rows.Count > 0)
                {

                    topReviewers = (from model in ds.Tables[2].AsEnumerable()
                               select new TopReviewers()
                               {
                                   Id = model.Field<int>("Id"),
                                   UserName = model.Field<string>("UserName"),
                                   CourseCount = model.Field<int>("CourseCount"),
                                   Photo = model.Field<string>("Photo"),
                               }).ToList();
                }
                if (ds != null && ds.Tables[1].Rows.Count > 0)
                {

                    topContributors = (from model in ds.Tables[1].AsEnumerable()
                                    select new TopContributors()
                                    {
                                        Id = model.Field<int>("Id"),
                                        UserName = model.Field<string>("UserName"),
                                        CourseCount = model.Field<int>("CourseCount"),
                                        Photo = model.Field<string>("Photo"),
                                    }).ToList();
                }
                reports.TopContributors = topContributors;
                reports.TopReviewers = topReviewers;
                return new DatabaseResponse { ResponseCode = result, Results = reports };
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
        public async Task<DatabaseResponse> UserDashboardData(int UserId)
        {
            try
            {
                SqlParameter[] parameters =
             {
                    new SqlParameter( "@UserId",  SqlDbType.Int )

                };

                parameters[0].Value = UserId;
                _DataHelper = new DataAccessHelper("sps_DashboardReportByUserId", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);


                UserDashboard reports = new UserDashboard();
                List<LatestCourse> latestContent = new List<LatestCourse>();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                    reports = (from model in ds.Tables[0].AsEnumerable()
                               select new UserDashboard()
                               {
                                   DraftCourses = model.Field<int>("DraftCourses"),
                                   DraftResources = model.Field<int>("DraftResources"),
                                   PublishedCourses = model.Field<int>("PublishedCourses"),
                                   PubishedResources = model.Field<int>("PubishedResources"),
                                   CourseToApprove = model.Field<int>("CourseToApprove"),
                                   ResourceToApprove = model.Field<int>("ResourceToApprove"),
                                   DownloadedResources = model.Field<int>("DownloadedResources"),
                                   DownloadedCourses = model.Field<int>("DownloadedCourses"),
                                   SharedCourses = model.Field<int>("SharedCourses"),
                                   SharedResources = model.Field<int>("SharedResources"),
                                   FirstName = model.Field<string>("FirstName"),
                                   MiddleName = model.Field<string>("MiddleName"),
                                   LastName = model.Field<string>("LastName"),
                                   Photo = model.Field<string>("Photo"),
                                   ProfileDescription = model.Field<string>("ProfileDescription"),
                               }).FirstOrDefault();
                }
                if (ds != null && ds.Tables[1].Rows.Count > 0)
                {

                    latestContent = (from model in ds.Tables[1].AsEnumerable()
                                    select new LatestCourse()
                                    {
                                        Id = model.Field<int>("Id"),
                                        Title = model.Field<string>("Title"),
                                        Description = model.Field<string>("Description"),
                                        ContentType = model.Field<int>("ContentType"),
                                        Thumbnail = model.Field<string>("Thumbnail"),
                                    }).ToList();
                }
                reports.LatestCourse= latestContent;
                return new DatabaseResponse { ResponseCode = result, Results = reports };
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
        public async Task<DatabaseResponse> ContentRejectedList(int pageNo, int pageSize, string paramIsSearch, string sortType, int sortField)
        {
            try
            {
                SqlParameter[] parameters =
             {
                    new SqlParameter( "@PageNo",  SqlDbType.Int ),
                    new SqlParameter( "@PageSize",  SqlDbType.Int ),
                     new SqlParameter( "@Keyword",  SqlDbType.NVarChar ),
                    new SqlParameter( "@SortType",  SqlDbType.NVarChar ),
                    new SqlParameter( "@SortField",  SqlDbType.Int )

                };
                parameters[0].Value = pageNo;
                parameters[1].Value = pageSize;
                parameters[2].Value = paramIsSearch;
                parameters[3].Value = sortType;
                parameters[4].Value = sortField;

                _DataHelper = new DataAccessHelper("sps_AdminRejectedList", parameters , _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<RejectedContent> content = new List<RejectedContent>();
                if (dt != null && dt.Rows.Count > 0)
                {

                    content = (from model in dt.AsEnumerable()
                               select new RejectedContent()
                               {
                                   ContentId = model.Field<int>("ContentId"),
                                   Title = model.Field<string>("Title"),
                                   ContentType = model.Field<int>("ContentType"),
                                   Rownumber = model.Field<Int64>("Rownumber"),
                                   TotalRows = model.Field<Int64>("Totalrows")
                               }).ToList();
                }
                return new DatabaseResponse { ResponseCode = result, Results = content };
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
        public async Task<DatabaseResponse> UserRecommendedContent(int? UserId, int pageNo, int pageSize)
        {
            try
            {
                SqlParameter[] parameters =
             {
                    new SqlParameter( "@UserId",  SqlDbType.Int),
                    new SqlParameter( "@PageNo",  SqlDbType.Int ),
                    new SqlParameter( "@PageSize",  SqlDbType.Int )

                };

                parameters[0].Value = UserId == null || UserId == 0 ? null : UserId;
                parameters[1].Value = pageNo;
                parameters[2].Value = pageSize;
                _DataHelper = new DataAccessHelper("sps_RecommendedContent", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                List<RecommendedContent> content = new List<RecommendedContent>();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                    content = (from model in ds.Tables[0].AsEnumerable()
                               select new RecommendedContent()
                               {
                                   Id = model.Field<decimal>("Id"),
                                   Title = model.Field<string>("Title"),
                                   Description = model.Field<string>("Description"),
                                   ContentType = model.Field<int>("ContentType"),
                                   Thumbnail = model.Field<string>("Thumbnail"),
                                   Rownumber = model.Field<Int64>("Rownumber"),
                                   TotalRows = model.Field<Int64>("Totalrows"),
                                   CTitle  = model.Field<string>("CTitle"),
                                   CTitleAr = model.Field<string>("CTitle_Ar"),
                                   CDesc = model.Field<string>("Cdescription"),
                                   CDescAr = model.Field<string>("CDescription_Ar"),
                                   Media =  model.Field<string>("media")

                               }).ToList();
                }
                return new DatabaseResponse { ResponseCode = result, Results = content };
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
        public async Task<DatabaseResponse> GetReportAbuseContent()
        {
            try
            {
                _DataHelper = new DataAccessHelper("sps_ReportAbuseContent", _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);
                List<ReportAbuseContent> reportAbuseContent = new List<ReportAbuseContent>();
                if (dt != null && dt.Rows.Count > 0)
                {

                    reportAbuseContent = (from model in dt.AsEnumerable()
                                          select new ReportAbuseContent()
                                          {
                                              Id = model.Field<decimal>("Id"),
                                              Title = model.Field<string>("Title"),
                                              ReportAbuseCount = model.Field<int?>("ReportAbuseCount"),
                                              Description = model.Field<string>("Description"),
                                              ContentType = model.Field<int>("ContentType"),
                                              Reason = model.Field<string>("Reason"),
                                              ReportReasons = model.Field<string>("ReportReasons"),
                                              IsDeleted = model.Field<bool?>("IsDeleted"),
                                              UpdatedDate = model.Field<DateTime?>("UpdateDate"),
                                              ContentId = model.Field<int>("ContentId"),
                                          }).ToList();
                }
                return new DatabaseResponse { ResponseCode = result, Results = reportAbuseContent };
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
        public async Task<DatabaseResponse> GetVisitersCount()
        {
            try
            {



                _DataHelper = new DataAccessHelper("sps_Visiters", _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                Visiters visiters = new Visiters();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {

                    visiters = (from model in ds.Tables[0].AsEnumerable()
                                select new Visiters()
                                {
                                    TotalVisits = model.Field<int?>("TotalVisit")
                                }).FirstOrDefault();
                }

                return new DatabaseResponse { ResponseCode = result, Results = visiters };
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
        public async Task<DatabaseResponse> GetQrcReport()
        {
            try
            {



                _DataHelper = new DataAccessHelper("sps_QRCReport", _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

               List<QrcReportData> qrcReportData = new List<QrcReportData>();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {

                    qrcReportData = (from model in ds.Tables[0].AsEnumerable()
                               select new QrcReportData()
                               {
                                   Id = model.Field<int>("Id"),
                                   Name = model.Field<string>("Name"),
                                   UserCount = model.Field<int?>("UserCount"),
                                   ApproveCount = model.Field<int?>("ApproveCount"),
                                   RejectCount = model.Field<int?>("RejectCount"),
                                   PendingAction = model.Field<int?>("PendingAction"),
                                   Submission = model.Field<int?>("Submission"),
                               }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = qrcReportData };
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
        public async Task<DatabaseResponse> GetUsers()
        {
            try
            {

              

                _DataHelper = new DataAccessHelper("sps_Users", _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

               List<Users> profile = new List<Users>();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {

                    profile = (from model in ds.Tables[0].AsEnumerable()
                               select new Users()
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
                                   IsAdmin = model.Field<bool?>("IsAdmin")
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
        public async Task<DatabaseResponse> GetSharedContentReport(SharedContentInput sharedContentInput)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@ContentId",  SqlDbType.Int ),
                    new SqlParameter( "@ContentTypeId",  SqlDbType.Int ),
                    new SqlParameter( "@UserId",  SqlDbType.Int )

                };

                parameters[0].Value = sharedContentInput.ContentId == null || sharedContentInput.ContentId == 0 ? null : sharedContentInput.ContentId;
                parameters[1].Value = sharedContentInput.ContentTypeId;
                parameters[2].Value = sharedContentInput.UserId == null || sharedContentInput.UserId == 0 ? null : sharedContentInput.UserId;

                _DataHelper = new DataAccessHelper("sps_GetContentCountInfo", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                SharedContentInfo sharedContentInfo = new SharedContentInfo();


                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {

                    sharedContentInfo = (from model in ds.Tables[0].AsEnumerable()
                                 select new SharedContentInfo()
                                 {
                                     ContentCount = model.Field<int?>("ContentCount"),
                                     UserSharedCount = model.Field<int?>("UserSharedCount"),
                                    
                                 }).FirstOrDefault();


                }


                return new DatabaseResponse { ResponseCode = result, Results = sharedContentInfo };
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
        public async Task<DatabaseResponse> GetResourceByUserID(int UserId)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@UserId",  SqlDbType.NVarChar )

                };

                parameters[0].Value = UserId;

                _DataHelper = new DataAccessHelper("sps_ResourcesByUserId", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                List<Resource> resources = new List<Resource>();

                
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {

                    resources = (from model in ds.Tables[0].AsEnumerable()
                                 select new Resource()
                                 {
                                     Id = model.Field<decimal>("Id"),
                                     Title = model.Field<string>("Title"),
                                     Category = new ShortCategory { Id = model.Field<int>("CategoryId"), Name = model.Field<string>("CategoryName") },
                                     SubCategory = model.Field<int?>("SubCategoryId") != null ? new ShortSubCategory { Id = model.Field<int>("SubCategoryId"), CategoryId = model.Field<int>("CategoryId"), Name = model.Field<string>("SubCategoryName") } : null,
                                     CopyRight = model.Field<int?>("CopyRightId") != null ? new ShortCopyright { Id = model.Field<int>("CopyRightId"), Title = model.Field<string>("CopyrightTitle"), Description = model.Field<string>("CopyrightDescription") } : null,
                                     Thumbnail = model.Field<string>("Thumbnail"),
                                     ResourceContent = model.Field<string>("ResourceContent"),
                                     MaterialType = new ShortMaterialType { Id = model.Field<int?>("MaterialTypeId"), Name = model.Field<string>("MaterialTypeName") },
                                     ResourceDescription = model.Field<string>("ResourceDescription"),
                                     Keywords = model.Field<string>("Keywords"),
                                     CreatedBy = model.Field<string>("CreatedBy"),
                                     CreatedById = model.Field<int>("CreatedById"),
                                     CreatedOn = model.Field<DateTime>("CreatedOn"),
                                     IsDraft = model.Field<bool>("IsDraft"),
                                     Rating = model.Field<double>("Rating"),
                                     ReadingTime = model.Field<int?>("ReadingTime"),
                                     AlignmentRating = model.Field<double>("AlignmentRating"),
                                     IsApproved = model.Field<bool?>("IsApproved"),
                                     ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
                                    // References = references != null && references.Count > 0 ? references.Where(r => r.ResourceId == model.Field<decimal>("Id")).ToList() : null,
                                    // ResourceFiles = resourceFilesList != null && resourceFilesList.Count > 0 ? resourceFilesList.Where(f => f.ResourceId == model.Field<decimal>("Id")).ToList() : null,
                                     //ResourceComments = comments != null && comments.Count > 0 ? comments.Where(c => c.ResourceId == model.Field<decimal>("Id")).ToList() : null
                                 }).ToList();


                }


                return new DatabaseResponse { ResponseCode = result, Results = resources };
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
        public async Task<DatabaseResponse> DeleteAbuseReport(int id, int contentType, string reason)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Int ),
                    new SqlParameter( "@ContentType",  SqlDbType.Int ),
                     new SqlParameter( "@Reason",  SqlDbType.NVarChar )
                };

                parameters[0].Value = id;
                parameters[1].Value = contentType;
                parameters[2].Value = reason;
                _DataHelper = new DataAccessHelper("spd_AbuseReport", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);
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
        public async Task<DatabaseResponse> AddVisiter(int? UserId)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@UserId",  SqlDbType.NVarChar )

                };

                parameters[0].Value = UserId == null || UserId == 0 ? null : UserId;

                _DataHelper = new DataAccessHelper("spi_Visiters", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);
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

        public async Task<DatabaseResponse> GetCourseByUserId(int UserId)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@UserId",  SqlDbType.Int )

                };

                parameters[0].Value = UserId;

                _DataHelper = new DataAccessHelper("sps_CoursesByUserId", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

               List<Course> courses = new List<Course>();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {

                    courses = (from model in ds.Tables[0].AsEnumerable()
                                       select new Course()
                                       {
                                           Id = model.Field<decimal>("Id"),
                                           Title = model.Field<string>("Title"),
                                           Category = new ShortCategory { Id = model.Field<int>("CategoryId"), Name = model.Field<string>("CategoryName") },
                                           SubCategory = model.Field<int?>("SubCategoryId") != null ? new ShortSubCategory { Id = model.Field<int>("SubCategoryId"), CategoryId = model.Field<int>("CategoryId"), Name = model.Field<string>("SubCategoryName") } : null,
                                           CopyRight = model.Field<int?>("CopyRightId") != null ? new ShortCopyright { Id = model.Field<int>("CopyRightId"), Title = model.Field<string>("CopyrightTitle"), Description = model.Field<string>("CopyrightDescription") } : null,
                                           Thumbnail = model.Field<string>("Thumbnail"),
                                           CourseContent = model.Field<string>("CourseContent"),
                                           CourseDescription = model.Field<string>("CourseDescription"),
                                           Keywords = model.Field<string>("Keywords"),
                                           CreatedBy = model.Field<string>("CreatedBy"),
                                           CreatedById = model.Field<int>("CreatedById"),
                                           CreatedOn = model.Field<DateTime>("CreatedOn"),
                                           IsDraft = model.Field<bool>("IsDraft"),
                                           Rating = model.Field<double>("Rating"),
                                           ReadingTime = model.Field<int?>("ReadingTime"),
                                           IsApproved = model.Field<bool?>("IsApproved"),
                                           ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
                                           Education = model.Field<int?>("EducationId") != null ? new Education { Id = model.Field<int>("EducationId"), Name = model.Field<string>("EducationName") } : null,
                                           Profession = model.Field<int?>("ProfessionId") != null ? new Profession { Id = model.Field<int>("ProfessionId"), Name = model.Field<string>("ProfessionName") } : null

                                       }).ToList();


                }


                return new DatabaseResponse { ResponseCode = result, Results = courses };
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
