using UnityEngine;

public class ServiceInstaller : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.Instance.RegisterService<IEventManager>(new EventManager());
        ServiceLocator.Instance.RegisterService<IFactory<Enemy>>(new EnemyFactory());
    }

    private void OnDestroy()
    {
        ServiceLocator.Instance.UnregisterService<IEventManager>();
        ServiceLocator.Instance.UnregisterService<IFactory<Enemy>>();
    }
}
