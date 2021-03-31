using System;
using System.IO;
using Autofac.Extensions.DependencyInjection;
using JobOrder.Jobs.Actions;
using JobOrder.Jobs.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

namespace JobOrder.Jobs
{
  class Program
  {
    static void Main(string[] args)
    {
      var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
      if (string.IsNullOrEmpty(environment))
      {
        environment = "Development";
      }

      var directory = Directory.GetCurrentDirectory();

      var config = new ConfigurationBuilder()
        .SetBasePath(directory)
        .AddCommandLine(args)
        .AddJsonFile($"appsettings.{environment}.json")
        .Build();

      var services = new ServiceCollection();
      services.AddLogging();

      var startup = new Startup(config);
      startup.ConfigureService(services);

      var containerBuilder = new Autofac.ContainerBuilder();
      containerBuilder.Populate(services);

      var container = containerBuilder.Build();
      var provider = new AutofacServiceProvider(container);

      var logger = provider.GetService<ILoggerFactory>();
      logger.AddNLog();

      GlobalDiagnosticsContext.Set("currentDirectory", directory);
      LogManager.LoadConfiguration("nlog.config");

      IAction action = null;

      var jobConfig = config.GetSection("JobConfig").Get<JobConfig>();

      switch (jobConfig.Action)
      {
        case "schedulePeriodicInspection":
          action = provider.GetService<SchedulePeriodicInspectionAction>();
          break;
      }

      try
      {
        action?.Invoke().GetAwaiter().GetResult();
      }
      catch (Exception ex)
      {
        var log = LogManager.GetCurrentClassLogger();
        log.Error(ex);
      }
    }
  }
}

