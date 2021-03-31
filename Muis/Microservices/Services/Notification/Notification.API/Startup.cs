using Autofac;
using Core.EventBus;
using Core.EventBus.RabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Notification.API.EventHandlers;
using Notification.API.Services;
using Notification.Service;
using Notification.Events;
using Core.API;
using Core.API.Provider;
using Dapper;
using Core.API.Repository;

namespace Notification.API
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
      services.AddControllers();

      services.AddTransient<INotificationService, NotificationService>()
        .AddTransient<IDbConnectionProvider, DbConnectionProvider>();

      services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

      RegisterEventBus(services);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });

      ConfigureEventBus(app);

      SqlMapper.AddTypeHandler(new UtcTypeHandler());
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

      services.AddSingleton<SendNotificationEventHandler>();
      services.AddSingleton<ISubscriptionManager, DefaultSubscriptionManager>();
    }

    void ConfigureEventBus(IApplicationBuilder app)
    {
      var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
      eventBus.Subscribe<SendNotificationEvent, SendNotificationEventHandler>();
      // Subscribe event & handlers here
    }
  }
}
