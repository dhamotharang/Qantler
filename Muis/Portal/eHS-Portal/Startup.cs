using System;
using System.Collections.Generic;
using Autofac;
using Core.EventBus;
using Core.EventBus.RabbitMQ;
using eHS.Portal.Client;
using eHS.Portal.Configs;
using eHS.Portal.EventHandlers;
using eHS.Portal.Events;
using eHS.Portal.Hubs;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Services;
using eHS.Portal.Services.Auth;
using eHS.Portal.Services.Bill;
using eHS.Portal.Services.Certificate;
using eHS.Portal.Services.Checklist;
using eHS.Portal.Services.Cluster;
using eHS.Portal.Services.Emails;
using eHS.Portal.Services.Master;
using eHS.Portal.Services.Payment;
using eHS.Portal.Services.RFA;
using eHS.Portal.Services.Bank;
using eHS.Portal.Services.Settings;
using eHS.Portal.Services.TransactionCode;
using Enyim.Caching.Configuration;
using IronPdf;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using eHS.Portal.Infrastructure.Filters;
using Microsoft.Net.Http.Headers;
using eHS.Portal.Services.Code;
using eHS.Portal.Services.Customer;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using eHS.Portal.Services.HalalLibrary;
using eHS.Portal.Services.Case;
using eHS.Portal.Services.Statistics;
using eHS.Portal.Services.Certificate360;
using eHS.Portal.Services.CaseLetter;

namespace eHS.Portal
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie();

      services.AddControllersWithViews(o =>
      {
        o.Filters.Add(typeof(ExceptionFilter));
      }).AddNewtonsoftJson(options =>
        {
          options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
          options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
          options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
        });

      services.AddSignalR();
      services.AddControllersWithViews().AddRazorRuntimeCompilation();
      RegisterAppServices(services);
      RegisterEventBus(services);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseWhen(o => o.Request.HttpContext.Request.Path.StartsWithSegments("/api"),
        c =>
        {
          c.UseApiExceptionMiddleware();
        });

      var culture = CultureInfo.CreateSpecificCulture("en-SG");
      var dateformat = new DateTimeFormatInfo
      {
        ShortDatePattern = "yyyy-MM-dd",
        LongDatePattern = "yyyy-MM-ddThh:mm:ssK"
      };
      culture.DateTimeFormat = dateformat;

      var supportedCultures = new[]
      {
        culture
      };

      app.UseRequestLocalization(new RequestLocalizationOptions
      {
        DefaultRequestCulture = new RequestCulture(culture),
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures
      });

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");

        endpoints.MapHub<SignalRHub>("/signalRHub");
      });

      ConfigureEventBus(app);

      License.LicenseKey = Configuration["Pdf:Key"];
    }

    void RegisterAppServices(IServiceCollection services)
    {
      services.AddSingleton<ISignalRClient, SignalRClient>()
        .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
        .AddTransient<ApiClient>()
        .AddTransient<IRequestService, RequestService>()
        .AddTransient<IChecklistService, ChecklistService>()
        .AddTransient<IJobOrderService, JobOrderService>()
        .AddTransient<IIdentityService, IdentityService>()
        .AddTransient<IRFAService, RFAService>()
        .AddTransient<ISettingsService, SettingsService>()
        .AddTransient<IMasterService, MasterService>()
        .AddTransient<IEmailService, EmailService>()
        .AddTransient<ITransactionCodeService, TransactionCodeService>()
        .AddTransient<IClusterService, ClusterService>()
        .AddTransient<ICertificateService, CertificateService>()
        .AddTransient<ICertificate360Service, Certificate360Service>()
        .AddTransient<IPaymentService, PaymentService>()
        .AddTransient<IBillService, BillService>()
        .AddTransient<IAuthService, AuthService>()
        .AddTransient<ICodeService, CodeService>()
        .AddTransient<ICustomerService, CustomerService>()
        .AddTransient<IHttpRequestProvider, HttpRequestProvider>()
        .AddTransient<IHalalLibraryService, HalalLibraryService>()
        .AddTransient<ICaseService, CaseService>()
        .AddTransient<IPremiseService, PremiseService>()
        .AddTransient<IStatisticsService, StatisticsService>()
        .AddTransient<IBankService, BankService>()
        .AddTransient<ICaseLetterService, CaseLetterService>();

      services.AddScoped<PermissionFilter>()
        .AddScoped<SessionAwareFilter>();

      services.AddSingleton<ICacheProvider, CacheProvider>();

      services.Configure<UrlConfig>(Configuration.GetSection("UrlConfig"))
        .Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));

      var cacheConfig = Configuration.GetSection("CacheConfig").Get<CacheConfig>();

      services.AddEnyimMemcached(o =>
      {
        o.Servers = new List<Server>
        {
          new Server
          {
            Address = cacheConfig.Host,
            Port = cacheConfig.Port
          }
        };
      });
    }

    void RegisterEventBus(IServiceCollection services)
    {
      var eventBusConfig = Configuration.GetSection("EventBus").Get<EventBusConfig>();

      if (eventBusConfig.Provider.Equals("rabbitmq"))
      {
        services.AddSingleton<IRabbitMQConnection>(sp =>
        {
          var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQConnection>>();
          var factory = new ConnectionFactory
          {
            HostName = eventBusConfig.Host,
            DispatchConsumersAsync = false,
            UserName = eventBusConfig.UserName,
            Password = eventBusConfig.Password
          };

          return new DefaultRabbitMQConnection(factory, logger, eventBusConfig.RetryCount);
        });

        services.AddSingleton<IEventBus, RabbitMQEventBus>(sp =>
        {
          var connection = sp.GetRequiredService<IRabbitMQConnection>();
          var lifetimeScope = sp.GetRequiredService<ILifetimeScope>();
          var logger = sp.GetRequiredService<ILogger<RabbitMQEventBus>>();
          var subsManager = sp.GetRequiredService<ISubscriptionManager>();

          return new RabbitMQEventBus(connection, logger, lifetimeScope, subsManager,
            eventBusConfig.Channel, eventBusConfig.ClientName, eventBusConfig.RetryCount);
        });
      }

      services.AddSingleton<OnNotificationSentEventHandler>();
      services.AddSingleton<ISubscriptionManager, DefaultSubscriptionManager>();
      // Register event handlers here
    }

    void ConfigureEventBus(IApplicationBuilder app)
    {
      var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
      eventBus.Subscribe<OnNotificationSentEvent, OnNotificationSentEventHandler>();
      // Subscribe event & handlers here
    }
  }
}
