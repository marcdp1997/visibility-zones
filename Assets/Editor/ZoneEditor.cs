using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Zone))]
public class ZoneEditor : Editor
{
    private SerializedProperty _idProperty;
    private SerializedProperty _neighboursIdsProperty;
    private ZoneLibrary _zoneLibrary;

    private string _prevName;
    private List<ZoneConfig>  _availableZoneConfigs;
    private List<string> _availableNames;

    private void OnEnable()
    {
        _idProperty = serializedObject.FindProperty("Id");
        _neighboursIdsProperty = serializedObject.FindProperty("NeighbourIds");

        _zoneLibrary = Resources.Load<ZoneLibrary>("ZoneLibrary");
        _prevName = target.name;

        _availableZoneConfigs = new List<ZoneConfig>();
        _availableNames = new List<string>();
        _availableZoneConfigs = _zoneLibrary.Zones.Where(z => z.Id != _idProperty.stringValue).ToList();
        _availableNames = _availableZoneConfigs.Select(z => z.Name).ToList();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CreateUniqueIdProperty();
        EditorGUILayout.Space(10);
        CreateNeighbourProperty();

        serializedObject.ApplyModifiedProperties();
    }

    private void CreateUniqueIdProperty()
    {
        EditorGUILayout.LabelField("Unique ID", EditorStyles.boldLabel);
        EditorGUILayout.LabelField(_idProperty.stringValue.ToString());

        if (_prevName != target.name)
        {
            _zoneLibrary.ChangeZoneName(_idProperty.stringValue, target.name);
            _prevName = target.name;
        }
    }

    private void CreateNeighbourProperty()
    {
        EditorGUILayout.LabelField("Neighbours", EditorStyles.boldLabel);

        for (int i = 0; i < _neighboursIdsProperty.arraySize; i++)
        {
            int selectedZoneIndex = _availableZoneConfigs.FindIndex(zone => zone.Id == _neighboursIdsProperty.GetArrayElementAtIndex(i).stringValue);

            if (selectedZoneIndex != -1)
            {
                int newSelectedZoneIndex = EditorGUILayout.Popup(selectedZoneIndex, _availableNames.ToArray());
                _neighboursIdsProperty.GetArrayElementAtIndex(i).stringValue = _availableZoneConfigs[newSelectedZoneIndex].Id;
            }
            else _neighboursIdsProperty.DeleteArrayElementAtIndex(i);
        }

        if (GUILayout.Button("Add"))
        {
            int arraySize = _neighboursIdsProperty.arraySize;
            _neighboursIdsProperty.InsertArrayElementAtIndex(arraySize);
            _neighboursIdsProperty.GetArrayElementAtIndex(arraySize).stringValue = _availableZoneConfigs[0].Id;
        }

        if (GUILayout.Button("Remove") && _neighboursIdsProperty.arraySize > 0)
        {
            _neighboursIdsProperty.DeleteArrayElementAtIndex(_neighboursIdsProperty.arraySize - 1);
        }
    }
}
