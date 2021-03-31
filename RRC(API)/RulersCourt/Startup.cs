using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RulersCourt.Models;
using RulersCourt.Services;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Text;
using Workflow;
using Workflow.Interface;
using Workflow.Utility;

namespace RulersCourt
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            IConfigurationBuilder encryptingBuilder =
            new ConfigurationBuilder()
                   .AddEncryptedProvider(configuration, Convert.FromBase64String(configuration["EncryptionKeys:Key"]), Convert.FromBase64String(configuration["EncryptionKeys:IV"]));

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors(c =>
            //{
            //    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowedToAllowWildcardSubdomains());
            //});

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new Info { Title = "RRC - General Module", Version = "1.0" });
            });
            services.Configure<ConnectionSettingsModel>(Configuration.GetSection("ConnectionSettings"));
            services.Configure<LdapConfig>(Configuration.GetSection("LdapConfig"));
            services.Configure<UIConfigModel>(Configuration.GetSection("UIConfig"));
            services.Configure<APIConfigModel>(Configuration.GetSection("APIConfig"));
            services.Configure<NotificationConfigModel>(Configuration.GetSection("NotificationConfig"));
            services.Configure<WrdUserLoginCredentialsModel>(Configuration.GetSection("WrdUserLoginCredentials"));
            var settings = new Settings()
            {
                ConnectionString = Configuration["ConnectionSettings:ConnectionString"],
                EmailHost = Configuration["EmailSettings:SMTPHost"],
                EmailPort = Convert.ToInt32(Configuration["EmailSettings:SMTPPort"]),
                EnableSsl = Convert.ToBoolean(Configuration["EmailSettings:EnableSsl"]),
                EmailId = Configuration["EmailSettings:Email"],
                Password = Configuration["EmailSettings:Password"],
                SMSProviderURL = string.Format("{0}{1}{2}", Configuration["SMSProvider:SMSProviderUrl"], "?uname=" + Configuration["SMSProvider:SMSProviderUserName"], "&passwd=" + Configuration["SMSProvider:SMSProviderPassword"]),
                SysLogHost = Configuration["Logging:Address"],
                SysLogPort = Convert.ToInt32(Configuration["Logging:Port"]),
                UIConfigUrl = Configuration["UIConfig:Url"]
            };
            services.AddTransient<IWorkflowClient>(s => new WorkflowClient(settings));

            var key = Encoding.ASCII.GetBytes(Configuration["ConnectionSettings:AuthSecret"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddScoped<IAuthenticationService, LdapAuthenticationService>();
            services.AddScoped<AccessControlAttribute>();
            services.AddTransient(ec => new EncryptionService(new KeyInfo(Configuration["EncryptionKeys:Key"], Configuration["EncryptionKeys:IV"])));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAuthentication();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            RegisterStaticFolder(app, "Downloads");
            RegisterStaticFolder(app, "Uploads");
            app.UseHttpsRedirection();
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowedToAllowWildcardSubdomains());
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1.0/swagger.json", "RRC (V 1.0)");
            });
            app.UseMvc();
            Func<string, LogLevel, bool> config = LogLevel;
            loggerFactory.AddSyslog(Configuration["Logging:Address"], Convert.ToInt32(Configuration["Logging:Port"]), config);
        }

        private static void RegisterStaticFolder(IApplicationBuilder app, string directoryName)
        {
            string fullDirectoryName = Path.Combine(Directory.GetCurrentDirectory(), directoryName);
            if (!Directory.Exists(fullDirectoryName)) { Directory.CreateDirectory(fullDirectoryName); }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(fullDirectoryName),
                RequestPath = "/" + directoryName
            });
        }

        private bool LogLevel(string str, LogLevel currentLogLevel)
        {
            str = Configuration["Logging:LogLevel"];
            LogLevel configLogLevel;
            Enum.TryParse(str, out configLogLevel);
            if (Convert.ToInt32(currentLogLevel) >= Convert.ToInt32(configLogLevel))
                return true;
            else
                return false;
        }
    }
}