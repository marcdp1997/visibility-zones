using UnityEngine;

public interface IFactory<T>
{
    public T Spawn(Vector3 position);

    public void Despawn(T objectToRelease);
}
