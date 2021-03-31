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
	public class WcmDataAccess
	{
		internal DataAccessHelper _DataHelper = null;

		private readonly IConfiguration _configuration;

		/// <summary>
		/// Constructor setting configuration
		/// </summary>
		/// <param name="configuration"></param>
		public WcmDataAccess(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public async Task<DatabaseResponse> GetPages(int CategoryId)
		{
			try
			{
				SqlParameter[] parameters = {
					new SqlParameter( "@category_id",  SqlDbType.Int )
				};
				parameters[0].Value = CategoryId;
				_DataHelper = new DataAccessHelper("sps_Pages", parameters, _configuration);
				DataTable dt = new DataTable();
				int result = await _DataHelper.RunAsync(dt);
				List<WebPages> categories = new List<WebPages>();
				if (dt != null && dt.Rows.Count > 0)
				{
					categories = (from model in dt.AsEnumerable()
								  select new WebPages()
								  {
									  Id = model.Field<int>("Id"),
									  PageName = model.Field<string>("PageName"),
									  PageName_Ar = model.Field<string>("PageName_Ar"),
								  }).ToList();
				}

				return new DatabaseResponse { ResponseCode = result, Results = categories };
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
		public async Task<DatabaseResponse> GetPageContentById(int pageId)
		{
			try
			{
				SqlParameter[] parameters =
			{
					new SqlParameter( "@PageId",  SqlDbType.Int )

				};

				parameters[0].Value = pageId;
				_DataHelper = new DataAccessHelper("sps_PageContentByPageId", parameters, _configuration);

				DataTable dt = new DataTable();

				int result = await _DataHelper.RunAsync(dt);

				List<WebContentPages> webContentPages = new List<WebContentPages>();

				if (dt != null && dt.Rows.Count > 0)
				{

					webContentPages = (from model in dt.AsEnumerable()
									   select new WebContentPages()
									   {
										   Id = model.Field<int>("Id"),
										   PageID = model.Field<int>("PageID"),
										   PageContent = model.Field<string>("PageContent"),
										   PageContent_Ar = model.Field<string>("PageContent_Ar"),
										   CreatedBy = model.Field<int>("CreatedBy"),
										   CreatedOn = model.Field<DateTime>("CreatedOn"),
										   UpdatedBy = model.Field<int?>("UpdatedBy"),
										   UpdatedOn = model.Field<DateTime?>("UpdatedOn"),
										   VideoLink = model.Field<string>("VideoLink")
									   }).ToList();
				}

				return new DatabaseResponse { ResponseCode = result, Results = webContentPages };
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

		public async Task<DatabaseResponse> GetPageContents()
		{
			try
			{
				_DataHelper = new DataAccessHelper("sps_PageContents", _configuration);

				DataTable dt = new DataTable();

				int result = await _DataHelper.RunAsync(dt);

				List<WebContentPages> webContentPages = new List<WebContentPages>();

				if (dt != null && dt.Rows.Count > 0)
				{
					webContentPages = (from model in dt.AsEnumerable()
									   select new WebContentPages()
									   {
										   Id = model.Field<int>("Id"),
										   PageID = model.Field<int>("PageID"),
										   WebPage = model.Field<string>("PageName"),
										   PageContent = model.Field<string>("PageContent"),
										   PageContent_Ar = model.Field<string>("PageContent_Ar"),
										   CreatedBy = model.Field<int>("CreatedBy"),
										   CreatedOn = model.Field<DateTime>("CreatedOn"),
										   UpdatedBy = model.Field<int?>("UpdatedBy"),
										   UpdatedOn = model.Field<DateTime?>("UpdatedOn"),
										   VideoLink = model.Field<string>("VideoLink")
									   }).ToList();
				}

				return new DatabaseResponse { ResponseCode = result, Results = webContentPages };
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

		public async Task<DatabaseResponse> AddPageContent(WebContentPages webContentPage)
		{
			try
			{
				SqlParameter[] parameters =
			{
					new SqlParameter( "@PageId",  SqlDbType.Int ),
					new SqlParameter( "@PageContent",  SqlDbType.NVarChar ),
					new SqlParameter( "@PageContent_Ar",  SqlDbType.NVarChar ),
					new SqlParameter( "@CreatedBy",  SqlDbType.Int ),
					new SqlParameter( "@VideoLink",  SqlDbType.NVarChar )
				};

				parameters[0].Value = webContentPage.PageID;
				parameters[1].Value = webContentPage.PageContent;
				parameters[2].Value = webContentPage.PageContent_Ar;
				parameters[3].Value = webContentPage.CreatedBy;
				parameters[4].Value = webContentPage.VideoLink;
				_DataHelper = new DataAccessHelper("spi_WebPageContent", parameters, _configuration);

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

		public async Task<DatabaseResponse> UpdatePageContent(WebContentPages webContentPage)
		{
			try
			{
				SqlParameter[] parameters =
			{
					new SqlParameter( "@PageId",  SqlDbType.Int ),
					new SqlParameter( "@PageContent",  SqlDbType.NVarChar ),
					new SqlParameter( "@PageContent_Ar",  SqlDbType.NVarChar ),
					new SqlParameter( "@UpdatedBy",  SqlDbType.Int ),
					new SqlParameter( "@VideoLink",  SqlDbType.NVarChar )
				};

				parameters[0].Value = webContentPage.PageID;
				parameters[1].Value = webContentPage.PageContent;
				parameters[2].Value = webContentPage.PageContent_Ar;
				parameters[3].Value = webContentPage.CreatedBy;
				parameters[4].Value = webContentPage.VideoLink;
				_DataHelper = new DataAccessHelper("spu_WebPageContent", parameters, _configuration);

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
	}
}
