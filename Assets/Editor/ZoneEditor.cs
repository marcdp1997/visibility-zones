using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Zone))]
public class ZoneEditor : Editor
{
    private SerializedProperty _idProperty;
    private SerializedProperty _neighboursIdsProperty;
    private ZoneLibrary _zoneLibrary;

    private string _prevName;

    private void OnEnable()
    {
        _zoneLibrary = Resources.Load<ZoneLibrary>("ZoneLibrary");
        _prevName = target.name;

        _idProperty = serializedObject.FindProperty("Id");
        _neighboursIdsProperty = serializedObject.FindProperty("NeighbourIds");
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

        List<string> zoneNames = new List<string>();
        for (int i = 0; i < _zoneLibrary.Zones.Count; i++)
            if (_zoneLibrary.Zones[i].Id != _idProperty.stringValue)
                zoneNames.Add(_zoneLibrary.Zones[i].Name);

        for (int i = 0; i < _neighboursIdsProperty.arraySize; i++)
        {
            int selectedZoneIndex = _zoneLibrary.Zones.FindIndex(zone => zone.Id == _neighboursIdsProperty.GetArrayElementAtIndex(i).stringValue);

            if (selectedZoneIndex != -1)
            {
                int newSelectedZoneIndex = EditorGUILayout.Popup(selectedZoneIndex, zoneNames.ToArray());
                _neighboursIdsProperty.GetArrayElementAtIndex(i).stringValue = _zoneLibrary.Zones[newSelectedZoneIndex].Id;
            }
            else _neighboursIdsProperty.DeleteArrayElementAtIndex(i);
        }

        if (GUILayout.Button("Add"))
        {
            int arraySize = _neighboursIdsProperty.arraySize;
            _neighboursIdsProperty.InsertArrayElementAtIndex(arraySize);
            _neighboursIdsProperty.GetArrayElementAtIndex(arraySize).stringValue = _zoneLibrary.Zones[0].Id;
        }

        if (GUILayout.Button("Remove") && _neighboursIdsProperty.arraySize > 0)
        {
            _neighboursIdsProperty.DeleteArrayElementAtIndex(_neighboursIdsProperty.arraySize - 1);
        }
    }
}
