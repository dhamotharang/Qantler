using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Core.Util;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Core.EventBus.RabbitMQ
{
  public class RabbitMQEventBus : IEventBus
  {
    const string AUTOFAC_SCOPE_NAME = "eventBus";

    private readonly IRabbitMQConnection _connection;
    private readonly ILogger<RabbitMQEventBus> _logger;
    private readonly ISubscriptionManager _subsManager;
    private readonly ILifetimeScope _autofac;

    private readonly int _retryCount;

    private IModel _channel;
    private string _broker;
    private string _queueName;

    public RabbitMQEventBus(IRabbitMQConnection connection, ILogger<RabbitMQEventBus> logger,
      ILifetimeScope autofac, ISubscriptionManager subsManager, string broker, string queueName = null,
      int retryCount = 5)
    {
      _connection = connection ?? throw new ArgumentNullException(nameof(connection));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _subsManager = subsManager ?? new DefaultSubscriptionManager();
      _broker = broker;
      _queueName = queueName;
      _channel = CreateConsumerChannel();
      _autofac = autofac;
      _retryCount = retryCount;
      _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
    }

    private void SubsManager_OnEventRemoved(object sender, string eventName)
    {
      if (!_connection.IsConnected)
      {
        _connection.TryConnect();
      }

      using (var channel = _connection.CreateModel())
      {
        channel.QueueUnbind(queue: _queueName,
            exchange: _broker,
            routingKey: eventName);

        if (_subsManager.IsEmpty)
        {
          _queueName = string.Empty;
          _channel.Close();
        }
      }
    }

    public void Publish(Event @event)
    {
      if (!_connection.IsConnected)
      {
        _connection.TryConnect();
      }

      var policy = RetryPolicy.Handle<BrokerUnreachableException>()
        .Or<SocketException>()
        .WaitAndRetry(_retryCount,
          retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
          {
            _logger.LogWarning(ex.ToString());
          });

      using (var channel = _connection.CreateModel())
      {
        var eventName = @event.GetType().Name;

        channel.ExchangeDeclare(exchange: _broker, type: "direct");

        var message = JsonConvert.SerializeObject(@event);
        var body = Encoding.UTF8.GetBytes(Gzip.Compress(message));

        policy.Execute(() =>
        {
          var properties = channel.CreateBasicProperties();
          properties.DeliveryMode = 2; // persistent

          channel.BasicPublish(exchange: _broker,
            routingKey: eventName,
            mandatory: true,
            basicProperties: properties,
            body: body);
        });
      }
    }

    public void Subscribe<T, TH>()
      where T : Event
      where TH : IEventHandler<T>
    {
      var eventName = _subsManager.GetEventKey<T>();
      DoInternalSubscription(eventName);
      _subsManager.AddSubscription<T, TH>();
    }

    public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicEventHandler
    {
      DoInternalSubscription(eventName);
      _subsManager.AddDynamicSubscription<TH>(eventName);
    }

    private void DoInternalSubscription(string eventName)
    {
      var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
      if (!containsKey)
      {
        if (!_connection.IsConnected)
        {
          _connection.TryConnect();
        }

        using (var channel = _connection.CreateModel())
        {
          channel.QueueBind(queue: _queueName,
          exchange: _broker,
          routingKey: eventName);
        }
      }
    }

    public void Unsubscribe<T, TH>()
      where TH : IEventHandler<T>
      where T : Event
    {
      _subsManager.RemoveSubscription<T, TH>();
    }

    public void UnsubscribeDynamic<TH>(string eventName)
      where TH : IDynamicEventHandler
    {
      _subsManager.RemoveDynamicSubscription<TH>(eventName);
    }

    private IModel CreateConsumerChannel()
    {
      if (!_connection.IsConnected)
      {
        _connection.TryConnect();
      }

      var channel = _connection.CreateModel();

      channel.ExchangeDeclare(exchange: _broker,
                              type: "direct");

      channel.QueueDeclare(queue: _queueName,
                           durable: true,
                           exclusive: false,
                           autoDelete: false,
                           arguments: null);


      var consumer = new EventingBasicConsumer(channel);
      consumer.Received += async (model, ea) =>
      {
        var eventName = ea.RoutingKey;
        var message = Encoding.UTF8.GetString(ea.Body.Span);

        await ProcessEvent(eventName, message);

        channel.BasicAck(ea.DeliveryTag, multiple: false);
      };

      channel.BasicConsume(queue: _queueName,
                           autoAck: false,
                           consumer: consumer);

      channel.CallbackException += (sender, ea) =>
      {
        _channel?.Dispose();
        _channel = CreateConsumerChannel();
      };

      return channel;
    }

    private async Task ProcessEvent(string eventName, string message)
    {
      message = Gzip.Decompress(message);

      if (_subsManager.HasSubscriptionsForEvent(eventName))
      {
        using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
        {
          var subscriptions = _subsManager.GetHandlersForEvent(eventName);

          foreach (var subscription in subscriptions)
          {
            if (subscription.IsDynamic)
            {
              var handler = scope.ResolveOptional(subscription.HandlerType) as IDynamicEventHandler;
              dynamic eventData = JObject.Parse(message);
              await handler.Handle(eventData);
            }
            else
            {
              var eventType = _subsManager.GetEventTypeByName(eventName);
              var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
              var handler = scope.ResolveOptional(subscription.HandlerType);
              var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);

              await (Task)concreteType.GetMethod("Handle").Invoke(
                handler,
                new object[] { integrationEvent });
            }
          }
        }
      }
    }

    public void Dispose()
    {
      if (_channel != null)
      {
        _channel.Dispose();
      }

      _subsManager.Clear();
    }
  }
}
