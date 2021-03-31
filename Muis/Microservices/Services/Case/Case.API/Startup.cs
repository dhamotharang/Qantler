using Autofac;
using Case.API.Middleware;
using Core.API;
using Core.API.Provider;
using Core.API.Repository;
using Case.API.Service;
using Core.EventBus;
using Core.EventBus.RabbitMQ;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Case.API.EventHandlers;
using Case.Events;
using Core.API.Smtp;

namespace Case.API
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers()
        .AddNewtonsoftJson(options =>
        {
          options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        });

      services.AddTransient<IDbConnectionProvider, DbConnectionProvider>()
        .AddTransient<ICaseService, CaseService>()
        .AddTransient<IMasterService, MasterService>()
        .AddTransient<IActivityService, ActivityService>()
        .AddTransient<IEmailService, EmailService>()
        .AddTransient<ILetterService, LetterService>()
        .AddTransient<ISmtpProvider, SmtpProvider>();

      services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"))
        .Configure<SmtpConfig>(Configuration.GetSection("SmtpConfig"));

      RegisterEventBus(services);

      SqlMapper.AddTypeHandler(new UtcTypeHandler());
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseMiddleware<ApiExceptionMiddleware>();

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });

      ConfigureEventBus(app);
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

      services.AddSingleton<ISubscriptionManager, DefaultSubscriptionManager>();

      // Register event handlers here
      services.AddTransient<OnJobOrderStatusChangedEventHandler>()
        .AddTransient<OnPaymentProcessedEventHandler>();
    }

    void ConfigureEventBus(IApplicationBuilder app)
    {
      var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
      eventBus.Subscribe<OnJobOrderStatusChangedEvent, OnJobOrderStatusChangedEventHandler>();
      eventBus.Subscribe<OnPaymentProcessedEvent, OnPaymentProcessedEventHandler>();
      // Subscribe event & handlers here
    }
  }
}
