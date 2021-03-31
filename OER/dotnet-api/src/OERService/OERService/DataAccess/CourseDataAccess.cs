using Core.Enums;
using Core.Helpers;
using Core.Models;
using DinkToPdf.Contracts;
using Microsoft.Extensions.Configuration;
using OERService.Helpers;
using OERService.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OERService.DataAccess
{
	public class CourseDataAccess
	{
		internal DataAccessHelper _DataHelper = null;
		private readonly IConverter _converter;
		private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="converter"></param>
        public CourseDataAccess(IConfiguration configuration, IConverter converter)
        {
            _configuration = configuration;
            _converter = converter;
        }
        public CourseDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<CourseFileInfo> DownloadCourseFiles(int CourseID)
		{
			CourseFileInfo courseFileInfo = null;
			try
			{
				courseFileInfo = new CourseFileInfo();
				SqlParameter[] parameters =
			{
				new SqlParameter( "@CourseID",  SqlDbType.Int )
				};

				parameters[0].Value = CourseID;
				_DataHelper = new DataAccessHelper("sps_CourseAssociatedFiles", parameters, _configuration);
				DataSet ds = new DataSet();
				List<CourseFiles> courseFiles = new List<CourseFiles>();
				List<ResourceFiles> resourceFiles = new List<ResourceFiles>();
				await _DataHelper.RunAsync(ds);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
				{
					courseFiles = (from model in ds.Tables[0].AsEnumerable()
								   select new CourseFiles()
								   {
									   Id = model.Field<decimal>("Id"),
									   CourseId = model.Field<decimal>("CourseId"),
									   AssociatedFile = model.Field<string>("AssociatedFile"),
									   CreatedOn = model.Field<DateTime>("CreatedOn"),
									   FileName = model.Field<string>("FileName")
								   }).ToList();
				}

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
				{
					resourceFiles = (from model in ds.Tables[1].AsEnumerable()
									 select new ResourceFiles()
									 {
										 Id = model.Field<decimal>("Id"),
										 ResourceId = model.Field<decimal>("ResourceId"),
										 AssociatedFile = model.Field<string>("AssociatedFile"),
										 UploadedDate = model.Field<DateTime>("UploadedDate"),
										 FileName = model.Field<string>("FileName"),
									 }).ToList();
				}

				courseFileInfo.CourseFiles = courseFiles;
				courseFileInfo.ResourceFiles = resourceFiles;
			}
			catch (Exception ex)
			{
				Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Medium));
				throw;
			}
			return courseFileInfo;
		}
		public async Task<DatabaseResponse> SubmitUserAnswers(UserAnswersOptions answerOptionS)
		{
			try
			{
				DataTable dtAnswer = new DataTable();
				dtAnswer.Columns.Add("QuestionId", typeof(int));
				dtAnswer.Columns.Add("AnswerId", typeof(int));
				dtAnswer.Columns.Add("OptionText", typeof(string));
				dtAnswer.Columns.Add("CorrectOption", typeof(string));
				DataRow drAns = null;
				foreach (Answers answer in answerOptionS.answers)
				{
					drAns = dtAnswer.NewRow();
					drAns["QuestionId"] = answer.QuestionId;
					drAns["AnswerId"] = answer.AnswerId;
					drAns["OptionText"] = "test";
					drAns["CorrectOption"] = true;
					dtAnswer.Rows.Add(drAns);

				}



				SqlParameter[] parameters =
			{
					new SqlParameter( "@UT_AnswerOptions",  SqlDbType.Structured ),
					 new SqlParameter( "@UserId",  SqlDbType.NVarChar ),
				new SqlParameter( "@CourseId",  SqlDbType.Int ),

				};

				parameters[0].Value = dtAnswer;
				parameters[1].Value = answerOptionS.UserId;
				parameters[2].Value = answerOptionS.CourseId;

				_DataHelper = new DataAccessHelper("spi_CourseTestTake", parameters, _configuration);

				DataTable dt1 = new DataTable();

				int result = await _DataHelper.RunAsync(dt1);

				if (dt1 != null && dt1.Rows.Count > 0)
				{
					var idlist = dt1.AsEnumerable().Select(r => r.Field<int>("ID")).ToArray();
					string res = string.Join(",", idlist);
					SqlParameter[] parametersCorrect =
				{
					 new SqlParameter( "@UserId",  SqlDbType.NVarChar ),
				new SqlParameter( "@CourseId",  SqlDbType.Int ),
				new SqlParameter( "@UserTestId",  SqlDbType.NVarChar )

				};
					parametersCorrect[0].Value = answerOptionS.UserId;
					parametersCorrect[1].Value = answerOptionS.CourseId;
					parametersCorrect[2].Value = res;
					_DataHelper = new DataAccessHelper("sps_TestResults", parametersCorrect, _configuration);
					DataTable dt = new DataTable();
					await _DataHelper.RunAsync(dt);
					if (dt != null && dt.Rows.Count > 0)
					{
						StringBuilder builder = new StringBuilder();
						int i = 1;
						builder.Append("<h2>Please find your test results below : </h2></br></br>");
						int skip = 0, innerSkip = 0;
						foreach (DataRow item in dt.Rows)
						{
							if (skip != 0)
							{
								skip--;
								continue;
							}
							string Id = item["Id"].ToString();
							string answerOption = item["UserAnswer"].ToString();
							DataTable dt2 = dt.Copy();
							builder.Append("<ul style='margin-bottom: 10px !important;background: #F8FAF5 !important;border: 1px solid #C3D1A3 !important;padding: 5px !important;list-style: none !important;'><b>" + i + ". Question : " + item["QuestionText"] + "</b>");
							foreach (DataRow dr in dt2.Rows)
							{
								if (innerSkip != 0)
								{
									innerSkip--;
									continue;
								}
								if (dr["Id"].ToString() == Id)
								{
									skip++;
									if (dr["CorrectOption"].ToString() == "True")
									{
										builder.Append("<li style='padding: 3px !important;margin-bottom: 2px !important;background-image: none !important;margin-left: 0 !important;list-style: none !important;border: 0 !important;background: #6DB46D !important;'><label><input class='wpProQuiz_questionInput' type='checkbox' name='question_1" + skip + "' value='" + skip + "' disabled='disabled' checked></label>" + dr["AnswerOption"] + "</li>");
									}
									else if (dr["AnswerOption"].ToString() == answerOption && dr["CorrectOption"].ToString() == "False")
									{
										builder.Append("<li style='padding: 3px !important;margin-bottom: 2px !important;background-image: none !important;margin-left: 0 !important;list-style: none !important;border: 0 !important;background: #FF9191 !important;'><label><input class='wpProQuiz_questionInput' type='checkbox' name='question_1" + skip + "' value='" + skip + "' disabled='disabled' checked></label>" + dr["AnswerOption"] + "</li>");
									}
									else
									{
										builder.Append("<li style='padding: 3px !important;margin-bottom: 2px !important;background-image: none !important;margin-left: 0 !important;list-style: none !important;border: 0 !important;'><label><input class='wpProQuiz_questionInput' type='checkbox' name='question_1" + skip + "' value='" + skip + "' disabled='disabled'></label>" + dr["AnswerOption"].ToString() + "</li>");
									}

									if (dt2.Rows.IndexOf(dr) == dt2.Rows.Count - 1)
									{
										builder.Append("</ul>");
										dt2.Clear();
										break;
									}
								}
								else
								{
									builder.Append("</ul>");
									dt2.Clear();
									break;
								}
							}

							i++;
							innerSkip = skip;
							skip--;


						}
						UserEmail userEmail = new UserEmail();
						userEmail.Email = Convert.ToString(dt.Rows[0]["Email"]);
						userEmail.Body = Emailer.CreateEmailBodyForTests(Convert.ToString(dt.Rows[0]["UserName"]), builder.ToString(),_configuration);
						userEmail.Subject = "Test Results";

						await Emailer.SendEmailAsync(userEmail, _configuration);
					}
				}

				return new DatabaseResponse { ResponseCode = result, Results = null };
			}
			catch (Exception ex)
			{
				Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Medium));
				throw;
			}
		}
		private async Task<DatabaseResponse> CreateTest(CreateCourseRequest course, int courseId
			)
		{
			try
			{

				DataTable dtQues = new DataTable();
				dtQues.Columns.Add("QuestionId", typeof(int));
				dtQues.Columns.Add("QuestionText", typeof(string));
				dtQues.Columns.Add("Media", typeof(string));
				dtQues.Columns.Add("FileName", typeof(string));

				DataTable dtAnswer = new DataTable();
				dtAnswer.Columns.Add("QuestionId", typeof(int));
				dtAnswer.Columns.Add("AnswerId", typeof(int));
				dtAnswer.Columns.Add("OptionText", typeof(string));
				dtAnswer.Columns.Add("CorrectOption", typeof(string));

				DataRow dr;
				DataRow drAns;
				int count = 0;
				foreach (Questions item in course.Tests.FirstOrDefault().Questions)
				{
					count = count + 1;
					dr = dtQues.NewRow();

					dr["QuestionId"] = count;
					dr["QuestionText"] = item.QuestionText;
					dr["Media"] = item.Media;
					dr["FileName"] = item.FileName;

					dtQues.Rows.Add(dr);
					int answerid = 100;
					foreach (AnswerOptions answer in item.AnswerOptions)
					{
						answerid = answerid + 1;
						drAns = dtAnswer.NewRow();
						drAns["QuestionId"] = count;
						drAns["AnswerId"] = answerid;
						drAns["OptionText"] = answer.OptionText;
						drAns["CorrectOption"] = answer.CorrectAnswer;

						dtAnswer.Rows.Add(drAns);

					}
				}



				SqlParameter[] parameters =
			{
					new SqlParameter( "@UT_Questions",  SqlDbType.Structured ),
					new SqlParameter( "@UT_AnswerOptions",  SqlDbType.Structured ),
					 new SqlParameter( "@CourseId",  SqlDbType.Int ),
					 new SqlParameter( "@TestName",  SqlDbType.NVarChar ),
					  new SqlParameter( "@CreatedBy",  SqlDbType.Int )

				};

				parameters[0].Value = dtQues;
				parameters[1].Value = dtAnswer;
				parameters[2].Value = courseId;
				parameters[3].Value = course.Tests.FirstOrDefault().TestName;
				parameters[4].Value = course.Tests.FirstOrDefault().CreatedBy;

				_DataHelper = new DataAccessHelper("spi_CourseTest", parameters, _configuration);

				DataTable dt1 = new DataTable();

				int result = await _DataHelper.RunAsync(dt1);
				return new DatabaseResponse { ResponseCode = result, Results = null };
			}
			catch (Exception ex)
			{
				Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Medium));
				throw;
			}
		}

		private async Task<DatabaseResponse> UpdateTest(Test test, int courseId)
		{
			try
			{

				DataTable dtQues = new DataTable();
				dtQues.Columns.Add("QuestionId", typeof(int));
				dtQues.Columns.Add("QuestionText", typeof(string));
				dtQues.Columns.Add("Media", typeof(string));
				dtQues.Columns.Add("FileName", typeof(string));

				DataTable dtAnswer = new DataTable();
				dtAnswer.Columns.Add("QuestionId", typeof(int));
				dtAnswer.Columns.Add("AnswerId", typeof(int));
				dtAnswer.Columns.Add("OptionText", typeof(string));
				dtAnswer.Columns.Add("CorrectOption", typeof(string));

				DataRow dr;
				DataRow drAns;
				int count = 0;
				foreach (Questions item in test.Questions)
				{
					count = count + 1;
					dr = dtQues.NewRow();

					dr["QuestionId"] = count;
					dr["QuestionText"] = item.QuestionText;
					dr["Media"] = item.Media;
					dr["FileName"] = item.FileName;

					dtQues.Rows.Add(dr);
					int answerid = 100;
					foreach (AnswerOptions answer in item.AnswerOptions)
					{
						answerid = answerid + 1;
						drAns = dtAnswer.NewRow();
						drAns["QuestionId"] = count;
						drAns["AnswerId"] = answerid;
						drAns["OptionText"] = answer.OptionText;
						drAns["CorrectOption"] = answer.CorrectAnswer;

						dtAnswer.Rows.Add(drAns);

					}
				}



				SqlParameter[] parameters =
			{
					new SqlParameter( "@UT_Questions",  SqlDbType.Structured ),
					new SqlParameter( "@UT_AnswerOptions",  SqlDbType.Structured ),
					 new SqlParameter( "@CourseId",  SqlDbType.Int ),
					 new SqlParameter( "@TestName",  SqlDbType.NVarChar ),
					  new SqlParameter( "@CreatedBy",  SqlDbType.Int )

				};

				parameters[0].Value = dtQues;
				parameters[1].Value = dtAnswer;
				parameters[2].Value = courseId;
				parameters[3].Value = test.TestName;
				parameters[4].Value = test.CreatedBy;

				_DataHelper = new DataAccessHelper("spu_CourseTest", parameters, _configuration);

				DataTable dt1 = new DataTable();

				int result = await _DataHelper.RunAsync(dt1);
				return new DatabaseResponse { ResponseCode = result, Results = null };
			}
			catch (Exception ex)
			{
				Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Medium));
				throw;
			}
		}
		public async Task<DatabaseResponse> SharedContent(SharedContentInfoCreate sharedContentInfoCreate)
		{
			try
			{



				SqlParameter[] parameters =
			   {
					new SqlParameter( "@ContentId",  SqlDbType.Int ),
					new SqlParameter( "@ContentTypeId",  SqlDbType.Int ),
					new SqlParameter( "@SocialMediaName",  SqlDbType.VarChar ),
					new SqlParameter( "@CreatedBy",  SqlDbType.Int )

				};

				parameters[0].Value = sharedContentInfoCreate.ContentId;
				parameters[1].Value = sharedContentInfoCreate.ContentTypeId;
				parameters[2].Value = sharedContentInfoCreate.SocialMediaName;
				parameters[3].Value = sharedContentInfoCreate.CreatedBy;

				_DataHelper = new DataAccessHelper("spi_ContentSharedInfo", parameters, _configuration);

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

		public async Task<DatabaseResponse> DownloadContent(DownloadContentInfoCreate downloadContentInfoCreate)
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
				parameters[1].Value = 1;
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

		public async Task<DatabaseResponse> CreateCourse(CreateCourseRequest course)
		{
			try
			{
				CommonHelper helper = new CommonHelper();


				DataTable dtSection = new DataTable();
				dtSection.Columns.Add("Id", typeof(int));
				dtSection.Columns.Add("Name", typeof(string));

				DataTable dtResource = new DataTable();
				dtResource.Columns.Add("Id", typeof(int));
				dtResource.Columns.Add("ResourceId", typeof(int));
				dtResource.Columns.Add("SectionId", typeof(int));

				DataRow dr;
				DataRow drRes;
				int count = 0;
				foreach (Section item in course.sections)
				{
					if (!string.IsNullOrEmpty(item.Name))
					{
						count = count + 1;
						dr = dtSection.NewRow();

						dr["Id"] = count;
						dr["Name"] = item.Name;

						dtSection.Rows.Add(dr);
						int answerid = 100;
						foreach (CourseResource item2 in item.courseResources)
						{
							answerid = answerid + 1;
							drRes = dtResource.NewRow();
							drRes["SectionId"] = count;
							drRes["Id"] = answerid;
							drRes["ResourceId"] = item2.ResourceId;
							dtResource.Rows.Add(drRes);

						}
					}
				}



				SqlParameter[] parameters =
			   {
					new SqlParameter( "@Title",  SqlDbType.NVarChar ),
					new SqlParameter( "@CategoryId",  SqlDbType.Int ),
					new SqlParameter( "@SubCategoryId",  SqlDbType.Int ),
					new SqlParameter( "@Thumbnail",  SqlDbType.NVarChar ),
					new SqlParameter( "@CourseDescription",  SqlDbType.NVarChar ),
					new SqlParameter( "@Keywords",  SqlDbType.NVarChar ),
					new SqlParameter( "@CourseContent",  SqlDbType.NVarChar ),
					new SqlParameter( "@CopyRightId",  SqlDbType.Int ),
					new SqlParameter( "@IsDraft",  SqlDbType.Bit ),
					new SqlParameter( "@CreatedBy",  SqlDbType.Int ),
					new SqlParameter( "@EducationId",  SqlDbType.Int ),
					new SqlParameter( "@ProfessionId",  SqlDbType.Int ),
					new SqlParameter( "@References",  SqlDbType.NVarChar ),
					new SqlParameter( "@CourseFiles",  SqlDbType.NVarChar ),
					new SqlParameter( "@ReadingTime",  SqlDbType.Int ),
					new SqlParameter( "@UT_Sections",  SqlDbType.Structured ),
					new SqlParameter( "@UT_Resource",  SqlDbType.Structured ),
					new SqlParameter( "@LevelId",  SqlDbType.Int ),
					new SqlParameter( "@EducationalStandardId",  SqlDbType.Int ),
					new SqlParameter( "@EducationalUseId",  SqlDbType.Int ),
				};
				if (course.References.Count > 0)
				{
					foreach (Reference item in course.References.ToList())
					{
						if (item.URLReferenceId == 0)
						{
							course.References.Remove(item);
						}
					}
				}
				parameters[0].Value = course.Title;
				parameters[1].Value = course.CategoryId;
				parameters[2].Value = course.SubCategoryId == null || course.SubCategoryId == 0 ? null : course.SubCategoryId;
				parameters[3].Value = course.Thumbnail;
				parameters[4].Value = course.CourseDescription;
				parameters[5].Value = course.Keywords;
				parameters[6].Value = course.CourseContent;
				parameters[7].Value = course.CopyRightId == null || course.CopyRightId == 0 ? null : course.CopyRightId;
				parameters[8].Value = course.IsDraft;
				parameters[9].Value = course.CreatedBy;
				parameters[10].Value = course.EducationId == null || course.EducationId == 0 ? null : course.EducationId;
				parameters[11].Value = course.ProfessionId == null || course.ProfessionId == 0 ? null : course.ProfessionId;
				parameters[12].Value = course.References != null ? helper.GetJsonString(course.References) : null;
				parameters[13].Value = course.ResourceFiles != null ? helper.GetJsonString(course.ResourceFiles) : null;
				parameters[14].Value = course.ReadingTime == null || course.ReadingTime == 0 ? null : course.ReadingTime;
				parameters[15].Value = dtSection;
				parameters[16].Value = dtResource;
				parameters[17].Value = course.LevelId == null || course.LevelId == 0 ? null : course.LevelId;
				parameters[18].Value = course.EducationalStandardId == null || course.EducationalStandardId == 0 ? null : course.EducationalStandardId;
				parameters[19].Value = course.EducationalUseId == null || course.EducationalUseId == 0 ? null : course.EducationalStandardId;
				_DataHelper = new DataAccessHelper("CreateCourse", parameters, _configuration);

				DataSet ds = new DataSet();

				int result = await _DataHelper.RunAsync(ds);

				Course Createdresource = new Course();
				int courseid = 0;
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
				{
					courseid = Convert.ToInt32(ds.Tables[1].Rows[0]["Id"]);
					if (course.Tests.Count > 0 && course.Tests[0] != null && !string.IsNullOrEmpty(course.Tests.FirstOrDefault().TestName))
					{
						await CreateTest(course, courseid);
					}
					Createdresource = (from model in ds.Tables[1].AsEnumerable()
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
										   IsApproved = model.Field<bool?>("IsApproved"),
										   ReportAbuseCount = model.Field<int?>("ReportAbuseCount"),
										   Education = model.Field<int?>("EducationId") != null ? new Education { Id = model.Field<int?>("EducationId"), Name = model.Field<string>("EducationName") } : null,
										   Profession = model.Field<int?>("ProfessionId") != null ? new Profession { Id = model.Field<int?>("ProfessionId"), Name = model.Field<string>("ProfessionName") } : null,
										   //LevelId = model.Field<int?>("LevelId"),
										   //LevelName = model.Field<string>("LevelName"),
										   //Level_Ar = model.Field<string>("Level_Ar"),
										   //EducationalStandardId = model.Field<int?>("EducationalStandardId"),
										   //Standard = model.Field<string>("Standard"),
										   //Standard_Ar = model.Field<string>("Standard_Ar"),
										   //EducationalUseId = model.Field<int?>("EducationalUseId"),
										   //EducationalUseName = model.Field<string>("EducationalUseName"),
										   //EducationalUse_Ar = model.Field<string>("EducationalUse_Ar")
										   EducationLevel = new EducationLevel { Id = model.Field<int?>("LevelId"), Level = model.Field<string>("LevelName"), Level_Ar = model.Field<string>("Level_Ar") },
										   EducationalUse = new EducationalUse { Id = model.Field<int?>("EducationalUseId"), Text = model.Field<string>("EducationalUseName"), Text_Ar = model.Field<string>("EducationalUse_Ar") },
										   EducationalStandard = new EducationalStandard { Id = model.Field<int?>("EducationalStandardId"), Standard = model.Field<string>("Standard"), Standard_Ar = model.Field<string>("Standard_Ar") }
									   }).FirstOrDefault();
				}

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
				{

					Createdresource.AssociatedFiles = (from model in ds.Tables[2].AsEnumerable()
													   select new CourseAssociatedFiles()
													   {
														   Id = model.Field<decimal>("Id"),
														   CourseId = model.Field<decimal>("CourseId"),
														   AssociatedFile = model.Field<string>("AssociatedFile"),
														   CreatedOn = model.Field<DateTime>("CreatedOn")
													   }).ToList();
				}


				if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
				{

					Createdresource.References = (from model in ds.Tables[3].AsEnumerable()
												  select new CourseUrlReferences()
												  {
													  Id = model.Field<decimal>("Id"),
													  CourseId = model.Field<decimal>("CourseId"),
													  URLReference = model.Field<string>("URLReference"),
													  CreatedOn = model.Field<DateTime>("CreatedOn")
												  }).ToList();
				}
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
				{
					CourseSection CourseSection = null;
					List<CourseSection> obj = null;

					foreach (DataRow drItem in ds.Tables[4].Rows)
					{
						CourseSection = new CourseSection();
						CourseSection.Name = Convert.ToString(drItem["Name"]);
						obj = new List<CourseSection>();
						List<CourseResources> courseResources = new List<CourseResources>();
						CourseResources tcourseResources = null;
						string FilterCond1 = "SectionId=" + Convert.ToInt32(drItem["Id"]);
						foreach (DataRow dritem2 in ds.Tables[5].Select(FilterCond1))
						{
							tcourseResources = new CourseResources();
							tcourseResources.ResourceName = Convert.ToString(dritem2["title"]);
							courseResources.Add(tcourseResources);
						}

						obj.Add(CourseSection);
					}
					Createdresource.CourseSection = obj;
				}


				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow item in ds.Tables[0].Rows)
					{
						UserEmail userEmail = new UserEmail();
						userEmail.Email = Convert.ToString(item["Email"]);
						string text = "You have assigned a course to approve/reject";
						string buttonText = "Review Content";
						userEmail.Body = Emailer.CreateEmailBody(Convert.ToString(item["UserName"]), course.EmailUrl, text, buttonText,_configuration);
						userEmail.Subject = "You have been assigned a submission to review.";
						await Emailer.SendEmailAsync(userEmail, _configuration);
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
		public async Task<DatabaseResponse> SearchCourses(string keyword, int pageNumber, int pageSize)
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
				_DataHelper = new DataAccessHelper("sps_CoursesByKeyword", parameters, _configuration);

				DataSet ds = new DataSet();

				int result = await _DataHelper.RunAsync(ds);

				List<Course> resources = new List<Course>();


				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
				{


					resources = (from model in ds.Tables[0].AsEnumerable()
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
									 IsApproved = model.Field<bool?>("IsApproved"),
									 TotalRows = model.Field<Int64>("Totalrows"),
									 ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
									 Education = model.Field<int?>("EducationId") != null ? new Education { Id = model.Field<int>("EducationId"), Name = model.Field<string>("EducationName") } : null,
									 Profession = model.Field<int?>("ProfessionId") != null ? new Profession { Id = model.Field<int>("ProfessionId"), Name = model.Field<string>("ProfessionName") } : null,
									 //  References = references != null && references.Count > 0 ? references.Where(r => r.CourseId == model.Field<decimal>("Id")).ToList() : null,
									 //AssociatedFiles = resourceFilesList != null && resourceFilesList.Count > 0 ? resourceFilesList.Where(f => f.CourseId == model.Field<decimal>("Id")).ToList() : null,
									 //CourseComments = comments != null && comments.Count > 0 ? comments.Where(c => c.CourseId == model.Field<decimal>("Id")).ToList() : null
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
		public async Task<DatabaseResponse> GetAllCourse(int pageNo, int pageSize, string paramIsSearch, string sortType, int sortField)
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

                _DataHelper = new DataAccessHelper("GetCourses", parameters, _configuration);

				DataSet ds = new DataSet();

				int result = await _DataHelper.RunAsync(ds);

				List<Course> resources = new List<Course>();

				List<CourseAssociatedFiles> resourceFilesList = new List<CourseAssociatedFiles>();

				List<CourseUrlReferences> references = new List<CourseUrlReferences>();

				List<CourseComment> comments = new List<CourseComment>();

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
				{


					if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
					{


						resourceFilesList = (from model in ds.Tables[1].AsEnumerable()
											 select new CourseAssociatedFiles()
											 {
												 Id = model.Field<decimal>("Id"),
												 CourseId = model.Field<decimal>("CourseId"),
												 AssociatedFile = model.Field<string>("AssociatedFile"),
												 CreatedOn = model.Field<DateTime>("CreatedOn")
											 }).ToList();
					}



					if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
					{

						references = (from model in ds.Tables[2].AsEnumerable()
									  select new CourseUrlReferences()
									  {
										  Id = model.Field<decimal>("Id"),
										  CourseId = model.Field<decimal>("CourseId"),
										  URLReference = model.Field<string>("URLReference"),
										  CreatedOn = model.Field<DateTime>("CreatedOn")
									  }).ToList();
					}

					if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
					{

						comments = (from model in ds.Tables[3].AsEnumerable()
									select new CourseComment()
									{
										Id = model.Field<decimal>("Id"),
										CourseId = model.Field<decimal>("CourseId"),
										Comments = model.Field<string>("Comments"),
										CommentedBy = model.Field<string>("CommentedBy"),
										CommentedById = model.Field<int>("CommentedById"),
										CommentorImage = model.Field<string>("CommentorImage"),
										CommentDate = model.Field<DateTime>("CommentDate"),
										ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
									}).ToList();
					}


					resources = (from model in ds.Tables[0].AsEnumerable()
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
									 Rownumber = model.Field<Int64>("Rownumber"),
									 TotalRows = model.Field<Int64>("Totalrows"),
									 ViewCount = model.Field<int?>("ViewCount"),
									 SharedCount = model.Field<int?>("SharedCount"),
									 DownloadCount = model.Field<int?>("DownloadCount"),
									 EnrollmentCount = model.Field<int?>("EnrollmentCount"),
									 ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
									 Education = model.Field<int?>("EducationId") != null ? new Education { Id = model.Field<int>("EducationId"), Name = model.Field<string>("EducationName") } : null,
									 Profession = model.Field<int?>("ProfessionId") != null ? new Profession { Id = model.Field<int>("ProfessionId"), Name = model.Field<string>("ProfessionName") } : null,
									 References = references != null && references.Count > 0 ? references.Where(r => r.CourseId == model.Field<decimal>("Id")).ToList() : null,
									 AssociatedFiles = resourceFilesList != null && resourceFilesList.Count > 0 ? resourceFilesList.Where(f => f.CourseId == model.Field<decimal>("Id")).ToList() : null,
									 CourseComments = comments != null && comments.Count > 0 ? comments.Where(c => c.CourseId == model.Field<decimal>("Id")).ToList() : null,
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
		public async Task<DatabaseResponse> CourseEnrolledStatus(CourseEnrollmentCreate courseEnrollmentCreate)
		{
			try
			{

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@CourseId",  SqlDbType.Int ),
					new SqlParameter( "@UserId",  SqlDbType.Int )

				};

				parameters[0].Value = courseEnrollmentCreate.CourseId;
				parameters[1].Value = courseEnrollmentCreate.UserId;
				_DataHelper = new DataAccessHelper("sps_CourseEnrollmentStatus", parameters, _configuration);

				DataTable dt = new DataTable();

				int result = await _DataHelper.RunAsync(dt);

				CourseEnrollmentStatus courseEnrollmentStatus = new CourseEnrollmentStatus();

				if (dt != null && dt.Rows.Count > 0)
				{

					courseEnrollmentStatus = (from model in dt.AsEnumerable()
											  select new CourseEnrollmentStatus()
											  {
												  Id = model.Field<int>("Id"),
												  Active = model.Field<bool>("Active"),

											  }).FirstOrDefault();


				}



				return new DatabaseResponse { ResponseCode = result, Results = courseEnrollmentStatus };
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

		public async Task<Course> GetCoursePdfDetail(decimal id)
		{
			try
			{

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@Id",  SqlDbType.NVarChar )

				};

				parameters[0].Value = id;

				_DataHelper = new DataAccessHelper("GetCourseById", parameters, _configuration);

				DataSet ds = new DataSet();


                await _DataHelper.RunAsync(ds);

                Course Createdresource = new Course();

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
				{

					Createdresource = (from model in ds.Tables[0].AsEnumerable()
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
										   SharedCount = model.Field<int?>("SharedCount"),
										   Rating = model.Field<double>("Rating"),
										   ReadingTime = model.Field<int?>("ReadingTime"),
										   IsApproved = model.Field<bool?>("IsApproved"),
										   ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
										   Education = model.Field<int?>("EducationId") != null ? new Education { Id = model.Field<int>("EducationId"), Name = model.Field<string>("EducationName") } : null,
										   Profession = model.Field<int?>("ProfessionId") != null ? new Profession { Id = model.Field<int>("ProfessionId"), Name = model.Field<string>("ProfessionName") } : null,
										   LastView = model.Field<DateTime?>("LastView"),
										   EducationLevel = new EducationLevel { Id = model.Field<int?>("LevelId"), Level = model.Field<string>("LevelName"), Level_Ar = model.Field<string>("Level_Ar") },
										   EducationalUse = new EducationalUse { Id = model.Field<int?>("EducationalUseId"), Text = model.Field<string>("EducationalUseName"), Text_Ar = model.Field<string>("EducationalUse_Ar") },
										   EducationalStandard = new EducationalStandard { Id = model.Field<int?>("EducationalStandardId"), Standard = model.Field<string>("Standard"), Standard_Ar = model.Field<string>("Standard_Ar") },
										   ViewCount = model.Field<int?>("ViewCount"),
										   CommunityBadge = model.Field<bool?>("CommunityBadge"),
										   MoEBadge = model.Field<bool?>("MoEBadge")

									   }).FirstOrDefault();


				}

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
				{

					Createdresource.AssociatedFiles = (from model in ds.Tables[1].AsEnumerable()
													   select new CourseAssociatedFiles()
													   {
														   Id = model.Field<decimal>("Id"),
														   CourseId = model.Field<decimal>("CourseId"),
														   AssociatedFile = model.Field<string>("AssociatedFile"),
														   CreatedOn = model.Field<DateTime>("CreatedOn")
													   }).ToList();
				}


				if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
				{

					Createdresource.References = (from model in ds.Tables[2].AsEnumerable()
												  select new CourseUrlReferences()
												  {
													  Id = model.Field<decimal>("Id"),
													  CourseId = model.Field<decimal>("CourseId"),
													  URLReference = model.Field<string>("URLReference"),
													  CreatedOn = model.Field<DateTime>("CreatedOn")
												  }).ToList();
				}

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
				{

					Createdresource.CourseComments = (from model in ds.Tables[3].AsEnumerable()
													  select new CourseComment()
													  {
														  Id = model.Field<decimal>("Id"),
														  CourseId = model.Field<decimal>("CourseId"),
														  Comments = model.Field<string>("Comments"),
														  CommentedBy = model.Field<string>("CommentedBy"),
														  CommentedById = model.Field<int>("CommentedById"),
														  CommentorImage = model.Field<string>("CommentorImage"),
														  CommentDate = model.Field<DateTime>("CommentDate"),
														  ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
													  }).ToList();
				}

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
				{
					CourseSection CourseSection = null;
					List<CourseSection> obj = null;

					foreach (DataRow drItem in ds.Tables[4].Rows)
					{
						CourseSection = new CourseSection();
						CourseSection.Name = Convert.ToString(drItem["Name"]);
						obj = new List<CourseSection>();
						List<Resource> courseResources = new List<Resource>();
						Resource tcourseResources = null;
						string FilterCond1 = "SectionId=" + Convert.ToInt32(drItem["Id"]);
						foreach (DataRow dritem2 in ds.Tables[5].Select(FilterCond1))
						{
							tcourseResources = new Resource();

							tcourseResources.Id = Convert.ToInt32(dritem2["Id"]);
							tcourseResources.Title = Convert.ToString(dritem2["Title"]);
							tcourseResources.Category = new ShortCategory { Id = Convert.ToInt32(dritem2["CategoryId"]), Name = Convert.ToString(dritem2["CategoryName"]) };
							tcourseResources.SubCategory = dritem2["SubCategoryId"] != null ? new ShortSubCategory { Id = Convert.ToInt32(dritem2["SubCategoryId"]), CategoryId = Convert.ToInt32(dritem2["CategoryId"]), Name = Convert.ToString(dritem2["SubCategoryName"]) } : null;
							// tcourseResources.CopyRight = model.Field<int?>("CopyRightId") != null ? new ShortCopyright { Id = model.Field<int>("CopyRightId"), Title = model.Field<string>("CopyrightTitle"), Description = model.Field<string>("CopyrightDescription") } : null,
							tcourseResources.Thumbnail = Convert.ToString(dritem2["Thumbnail"]);
							tcourseResources.ResourceContent = Convert.ToString(dritem2["ResourceContent"]);
							tcourseResources.MaterialType = new ShortMaterialType { Id = Convert.ToInt32(dritem2["MaterialTypeId"]), Name = Convert.ToString(dritem2["MaterialTypeName"]) };
							tcourseResources.ResourceDescription = Convert.ToString(dritem2["ResourceDescription"]);
							tcourseResources.Keywords = Convert.ToString(dritem2["Keywords"]);
							tcourseResources.CreatedBy = Convert.ToString(dritem2["CreatedBy"]);
							tcourseResources.CreatedById = Convert.ToInt32(dritem2["CreatedById"]);
							tcourseResources.CreatedOn = Convert.ToDateTime(dritem2["CreatedOn"]);
							tcourseResources.IsDraft = Convert.ToBoolean(dritem2["IsDraft"]);
							tcourseResources.Rating = Convert.ToDouble(dritem2["Rating"]);
							tcourseResources.ReadingTime = Convert.ToInt32(dritem2["ReadingTime"]);
							tcourseResources.AlignmentRating = Convert.ToDouble(dritem2["AlignmentRating"]);
							tcourseResources.IsApproved = Convert.ToBoolean(dritem2["IsApproved"]);
							tcourseResources.ReportAbuseCount = Convert.ToInt32(dritem2["ReportAbuseCount"]);
							tcourseResources.LastView = dritem2["LastView"] == null ? DateTime.Now : (Convert.ToDateTime(dritem2["LastView"]));
							tcourseResources.Standard = Convert.ToString(dritem2["Standard"]);
							tcourseResources.Objective = Convert.ToString(dritem2["Objective"]);
							tcourseResources.Format = Convert.ToString(dritem2["Format"]);
							courseResources.Add(tcourseResources);
						}
						CourseSection.courseResources = courseResources;
						obj.Add(CourseSection);
					}
					Createdresource.CourseSection = obj;
				}
				return Createdresource;
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

		public async Task<DatabaseResponse> GetCourse(decimal id)
		{
			try
			{

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@Id",  SqlDbType.NVarChar )

				};

				parameters[0].Value = id;

				_DataHelper = new DataAccessHelper("GetCourseById", parameters, _configuration);

				DataSet ds = new DataSet();

				int result = await _DataHelper.RunAsync(ds);

				List<Rating> ratings = new List<Rating>();

				Course Createdresource = new Course();
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[6] != null && ds.Tables[6].Rows.Count > 0)
				{

					ratings = (from model in ds.Tables[6].AsEnumerable()
							   select new Rating()
							   {
								   Star = model.Field<double>("Rating"),
								   UserCount = model.Field<int>("NoOfUsers")
							   }).ToList();
				}

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
				{

					Createdresource = (from model in ds.Tables[0].AsEnumerable()
									   select new Course()
									   {
										   Id = model.Field<decimal>("Id"),
										   Title = model.Field<string>("Title"),
										   Category = new ShortCategory { Id = model.Field<int>("CategoryId"), Name = model.Field<string>("CategoryName"), Name_Ar = model.Field<string>("CategoryName_Ar") },
										   SubCategory = model.Field<int?>("SubCategoryId") != null ? new ShortSubCategory { Id = model.Field<int>("SubCategoryId"), CategoryId = model.Field<int>("CategoryId"), Name = model.Field<string>("SubCategoryName"), Name_Ar = model.Field<string>("SubCategoryName_Ar") } : null,
										   CopyRight = model.Field<int?>("CopyRightId") != null ? new ShortCopyright { Id = model.Field<int>("CopyRightId"), Title_Ar = model.Field<string>("CopyrightTitle_Ar"), Title = model.Field<string>("CopyrightTitle"), Description = model.Field<string>("CopyrightDescription"), Description_Ar = model.Field<string>("CopyrightDescription_Ar"), Media = model.Field<string>("Media"), Protected = model.Field<bool>("Protected") } : null,
										   Thumbnail = model.Field<string>("Thumbnail"),
										   CourseContent = model.Field<string>("CourseContent"),
										   CourseDescription = model.Field<string>("CourseDescription"),
										   Keywords = model.Field<string>("Keywords"),
										   CreatedBy = model.Field<string>("CreatedBy"),
										   CreatedById = model.Field<int>("CreatedById"),
										   CreatedOn = model.Field<DateTime>("CreatedOn"),
										   IsDraft = model.Field<bool>("IsDraft"),
										   SharedCount = model.Field<int?>("SharedCount"),
										   Rating = model.Field<double>("Rating"),
										   AllRatings = ratings,
										   ReadingTime = model.Field<int?>("ReadingTime"),
										   IsApproved = model.Field<bool?>("IsApproved"),
										   ReportAbuseCount = model.Field<int?>("ReportAbuseCount"),
										   Education = model.Field<int?>("EducationId") != null ? new Education { Id = model.Field<int>("EducationId"), Name = model.Field<string>("EducationName"), Name_Ar = model.Field<string>("EducationName_Ar") } : null,
										   Profession = model.Field<int?>("ProfessionId") != null ? new Profession { Id = model.Field<int>("ProfessionId"), Name = model.Field<string>("ProfessionName"), Name_Ar = model.Field<string>("ProfessionName_Ar") } : null,
										   LastView = model.Field<DateTime?>("LastView"),
										   EducationLevel = new EducationLevel { Id = model.Field<int?>("LevelId"), Level = model.Field<string>("LevelName"), Level_Ar = model.Field<string>("Level_Ar") },
										   EducationalUse = new EducationalUse { Id = model.Field<int?>("EducationalUseId"), Text = model.Field<string>("EducationalUseName"), Text_Ar = model.Field<string>("EducationalUse_Ar") },
										   EducationalStandard = new EducationalStandard { Id = model.Field<int?>("EducationalStandardId"), Standard = model.Field<string>("Standard"), Standard_Ar = model.Field<string>("Standard_Ar") },
										   ViewCount = model.Field<int?>("ViewCount"),
										   CommunityBadge = model.Field<bool?>("CommunityBadge"),
										   MoEBadge = model.Field<bool?>("MoEBadge")

									   }).FirstOrDefault();


				}

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
				{

					Createdresource.AssociatedFiles = (from model in ds.Tables[1].AsEnumerable()
													   select new CourseAssociatedFiles()
													   {
														   Id = model.Field<decimal>("Id"),
														   CourseId = model.Field<decimal>("CourseId"),
														   AssociatedFile = model.Field<string>("AssociatedFile"),
														   FileName = model.Field<string>("FileName"),
														   CreatedOn = model.Field<DateTime>("CreatedOn")
													   }).ToList();
				}


				if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
				{

					Createdresource.References = (from model in ds.Tables[2].AsEnumerable()
												  select new CourseUrlReferences()
												  {
													  Id = model.Field<decimal>("Id"),
													  CourseId = model.Field<decimal>("CourseId"),
													  URLReference = model.Field<string>("URLReference"),
													  CreatedOn = model.Field<DateTime>("CreatedOn"),
													  IsActive = model.Field<bool>("IsActive"),
													  IsApproved = model.Field<bool>("IsApproved")
												  }).ToList();
				}

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
				{

					Createdresource.CourseComments = (from model in ds.Tables[3].AsEnumerable()
													  select new CourseComment()
													  {
														  Id = model.Field<decimal>("Id"),
														  CourseId = model.Field<decimal>("CourseId"),
														  Comments = model.Field<string>("Comments"),
														  CommentedBy = model.Field<string>("CommentedBy"),
														  CommentedById = model.Field<int>("CommentedById"),
														  CommentorImage = model.Field<string>("CommentorImage"),
														  CommentDate = model.Field<DateTime>("CommentDate"),
														  ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
													  }).ToList();
				}

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
				{
					CourseSection CourseSection = null;
					List<CourseSection> obj = null;
					obj = new List<CourseSection>();
					foreach (DataRow drItem in ds.Tables[4].Rows)
					{
						CourseSection = new CourseSection();
						CourseSection.Name = Convert.ToString(drItem["Name"]);

						List<Resource> courseResources = new List<Resource>();
						Resource tcourseResources = null;
						string FilterCond1 = "SectionId=" + Convert.ToInt32(drItem["Id"]);
						foreach (DataRow dritem2 in ds.Tables[5].Select(FilterCond1))
						{

							List<Rating> ratings1 = new List<Rating>();
							if (ds != null && ds.Tables.Count > 0 && ds.Tables[7] != null && ds.Tables[7].Rows.Count > 0)
							{

								string FilterCond2 = "ResourceId=" + Convert.ToInt32(dritem2["Id"]);
								foreach (DataRow item3 in ds.Tables[7].Select(FilterCond2))
								{
									Rating objRating = new Rating();
									objRating.Star = Convert.ToDouble(item3["Rating"]);
									objRating.UserCount = Convert.ToInt32(item3["NoOfUsers"]);
									ratings1.Add(objRating);
								}

							}

							tcourseResources = new Resource();
							tcourseResources.AllRatings = ratings1;
							tcourseResources.Id = Convert.ToInt32(dritem2["Id"]);
							tcourseResources.Title = Convert.ToString(dritem2["Title"]);
							tcourseResources.Category = new ShortCategory { Id = Convert.ToInt32(dritem2["CategoryId"]), Name = Convert.ToString(dritem2["CategoryName"]), Name_Ar = Convert.ToString(dritem2["CategoryName_Ar"]) };
							tcourseResources.SubCategory = dritem2["SubCategoryId"] != null ? new ShortSubCategory { Id = Convert.ToInt32(dritem2["SubCategoryId"]), CategoryId = Convert.ToInt32(dritem2["CategoryId"]), Name = Convert.ToString(dritem2["SubCategoryName"]), Name_Ar = Convert.ToString(dritem2["SubCategoryName_Ar"]) } : null;
							// tcourseResources.CopyRight = model.Field<int?>("CopyRightId") != null ? new ShortCopyright { Id = model.Field<int>("CopyRightId"), Title = model.Field<string>("CopyrightTitle"), Description = model.Field<string>("CopyrightDescription") } : null,
							tcourseResources.Thumbnail = Convert.ToString(dritem2["Thumbnail"]);
							tcourseResources.ResourceContent = Convert.ToString(dritem2["ResourceContent"]);
							tcourseResources.MaterialType = new ShortMaterialType { Id = Convert.ToInt32(dritem2["MaterialTypeId"]), Name = Convert.ToString(dritem2["MaterialTypeName"]) };
							tcourseResources.ResourceDescription = Convert.ToString(dritem2["ResourceDescription"]);
							tcourseResources.Keywords = Convert.ToString(dritem2["Keywords"]);
							tcourseResources.CreatedBy = Convert.ToString(dritem2["CreatedBy"]);
							tcourseResources.CreatedById = Convert.ToInt32(dritem2["CreatedById"]);
							tcourseResources.CreatedOn = Convert.ToDateTime(dritem2["CreatedOn"]);
							tcourseResources.IsDraft = Convert.ToBoolean(dritem2["IsDraft"]);
							tcourseResources.Rating = Convert.ToDouble(dritem2["Rating"]);
							tcourseResources.ReadingTime = Convert.ToInt32(dritem2["ReadingTime"]);
							tcourseResources.AlignmentRating = Convert.ToDouble(dritem2["AlignmentRating"]);
							tcourseResources.IsApproved = Convert.ToBoolean(dritem2["IsApproved"]);
							tcourseResources.ReportAbuseCount = Convert.ToInt32(dritem2["ReportAbuseCount"]);

							tcourseResources.Standard = Convert.ToString(dritem2["Standard"]);
							tcourseResources.Objective = Convert.ToString(dritem2["Objective"]);
							tcourseResources.Format = Convert.ToString(dritem2["Format"]);
							courseResources.Add(tcourseResources);
						}
						CourseSection.courseResources = courseResources;
						obj.Add(CourseSection);
					}
					Createdresource.CourseSection = obj;
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

		public async Task<DatabaseResponse> DeleteCourse(decimal id)
		{
			try
			{

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@Id",  SqlDbType.NVarChar )

				};

				parameters[0].Value = id;

				_DataHelper = new DataAccessHelper("DeleteCourse", parameters, _configuration);

				DataSet ds = new DataSet();

				int result = await _DataHelper.RunAsync(ds);
				if (result == 103)
				{
					string uri = _configuration.GetValue<string>("ElasticURL");
					HttpClient client = new HttpClient();
					await client.DeleteAsync(uri + @"courses/_doc/" + id);
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

		public async Task<DatabaseResponse> UpdateCourse(UpdateCourseRequest course, int id)
		{
			try
			{
				CommonHelper helper = new CommonHelper();

				DataTable dtSection = new DataTable();
				dtSection.Columns.Add("Id", typeof(int));
				dtSection.Columns.Add("Name", typeof(string));

				DataTable dtResource = new DataTable();
				dtResource.Columns.Add("Id", typeof(int));
				dtResource.Columns.Add("ResourceId", typeof(int));
				dtResource.Columns.Add("SectionId", typeof(int));

				DataRow dr;
				DataRow drRes;
				int count = 0;
				foreach (Section item in course.sections)
				{
					if (!string.IsNullOrEmpty(item.Name))
					{
						count = count + 1;
						dr = dtSection.NewRow();

						dr["Id"] = count;
						dr["Name"] = item.Name;

						dtSection.Rows.Add(dr);
						int answerid = 100;
						foreach (CourseResource item2 in item.courseResources)
						{
							answerid = answerid + 1;
							drRes = dtResource.NewRow();
							drRes["SectionId"] = count;
							drRes["Id"] = answerid;
							drRes["ResourceId"] = item2.ResourceId;
							dtResource.Rows.Add(drRes);

						}
					}
				}

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@Title",  SqlDbType.NVarChar ),
					new SqlParameter( "@CategoryId",  SqlDbType.Int ),
					new SqlParameter( "@SubCategoryId",  SqlDbType.Int ),
					new SqlParameter( "@Thumbnail",  SqlDbType.NVarChar ),
					new SqlParameter( "@CourseDescription",  SqlDbType.NVarChar ),
					new SqlParameter( "@Keywords",  SqlDbType.NVarChar ),
					new SqlParameter( "@CourseContent",  SqlDbType.NVarChar ),
					new SqlParameter( "@CopyRightId",  SqlDbType.Int ),
					new SqlParameter( "@IsDraft",  SqlDbType.Bit ),
					new SqlParameter( "@EducationId",  SqlDbType.Int ),
					new SqlParameter( "@ProfessionId",  SqlDbType.Int ),
					new SqlParameter( "@References",  SqlDbType.NVarChar ),
					new SqlParameter( "@CourseFiles",  SqlDbType.NVarChar ),
					new SqlParameter( "@Id",  SqlDbType.Int ),
					new SqlParameter( "@ReadingTime",  SqlDbType.Int ),
					new SqlParameter( "@LevelId",  SqlDbType.Int ),
					new SqlParameter( "@EducationalStandardId",  SqlDbType.Int ),
					new SqlParameter( "@EducationalUseId",  SqlDbType.Int ),
					new SqlParameter( "@UT_Sections",  SqlDbType.Structured ),
					new SqlParameter( "@UT_Resource",  SqlDbType.Structured )
				};
				if (course.References.Count > 0)
				{
					foreach (Reference item in course.References.ToList())
					{
						if (item.URLReferenceId == 0)
						{
							course.References.Remove(item);
						}
					}
				}
				parameters[0].Value = course.Title;
				parameters[1].Value = course.CategoryId;
				parameters[2].Value = course.SubCategoryId == null || course.SubCategoryId == 0 ? null : course.SubCategoryId;
				parameters[3].Value = course.Thumbnail;
				parameters[4].Value = course.CourseDescription;
				parameters[5].Value = course.Keywords;
				parameters[6].Value = course.CourseContent;
				parameters[7].Value = course.CopyRightId == null || course.CopyRightId == 0 ? null : course.CopyRightId;
				parameters[8].Value = course.IsDraft;
				parameters[9].Value = course.EducationId == null || course.EducationId == 0 ? null : course.EducationId;
				parameters[10].Value = course.ProfessionId == null || course.ProfessionId == 0 ? null : course.ProfessionId;
				parameters[11].Value = course.References != null ? helper.GetJsonString(course.References) : null;
				parameters[12].Value = course.ResourceFiles != null ? helper.GetJsonString(course.ResourceFiles) : null;
				parameters[13].Value = course.Id;
				parameters[14].Value = course.ReadingTime == null || course.ReadingTime == 0 ? null : course.ReadingTime;
				parameters[15].Value = course.LevelId == null || course.LevelId == 0 ? null : course.LevelId;
				parameters[16].Value = course.EducationalStandardId == null || course.EducationalStandardId == 0 ? null : course.EducationalStandardId;
				parameters[17].Value = course.EducationalUseId == null || course.EducationalUseId == 0 ? null : course.EducationalUseId;
				parameters[18].Value = dtSection;
				parameters[19].Value = dtResource;
				_DataHelper = new DataAccessHelper("UpdateCourse", parameters, _configuration);

				DataSet ds = new DataSet();

				int result = await _DataHelper.RunAsync(ds);
				if (course.Tests.Count > 0)
				{
					await UpdateTest(course.Tests.FirstOrDefault(), id);
				}

				Course updatedCesource = new Course();

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
				{

					updatedCesource = (from model in ds.Tables[0].AsEnumerable()
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
										   IsApproved = model.Field<bool?>("IsApproved"),
										   ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
										   Education = model.Field<int?>("EducationId") != null ? new Education { Id = model.Field<int>("EducationId"), Name = model.Field<string>("EducationName") } : null,
										   Profession = model.Field<int?>("ProfessionId") != null ? new Profession { Id = model.Field<int>("ProfessionId"), Name = model.Field<string>("ProfessionName") } : null,
										   EducationLevel = new EducationLevel { Id = model.Field<int?>("LevelId"), Level = model.Field<string>("LevelName"), Level_Ar = model.Field<string>("Level_Ar") },
										   EducationalUse = new EducationalUse { Id = model.Field<int?>("EducationalUseId"), Text = model.Field<string>("EducationalUseName"), Text_Ar = model.Field<string>("EducationalUse_Ar") },
										   EducationalStandard = new EducationalStandard { Id = model.Field<int?>("EducationalStandardId"), Standard = model.Field<string>("Standard"), Standard_Ar = model.Field<string>("Standard_Ar") }


									   }).FirstOrDefault();


				}

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
				{

					updatedCesource.AssociatedFiles = (from model in ds.Tables[1].AsEnumerable()
													   select new CourseAssociatedFiles()
													   {
														   Id = model.Field<decimal>("Id"),
														   CourseId = model.Field<decimal>("CourseId"),
														   AssociatedFile = model.Field<string>("AssociatedFile"),
														   CreatedOn = model.Field<DateTime>("CreatedOn")
													   }).ToList();
				}


				if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
				{

					updatedCesource.References = (from model in ds.Tables[2].AsEnumerable()
												  select new CourseUrlReferences()
												  {
													  Id = model.Field<decimal>("Id"),
													  CourseId = model.Field<decimal>("CourseId"),
													  URLReference = model.Field<string>("URLReference"),
													  CreatedOn = model.Field<DateTime>("CreatedOn")
												  }).ToList();
				}




				return new DatabaseResponse { ResponseCode = result, Results = updatedCesource };
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

		public async Task<DatabaseResponse> ApproveCourse(decimal courseId, int approvedBy)
		{
			try
			{

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@CourseId",  SqlDbType.Decimal),
					new SqlParameter( "@CreatedBy",  SqlDbType.Int )
				};

				parameters[0].Value = courseId;

				parameters[1].Value = approvedBy;

				_DataHelper = new DataAccessHelper("ApproveCourse", parameters, _configuration);

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
		public async Task<DatabaseResponse> ContentWithdrawal(int contentId, int contentTypeId)
		{
			try
			{

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@ContentId",  SqlDbType.Int),
					new SqlParameter( "@ContentType",  SqlDbType.Int )
				};

				parameters[0].Value = contentId;

				parameters[1].Value = contentTypeId;

				_DataHelper = new DataAccessHelper("spu_ContentWithdrawal", parameters, _configuration);

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
		public async Task<DatabaseResponse> ResportCourse(decimal id)
		{
			try
			{

				SqlParameter[] parameters =
				{
					new SqlParameter( "@Id",  SqlDbType.Decimal )

				};

				parameters[0].Value = id;

				_DataHelper = new DataAccessHelper("ReportCourse", parameters, _configuration);

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
		public async Task<DatabaseResponse> CourseEnrollment(CourseEnrollmentCreate courseEnrollmentCreate)
		{
			try
			{

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@CourseId",  SqlDbType.Int ),
					new SqlParameter( "@UserId",  SqlDbType.NVarChar )
				};

				parameters[0].Value = courseEnrollmentCreate.CourseId;
				parameters[1].Value = courseEnrollmentCreate.UserId;


				_DataHelper = new DataAccessHelper("spi_CourseEnrollment", parameters, _configuration);

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
		public async Task<DatabaseResponse> CommentOnCourse(CourseCommentRequest courseComment)
		{
			try
			{

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@CourseId",  SqlDbType.Int ),
					new SqlParameter( "@Comments",  SqlDbType.NVarChar ),
					new SqlParameter( "@CommentedBy",  SqlDbType.Int )
				};

				parameters[0].Value = courseComment.CourseId;
				parameters[1].Value = courseComment.Comments;
				parameters[2].Value = courseComment.UserId;


				_DataHelper = new DataAccessHelper("CommentOnCourse", parameters, _configuration);

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

		public async Task<DatabaseResponse> ResportCourseComment(decimal id)
		{
			try
			{

				SqlParameter[] parameters =
				{
					new SqlParameter( "@Id",  SqlDbType.Decimal )

				};

				parameters[0].Value = id;

				_DataHelper = new DataAccessHelper("ReportCourseComment", parameters, _configuration);

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

		public async Task<DatabaseResponse> UpdateCourseComment(CourseCommentUpdateRequest courseUpdate)
		{
			try
			{

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@Id",  SqlDbType.Decimal ),
					new SqlParameter( "@CourseId",  SqlDbType.Decimal ),
					new SqlParameter( "@Comments",  SqlDbType.NVarChar ),
					new SqlParameter( "@CommentedBy",  SqlDbType.Int )
				};

				parameters[0].Value = courseUpdate.Id;
				parameters[1].Value = courseUpdate.CourseId;
				parameters[2].Value = courseUpdate.Comments;
				parameters[3].Value = courseUpdate.UserId;

				_DataHelper = new DataAccessHelper("UpdateCourseComment", parameters, _configuration);

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

		public async Task<DatabaseResponse> DeleteCourseComment(decimal id, int requestedBy)
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

				_DataHelper = new DataAccessHelper("DeleteCourseComment", parameters, _configuration);

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

		public async Task<DatabaseResponse> HideCourseCommentByAuthor(decimal id, decimal courseId, int requestedBy)
		{
			try
			{

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@Id",  SqlDbType.Decimal ),
					new SqlParameter( "@CourseId",  SqlDbType.Decimal ),
					new SqlParameter( "@RequestedBy",  SqlDbType.Int )
				};

				parameters[0].Value = id;
				parameters[1].Value = courseId;
				parameters[2].Value = requestedBy;

				_DataHelper = new DataAccessHelper("HideCourseCommentByAuthor", parameters, _configuration);

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

		public async Task<DatabaseResponse> ReportCourseWithComment(CourseReportAbuseWithComment courseAbuseComment)
		{
			try
			{
				SqlParameter[] parameters =
			   {
					new SqlParameter( "@CourseId",  SqlDbType.Decimal ),
					new SqlParameter( "@ReportReasons",  SqlDbType.NVarChar ),
					new SqlParameter( "@Comments",  SqlDbType.NVarChar ),
					new SqlParameter( "@ReportedBy",  SqlDbType.Int )
				};

				parameters[0].Value = courseAbuseComment.CourseId;
				parameters[1].Value = courseAbuseComment.ReportReasons;
				parameters[2].Value = courseAbuseComment.Comments;
				parameters[3].Value = courseAbuseComment.ReportedBy;

				_DataHelper = new DataAccessHelper("ReportCourseWithComment", parameters, _configuration);

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

		public async Task<DatabaseResponse> RateCourse(CourseRatingRequest resourceRatingRequest)
		{
			try
			{
				SqlParameter[] parameters =
			   {
					new SqlParameter( "@CourseId",  SqlDbType.Decimal ),
					new SqlParameter( "@Rating",  SqlDbType.NVarChar ),
					new SqlParameter( "@Comments",  SqlDbType.NVarChar ),
					new SqlParameter( "@RatedBy",  SqlDbType.Int )
				};

				parameters[0].Value = resourceRatingRequest.CourseId;
				parameters[1].Value = resourceRatingRequest.Rating;
				parameters[2].Value = resourceRatingRequest.Comments;
				parameters[3].Value = resourceRatingRequest.RatedBy;

				_DataHelper = new DataAccessHelper("RateCourse", parameters, _configuration);

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
		public async Task<DatabaseResponse> GetCourseTestById(int courseId)
		{
			try
			{

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@CourseId",  SqlDbType.Int )

				};


				parameters[0].Value = courseId;
				_DataHelper = new DataAccessHelper("sps_GetCourseTest", parameters, _configuration);

				DataSet ds = new DataSet();

				int result = await _DataHelper.RunAsync(ds);



				List<Test> tests = new List<Test>();

				List<Questions> questions = new List<Questions>();
				CourseTestsResponse CourseTestsResponse = new CourseTestsResponse();
				List<AnswerOptions> answerOptions = new List<AnswerOptions>();
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
				{

					tests = (from model in ds.Tables[0].AsEnumerable()
							 select new Test()
							 {
								 Id = model.Field<int>("Id"),
								 TestName = model.Field<string>("TestName")

							 }).ToList();

					questions = (from model in ds.Tables[1].AsEnumerable()
								 select new Questions()
								 {
									 Id = model.Field<int>("Id"),
									 QuestionText = model.Field<string>("QuestionText"),
									 TestId = model.Field<int>("TestId"),
									 Media = model.Field<string>("Media"),
									 FileName = model.Field<string>("FileName")

								 }).ToList();

					answerOptions = (from model in ds.Tables[2].AsEnumerable()
									 select new AnswerOptions()
									 {
										 Id = model.Field<int>("Id"),
										 QuestionId = model.Field<int>("QuestionId"),
										 OptionText = model.Field<string>("AnswerOption"),
										 CorrectAnswer = model.Field<bool>("CorrectOption")

									 }).ToList();
				}

				CourseTestsResponse.tests = tests;
				CourseTestsResponse.questions = questions;
				CourseTestsResponse.answers = answerOptions;

				return new DatabaseResponse { ResponseCode = result, Results = CourseTestsResponse };
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
		public async Task<DatabaseResponse> ReportCourseCommentWithComment(CourseCommentReportAbuseWithComment coursecommentAbuseComment)
		{
			try
			{
				SqlParameter[] parameters =
			   {
					new SqlParameter( "@CourseCommentId",  SqlDbType.Decimal ),
					new SqlParameter( "@ReportReasons",  SqlDbType.NVarChar ),
					new SqlParameter( "@Comments",  SqlDbType.NVarChar ),
					new SqlParameter( "@ReportedBy",  SqlDbType.Int )
				};

				parameters[0].Value = coursecommentAbuseComment.CourseCommentId;
				parameters[1].Value = coursecommentAbuseComment.ReportReasons;
				parameters[2].Value = coursecommentAbuseComment.Comments;
				parameters[3].Value = coursecommentAbuseComment.ReportedBy;

				_DataHelper = new DataAccessHelper("ReportCourseCommentWithComment", parameters, _configuration);

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

		public async Task<DatabaseResponse> InsertAssociatedCourseFile(int id, string filePath)
		{
			try
			{

				var fileName = filePath.Substring(filePath.LastIndexOf(("/")) + 1);
				SqlParameter[] parameters =
			   {
					new SqlParameter( "@CourseId",  SqlDbType.Int ),
					new SqlParameter( "@AssociatedFile",  SqlDbType.NVarChar ),
					new SqlParameter( "@FileName",  SqlDbType.NVarChar )

				};

				parameters[0].Value = id;
				parameters[1].Value = filePath;
				parameters[2].Value = fileName;
				_DataHelper = new DataAccessHelper("spi_CourseAssociatedFiles", parameters, _configuration);

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
