public class OnEntityZoneChangedEventData : EventData
{
    public readonly Entity ChangingEntity;
    public readonly Zone Zone;
    public readonly bool Entered;

    public OnEntityZoneChangedEventData(Entity changingEntity, Zone zone, bool entered) : base(EventIds.OnEntityZoneChanged)
    {
        ChangingEntity = changingEntity;
        Zone = zone;
        Entered = entered;
    }
}
