using System;
using Autofac;
using Core.Base;
using Core.Base.Repository;
using Core.EventBus;
using Core.EventBus.RabbitMQ;
using JobOrder.Jobs.Actions;
using JobOrder.Jobs.Configs;
using JobOrder.Jobs.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace JobOrder.Jobs
{
  public class Startup
  {
    readonly IConfiguration Configuration;

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public void ConfigureService(IServiceCollection services)
    {
      // Register services here
      services.AddTransient<SchedulePeriodicInspectionAction>()
        .AddTransient<IPeriodicInspectionService, PeriodicInspectionService>()
        .AddTransient<IDbConnectionProvider, DbConnectionProvider>();

      RegisterEventBus(services);

      services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
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
    }
  }
}
