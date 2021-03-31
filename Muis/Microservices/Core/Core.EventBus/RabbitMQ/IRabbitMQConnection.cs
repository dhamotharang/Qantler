using System;
using RabbitMQ.Client;

namespace Core.EventBus.RabbitMQ
{
  public interface IRabbitMQConnection : IDisposable
  {
    bool IsConnected { get; }

    bool TryConnect();

    IModel CreateModel();
  }
}
