using UnityEngine;

[CreateAssetMenu(fileName = "PrefabLibrary", menuName = "ScriptableObjects/PrefabLibrary", order = 1)]
public class PrefabLibrary : ScriptableObject
{
    public Player PlayerPrefab;
    public Enemy EnemyPrefab;
}
