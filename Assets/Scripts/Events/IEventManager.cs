using System;

public interface IEventManager
{
    public void Subscribe(EventIds eventId, Action<EventData> listener);

    public void Unsubscribe(EventIds eventId, Action<EventData> listener);

    public void Publish(EventData eventData);
}
