using System;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Core.EventBus.RabbitMQ
{
  public class DefaultRabbitMQConnection : IRabbitMQConnection
  {
    readonly object lockObj = new object();

    public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

    int _retryCount;
    bool _disposed;

    IConnection _connection;
    IConnectionFactory _connectionFactory;

    ILogger<DefaultRabbitMQConnection> _logger;

    public DefaultRabbitMQConnection(IConnectionFactory connectionFactory,
      ILogger<DefaultRabbitMQConnection> logger,
      int retryCount = 5)
    {
      _retryCount = retryCount;
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _connectionFactory = connectionFactory
                        ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public IModel CreateModel()
    {
      if (!IsConnected)
      {
        throw new InvalidOperationException("No RabbitMQ connections are available to " +
          "perform this action");
      }

      return _connection.CreateModel();
    }

    public bool TryConnect()
    {
      _logger.LogInformation("RabbitMQ Client is trying to connect");

      lock (lockObj)
      {
        var policy = RetryPolicy.Handle<SocketException>()
          .Or<BrokerUnreachableException>()
          .WaitAndRetry(_retryCount,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
            {
              _logger.LogWarning(ex, "RabbitMQ Client could not connect after " +
                "{TimeOut}s ({ExceptionMessage})",
                $"{time.TotalSeconds:n1}",
                ex.Message);
            }
          );

        policy.Execute(() =>
        {
          _connection = _connectionFactory.CreateConnection();
        });

        if (IsConnected)
        {
          _connection.ConnectionShutdown += OnConnectionShutdown;
          _connection.CallbackException += OnCallbackException;
          _connection.ConnectionBlocked += OnConnectionBlocked;

          _logger.LogInformation("RabbitMQ Client acquired a persistent connection " +
            "to '{HostName}' and is subscribed to failure events",
            _connection.Endpoint.HostName);

          return true;
        }
        else
        {
          _logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");

          return false;
        }
      }
    }

    private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
    {
      if (_disposed)
      {
        return;
      }

      _logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

      TryConnect();
    }

    void OnCallbackException(object sender, CallbackExceptionEventArgs e)
    {
      if (_disposed)
      {
        return;
      }

      _logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

      TryConnect();
    }

    void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
    {
      if (_disposed)
      {
        return;
      }

      _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

      TryConnect();
    }

    public void Dispose()
    {
      if (_disposed)
      {
        return;
      }
      _disposed = true;

      try
      {
        _connection.Dispose();
      }
      catch (Exception ex)
      {
        _logger.LogCritical(ex.ToString());
      }
    }
  }
}
