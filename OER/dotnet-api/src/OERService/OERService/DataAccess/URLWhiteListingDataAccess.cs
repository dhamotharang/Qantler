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
using System.Text.RegularExpressions;

namespace OERService.DataAccess
{
	public class UrlWhiteListingDataAccess
	{
		internal DataAccessHelper _DataHelper = null;

		private readonly IConfiguration _configuration;

		/// <summary>
		/// Constructor setting configuration
		/// </summary>
		/// <param name="configuration"></param>
		public UrlWhiteListingDataAccess(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<DatabaseResponse> RequestWhitelisting(WhiteListingRequest request)
		{
			try
			{

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@URL",  SqlDbType.NVarChar ),

					new SqlParameter( "@RequestedBy",  SqlDbType.Int )
				};
				parameters[0].Value = request.URL;

				parameters[1].Value = request.RequestedBy;

				_DataHelper = new DataAccessHelper("CreateWhiteListingRequest", parameters, _configuration);

				DataTable dt = new DataTable();

				int result = await _DataHelper.RunAsync(dt);

				List<WhiteListingURLsIncludingId> whiteListingURL = new List<WhiteListingURLsIncludingId>();

				if (dt != null && dt.Rows.Count > 0)
				{

					whiteListingURL = (from model in dt.AsEnumerable()
									   select new WhiteListingURLsIncludingId()
									   {
										   Id = model.Field<decimal>("Id"),
										   RequestedBy = model.Field<string>("RequestedBy"),
										   RequestedOn = model.Field<DateTime>("RequestedOn"),
										   URL = model.Field<string>("URL"),
										   VerifiedBy = model.Field<string>("VerifiedBy"),
										   VerifiedOn = model.Field<DateTime?>("VerifiedOn") == null ? null : model.Field<DateTime?>("VerifiedOn"),
										   IsApproved = model.Field<bool>("IsApproved"),
										   RejectedReason = model.Field<string>("RejectedReason")
										   // VerifiedById = model.Field<int?>("VerifiedById")
									   }).ToList();
				}

				return new DatabaseResponse { ResponseCode = result, Results = whiteListingURL };
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

		public async Task<DatabaseResponse> CreateWhitelistingAfterCheck(InsertWhiteListingUrl request)
		{
			try
			{

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@URL",  SqlDbType.NVarChar ),
					new SqlParameter( "@RequestedBy",  SqlDbType.Int ),
					new SqlParameter( "@VerifiedBy",  SqlDbType.Int ),
					new SqlParameter( "@IsApproved",  SqlDbType.Bit ),
					new SqlParameter( "@RequestedOn",  SqlDbType.DateTime ),
					new SqlParameter( "@VerifiedOn",  SqlDbType.DateTime )
				};
				parameters[0].Value = request.URL;
				parameters[1].Value = request.RequestedBy;
				parameters[2].Value = request.VerifiedBy;
				parameters[3].Value = request.IsApproved;
				parameters[4].Value = request.RequestedOn;
				parameters[5].Value = request.VerifiedOn;

				_DataHelper = new DataAccessHelper("CreateWhiteListingRequestAfterCheck", parameters, _configuration);

				DataTable dt = new DataTable();
				int result = await _DataHelper.RunAsync(dt);

				WhiteListingURLsAsID whiteListingURLs = new WhiteListingURLsAsID();

				if (dt != null && dt.Rows.Count > 0)
				{

					whiteListingURLs = (from model in dt.AsEnumerable()
										select new WhiteListingURLsAsID()
										{
											Id = model.Field<decimal>("Id"),
											RequestedBy = model.Field<int?>("RequestedBy"),
											RequestedOn = model.Field<DateTime>("RequestedOn"),
											URL = model.Field<string>("URL"),
											VerifiedBy = model.Field<int?>("VerifiedBy"),
											VerifiedOn = model.Field<DateTime?>("VerifiedOn") == null ? null : model.Field<DateTime?>("VerifiedOn"),
											IsApproved = model.Field<bool>("IsApproved"),
											RejectedReason = model.Field<string>("RejectedReason")
										}).FirstOrDefault();
				}

				return new DatabaseResponse { Results = whiteListingURLs, ResponseCode = result };
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

		public async Task<DatabaseResponse> GetWhiteListedUrls(bool isApproved)
		{
			try
			{
				SqlParameter[] parameters =
			 {
					new SqlParameter( "@IsApproved",  SqlDbType.Bit )
				};
				parameters[0].Value = isApproved;
				_DataHelper = new DataAccessHelper("GetWhitelistingRequests", parameters, _configuration);

				DataTable dt = new DataTable();

				int result = await _DataHelper.RunAsync(dt);

				List<WhiteListingURLs> whiteListingURLs = new List<WhiteListingURLs>();

				if (dt != null && dt.Rows.Count > 0)
				{

					whiteListingURLs = (from model in dt.AsEnumerable()
										select new WhiteListingURLs()
										{
											Id = model.Field<decimal>("Id"),
											RequestedBy = model.Field<string>("RequestedBy"),
											RequestedOn = model.Field<DateTime>("RequestedOn"),
											URL = model.Field<string>("URL"),
											VerifiedBy = model.Field<string>("VerifiedBy"),
											VerifiedOn = model.Field<DateTime?>("VerifiedOn") == null ? null : model.Field<DateTime?>("VerifiedOn"),
											IsApproved = model.Field<bool>("IsApproved"),
											RejectedReason = model.Field<string>("RejectedReason")
										}).ToList();
				}

				return new DatabaseResponse { ResponseCode = result, Results = whiteListingURLs };
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
		public async Task<DatabaseResponse> DeleteWhiteListUrl(int id)
		{
			try
			{

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@Id",  SqlDbType.Int )

				};

				parameters[0].Value = id;

				_DataHelper = new DataAccessHelper("spd_WhiteListingURLs", parameters, _configuration);

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
		public async Task<DatabaseResponse> WhiteListUrl(WhiteListUrl request, int id)
		{
			try
			{

				SqlParameter[] parameters =
			   {
					new SqlParameter( "@Id",  SqlDbType.Decimal ),
					new SqlParameter( "@VerifiedBy",  SqlDbType.Int ),
					new SqlParameter( "@IsApproved",  SqlDbType.Bit ),
					new SqlParameter( "@RejectedReason",  SqlDbType.NVarChar ),
					new SqlParameter( "@EmailUrl",  SqlDbType.NVarChar )
				};


				parameters[0].Value = id;
				parameters[1].Value = request.VerifiedBy;
				parameters[2].Value = request.IsApproved;
				parameters[3].Value = request.RejectedReason;
				parameters[4].Value = request.EmailUrl;

				_DataHelper = new DataAccessHelper("VerifyWhiteListingRequest", parameters, _configuration);
				DataTable dt = new DataTable();
				int result = await _DataHelper.RunAsync(dt);
				UserEmail userEmail = null;
				if (result == (int)DbReturnValue.UpdateSuccess)
				{
					if (dt.Rows.Count > 0)
					{
						userEmail = new UserEmail();
						userEmail.Email = Convert.ToString(dt.Rows[0]["Email"]);
						int portalLanguageId = Convert.ToInt32(dt.Rows[0]["PortalLanguageId"]);
						string text = string.Empty;
						string buttonText = string.Empty;
						if (request.IsApproved)
						{
							//approved

							if (portalLanguageId == 2)
							{
								text = "Your requested URL has been whitelisted. Click below button to view";
								buttonText = "URL Approved";
								userEmail.Subject = "URL has been whitelisted";
							}
							else
							{
								text = "الرابط المرسل من قبلكم أصبح ضمن قائمة الروابط المعتمدة في منصة منارة، الرجاء الضغط على الرابط أدناه للعرض";
								buttonText = "الرابط المعتمد";
								userEmail.Subject = "تمت الموافقة على طلب اعتماد الرابط المرسل من قبلكم";
							}
							userEmail.Body = Emailer.CreateEmailBody(Convert.ToString(dt.Rows[0]["UserName"]), request.EmailUrl, text, buttonText, portalLanguageId,_configuration);


						}
						else
						{
							if (portalLanguageId == 2)
							{
								text = "Your requested URL has been rejected due to " + request.RejectedReason + ". Click below button to view";
								buttonText = "View URL Rejected";
								userEmail.Subject = "URL has been rejected";
							}
							else
							{
								text = "لقد تم رفض اعتماد الرابط الذي طلبته ليكون من ضمن قائمة الروابط المعتمدة في منصة منارة بسبب .<br><br>" + request.RejectedReason + "";
								buttonText = "عرض الرابط المرفوض";
								userEmail.Subject = "لقد تم رفض اعتماد الرابط الذي طلبته ليكون من ضمن قائمة الروابط المعتمدة في منصة";
							}
							userEmail.Body = Emailer.CreateEmailBody(Convert.ToString(dt.Rows[0]["UserName"]), request.EmailUrl, text, buttonText, portalLanguageId,_configuration);

						}
					}
					await Emailer.SendEmailAsync(userEmail, _configuration);
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

		public async Task<DatabaseResponse> IsWhiteListed(string url, int requestedBy)
		{
			try
			{
				SqlParameter[] parameters =
				 {
					new SqlParameter( "@URL",  SqlDbType.NVarChar)
				};
				parameters[0].Value = url;
				_DataHelper = new DataAccessHelper("UrlIsWhitelisted", parameters, _configuration);
				DataTable dt = new DataTable();
				int result = await _DataHelper.RunAsync(dt);
				List<WhiteListingURLsIncludingId> whiteListingURL = new List<WhiteListingURLsIncludingId>();

				if (dt != null && dt.Rows.Count > 0)
				{
					whiteListingURL = (from model in dt.AsEnumerable()
									   select new WhiteListingURLsIncludingId()
									   {
										   Id = model.Field<decimal>("Id"),
										   RequestedBy = model.Field<string>("RequestedBy"),
										   RequestedOn = model.Field<DateTime>("RequestedOn"),
										   URL = model.Field<string>("URL"),
										   VerifiedBy = model.Field<string>("VerifiedBy"),
										   VerifiedOn = model.Field<DateTime?>("VerifiedOn") == null ? null : model.Field<DateTime?>("VerifiedOn"),
										   IsApproved = model.Field<bool>("IsApproved"),
										   RejectedReason = model.Field<string>("RejectedReason"),
										   VerifiedById = model.Field<int?>("VerifiedById")
									   }).ToList();
				}
				WhiteListingURLsIncludingId returnUrl = new WhiteListingURLsIncludingId();
				foreach (WhiteListingURLsIncludingId urlDetail in whiteListingURL)
				{
					if (String.Compare(returnDomain(urlDetail.URL), returnDomain(url), StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						returnUrl = urlDetail;
						break;
					}
				}

				if (!String.IsNullOrEmpty(returnUrl.URL))
				{
					InsertWhiteListingUrl whiteUrl = new InsertWhiteListingUrl
					{
						IsApproved = true,
						RequestedBy = requestedBy,
						RequestedOn = DateTime.Now,
						URL = url,
						VerifiedBy = returnUrl.VerifiedById,
						VerifiedOn = DateTime.Now
					};
					DatabaseResponse response = await CreateWhitelistingAfterCheck(whiteUrl);
					if (response.ResponseCode == 100)
					{
						response.ResponseCode = 105;
					}
					return response;
				}
				return new DatabaseResponse { ResponseCode = result, Results = returnUrl };
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

		private string returnDomain(string url)
		{
			string value = string.Empty;
			string myUrl = url.Replace("*", "");
			if (myUrl.Contains("/"))
			{
				if (myUrl.Contains("https://") || myUrl.Contains("http://"))
				{
					if (myUrl.Count(f => f == '/') > 2)
					{
						var offset = myUrl.IndexOf('/');
						offset = myUrl.IndexOf('/', offset + 1);
						var res = myUrl.IndexOf('/', offset + 1);
						string newURL = myUrl.Substring(0, res);

						int count = newURL.Count(f => f == '.');
						if (count == 1)
						{
							value = newURL.Replace("https://", "").Replace("http://", "");

						}
						else if (count > 1)
						{
							value = Regex.Match(newURL, @"(?!\.)[^\.]*\.[^\.]*$").Value;
						}

					}
					else
					{
						int count = myUrl.Count(f => f == '.');
						if (count == 1)
						{
							value = myUrl.Replace("https://", "").Replace("http://", "");

						}
						else if (count > 1)
						{
							value = Regex.Match(myUrl, @"(?!\.)[^\.]*\.[^\.]*$").Value;
						}
					}
				}
				else
				{
					var offset = myUrl.IndexOf('/');
					string newURL = myUrl.Substring(0, offset);
					int count = newURL.Count(f => f == '.');
					if (count == 1)
					{
						value = newURL.Replace("https://", "").Replace("http://", "");

					}
					else if (count > 1)
					{
						value = Regex.Match(newURL, @"(?!\.)[^\.]*\.[^\.]*$").Value;
					}
				}
			}
			else
			{
				int count = myUrl.Count(f => f == '.');
				if (count == 1)
				{
					value = myUrl.Replace("https://", "").Replace("http://", "");

				}
				else if (count > 1)
				{
					value = Regex.Match(myUrl, @"(?!\.)[^\.]*\.[^\.]*$").Value;
				}
			}
			return value;
		}
	}
}
