using System;
using System.Linq;
using UnityEngine;

public class Player : Entity
{
    private const float DestroyRadius = 1.5f;
    private Zone[] _zones;
    private int _currZone;
    private IFactory<Enemy> _enemyFactory;

    protected override void Awake()
    {
        base.Awake();

        _entityType = EntityType.Player;
        _zones = FindObjectsOfType<Zone>();
        _enemyFactory = ServiceLocator.Instance.GetService<IFactory<Enemy>>();

        MoveToZone(false);
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
            MoveToZone(true);

        if (Input.GetKeyDown(KeyCode.G) && IsInAnyZone())
            _enemyFactory.Spawn(_enteredZones.Last().transform.position);
    }

    private void FixedUpdate()
    {
        CheckClosestEnemies();
    }

    protected override void Move()
    {
        _targetMovement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.position += _speed * Time.deltaTime * _targetMovement;
    }

    private void CheckClosestEnemies()
    {
        if (!IsInAnyZone()) return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, DestroyRadius, 1 << LayerMask.NameToLayer("Enemy"));
        foreach(Collider collider in colliders)
        {
            Enemy deadEnemy = collider.gameObject.GetComponent<Enemy>();
            _enemyFactory.Despawn(deadEnemy);
        }
    }

    private void MoveToZone(bool next)
    {
        if (_zones.Length > 0)
        {
            if (next) _currZone = (_currZone < _zones.Length - 1) ? _currZone + 1 : 0;
            transform.position = _zones[_currZone].transform.position;
        }
    }

    protected override void OnEntityZoneChanged(EventData eventData)
    {
        OnEntityZoneChangedEventData data = (OnEntityZoneChangedEventData)eventData;

        if (data.ChangingEntity == this && IsInAnyZone())
        {
            _currZone = Array.IndexOf(_zones, _enteredZones.Last());
        }

        if (data.ChangingEntity.GetEntityType() == EntityType.Enemy)
        {
            Enemy enemy = (Enemy)data.ChangingEntity;

            foreach (Zone enemyZone in enemy.GetEnteredZones())
            {
                foreach (Zone playerZone in _enteredZones)
                {
                    if (enemyZone.Id == playerZone.Id || playerZone.GetNeighbourZones().Contains(enemyZone))
                    {
                        enemy.EnableMesh(true);
                        return;
                    }
                }
            }

            enemy.EnableMesh(false);
        }
    }

    private bool IsInAnyZone() { return _enteredZones.Count > 0; }
}
