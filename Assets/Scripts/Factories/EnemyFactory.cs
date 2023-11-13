using UnityEngine;
using UnityEngine.Pool;

public class EnemyFactory : IFactory<Enemy>
{
    private readonly Enemy _prefab;
    private readonly ObjectPool<Enemy> _poolEnemies;

    public EnemyFactory()
    {
        _prefab = Resources.Load<PrefabLibrary>("PrefabLibrary").EnemyPrefab;
        _poolEnemies = new ObjectPool<Enemy>(OnCreate, OnGet, OnRelease);
    }

    public Enemy Spawn(Vector3 position)
    {
        Enemy enemy = _poolEnemies.Get();
        enemy.transform.position = position;
        return enemy;
    }

    public void Despawn(Enemy enemy)
    {
        _poolEnemies.Release(enemy);
    }

    private Enemy OnCreate()
    {
        Enemy enemy = Object.Instantiate(_prefab);
        return enemy;
    }

    private void OnGet(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    private void OnRelease(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }
}
