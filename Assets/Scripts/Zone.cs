using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Zone : MonoBehaviour
{
    [SerializeField] public string Id;
    [SerializeField] private List<string> NeighbourIds = new List<string>();

    private List<Zone> _neighbourZones;
    private Collider _collider;

    private void Awake()
    {
        _neighbourZones = FindObjectsOfType<Zone>().
            Where(zone => NeighbourIds.Contains(zone.Id)).ToList();
    }

    private void OnDrawGizmos()
    {
        _collider = GetComponent<Collider>();
        Gizmos.color = new Vector4(0.5f, 0.5f, 0.5f, 0.7f);

        if (_collider is BoxCollider boxCollider)
        {
            DrawBoxColliderGizmo(boxCollider);
        }
        else if (_collider is SphereCollider sphereCollider)
        {
            DrawSphereColliderGizmo(sphereCollider);
        }
    }

    private void DrawBoxColliderGizmo(BoxCollider boxCollider)
    {
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
    }

    private void DrawSphereColliderGizmo(SphereCollider sphereCollider)
    {
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawWireSphere(sphereCollider.center, sphereCollider.radius);
    }

    public List<Zone> GetNeighbourZones() { return _neighbourZones; }
}
