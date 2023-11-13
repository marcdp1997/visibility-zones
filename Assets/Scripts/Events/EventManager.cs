using System;
using System.Collections.Generic;

public class EventData
{
    public readonly EventIds EventId;

    public EventData(EventIds eventId)
    {
        EventId = eventId;
    }
}

public enum EventIds
{
    OnEntityZoneChanged,
}

public class EventManager : IEventManager
{
    private readonly Dictionary<EventIds, Action<EventData>> _eventDictionary;

    public EventManager()
    {
        _eventDictionary ??= new Dictionary<EventIds, Action<EventData>>();
    }

    public void Subscribe(EventIds eventId, Action<EventData> listener)
    {
        if (_eventDictionary.ContainsKey(eventId))
        {
            _eventDictionary[eventId] += listener;
        }
        else
        {
            _eventDictionary.Add(eventId, listener);
        }
    }

    public void Unsubscribe(EventIds eventId, Action<EventData> listener)
    {
        if (_eventDictionary.ContainsKey(eventId))
        {
            _eventDictionary[eventId] -= listener;
        }
    }

    public void Publish(EventData eventData)
    {
        if (_eventDictionary.TryGetValue(eventData.EventId, out Action<EventData> thisEvent))
        {
            thisEvent?.Invoke(eventData);
        }
    }
}

