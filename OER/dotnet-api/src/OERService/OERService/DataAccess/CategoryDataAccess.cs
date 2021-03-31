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
	public class CategoryDataAccess
	{
		internal DataAccessHelper _DataHelper = null;

		private readonly IConfiguration _configuration;

		/// <summary>
		/// Constructor setting configuration
		/// </summary>
		/// <param name="configuration"></param>
		public CategoryDataAccess(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<DatabaseResponse> CreateCategory(CreateCategoryMaster categoryMaster)
		{
			try
			{

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@Name",  SqlDbType.NVarChar ),
					new SqlParameter( "@CreatedBy",  SqlDbType.Int ),
					new SqlParameter( "@Name_Ar",  SqlDbType.NVarChar ),
					new SqlParameter( "@Weight",  SqlDbType.Int )
				};

				parameters[0].Value = categoryMaster.Name;
				parameters[1].Value = categoryMaster.CreatedBy;
				parameters[2].Value = categoryMaster.Name_Ar;
				parameters[3].Value = categoryMaster.Weight;
				_DataHelper = new DataAccessHelper("CreateCategory", parameters, _configuration);

				DataTable dt = new DataTable();

				int result = await _DataHelper.RunAsync(dt);

				List<CategoryMaster> categories = new List<CategoryMaster>();

				if (dt != null && dt.Rows.Count > 0)
				{

					categories = (from model in dt.AsEnumerable()
								  select new CategoryMaster()
								  {
									  Id = model.Field<int>("Id"),
									  Name = model.Field<string>("Name"),
									  CreatedBy = model.Field<string>("CreatedBy"),
									  CreatedOn = model.Field<DateTime>("CreatedOn"),
									  UpdatedOn = model.Field<DateTime>("UpdatedOn"),
									  UpdatedBy = model.Field<string>("UpdatedBy"),
									  Active = model.Field<bool>("Active"),
									  Name_Ar = model.Field<string>("Name_Ar")

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
		public async Task<DatabaseResponse> GetCommunityCategory(int userId)
		{
			try
			{

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@UserId",  SqlDbType.NVarChar )
				};

				parameters[0].Value = userId;
				_DataHelper = new DataAccessHelper("sps_GetCommunityCategories", parameters, _configuration);

				DataTable dt = new DataTable();

				int result = await _DataHelper.RunAsync(dt);

				List<CategoryMaster> categories = new List<CategoryMaster>();

				if (dt != null && dt.Rows.Count > 0)
				{

					categories = (from model in dt.AsEnumerable()
								  select new CategoryMaster()
								  {
									  Id = model.Field<int>("Id"),
									  Name = model.Field<string>("Name"),
									  Name_Ar = model.Field<string>("Name_Ar")

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
		public async Task<DatabaseResponse> GetMoeCategory(int userId)
		{
			try
			{

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@UserId",  SqlDbType.NVarChar )
				};

				parameters[0].Value = userId;
				_DataHelper = new DataAccessHelper("sps_GetMoEcategories", parameters, _configuration);

				DataTable dt = new DataTable();

				int result = await _DataHelper.RunAsync(dt);

				List<CategoryMaster> categories = new List<CategoryMaster>();

				if (dt != null && dt.Rows.Count > 0)
				{

					categories = (from model in dt.AsEnumerable()
								  select new CategoryMaster()
								  {
									  Id = model.Field<int>("Id"),
									  Name = model.Field<string>("Name"),
									  Name_Ar = model.Field<string>("Name_Ar")

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
		public async Task<DatabaseResponse> GetSensoryCategory(int userId)
		{
			try
			{

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@UserId",  SqlDbType.NVarChar )
				};

				parameters[0].Value = userId;
				_DataHelper = new DataAccessHelper("sps_GetSensoryCategory", parameters, _configuration);

				DataTable dt = new DataTable();

				int result = await _DataHelper.RunAsync(dt);

				List<CategoryMaster> categories = new List<CategoryMaster>();

				if (dt != null && dt.Rows.Count > 0)
				{

					categories = (from model in dt.AsEnumerable()
								  select new CategoryMaster()
								  {
									  Id = model.Field<int>("Id"),
									  Name = model.Field<string>("Name"),
									  Name_Ar = model.Field<string>("Name_Ar")

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
        public async Task<DatabaseResponse> GetCategories(string paramIsActive)
        {
            try
            {
                SqlParameter[] parameters =
               {
                    new SqlParameter( "@IsActive",  SqlDbType.NVarChar )

                };

                parameters[0].Value = paramIsActive;
                _DataHelper = new DataAccessHelper("GetCategory", parameters, _configuration);

				DataTable dt = new DataTable();

				int result = await _DataHelper.RunAsync(dt);

				List<CategoryMaster> categories = new List<CategoryMaster>();

				if (dt != null && dt.Rows.Count > 0)
				{

					categories = (from model in dt.AsEnumerable()
								  select new CategoryMaster()
								  {
									  Id = model.Field<int>("Id"),
									  Name = model.Field<string>("Name"),
									  Name_Ar = model.Field<string>("Name_Ar"),
									  CreatedBy = model.Field<string>("CreatedBy"),
									  CreatedOn = model.Field<DateTime>("CreatedOn"),
									  UpdatedOn = model.Field<DateTime>("UpdatedOn"),
									  UpdatedBy = model.Field<string>("UpdatedBy"),
									  Active = model.Field<bool>("Active"),
									  Weight = model.Field<int>("Weight")

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
		public async Task<DatabaseResponse> GetCategoriesNotInQRC()
		{
			try
			{
				_DataHelper = new DataAccessHelper("sps_CategoryNotInQRC", _configuration);

				DataTable dt = new DataTable();

				int result = await _DataHelper.RunAsync(dt);

				List<CategoryMaster> categories = new List<CategoryMaster>();

				if (dt != null && dt.Rows.Count > 0)
				{

					categories = (from model in dt.AsEnumerable()
								  select new CategoryMaster()
								  {
									  Id = model.Field<int>("Id"),
									  Name = model.Field<string>("Name"),
									  Name_Ar = model.Field<string>("Name_Ar"),
									  CreatedBy = model.Field<string>("CreatedBy"),
									  CreatedOn = model.Field<DateTime>("CreatedOn"),
									  UpdatedOn = model.Field<DateTime>("UpdatedOn"),
									  UpdatedBy = model.Field<string>("UpdatedBy"),
									  Active = model.Field<bool>("Active")

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
		public async Task<DatabaseResponse> GetCategory(int id)
		{
			try
			{

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@Id",  SqlDbType.NVarChar )

				};

				parameters[0].Value = id;

				_DataHelper = new DataAccessHelper("GetCategoryById", parameters, _configuration);

				DataTable dt = new DataTable();

				int result = await _DataHelper.RunAsync(dt);

				List<CategoryMaster> categories = new List<CategoryMaster>();

				if (dt != null && dt.Rows.Count > 0)
				{

					categories = (from model in dt.AsEnumerable()
								  select new CategoryMaster()
								  {
									  Id = model.Field<int>("Id"),
									  Name = model.Field<string>("Name"),
									  Name_Ar = model.Field<string>("Name_Ar"),
									  CreatedBy = model.Field<string>("CreatedBy"),
									  CreatedOn = model.Field<DateTime>("CreatedOn"),
									  UpdatedOn = model.Field<DateTime>("UpdatedOn"),
									  UpdatedBy = model.Field<string>("UpdatedBy"),
									  Active = model.Field<bool>("Active")

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

		public async Task<DatabaseResponse> DeleteCategory(int id)
		{
			try
			{

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@Id",  SqlDbType.NVarChar )

				};

				parameters[0].Value = id;

				_DataHelper = new DataAccessHelper("DeleteCategory", parameters, _configuration);

				DataTable dt = new DataTable();

				int result = await _DataHelper.RunAsync(dt);

				List<CategoryMaster> categories = new List<CategoryMaster>();

				if (dt != null && dt.Rows.Count > 0)
				{

					categories = (from model in dt.AsEnumerable()
								  select new CategoryMaster()
								  {
									  Id = model.Field<int>("Id"),
									  Name = model.Field<string>("Name"),
									  CreatedBy = model.Field<string>("CreatedBy"),
									  CreatedOn = model.Field<DateTime>("CreatedOn"),
									  UpdatedOn = model.Field<DateTime>("UpdatedOn"),
									  UpdatedBy = model.Field<string>("UpdatedBy"),
									  Active = model.Field<bool>("Active")
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

		public async Task<DatabaseResponse> UpdateCategory(UpdateCategoryMaster categoryMaster, int id)
		{
			try
			{

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@Id",  SqlDbType.Int ),
					new SqlParameter( "@Name",  SqlDbType.NVarChar ),
					 new SqlParameter( "@Name_Ar",  SqlDbType.NVarChar ),
					new SqlParameter( "@UpdatedBy",  SqlDbType.Int ),
					new SqlParameter( "@Active",  SqlDbType.Bit ),
					 new SqlParameter( "@Weight",  SqlDbType.Int )
				};


				parameters[0].Value = id;
				parameters[1].Value = categoryMaster.Name;
				parameters[2].Value = categoryMaster.Name_Ar;
				parameters[3].Value = categoryMaster.UpdatedBy;
				parameters[4].Value = categoryMaster.Active;
				parameters[5].Value = categoryMaster.Weight;
				_DataHelper = new DataAccessHelper("UpdateCategory", parameters, _configuration);

				DataTable dt = new DataTable();

				int result = await _DataHelper.RunAsync(dt);

				List<CategoryMaster> categories = new List<CategoryMaster>();

				if (dt != null && dt.Rows.Count > 0)
				{

					categories = (from model in dt.AsEnumerable()
								  select new CategoryMaster()
								  {
									  Id = model.Field<int>("Id"),
									  Name = model.Field<string>("Name"),
									  Name_Ar = model.Field<string>("Name_Ar"),
									  CreatedBy = model.Field<string>("CreatedBy"),
									  CreatedOn = model.Field<DateTime>("CreatedOn"),
									  UpdatedOn = model.Field<DateTime>("UpdatedOn"),
									  UpdatedBy = model.Field<string>("UpdatedBy"),
									  Active = model.Field<bool>("Active"),
									  Weight = model.Field<int>("Weight")
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

	}
}
