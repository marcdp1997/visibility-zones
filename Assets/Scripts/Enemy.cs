using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private GameObject _mesh;

    private const float AwareDistance = 3;
    private const float ChangeDirectionCooldown = 1.5f;
    private float _changeDirectionTimer;

    protected override void Awake()
    {
        base.Awake();
        _entityType = EntityType.Enemy;
    }

    private void FixedUpdate()
    {
        CheckCollision();
    }

    protected override void Move()
    {
        transform.position += _speed * Time.deltaTime * transform.forward;

        if ((_changeDirectionTimer -= Time.deltaTime) <= 0)
        {
            _changeDirectionTimer = ChangeDirectionCooldown;
            _targetMovement = Quaternion.AngleAxis(Random.Range(-90, 90), transform.up) * transform.forward;
        }
    }

    private void CheckCollision()
    {
        if (Physics.Raycast(transform.position, _targetMovement, out RaycastHit hit, AwareDistance, 1 << LayerMask.NameToLayer("Wall")))
        {
            _changeDirectionTimer = ChangeDirectionCooldown;
            _targetMovement = Vector3.Reflect(_targetMovement, hit.normal);
        }
    }

    protected override void OnEntityZoneChanged(EventData eventData)
    {
        OnEntityZoneChangedEventData data = (OnEntityZoneChangedEventData)eventData;

        if (data.ChangingEntity.GetEntityType() == EntityType.Player)
        {
            Player player = (Player)data.ChangingEntity;

            foreach (Zone enemyZone in _enteredZones)
            {
                foreach (Zone playerZone in player.GetEnteredZones())
                {
                    if (enemyZone.Id == playerZone.Id || playerZone.GetNeighbourZones().Contains(enemyZone))
                    {
                        EnableMesh(true);
                        return;
                    }
                }
            }

            EnableMesh(false);
        }
    }

    public void EnableMesh(bool state) 
    {
        _mesh.SetActive(state);
    }
}
