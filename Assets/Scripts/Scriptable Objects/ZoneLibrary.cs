using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ZoneConfig
{
    public string Name;
    public string Id;

    public ZoneConfig(string id, string name)
    {
        Id = id;
        Name = name;
    }
}

[CreateAssetMenu(fileName = "ZoneLibrary", menuName = "ScriptableObjects/ZoneLibrary", order = 2)]
public class ZoneLibrary : ScriptableObject
{
    public List<ZoneConfig> Zones;

    public void ChangeZoneName(string id, string name)
    {
        ZoneConfig zoneConfig = Zones.FirstOrDefault(zoneConfig => zoneConfig.Id == id);

        if (zoneConfig != null) zoneConfig.Name = name;
        else Debug.Log($"Zone with id '{id}' not found to change its name");
    }

    public void AddZone(string id, string name)
    {
        if (!Zones.Any(zone => zone.Id == id))
        {
            Zones.Add(new ZoneConfig(id, name));
        }
    }

    public void RemoveZone(string id)
    {
        ZoneConfig zoneConfig = Zones.FirstOrDefault(zoneConfig => zoneConfig.Id == id);

        if (zoneConfig != null) Zones.Remove(zoneConfig);
        else Debug.Log($"Zone with id '{id}' not found to remove it");
    }
}
