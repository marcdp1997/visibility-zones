using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Zone : MonoBehaviour
{
    [SerializeField] public string Id;
    [SerializeField] public string Name;
    [SerializeField] private List<string> NeighbourIds = new List<string>();

    private List<Zone> _neighbourZones;

    private void Awake()
    {
        _neighbourZones = FindObjectsOfType<Zone>().
            Where(zone => NeighbourIds.Contains(zone.Id)).ToList();
    }

    public List<Zone> GetNeighbourZones() { return _neighbourZones; }
}
