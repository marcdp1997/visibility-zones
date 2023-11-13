using System;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Zone))]
public class ZoneIdConfig : MonoBehaviour
{
    private ZoneLibrary _zoneLibrary;
    private Zone _zone;

    private void Start()
    {
        _zone = GetComponent<Zone>();
        _zoneLibrary = Resources.Load<ZoneLibrary>("ZoneLibrary");

        if (string.IsNullOrEmpty(_zone.Id) || IsDuplicated()) GenerateIdAndAddToZone();
        else AddToZone();
    }

    private void OnDestroy()
    {
        _zoneLibrary.RemoveZone(_zone.Id);
    }

    private void GenerateIdAndAddToZone()
    {
        _zone.Id = Guid.NewGuid().ToString();
        EditorUtility.SetDirty(_zone);

        AddToZone();
    }

    private void AddToZone()
    {
        _zoneLibrary.AddZone(_zone.Id, name);
        EditorUtility.SetDirty(_zoneLibrary);
    }

    private bool IsDuplicated()
    {
        Zone[] allZones = FindObjectsOfType<Zone>();

        foreach (Zone otherZone in allZones)
        {
            if (otherZone != _zone && otherZone.Id == _zone.Id)
                return true;
        }

        return false;
    }
}
