using System.Collections.Generic;
using UnityEngine;

public enum EntityType { Player, Enemy }

public class Entity : MonoBehaviour
{
    [SerializeField] protected float _speed;
    [SerializeField] protected float _rotationSpeed;

    protected EntityType _entityType;
    protected Vector3 _targetMovement;
    protected IEventManager _eventManager;
    protected List<Zone> _enteredZones;

    protected virtual void Awake()
    {
        _enteredZones = new List<Zone>();
        _eventManager = ServiceLocator.Instance.GetService<IEventManager>();
        _eventManager.Subscribe(EventIds.OnEntityZoneChanged, OnEntityZoneChanged);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Zone"))
        {
            Zone zone = other.gameObject.GetComponent<Zone>();
            _enteredZones.Add(zone);
            _eventManager.Publish(new OnEntityZoneChangedEventData(this, zone, true));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Zone"))
        {
            Zone zone = other.gameObject.GetComponent<Zone>();
            _enteredZones.Remove(zone);
            _eventManager.Publish(new OnEntityZoneChangedEventData(this, zone, false));
        }
    }

    private void OnDestroy()
    {
        _eventManager.Unsubscribe(EventIds.OnEntityZoneChanged, OnEntityZoneChanged);
    }

    protected virtual void Update()
    {
        Move();
        Rotate();
    }

    private void OnDisable()
    {
        _enteredZones.Clear();
    }

    protected virtual void Move() { }

    private void Rotate()
    {
        if (_targetMovement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_targetMovement, transform.up);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            transform.rotation = rotation;
        }
    }

    protected virtual void OnEntityZoneChanged(EventData eventData) { }

    public EntityType GetEntityType() { return _entityType; }

    public List<Zone> GetEnteredZones() { return _enteredZones; }
}
