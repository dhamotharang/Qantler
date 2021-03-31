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
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;

namespace OERService.DataAccess
{
    public class ResourceDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

		private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public ResourceDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<DatabaseResponse> GetResourceMaster()
        {
            try
            {

                _DataHelper = new DataAccessHelper("sps_ResourceMasterData", _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                List<EducationalStandard> educationalStandard = new List<EducationalStandard>();

                List<EducationalUse> educationalUse = new List<EducationalUse>();

                List<Level> level = new List<Level>();


                List<CategoryMasterData> categoryMasterData = new List<CategoryMasterData>();

                List<SubCategoryMasterData> subCategoryMasterData = new List<SubCategoryMasterData>();

                List<MaterialTypeMasterData> materialTypeMasterData = new List<MaterialTypeMasterData>();

                List<EducationMasterData> educationMasterData = new List<EducationMasterData>();

                List<ProfessionMasterData> professionMasterData = new List<ProfessionMasterData>();

                List<CopyrightMasterData> copyrightMasterData = new List<CopyrightMasterData>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    //titles
                    educationalStandard = (from model in ds.Tables[0].AsEnumerable()
                                           select new EducationalStandard()
                                           {
                                               Id = model.Field<int>("Id"),
                                               Standard = model.Field<string>("Standard"),
                                               Standard_Ar = model.Field<string>("Standard_Ar")
                                           }).ToList();
                }



                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    // country
                    educationalUse = (from model in ds.Tables[1].AsEnumerable()
                                      select new EducationalUse()
                                      {
                                          Id = model.Field<int>("Id"),
                                          Text = model.Field<string>("EducationalUse"),
                                          Text_Ar = model.Field<string>("EducationalUse_Ar")
                                      }).ToList();
                }



                if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {
                    level = (from model in ds.Tables[2].AsEnumerable()
                             select new Level()
                             {
                                 Id = model.Field<int>("Id"),
                                 LevelText = model.Field<string>("Level"),
                                 LevelText_Ar = model.Field<string>("Level_Ar")
                             }).ToList();
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                {
                    categoryMasterData = (from model in ds.Tables[3].AsEnumerable()
                                          select new CategoryMasterData()
                                          {
                                              Id = model.Field<int>("Id"),
                                              Name = model.Field<string>("Name"),
                                              Name_Ar = model.Field<string>("Name_Ar")
                                          }).ToList();
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                {
                    subCategoryMasterData = (from model in ds.Tables[4].AsEnumerable()
                                             select new SubCategoryMasterData()
                                             {
                                                 Id = model.Field<int>("Id"),
                                                 Name = model.Field<string>("Name"),
                                                 Name_Ar = model.Field<string>("Name_Ar"),
                                                 CategoryId = model.Field<int>("CategoryId"),
                                                 CategoryName = model.Field<string>("CategoryName"),
                                             }).ToList();
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[5] != null && ds.Tables[5].Rows.Count > 0)
                {
                    materialTypeMasterData = (from model in ds.Tables[5].AsEnumerable()
                                              select new MaterialTypeMasterData()
                                              {
                                                  Id = model.Field<int>("Id"),
                                                  Name = model.Field<string>("Name"),
                                                  Name_Ar = model.Field<string>("Name_Ar")
                                              }).ToList();
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[6] != null && ds.Tables[6].Rows.Count > 0)
                {
                    educationMasterData = (from model in ds.Tables[6].AsEnumerable()
                                           select new EducationMasterData()
                                           {
                                               Id = model.Field<int>("Id"),
                                               Name = model.Field<string>("Name"),
                                               Name_Ar = model.Field<string>("Name_Ar")
                                           }).ToList();
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[7] != null && ds.Tables[7].Rows.Count > 0)
                {
                    professionMasterData = (from model in ds.Tables[7].AsEnumerable()
                                            select new ProfessionMasterData()
                                            {
                                                Id = model.Field<int>("Id"),
                                                Name = model.Field<string>("Name"),
                                                Name_Ar = model.Field<string>("Name_Ar")
                                            }).ToList();
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[8] != null && ds.Tables[8].Rows.Count > 0)
                {
                    copyrightMasterData = (from model in ds.Tables[8].AsEnumerable()
                                           select new CopyrightMasterData()
                                           {
                                               Id = model.Field<int>("Id"),
                                               Title = model.Field<string>("Title"),
                                               Description = model.Field<string>("Description"),
                                               Title_Ar = model.Field<string>("Title_Ar"),
                                               Description_Ar = model.Field<string>("Description_Ar"),
                                               Media = model.Field<string>("Media")
                                           }).ToList();
                }
                ResourceMasterData appData = new ResourceMasterData
                {
                    EducationalStandard = educationalStandard,
                    EducationalUse = educationalUse,
                    Level = level,
                    CategoryMasterData = categoryMasterData,
                    SubCategoryMasterData = subCategoryMasterData,
                    MaterialTypeMasterData = materialTypeMasterData,
                    EducationMasterData = educationMasterData,
                    ProfessionMasterData = professionMasterData,
                    CopyrightMasterData = copyrightMasterData
                };

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

        public async Task<DatabaseResponse> DownloadContent(DownloadContentInfoCreateResource downloadContentInfoCreate)
        {
            try
            {



                SqlParameter[] parameters =
               {
                    new SqlParameter( "@ContentId",  SqlDbType.Int ),
                    new SqlParameter( "@ContentTypeId",  SqlDbType.Int ),
                    new SqlParameter( "@DownloadedBy",  SqlDbType.Int )

                };

                parameters[0].Value = downloadContentInfoCreate.ContentId;
                parameters[1].Value = 2;
                parameters[2].Value = downloadContentInfoCreate.DownloadedBy;

                _DataHelper = new DataAccessHelper("spi_ContentDownloadInfo", parameters, _configuration);

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

        public async Task<DatabaseResponse> CreateResource(CreateResourceRequest resource)
        {
            try
            {
                CommonHelper helper = new CommonHelper();

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Title",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CategoryId",  SqlDbType.Int ),
                    new SqlParameter( "@SubCategoryId",  SqlDbType.Int ),
                    new SqlParameter( "@Thumbnail",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ResourceDescription",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Keywords",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ResourceContent",  SqlDbType.NVarChar ),
                    new SqlParameter( "@MaterialTypeId",  SqlDbType.Int ),
                    new SqlParameter( "@CopyRightId",  SqlDbType.Int ),
                    new SqlParameter( "@IsDraft",  SqlDbType.Bit ),
                    new SqlParameter( "@CreatedBy",  SqlDbType.Int ),
                    new SqlParameter( "@References",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ResourceFiles",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ReadingTime",  SqlDbType.Int ),
                    new SqlParameter( "@ResourceSourceId",  SqlDbType.Int ),
                    new SqlParameter( "@LevelId",  SqlDbType.Int ),
                    new SqlParameter( "@EducationalStandardId",  SqlDbType.Int ),
                    new SqlParameter( "@EducationalUseId",  SqlDbType.Int ),
                    new SqlParameter( "@Format",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Objective",  SqlDbType.NVarChar ),
                };

                parameters[0].Value = resource.Title;
                parameters[1].Value = resource.CategoryId;
                parameters[2].Value = resource.SubCategoryId == null || resource.SubCategoryId == 0 ? null : resource.SubCategoryId;
                parameters[3].Value = resource.Thumbnail;
                parameters[4].Value = resource.ResourceDescription;
                parameters[5].Value = resource.Keywords;
                parameters[6].Value = resource.ResourceContent;
                parameters[7].Value = resource.MaterialTypeId==null || resource.MaterialTypeId==0?null:resource.MaterialTypeId;
                parameters[8].Value = resource.CopyRightId == null || resource.CopyRightId == 0 ? null : resource.CopyRightId;
                parameters[9].Value = resource.IsDraft;
                parameters[10].Value = resource.CreatedBy;
                parameters[11].Value = resource.References != null ? helper.GetJsonString(resource.References) : null;
                parameters[12].Value = resource.ResourceFiles != null ? helper.GetJsonString(resource.ResourceFiles) : null;
                parameters[13].Value = resource.ReadingTime;
                parameters[14].Value = resource.ResourceSourceId;
                parameters[15].Value = resource.LevelId == 0 ? null : resource.LevelId;
                parameters[16].Value = resource.EducationalStandardId == 0 ? null : resource.EducationalStandardId;
                parameters[17].Value = resource.EducationalUseId == 0 ? null : resource.EducationalUseId;
                parameters[18].Value = resource.Format;
                parameters[19].Value = resource.Objective;
                _DataHelper = new DataAccessHelper("CreateResource", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);
                if (result == (int)DbReturnValue.CreateSuccess)
                {
                    //email for accept /reject
                }
                Resource Createdresource = new Resource();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {

                    Createdresource = (from model in ds.Tables[1].AsEnumerable()
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
                                           AlignmentRating = model.Field<double>("AlignmentRating"),
                                           IsApproved = model.Field<bool?>("IsApproved"),
                                           ReportAbuseCount = model.Field<int?>("ReportAbuseCount"),

                                       }).FirstOrDefault();


                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {

                    Createdresource.ResourceFiles = (from model in ds.Tables[2].AsEnumerable()
                                                     select new ResourceFileMaster()
                                                     {
                                                         Id = model.Field<decimal>("Id"),
                                                         ResourceId = model.Field<decimal>("ResourceId"),
                                                         AssociatedFile = model.Field<string>("AssociatedFile"),
                                                         UploadedDate = model.Field<DateTime>("UploadedDate")
                                                     }).ToList();
                }


                if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                {

                    Createdresource.References = (from model in ds.Tables[3].AsEnumerable()
                                                  select new ReferenceMaster()
                                                  {
                                                      Id = model.Field<decimal>("Id"),
                                                      ResourceId = model.Field<decimal>("ResourceId"),
                                                      URLReference = model.Field<string>("URLReference"),
                                                      CreatedOn = model.Field<DateTime>("CreatedOn")
                                                  }).ToList();
                }


                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        UserEmail userEmail = new UserEmail();
                        userEmail.Email = Convert.ToString(item["Email"]);
                        string text = "You have assigned a resource to approve/reject";
                        string buttonText = "Review Content";
                        userEmail.Subject = "You have been assigned a submission to review.";
                        userEmail.Body = Emailer.CreateEmailBody(Convert.ToString(item["UserName"]), resource.EmailUrl, text, buttonText,_configuration);
                     
                        await Emailer.SendEmailAsync(userEmail,_configuration);
                    }

                }

                return new DatabaseResponse { ResponseCode = result, Results = Createdresource };

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
        public async Task<List<ContentFiles>> GetContentFileNames(int contentId, int contentType)
        {
            try
            {
                SqlParameter[] parameters =
             {
                    new SqlParameter( "@ContentId",  SqlDbType.Int ),
                    new SqlParameter( "@ContentType",  SqlDbType.Int )

                };

                parameters[0].Value = contentId;
                parameters[1].Value = contentType;
                _DataHelper = new DataAccessHelper("sps_ContentFileNames", parameters, _configuration);

                DataTable dt = new DataTable();
                List<ContentFiles> ContentFiles = new List<ContentFiles>();
                await _DataHelper.RunAsync(dt);
                if (dt != null && dt.Rows.Count > 0)
                {

                    ContentFiles = (from model in dt.AsEnumerable()
                                    select new ContentFiles()
                                    {
                                        Id = model.Field<decimal>("Id"),
                                        ContentId = model.Field<decimal>("ContentId"),
                                        AssociatedFiles = model.Field<string>("associatedfile"),
                                        FileName = model.Field<string>("FileName")

                                    }).ToList();
                }
                return ContentFiles;
            }
            catch (Exception ex)
            {
				Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Medium));
				throw;
            }


        }
        public async Task<DatabaseResponse> GetResourceByCourseId(int courseId)
        {
            try
            {
                SqlParameter[] parameters =
              {
                    new SqlParameter( "@CourseId",  SqlDbType.NVarChar )

                };

                parameters[0].Value = courseId;

                _DataHelper = new DataAccessHelper("GetResourceByCourseId", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);
                List<Resource> resources = new List<Resource>();

                List<ResourceFileMaster> resourceFilesList = new List<ResourceFileMaster>();

                List<ReferenceMaster> references = new List<ReferenceMaster>();

                List<ResourceComments> comments = new List<ResourceComments>();

                List<Rating> ratings = new List<Rating>();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {

                        resourceFilesList = (from model in ds.Tables[1].AsEnumerable()
                                             select new ResourceFileMaster()
                                             {
                                                 Id = model.Field<decimal>("Id"),
                                                 ResourceId = model.Field<decimal>("ResourceId"),
                                                 AssociatedFile = model.Field<string>("AssociatedFile"),
                                                 UploadedDate = model.Field<DateTime>("UploadedDate")
                                             }).ToList();
                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                    {

                        ratings = (from model in ds.Tables[4].AsEnumerable()
                                   select new Rating()
                                   {
                                       Star = model.Field<double>("Rating"),
                                       UserCount = model.Field<int>("NoOfUsers")
                                   }).ToList();
                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    {

                        references = (from model in ds.Tables[2].AsEnumerable()
                                      select new ReferenceMaster()
                                      {
                                          Id = model.Field<decimal>("Id"),
                                          ResourceId = model.Field<decimal>("ResourceId"),
                                          URLReference = model.Field<string>("URLReference"),
                                          CreatedOn = model.Field<DateTime>("CreatedOn")
                                      }).ToList();
                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                    {

                        comments = (from model in ds.Tables[3].AsEnumerable()
                                    select new ResourceComments()
                                    {
                                        Id = model.Field<decimal>("Id"),
                                        ResourceId = model.Field<decimal>("ResourceId"),
                                        Comments = model.Field<string>("Comments"),
                                        CommentedBy = model.Field<string>("CommentedBy"),
                                        CommentedById = model.Field<int>("CommentedById"),
                                        CommentorImage = model.Field<string>("CommentorImage"),
                                        CommentDate = model.Field<DateTime>("CommentDate"),
                                        ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
                                    }).ToList();
                    }


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
                                     AllRatings = ratings,
                                     AlignmentRating = model.Field<double>("AlignmentRating"),
                                     IsApproved = model.Field<bool?>("IsApproved"),
                                     ReportAbuseCount = model.Field<int?>("ReportAbuseCount"),
                                     LastView = model.Field<DateTime?>("LastView"),
                                     References = references != null && references.Count > 0 ? references.Where(r => r.ResourceId == model.Field<decimal>("Id")).ToList() : null,
                                     ResourceFiles = resourceFilesList != null && resourceFilesList.Count > 0 ? resourceFilesList.Where(f => f.ResourceId == model.Field<decimal>("Id")).ToList() : null,
                                     ResourceComments = comments != null && comments.Count > 0 ? comments.Where(c => c.ResourceId == model.Field<decimal>("Id")).ToList() : null
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
        public async Task<DatabaseResponse> GetAllResource(int pageNo, int pageSize, string paramIsSearch,string ascDescNo, int columnNo)
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
                parameters[3].Value = ascDescNo;
                parameters[4].Value = columnNo;
                _DataHelper = new DataAccessHelper("GetResource", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                List<Resource> resources = new List<Resource>();

                List<ResourceFileMaster> resourceFilesList = new List<ResourceFileMaster>();

                List<ReferenceMaster> references = new List<ReferenceMaster>();

                List<ResourceComments> comments = new List<ResourceComments>();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {

                        resourceFilesList = (from model in ds.Tables[1].AsEnumerable()
                                             select new ResourceFileMaster()
                                             {
                                                 Id = model.Field<decimal>("Id"),
                                                 ResourceId = model.Field<decimal>("ResourceId"),
                                                 AssociatedFile = model.Field<string>("AssociatedFile"),
                                                 UploadedDate = model.Field<DateTime>("UploadedDate")
                                             }).ToList();
                    }


                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    {

                        references = (from model in ds.Tables[2].AsEnumerable()
                                      select new ReferenceMaster()
                                      {
                                          Id = model.Field<decimal>("Id"),
                                          ResourceId = model.Field<decimal>("ResourceId"),
                                          URLReference = model.Field<string>("URLReference"),
                                          CreatedOn = model.Field<DateTime>("CreatedOn")
                                      }).ToList();
                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                    {

                        comments = (from model in ds.Tables[3].AsEnumerable()
                                    select new ResourceComments()
                                    {
                                        Id = model.Field<decimal>("Id"),
                                        ResourceId = model.Field<decimal>("ResourceId"),
                                        Comments = model.Field<string>("Comments"),
                                        CommentedBy = model.Field<string>("CommentedBy"),
                                        CommentedById = model.Field<int>("CommentedById"),
                                        CommentorImage = model.Field<string>("CommentorImage"),
                                        CommentDate = model.Field<DateTime>("CommentDate"),
                                        ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
                                    }).ToList();
                    }


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
                                     MaterialType = new ShortMaterialType { Id = model.Field<int>("MaterialTypeId"), Name = model.Field<string>("MaterialTypeName") },
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
                                     Standard = model.Field<string>("Standard"),
                                     Objective = model.Field<string>("Objective"),
                                     Format = model.Field<string>("Format"),
                                     Rownumber = model.Field<Int64>("Rownumber"),
                                     TotalRows = model.Field<Int64>("Totalrows"),
                                     ViewCount = model.Field<int?>("ViewCount"),
                                     SharedCount = model.Field<int?>("SharedCount"),
                                     DownloadCount = model.Field<int?>("DownloadCount"),
                                     References = references != null && references.Count > 0 ? references.Where(r => r.ResourceId == model.Field<decimal>("Id")).ToList() : null,
                                     ResourceFiles = resourceFilesList != null && resourceFilesList.Count > 0 ? resourceFilesList.Where(f => f.ResourceId == model.Field<decimal>("Id")).ToList() : null,
                                     ResourceComments = comments != null && comments.Count > 0 ? comments.Where(c => c.ResourceId == model.Field<decimal>("Id")).ToList() : null,
                                     CommunityBadge = model.Field<bool?>("CommunityBadge"),
                                     MoEBadge = model.Field<bool?>("MoEBadge")
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
        public async Task<DatabaseResponse> GetRemixVersion(decimal id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@ResourceRemixedID",  SqlDbType.NVarChar )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("sps_RemixPreviousVersion", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                List<Resource> resources = new List<Resource>();

                List<Rating> ratings = new List<Rating>();

                List<ResourceFileMaster> resourceFilesList = new List<ResourceFileMaster>();

                List<ReferenceMaster> references = new List<ReferenceMaster>();

                List<ResourceComments> comments = new List<ResourceComments>();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {


                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {


                        resourceFilesList = (from model in ds.Tables[1].AsEnumerable()
                                             select new ResourceFileMaster()
                                             {
                                                 Id = model.Field<decimal>("Id"),
                                                 ResourceId = model.Field<decimal>("ResourceId"),
                                                 AssociatedFile = model.Field<string>("AssociatedFile"),
                                                 UploadedDate = model.Field<DateTime>("UploadedDate")
                                             }).ToList();
                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                    {

                        ratings = (from model in ds.Tables[4].AsEnumerable()
                                   select new Rating()
                                   {
                                       Star = model.Field<double>("Rating"),
                                       UserCount = model.Field<int>("NoOfUsers")
                                   }).ToList();
                    }



                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    {

                        references = (from model in ds.Tables[2].AsEnumerable()
                                      select new ReferenceMaster()
                                      {
                                          Id = model.Field<decimal>("Id"),
                                          ResourceId = model.Field<decimal>("ResourceId"),
                                          URLReference = model.Field<string>("URLReference"),
                                          CreatedOn = model.Field<DateTime>("CreatedOn")
                                      }).ToList();
                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                    {

                        comments = (from model in ds.Tables[3].AsEnumerable()
                                    select new ResourceComments()
                                    {
                                        Id = model.Field<decimal>("Id"),
                                        ResourceId = model.Field<decimal>("ResourceId"),
                                        Comments = model.Field<string>("Comments"),
                                        CommentedBy = model.Field<string>("CommentedBy"),
                                        CommentedById = model.Field<int>("CommentedById"),
                                        CommentorImage = model.Field<string>("CommentorImage"),
                                        CommentDate = model.Field<DateTime>("CommentDate"),
                                        ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
                                    }).ToList();
                    }

                    resources = (from model in ds.Tables[0].AsEnumerable()
                                 select new Resource()
                                 {
                                     Id = model.Field<decimal>("Id"),
                                     Title = model.Field<string>("Title"),
                                     Category = new ShortCategory { Id = model.Field<int>("CategoryId"), Name = model.Field<string>("CategoryName"), Name_Ar = model.Field<string>("CategoryName_Ar") },
                                     SubCategory = model.Field<int?>("SubCategoryId") != null ? new ShortSubCategory { Id = model.Field<int>("SubCategoryId"), CategoryId = model.Field<int>("CategoryId"), Name = model.Field<string>("SubCategoryName"), Name_Ar = model.Field<string>("SubCategoryName_Ar") } : null,
                                     CopyRight = model.Field<int?>("CopyRightId") != null ? new ShortCopyright { Id = model.Field<int>("CopyRightId"), Title = model.Field<string>("CopyrightTitle"), Title_Ar = model.Field<string>("CopyrightTitle_Ar"), Description = model.Field<string>("CopyrightDescription") } : null,
                                     Thumbnail = model.Field<string>("Thumbnail"),
                                     ResourceContent = model.Field<string>("ResourceContent"),
                                     MaterialType = new ShortMaterialType { Id = model.Field<int>("MaterialTypeId"), Name = model.Field<string>("MaterialTypeName"), Name_Ar = model.Field<string>("MaterialTypeName_Ar") },
                                     ResourceDescription = model.Field<string>("ResourceDescription"),
                                     Keywords = model.Field<string>("Keywords"),
                                     CreatedBy = model.Field<string>("CreatedBy"),
                                     CreatedById = model.Field<int>("CreatedById"),
                                     CreatedOn = model.Field<DateTime>("CreatedOn"),
                                     IsDraft = model.Field<bool>("IsDraft"),
                                     Rating = model.Field<double>("Rating"),
                                     AllRatings = ratings,
                                     ReadingTime = model.Field<int?>("ReadingTime"),
                                     AlignmentRating = model.Field<double>("AlignmentRating"),
                                     IsApproved = model.Field<bool?>("IsApproved"),
                                     ReportAbuseCount = model.Field<int?>("ReportAbuseCount"),
                                     ViewCount = model.Field<int?>("ViewCount"),
                                     SharedCount = model.Field<int?>("SharedCount"),
                                     DownloadCount = model.Field<int?>("DownloadCount"),
                                     References = references != null && references.Count > 0 ? references.Where(r => r.ResourceId == model.Field<decimal>("Id")).ToList() : null,
                                     ResourceFiles = resourceFilesList != null && resourceFilesList.Count > 0 ? resourceFilesList.Where(f => f.ResourceId == model.Field<decimal>("Id")).ToList() : null,
                                     ResourceComments = comments != null && comments.Count > 0 ? comments.Where(c => c.ResourceId == model.Field<decimal>("Id")).ToList() : null
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
        public async Task<Resource> GetResourcePdfDetail(decimal id)
        {
            try
            {
                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )

                };
                parameters[0].Value = id;
                _DataHelper = new DataAccessHelper("GetResourceById", parameters, _configuration);
                DataSet ds = new DataSet();
                await _DataHelper.RunAsync(ds);
                Resource resources = new Resource();
                List<ResourceFileMaster> resourceFilesList = new List<ResourceFileMaster>();
                List<ReferenceMaster> references = new List<ReferenceMaster>();
                List<ResourceComments> comments = new List<ResourceComments>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        resourceFilesList = (from model in ds.Tables[1].AsEnumerable()
                                             select new ResourceFileMaster()
                                             {
                                                 Id = model.Field<decimal>("Id"),
                                                 ResourceId = model.Field<decimal>("ResourceId"),
                                                 AssociatedFile = model.Field<string>("AssociatedFile"),
                                                 UploadedDate = model.Field<DateTime>("UploadedDate")
                                             }).ToList();
                    }



                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    {

                        references = (from model in ds.Tables[2].AsEnumerable()
                                      select new ReferenceMaster()
                                      {
                                          Id = model.Field<decimal>("Id"),
                                          ResourceId = model.Field<decimal>("ResourceId"),
                                          URLReference = model.Field<string>("URLReference"),
                                          CreatedOn = model.Field<DateTime>("CreatedOn")
                                      }).ToList();
                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                    {

                        comments = (from model in ds.Tables[3].AsEnumerable()
                                    select new ResourceComments()
                                    {
                                        Id = model.Field<decimal>("Id"),
                                        ResourceId = model.Field<decimal>("ResourceId"),
                                        Comments = model.Field<string>("Comments"),
                                        CommentedBy = model.Field<string>("CommentedBy"),
                                        CommentedById = model.Field<int>("CommentedById"),
                                        CommentorImage = model.Field<string>("CommentorImage"),
                                        CommentDate = model.Field<DateTime>("CommentDate"),
                                        ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
                                    }).ToList();
                    }

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
                                     ReportAbuseCount = model.Field<int?>("ReportAbuseCount"),
                                     IsRemix = model.Field<bool>("IsRemix"),
                                     Standard = model.Field<string>("Standard"),
                                     //EducationalUse = model.Field<string>("EducationalUse"),
                                     //Level = model.Field<string>("Level"),
                                     Objective = model.Field<string>("Objective"),
                                     EducationLevel = new EducationLevel { Id = model.Field<int?>("LevelId"), Level = model.Field<string>("LevelName"), Level_Ar = model.Field<string>("Level_Ar") },
                                     EducationalUse = new EducationalUse { Id = model.Field<int?>("EducationalUseId"), Text = model.Field<string>("EducationalUseName"), Text_Ar = model.Field<string>("EducationalUse_Ar") },
                                     EducationalStandard = new EducationalStandard { Id = model.Field<int?>("EducationalStandardId"), Standard = model.Field<string>("Standard"), Standard_Ar = model.Field<string>("Standard_Ar") },
                                     Format = model.Field<string>("Format"),
                                     LastView = model.Field<DateTime?>("LastView"),
                                     SharedCount = model.Field<int?>("SharedCount"),
                                     References = references != null && references.Count > 0 ? references.Where(r => r.ResourceId == model.Field<decimal>("Id")).ToList() : null,
                                     ResourceFiles = resourceFilesList != null && resourceFilesList.Count > 0 ? resourceFilesList.Where(f => f.ResourceId == model.Field<decimal>("Id")).ToList() : null,
                                     ResourceComments = comments != null && comments.Count > 0 ? comments.Where(c => c.ResourceId == model.Field<decimal>("Id")).ToList() : null,
                                     CommunityBadge = model.Field<bool?>("CommunityBadge"),
                                     MoEBadge = model.Field<bool?>("MoEBadge")
                                 }).FirstOrDefault();


                }


                return resources;
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
        public async Task<DatabaseResponse> GetResource(decimal id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("GetResourceById", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                List<Resource> resources = new List<Resource>();

                List<ResourceFileMaster> resourceFilesList = new List<ResourceFileMaster>();

                List<ReferenceMaster> references = new List<ReferenceMaster>();

                List<ResourceComments> comments = new List<ResourceComments>();

                List<Rating> ratings = new List<Rating>();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {


                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {


                        resourceFilesList = (from model in ds.Tables[1].AsEnumerable()
                                             select new ResourceFileMaster()
                                             {
                                                 Id = model.Field<decimal>("Id"),
                                                 ResourceId = model.Field<decimal>("ResourceId"),
                                                 AssociatedFile = model.Field<string>("AssociatedFile"),
                                                 UploadedDate = model.Field<DateTime>("UploadedDate"),
                                                 FileName = model.Field<string>("FileName"),                                                 
                                             }).ToList();
                    }



                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    {

                        references = (from model in ds.Tables[2].AsEnumerable()
                                      select new ReferenceMaster()
                                      {
                                          Id = model.Field<decimal>("Id"),
                                          ResourceId = model.Field<decimal>("ResourceId"),
                                          URLReference = model.Field<string>("URLReference"),
                                          CreatedOn = model.Field<DateTime>("CreatedOn"),
                                          IsActive = model.Field<bool>("IsActive"),
                                          IsApproved = model.Field<bool>("IsApproved")
                                      }).ToList();
                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                    {

                        ratings = (from model in ds.Tables[4].AsEnumerable()
                                      select new Rating()
                                      {
                                          Star = model.Field<double>("Rating"),
                                          UserCount = model.Field<int>("NoOfUsers")
                                      }).ToList();
                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                    {

                        comments = (from model in ds.Tables[3].AsEnumerable()
                                    select new ResourceComments()
                                    {
                                        Id = model.Field<decimal>("Id"),
                                        ResourceId = model.Field<decimal>("ResourceId"),
                                        Comments = model.Field<string>("Comments"),
                                        CommentedBy = model.Field<string>("CommentedBy"),
                                        CommentedById = model.Field<int>("CommentedById"),
                                        CommentorImage = model.Field<string>("CommentorImage"),
                                        CommentDate = model.Field<DateTime>("CommentDate"),
                                        ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
                                    }).ToList();
                    }

                    resources = (from model in ds.Tables[0].AsEnumerable()
                                 select new Resource()
                                 {
                                     Id = model.Field<decimal>("Id"),
                                     Title = model.Field<string>("Title"),
                                     Category = new ShortCategory { Id = model.Field<int>("CategoryId"), Name = model.Field<string>("CategoryName"), Name_Ar = model.Field<string>("CategoryName_Ar") },
                                     SubCategory = model.Field<int?>("SubCategoryId") != null ? new ShortSubCategory { Id = model.Field<int>("SubCategoryId"), CategoryId = model.Field<int>("CategoryId"), Name = model.Field<string>("SubCategoryName") , Name_Ar = model.Field<string>("SubCategoryName_Ar") } : null,
                                     CopyRight = model.Field<int?>("CopyRightId") != null ? new ShortCopyright { Id = model.Field<int>("CopyRightId"), Title_Ar = model.Field<string>("CopyrightTitle_Ar"), Title = model.Field<string>("CopyrightTitle"), Description = model.Field<string>("CopyrightDescription"), Description_Ar = model.Field<string>("CopyrightDescription_Ar"), Media = model.Field<string>("Media"), Protected = model.Field<bool>("Protected") } : null,
                                     Thumbnail = model.Field<string>("Thumbnail"),
                                     ResourceContent = model.Field<string>("ResourceContent"),
                                     MaterialType = new ShortMaterialType { Id = model.Field<int?>("MaterialTypeId"), Name = model.Field<string>("MaterialTypeName") , Name_Ar = model.Field<string>("MaterialTypeName_Ar") },
                                     ResourceDescription = model.Field<string>("ResourceDescription"),
                                     Keywords = model.Field<string>("Keywords"),
                                     CreatedBy = model.Field<string>("CreatedBy"),
                                     CreatedById = model.Field<int>("CreatedById"),
                                     CreatedOn = model.Field<DateTime>("CreatedOn"),
                                     IsDraft = model.Field<bool>("IsDraft"),
                                     Rating = model.Field<double>("Rating"),
                                     AllRatings = ratings,
                                     ReadingTime = model.Field<int?>("ReadingTime"),
                                     AlignmentRating = model.Field<double>("AlignmentRating"),
                                     IsApproved = model.Field<bool?>("IsApproved"),
                                     ReportAbuseCount = model.Field<int?>("ReportAbuseCount"),
                                     IsRemix = model.Field<bool>("IsRemix"),
                                     Standard = model.Field<string>("Standard"),
                                   //  EducationalUse = model.Field<string>("EducationalUse"),
                                     //Level = model.Field<string>("Level"),
                                     Objective = model.Field<string>("Objective"),
                                     Format = model.Field<string>("Format"),
                                     LastView = model.Field<DateTime?>("LastView"),
                                     SharedCount = model.Field<int?>("SharedCount"),
                                     ViewCount = model.Field<int?>("ViewCount"),
                                     EducationLevel = new EducationLevel { Id = model.Field<int?>("LevelId"), Level = model.Field<string>("LevelName"), Level_Ar = model.Field<string>("Level_Ar") },
                                     EducationalUse = new EducationalUse { Id = model.Field<int?>("EducationalUseId"), Text = model.Field<string>("EducationalUseName"), Text_Ar = model.Field<string>("EducationalUse_Ar") },
                                     EducationalStandard = new EducationalStandard { Id = model.Field<int?>("EducationalStandardId"), Standard = model.Field<string>("Standard"), Standard_Ar = model.Field<string>("Standard_Ar") },
                                     References = references != null && references.Count > 0 ? references.Where(r => r.ResourceId == model.Field<decimal>("Id")).ToList() : null,
                                     ResourceFiles = resourceFilesList != null && resourceFilesList.Count > 0 ? resourceFilesList.Where(f => f.ResourceId == model.Field<decimal>("Id")).ToList() : null,
                                     ResourceComments = comments != null && comments.Count > 0 ? comments.Where(c => c.ResourceId == model.Field<decimal>("Id")).ToList() : null,
                                     CommunityBadge = model.Field<bool?>("CommunityBadge"),
                                     MoEBadge = model.Field<bool?>("MoEBadge")
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

        public async Task<DatabaseResponse> GetRatingsByContent(List<RatingRequest> ratingRequest)
        {
            CommonHelper helper = new CommonHelper();
            try
            {
                SqlParameter[] parameters =
                 {
                    new SqlParameter( "@Content",  SqlDbType.NVarChar )
                };

                parameters[0].Value = ratingRequest != null ? helper.GetJsonString(ratingRequest) : null;
                _DataHelper = new DataAccessHelper("GetRatingsByContent", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<RatingResponse> ratingResponse = new List<RatingResponse>();
                if(dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {
                    ratingResponse = (from model in dt.AsEnumerable()
                                      select new RatingResponse()
                                      {   
                                          ContentId = model.Field<int>("ContentId"),
                                          ContentType = model.Field<int>("ContentType"),
                                          Rating = model.Field<decimal?>("Rating"),
                                          AllRatings = model.Field<string>("AllRatings") != null ? JsonConvert.DeserializeObject<List<Rating>>(model.Field<string>("AllRatings")) : null ,
                                          
                                      }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = ratingResponse };
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

        public async Task<DatabaseResponse> SearchResources(string keyword, int pageNumber, int pageSize)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@SearchKeyword",  SqlDbType.NVarChar ),
                    new SqlParameter( "@PageNo",  SqlDbType.Int ),
                    new SqlParameter( "@PageSize",  SqlDbType.Int )

                };

                parameters[0].Value = keyword;
                parameters[1].Value = pageNumber;
                parameters[2].Value = pageSize;
                _DataHelper = new DataAccessHelper("sps_ResourcesByKeyword", parameters, _configuration);

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
                                     CopyRight = model.Field<int?>("CopyRightId") != null ? new ShortCopyright { Id = model.Field<int>("CopyRightId"), Title = model.Field<string>("CopyrightTitle"), Description = model.Field<string>("CopyrightDescription"), Media = model.Field<string>("Media"), Protected = model.Field<bool>("Protected") , IsResourceProtect = model.Field<bool>("IsResourceProtect") } : null,
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
                                     AlignmentRating = model.Field<double>("AlignmentRating"),
                                     IsApproved = model.Field<bool?>("IsApproved"),
                                     ReportAbuseCount = model.Field<int?>("ReportAbuseCount"),
                                     TotalRows = model.Field<Int64>("Totalrows")
                                     //  References = references != null && references.Count > 0 ? references.Where(r => r.ResourceId == model.Field<decimal>("Id")).ToList() : null,
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
        public async Task<DatabaseResponse> DeleteResource(int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Decimal )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("DeleteResource", parameters, _configuration);

                int result = await _DataHelper.RunAsync();
                if (result == 103)
                {
                    string uri = _configuration.GetValue<string>("ElasticURL");
                    HttpClient client = new HttpClient();
                    await client.DeleteAsync(uri + @"resources/_doc/" + id);
                }
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

        public async Task<DatabaseResponse> InsertAssociatedResourceFile(int id,string filePath)
        {
            try
            {
                var fileName = filePath.Substring(filePath.LastIndexOf(("/")) + 1);
                SqlParameter[] parameters =
               {
                    new SqlParameter( "@ResourceId",  SqlDbType.Int ),
                    new SqlParameter( "@AssociatedFile",  SqlDbType.NVarChar ),
                    new SqlParameter( "@FileName",  SqlDbType.NVarChar ),
                    new SqlParameter( "@IsInclude",SqlDbType.Bit)

                };

                parameters[0].Value = id;
                parameters[1].Value = filePath;
                parameters[2].Value = fileName;
                parameters[3].Value = 1;
                _DataHelper = new DataAccessHelper("spi_ResourceAssociatedFiles", parameters, _configuration);

                int result = await _DataHelper.RunAsync();

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
        public async Task<DatabaseResponse> UpdateResource(UpdateResourceRequest resource, int id)
        {
            try
            {
                CommonHelper helper = new CommonHelper();

                SqlParameter[] parameters =
                {
                    new SqlParameter( "@Title",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CategoryId",  SqlDbType.Int ),
                    new SqlParameter( "@SubCategoryId",  SqlDbType.Int ),
                    new SqlParameter( "@Thumbnail",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ResourceDescription",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Keywords",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ResourceContent",  SqlDbType.NVarChar ),
                    new SqlParameter( "@MaterialTypeId",  SqlDbType.Int ),
                    new SqlParameter( "@CopyRightId",  SqlDbType.Int ),
                    new SqlParameter( "@IsDraft",  SqlDbType.Bit ),
                    new SqlParameter( "@References",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ResourceFiles",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Id",  SqlDbType.Decimal ),
                    new SqlParameter( "@ReadingTime",  SqlDbType.Int ),
                    new SqlParameter( "@LevelId",  SqlDbType.Int ),
                    new SqlParameter( "@EducationalStandardId",  SqlDbType.Int ),
                    new SqlParameter( "@EducationalUseId",  SqlDbType.Int ),
                    new SqlParameter( "@Format",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Objective",  SqlDbType.NVarChar ),

                };

                parameters[0].Value = resource.Title;
                parameters[1].Value = resource.CategoryId;
                parameters[2].Value = resource.SubCategoryId == null || resource.SubCategoryId == 0 ? null : resource.SubCategoryId;
                parameters[3].Value = resource.Thumbnail;
                parameters[4].Value = resource.ResourceDescription;
                parameters[5].Value = resource.Keywords;
                parameters[6].Value = resource.ResourceContent;
                parameters[7].Value = resource.MaterialTypeId == null || resource.MaterialTypeId == 0 ? null : resource.MaterialTypeId;
                parameters[8].Value = resource.CopyRightId == null || resource.CopyRightId == 0 ? null : resource.CopyRightId;
                parameters[9].Value = resource.IsDraft;
                parameters[10].Value = resource.References != null ? helper.GetJsonString(resource.References) : null;
                parameters[11].Value = resource.ResourceFiles != null ? helper.GetJsonString(resource.ResourceFiles) : null;
                parameters[12].Value = resource.Id;
                parameters[13].Value = resource.ReadingTime;
                parameters[14].Value = resource.LevelId == 0 ? null : resource.LevelId;
                parameters[15].Value = resource.EducationalStandardId == 0 ? null : resource.EducationalStandardId;
                parameters[16].Value = resource.EducationalUseId == 0 ? null : resource.EducationalUseId;
                parameters[17].Value = resource.Format;
                parameters[18].Value = resource.Objective;
                _DataHelper = new DataAccessHelper("UpdateResource", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                List<Resource> resources = new List<Resource>();

                List<ResourceFileMaster> resourceFilesList = new List<ResourceFileMaster>();

                List<ReferenceMaster> references = new List<ReferenceMaster>();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {


                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {


                        resourceFilesList = (from model in ds.Tables[1].AsEnumerable()
                                             select new ResourceFileMaster()
                                             {
                                                 Id = model.Field<decimal>("Id"),
                                                 ResourceId = model.Field<decimal>("ResourceId"),
                                                 AssociatedFile = model.Field<string>("AssociatedFile"),
                                                 UploadedDate = model.Field<DateTime>("UploadedDate")
                                             }).ToList();
                    }



                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    {

                        references = (from model in ds.Tables[2].AsEnumerable()
                                      select new ReferenceMaster()
                                      {
                                          Id = model.Field<decimal>("Id"),
                                          ResourceId = model.Field<decimal>("ResourceId"),
                                          URLReference = model.Field<string>("URLReference"),
                                          CreatedOn = model.Field<DateTime>("CreatedOn")
                                      }).ToList();
                    }

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
                                     AlignmentRating = model.Field<double>("AlignmentRating"),
                                     IsApproved = model.Field<bool?>("IsApproved"),
                                     ReportAbuseCount = model.Field<int?>("ReportAbuseCount"),
                                     References = references != null && references.Count > 0 ? references.Where(r => r.ResourceId == model.Field<decimal>("Id")).ToList() : null,
                                     ResourceFiles = resourceFilesList != null && resourceFilesList.Count > 0 ? resourceFilesList.Where(f => f.ResourceId == model.Field<decimal>("Id")).ToList() : null

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

        public async Task<DatabaseResponse> ApproveResource(decimal resourceId, int approvedBy)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@ResourceId",  SqlDbType.Decimal),
                    new SqlParameter( "@CreatedBy",  SqlDbType.Int )
                };

                parameters[0].Value = resourceId;
                parameters[1].Value = approvedBy;

                _DataHelper = new DataAccessHelper("ApproveResource", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds); // 115/116

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

        public async Task<DatabaseResponse> ResportResource(decimal id)
        {
            try
            {

                SqlParameter[] parameters =
                {
                    new SqlParameter( "@Id",  SqlDbType.Decimal )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("ReportResource", parameters, _configuration);

                int result = await _DataHelper.RunAsync();

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

        public async Task<DatabaseResponse> CommentOnResource(ResourceCommentRequest resourceComment)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@ResourceId",  SqlDbType.Decimal ),
                    new SqlParameter( "@Comments",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CommentedBy",  SqlDbType.Int )
                };

                parameters[0].Value = resourceComment.ResourceId;
                parameters[1].Value = resourceComment.Comments;
                parameters[2].Value = resourceComment.UserId;


                _DataHelper = new DataAccessHelper("CommentOnResource", parameters, _configuration);

                int result = await _DataHelper.RunAsync();

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

        public async Task<DatabaseResponse> ResportResourceComment(decimal id)
        {
            try
            {

                SqlParameter[] parameters =
                {
                    new SqlParameter( "@Id",  SqlDbType.Decimal )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("ReportResourceComment", parameters, _configuration);

                int result = await _DataHelper.RunAsync();

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

        public async Task<DatabaseResponse> UpdateResourceComment(ResourceCommentUpdateRequest resourceUpdate)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Decimal ),
                    new SqlParameter( "@ResourceId",  SqlDbType.Decimal ),
                    new SqlParameter( "@Comments",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CommentedBy",  SqlDbType.Int )
                };

                parameters[0].Value = resourceUpdate.Id;
                parameters[1].Value = resourceUpdate.ResourceId;
                parameters[2].Value = resourceUpdate.Comments;
                parameters[3].Value = resourceUpdate.UserId;

                _DataHelper = new DataAccessHelper("UpdateResourceComment", parameters, _configuration);

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

        public async Task<DatabaseResponse> DeleteResourceComment(decimal id, int requestedBy)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Decimal ),
                    new SqlParameter( "@RequestedBy",  SqlDbType.Int )

                };

                parameters[0].Value = id;

                parameters[1].Value = requestedBy;

                _DataHelper = new DataAccessHelper("DeleteResourceComment", parameters, _configuration);

                int result = await _DataHelper.RunAsync(); //103/102/114

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

        public async Task<DatabaseResponse> HideResourceCommentByAuthor(decimal id, decimal resourceId, int requestedBy)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Decimal ),
                    new SqlParameter( "@ResourceId",  SqlDbType.Decimal ),
                    new SqlParameter( "@RequestedBy",  SqlDbType.Int )
                };

                parameters[0].Value = id;
                parameters[1].Value = resourceId;
                parameters[2].Value = requestedBy;

                _DataHelper = new DataAccessHelper("HideResourceCommentByAuthor", parameters, _configuration);

                int result = await _DataHelper.RunAsync(); //101/106/117

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

        public async Task<DatabaseResponse> ReportResourceWithComment(ResourceReportAbuseWithComment resourceAbuseComment)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@ResourceId",  SqlDbType.Decimal ),
                    new SqlParameter( "@ReportReasons",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Comments",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ReportedBy",  SqlDbType.Int )
                };

                parameters[0].Value = resourceAbuseComment.ResourceId;
                parameters[1].Value = resourceAbuseComment.ReportReasons;
                parameters[2].Value = resourceAbuseComment.Comments;
                parameters[3].Value = resourceAbuseComment.ReportedBy;

                _DataHelper = new DataAccessHelper("ReportResourceWithComment", parameters, _configuration);

                int result = await _DataHelper.RunAsync();

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

        public async Task<DatabaseResponse> RateResource(ResourceRatingRequest resourceRatingRequest)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@ResourceId",  SqlDbType.Decimal ),
                    new SqlParameter( "@Rating",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Comments",  SqlDbType.NVarChar ),
                    new SqlParameter( "@RatedBy",  SqlDbType.Int )
                };

                parameters[0].Value = resourceRatingRequest.ResourceId;
                parameters[1].Value = resourceRatingRequest.Rating;
                parameters[2].Value = resourceRatingRequest.Comments;
                parameters[3].Value = resourceRatingRequest.RatedBy;

                _DataHelper = new DataAccessHelper("RateResource", parameters, _configuration);

                int result = await _DataHelper.RunAsync();

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

        public async Task<DatabaseResponse> ReportResourceCommentWithComment(ResourceCommentReportAbuseWithComment resourceAbuseComment)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@ResourceCommentId",  SqlDbType.Decimal ),
                    new SqlParameter( "@ReportReasons",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Comments",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ReportedBy",  SqlDbType.Int )
                };

                parameters[0].Value = resourceAbuseComment.ResourceCommentId;
                parameters[1].Value = resourceAbuseComment.ReportReasons;
                parameters[2].Value = resourceAbuseComment.Comments;
                parameters[3].Value = resourceAbuseComment.ReportedBy;

                _DataHelper = new DataAccessHelper("ReportResourceCommentWithComment", parameters, _configuration);

                int result = await _DataHelper.RunAsync();

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

        public async Task<DatabaseResponse> ResourceAlignmentRating(ResourceAlignmentRatingRequest resourceRatingRequest)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@ResourceId",  SqlDbType.Decimal ),
                    new SqlParameter( "@CategoryId",  SqlDbType.Int ),
                    new SqlParameter( "@LevelId",  SqlDbType.Int),
                    new SqlParameter( "@Rating",  SqlDbType.Int ),
                    new SqlParameter( "@RatedBy",  SqlDbType.Int )
                };

                parameters[0].Value = resourceRatingRequest.ResourceId;
                parameters[1].Value = resourceRatingRequest.CategoryId;
                parameters[2].Value = resourceRatingRequest.LevelId;
                parameters[3].Value = resourceRatingRequest.Rating;
                parameters[4].Value = resourceRatingRequest.RatedBy;

                _DataHelper = new DataAccessHelper("RateResourceAlignment", parameters, _configuration);

                int result = await _DataHelper.RunAsync();

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
    }
}
