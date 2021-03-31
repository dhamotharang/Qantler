using Core.Enums;
using Core.Helpers;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OERService.Core.Helpers;
using OERService.Helpers;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;

namespace OERService
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public Startup(IConfiguration configuration, IHostingEnvironment env )
        {
            try
            {
                Configuration = configuration;
                _hostingEnvironment = env;
            }
            catch(Exception ex)
            {
				Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Medium));
				throw;
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
				services.AddCors(o => o.AddPolicy("OERCorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                }));
                services.AddMvc();

                //Keyclock atuth

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                }).AddJwtBearer(o =>
                {
                    o.Authority = Configuration["Jwt:Authority"];
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                    o.RequireHttpsMetadata = false;
					o.SaveToken = true;

					o.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();

                            c.Response.StatusCode = 500;

                            c.Response.ContentType = "text/plain";
                          
                            return c.Response.WriteAsync(c.Exception.ToString());
                            
                        }

                    };

                });
                //end auth section


                services.Configure<MvcOptions>(options =>
                {
                    options.Filters.Add(new CorsAuthorizationFilterFactory("OERCorsPolicy"));
                });

                services.AddResponseCompression(options =>
                {
                    options.Providers.Add<GzipCompressionProvider>();
                    options.MimeTypes =
                        ResponseCompressionDefaults.MimeTypes.Concat(
                            new[] { "image/svg+xml" });
                });

                services.Configure<GzipCompressionProviderOptions>(options =>
                {
                    options.Level = CompressionLevel.Fastest;
                });

                services.Configure<FormOptions>(o => {
                    o.ValueLengthLimit = int.MaxValue;
                    o.MultipartBodyLengthLimit = long.MaxValue;
                    o.MemoryBufferThreshold = int.MaxValue;
                });

                // Register the Swagger generator, defining 1 or more Swagger documents
                services.AddSwaggerGen(c =>
                {
                    c.AddSecurityDefinition("oauth2", new ApiKeyScheme
                    {
                        Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                        In = "header",
                        Name = "Authorization",
                        Type = "apiKey"
                    });
                    c.SwaggerDoc("v1", new Info { Title = "OER Service API", Version = "v1" });
                    var xmlDocPath = System.AppDomain.CurrentDomain.BaseDirectory + @"OERService.xml";
                    c.IncludeXmlComments(xmlDocPath);
                });

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    var libContext = new CustomAssemblyLoadContext();
                    libContext.LoadUnmanagedLibrary(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "libwkhtmltox.so"));
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    //var libContext = new CustomAssemblyLoadContext();
                    //libContext.LoadUnmanagedLibrary(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "libwkhtmltox.dll"));
                }

                services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            }
            catch(Exception ex)
            {
				Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Medium));
				throw;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory)
        {
            try
            {
                loggerFactory.AddSerilog();
                app.UseSwagger();

				app.UseAuthentication();
				// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
				// specifying the Swagger JSON endpoint.
				app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "OER API V1");
                });

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseHsts();
                    app.ConfigureExceptionHandler(Log.Logger);
                }
                // Enable Cors
                app.UseCors("OERCorsPolicy");
                app.UseMvc();
                app.Run(async (context) =>
                {
                 
                    await context.Response.WriteAsync("OER Service!");
                });
            }
            catch(Exception ex)
            {
				Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Medium));
				throw;
            }

        }
    }
}
