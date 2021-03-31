using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.EventBus
{
  public class DefaultSubscriptionManager : ISubscriptionManager
  {
    readonly List<Type> _eventTypes;
    readonly Dictionary<string, List<SubscriptionInfo>> _handlers;

    public event EventHandler<string> OnEventRemoved;

    public bool IsEmpty => !_handlers.Keys.Any();
    public void Clear() => _handlers.Clear();

    public DefaultSubscriptionManager()
    {
      _handlers = new Dictionary<string, List<SubscriptionInfo>>();
      _eventTypes = new List<Type>();
    }

    public void AddDynamicSubscription<TH>(string eventName)
      where TH : IDynamicEventHandler
    {
      DoAddSubscription(typeof(TH), eventName, isDynamic: true);
    }

    public void AddSubscription<T, TH>()
      where T : Event
      where TH : IEventHandler<T>
    {
      var eventName = GetEventKey<T>();
      DoAddSubscription(typeof(TH), eventName, isDynamic: false);

      _eventTypes.Add(typeof(T));
    }

    void DoAddSubscription(Type handlerType, string eventName, bool isDynamic)
    {
      if (!HasSubscriptionsForEvent(eventName))
      {
        _handlers.Add(eventName, new List<SubscriptionInfo>());
      }

      if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
      {
        throw new ArgumentException(
            $"Handler Type {handlerType.Name} already registered for '{eventName}'",
            nameof(handlerType));
      }

      if (isDynamic)
      {
        _handlers[eventName].Add(SubscriptionInfo.Dynamic(handlerType));
      }
      else
      {
        _handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
      }
    }

    public void RemoveDynamicSubscription<TH>(string eventName)
      where TH : IDynamicEventHandler
    {
      var handlerToRemove = FindDynamicSubscriptionToRemove<TH>(eventName);
      DoRemoveHandler(eventName, handlerToRemove);
    }

    public void RemoveSubscription<T, TH>()
      where T : Event
      where TH : IEventHandler<T>
    {
      var handlerToRemove = FindSubscriptionToRemove<T, TH>();
      var eventName = GetEventKey<T>();
      DoRemoveHandler(eventName, handlerToRemove);
    }

    SubscriptionInfo FindDynamicSubscriptionToRemove<TH>(string eventName)
      where TH : IDynamicEventHandler
    {
      return DoFindSubscriptionToRemove(eventName, typeof(TH));
    }

    SubscriptionInfo FindSubscriptionToRemove<T, TH>()
      where T : Event
      where TH : IEventHandler<T>
    {
      var eventName = GetEventKey<T>();
      return DoFindSubscriptionToRemove(eventName, typeof(TH));
    }

    SubscriptionInfo DoFindSubscriptionToRemove(string eventName, Type handlerType)
    {
      if (!HasSubscriptionsForEvent(eventName))
      {
        return null;
      }

      return _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
    }

    void DoRemoveHandler(string eventName, SubscriptionInfo subsToRemove)
    {
      if (subsToRemove != null)
      {
        _handlers[eventName].Remove(subsToRemove);

        if (!_handlers[eventName].Any())
        {
          _handlers.Remove(eventName);

          var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
          if (eventType != null)
          {
            _eventTypes.Remove(eventType);
          }
          OnEventRemoved?.Invoke(this, eventName);
        }
      }
    }

    public bool HasSubscriptionsForEvent(string eventName)
    {
      return _handlers.ContainsKey(eventName);
    }

    public bool HasSubscriptionsForEvent<T>() where T : Event
    {
      var key = GetEventKey<T>();
      return HasSubscriptionsForEvent(key);
    }

    public Type GetEventTypeByName(string eventName)
    {
      return _eventTypes.SingleOrDefault(t => t.Name == eventName);
    }

    public string GetEventKey<T>()
    {
      return typeof(T).Name;
    }

    public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : Event
    {
      var key = GetEventKey<T>();
      return GetHandlersForEvent(key);
    }

    public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName)
    {
      return _handlers[eventName];
    }
  }
}
